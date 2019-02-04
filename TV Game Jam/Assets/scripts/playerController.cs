using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public float maxSpeed = 10f;
	bool faceRight = true;
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
			float move = Input.GetAxis ("Horizontal");
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
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
}
