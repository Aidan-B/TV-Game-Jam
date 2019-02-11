using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct archive {
    public bool right;
    public Vector3 position;
    
    public archive(bool right, Vector3 position) {
        this.right = right;
        this.position = position;
    }
}

public class playerController : MonoBehaviour {

    public int playerLayer = 9;
    public int groundLayer = 10;
    public int passthroughLayer = 13;


    [Header("Movement Speeds")]
    public float walkSpeed = 2f;
    public float runMultiplier = 2.5f;
    public float jumpSpeed = 5f;
    

    [Header("Jump")]
    public bool onGround = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float fallGravity = 2.5f;
    public float lowJumpGravity = 2f;
    public float defaultGravity;

    [Header("Time Echos")]
    public int current;
    public int counter;
    public int echodelay;
    public GameObject echo;
    private GameObject madeEcho;
    public List<archive> TimeLine = new List<archive>();
    public List<GameObject> Echoes = new List<GameObject>();

    private Animator animate;

    [Header("Zombie")]
    public GameObject TheZombie;

    [Header("Status")]
    public float move = 0f;
    public bool jumpReq = false;
    public bool crouched = false;


    private bool faceRight = true;
    private Vector2 groundNormal = Vector2.zero;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
    }

    void Update ()
    {
        move = Input.GetAxis("Horizontal");
        if (Input.GetButton("Jump"))
        {
            jumpReq = true;
        }
        else
        {
            jumpReq = false;
        }
        if (Input.GetButton("Crouch"))
            crouched = true;
        else
            crouched = false;
    }

	void FixedUpdate () {

        TimeLine.Add(new archive(faceRight, transform.position));
        if (TimeLine.Count > 2000) {// remove early frames if there are too many
            TimeLine.RemoveAt(0);
        }
        counter++;
        if(counter == echodelay){
            counter = 0;
            
            if(Echoes.Count < 2) {
                madeEcho = Instantiate(echo, TimeLine[TimeLine.Count - echodelay].position, transform.rotation);
                madeEcho.GetComponent<echoScript>().start = echodelay * (current+1);
                madeEcho.GetComponent<echoScript>().player = this.gameObject;
                //madeEcho.GetComponent<echoScript>().iter = current;
                Echoes.Add(madeEcho);
                current++;
            }
            
        }

        //player control

        
        //Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + new Vector2(move * walkSpeed, rb.velocity.y - groundNormal.x * move));
        //Debug.DrawLine(Vector3.zero, groundNormal);
        
        //walking
        if (Input.GetButton("Run"))
        {
            rb.velocity = new Vector2(move * walkSpeed * runMultiplier, rb.velocity.y - groundNormal.x * move);   
            animate.speed = runMultiplier;
        }
        else
        {
            rb.velocity = new Vector2(move * walkSpeed, rb.velocity.y - groundNormal.x * move);
            animate.speed = 1f;
        }
        if (move < 0.1f && move > -0.1)
        {
            animate.speed = 0f;
        }

        Debug.DrawLine(transform.position, rb.velocity + new Vector2(transform.position.x, transform.position.y));
        
        //rb.velocity = new Vector2(move * walkSpeed, rb.velocity.y);

        //jumping


        if (jumpReq && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);// , rb.velocity.y);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpReq = false;
            //onGround = false;
        }
        if (rb.velocity.y < 0) //falling
        {
            rb.gravityScale = fallGravity;
            //onGround = false;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
        
        if (move == 0f && onGround)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        

        //crouching
        //if (crouched)
        //{
        //    ///crouch script
        //}


        //directional control
        if (move > 0 && !faceRight) {
			Flip();
		} else if (move < 0 && faceRight) {
			Flip();
		}
	}

	void Flip() {
		faceRight = !faceRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

    

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == groundLayer)
        {
            onGround = false;//set to false so that player cannot transfer from floor to ceiling and hold that jump
            foreach (ContactPoint2D point in other.contacts)
            {
                allContact = point.point;
                Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + point.normal * 2, Color.green);
                Debug.DrawLine(point.point, point.normal+point.point, Color.red);
                Debug.DrawLine(point.point, new Vector2(point.normal.y+point.point.x, -point.normal.x + point.point.y), Color.blue);
                //Debug.Log(groundCheck.position + "vs. " + point.point);
                if (point.normal.normalized.y > 0.5f && point.point.y < groundCheck.position.y) //angle is less than 60 degrees and the contact is at the feet
                {
                    contactpoint = point.point;
                    //Debug.Log("Grounded");
                    onGround = true;
                    groundNormal = point.normal.normalized;
                }
            }
        }
    }

    //Debug
    private Vector3 contactpoint;
    private Vector3 allContact;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(contactpoint, 0.1f);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(allContact, 0.1f);
    }

    void OnCollisionExit2D(Collision2D other) // when jumping
    {
        if (other.gameObject.layer == groundLayer)
        {
            //Debug.Log("left");
            onGround = false;
            groundNormal = Vector2.up;
        }
    }

    void OnTriggerExit2D(Collider2D other) //when crouch jumping on platforms
    {
        if (other.gameObject.layer == groundLayer)
        {
            //Debug.Log("Left");
            onGround = false;
            groundNormal = Vector2.up;
        }
    }

    void die(int version, bool notmerge) {
        if(Echoes.Count > 0) {
            if (notmerge) {
                //Instantiate(zombieprefab, transform.position, transform.rotation);
                TheZombie.transform.position = transform.position;
                transform.position = Echoes[version].transform.position;
            }

            for (int i = 0; i <= version; i++) {
                Destroy(Echoes[0]);
                Echoes.RemoveAt(0);
            }
            for (int i = 0; i < Echoes.Count; i++) {
                Debug.Log(i.ToString());
                Echoes[i].GetComponent<echoScript>().start -= (version + 1) * echodelay;
            }
            counter = 0;
            TimeLine.RemoveRange(TimeLine.Count - 1 - (version + 1) * echodelay, (version + 1) * echodelay);
            current -= version + 1;
        } else {
            truedie();
        }
        
    }
    void truedie() {

    }

    void OnTriggerStay2D(Collider2D other) {
        
        if (other.tag == "Echo") {
            for (int i = 0; i < Echoes.Count; i++) {
                if(Echoes[i] == other.GetComponent<echoScript>().me) {
                    die(i,false);
                }
            }
        }
        if (other.tag == "trap") {
            die(0, true);
        }

    }
}
