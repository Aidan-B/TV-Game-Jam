using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class platformEffectorPassthrough : MonoBehaviour
{
    Collider2D collider;
    public bool passthrough = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            passthrough = other.transform.GetComponent<playerController>().crouched;
            //if(other.transform.position.y-0.9f < transform.position.y) {
                //passthrough = true;
            //}
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            passthrough = false;
        }
    }

    void FixedUpdate()
    {
        
        collider.isTrigger = passthrough;
    }
}
