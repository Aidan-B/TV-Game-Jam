using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class platformEffectorPassthrough : MonoBehaviour
{
    Collider2D thisCollider;
    public bool passthrough = false;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider = GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (other.transform.GetComponent<playerController>().crouched)
            {
                Collider2D otherCollider = other.gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(otherCollider, thisCollider, true);
                StartCoroutine("EnableCollision", otherCollider);
                //Debug.Log("Ignored");
            }
        }
        else if (other.transform.CompareTag("Zombie"))
        {
            if (other.transform.GetComponent<zombiescript>().crouched)
            {
                Debug.Log("We know your crouched!");
                Collider2D otherCollider = other.gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(otherCollider, thisCollider, true);
                StartCoroutine("EnableCollision", otherCollider);
                //Debug.Log("Ignored");
            }
        }
    }
    IEnumerator EnableCollision(Collider2D other)
    {
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), thisCollider, false);
        //Debug.Log("No longer ignoring");
        yield return null;
    }

}
