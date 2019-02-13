using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flametrap : MonoBehaviour
{

    public GameObject particlesystem, player,hitbox;
    private ParticleSystem fire;
    public bool on;
    public float  speed;
    private float deletewait;
    [Header("Settings")]
    public bool proximity;
    public float ontimer, offtimer, proxrange;
    public int proxdelay;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fire = particlesystem.GetComponent<ParticleSystem>();
        fire.playbackSpeed = speed;
        //fire.lifet
    }

    // Update is called once per frame
    void Update()
    {
        if (proximity && player.GetComponent<playerController>().TimeLine.Count > proxdelay) {
            if (
                player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay-1)].position.y < transform.position.y+9f 
                && player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay-1)].position.y > transform.position.y 
                && player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay-1)].position.x > transform.position.x-proxrange
                && player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay-1)].position.x < transform.position.x + proxrange
              ) {
                on = true;
            } else {
                on = false;
            }
        } else {

        }
        if (on) {
            fire.Emit(1);
            fire.emissionRate = 1000;
            deletewait = 0.5f;
            hitbox.SetActive(on);
        } else {
            fire.emissionRate = 0;
            if (deletewait > 0) {
                deletewait -= Time.deltaTime;
            } else {
                fire.Clear();
            }
        }
        hitbox.SetActive(on);
    }
}
