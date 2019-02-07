using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int passthroughLayer = 13;


    [Header("Movement Speeds")]
    public float maxSpeed = 2f;
    public float jumpSpeed = 5f;
    

    [Header("Jump")]
    public bool onGround = false;
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

    [Header("Zombie")]
    public GameObject TheZombie;


    private float move = 0f;
    private bool jumpReq = false;
    private bool crouched = false;

    private bool faceRight = true;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update ()
    {
        move = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpReq = true;
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
        
        //walking
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        //jumping
        if (jumpReq && onGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpReq = false;
            onGround = false;
        }
        if (rb.velocity.y < 0) //falling
        {
            rb.gravityScale = fallGravity;
            onGround = false;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        //crouching
        //if (crouched)
        //{
        //    ///crouch script
        //}

        //platforms
        if ((rb.velocity.y > 0 || crouched) && !Physics2D.GetIgnoreLayerCollision(playerLayer, passthroughLayer))
        {
            Physics2D.IgnoreLayerCollision(playerLayer, passthroughLayer, true);
        }
        else if (rb.velocity.y <= 0 && !crouched && Physics2D.GetIgnoreLayerCollision(playerLayer, passthroughLayer))
        {
            Physics2D.IgnoreLayerCollision(playerLayer, passthroughLayer, false);
        }
        Debug.Log(Physics2D.GetIgnoreLayerCollision(playerLayer, passthroughLayer) + ", " + rb.velocity.y);

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

    void OnCollisionEnter2D(Collision2D other)
    {
        foreach (ContactPoint2D point in other.contacts)
        {
            if (point.normal.y > 0)
                onGround = true;
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
