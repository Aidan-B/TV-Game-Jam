using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class platformEffectorPassthrough : MonoBehaviour
{
    Collider2D tileCollider;
    public bool passthrough = false;

    // Start is called before the first frame update
    void Start()
    {
        tileCollider = GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            passthrough = other.transform.GetComponent<playerController>().crouched;
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
        tileCollider.isTrigger = passthrough;
    }
}
