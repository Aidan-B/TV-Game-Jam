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
    public float jump = 400f;
    

    [Header("Jump")]
    public bool onGround = false;
    public Transform groundCheck;
    float groundRadius = 0.1f;
    public LayerMask whatIsGround;

    [Header("Time Echos")]
    public int current;
    public int counter;
    public GameObject echo;
    private GameObject madeEcho;
    public List<archive> TimeLine = new List<archive>();
    public List<GameObject> Echoes = new List<GameObject>();



    bool faceRight = true;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
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
                madeEcho = Instantiate(echo, TimeLine[TimeLine.Count - 5].position, transform.rotation);
                madeEcho.GetComponent<Echoscript>().start = 100 * current;
                madeEcho.GetComponent<Echoscript>().player = this.gameObject;
                madeEcho.GetComponent<Echoscript>().iter = current;
                Echoes.Add(madeEcho);
            }
            current++;
        }

        //player control
        
        float move = Input.GetAxis ("Horizontal");
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        if (Input.GetAxis("Jump") > 0 && onGround)
        {
            rb.AddForce(Vector2.up * jump);
        }

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




    void die(int version) {
        transform.position = Echoes[version].transform.position;
        for(int i = 0; i <= version; i++) {
            Destroy(Echoes[i]);
            Echoes.RemoveAt(i);
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("yeet");
        if (other.tag == "Echo") {
            die(other.GetComponent<Echoscript>().iter);
        }

    }
}
