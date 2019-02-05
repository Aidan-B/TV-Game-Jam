using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct archive {
    public int health;
    public Vector3 position;
    
    public archive(int health, Vector3 position) {
        this.health = health;
        this.position = position;
    }
}

public class playerController : MonoBehaviour {

    [Header("Movement Speeds")]
    public float maxSpeed = 2f;
    public float jumpSpeed = 5f;
    

    [Header("Jump")]
    public bool onGround = false;
    public Transform groundCheck;
    float groundRadius = 0.1f;
    public LayerMask whatIsGround;
    public float fallGravity = 2.5f;
    public float lowJumpGravity = 2f;

    [Header("Time Echos")]
    public int current;
    public int counter;
    public GameObject echo;
    private GameObject madeEcho;
    public List<archive> TimeLine = new List<archive>();
    public List<GameObject> Echoes = new List<GameObject>();


    private float move = 0f;
    private bool jumpReq = false;
    private bool faceRight = true;
    Rigidbody2D rb;
    private float gravity;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
	}

    void Update ()
    {
        move = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpReq = true;
        }
    }

	void FixedUpdate () {


        TimeLine.Add(new archive(0, transform.position));
        if (TimeLine.Count > 2000) {// remove early frames if there are too many
            TimeLine.RemoveAt(0);
        }
        counter++;
        if(counter == 100){
            counter = 0;
            
            if(Echoes.Count < 2) {
                madeEcho = Instantiate(echo, TimeLine[TimeLine.Count - 100].position, transform.rotation);
                madeEcho.GetComponent<echoScript>().start = 100 * (current+1);
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
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        if (jumpReq && onGround)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpReq = false;
        }
        if (rb.velocity.y < 0) //falling
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpGravity;
        }
        else
        {
            rb.gravityScale = gravity;
        }




        //directional controll
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




    void die(int version, bool relocate) {
        if (relocate) {
            transform.position = Echoes[version].transform.position;
        }
        for(int i = 0; i <= version; i++) {
            Destroy(Echoes[0]);
            //Debug.Log(i.ToString() + Echoes.Count.ToString());
            Echoes.RemoveAt(0);
            //current = Echoes.Count;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        
        if (other.tag == "Echo") {
            Debug.Log("yeet");
            for (int i = 0; i < Echoes.Count; i++) {
                if(Echoes[i] == other.GetComponent<echoScript>().me) {
                    die(i,false);
                }
            }
            //die(other.GetComponent<echoScript>().iter);
        }

    }
}
