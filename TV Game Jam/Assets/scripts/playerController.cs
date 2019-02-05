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

    public float maxSpeed = 2f, jump;
    public int current, counter;
	bool faceRight = true;
    Rigidbody2D rb;
    public GameObject echo;
    private GameObject madeEcho;
    public List<archive> TimeLine = new List<archive>();
    public List<GameObject> Echoes = new List<GameObject>();

    
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            jump = 6f;
        }else {
            jump = 0f;
        }

                float move = Input.GetAxis ("Horizontal");
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y+jump);
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
