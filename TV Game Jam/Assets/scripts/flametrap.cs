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
    private float tracker;
    public float length;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fire = particlesystem.GetComponent<ParticleSystem>();
        fire.playbackSpeed = speed;
        hitbox.transform.localScale = new Vector3(1, length, 1);
        hitbox.transform.position = new Vector3(transform.position.x, (length - 1) / 2+transform.position.y, 1);
        particlesystem.transform.localScale = new Vector3(0.25f, 1f / 18f * length, 1);
        //particlesystem.transform.position = new Vector3(particlesystem.transform.position.x, particlesystem.transform.position.y / 18 * length, particlesystem.transform.position.z);
        //fire.lifet
    }

    // Update is called once per frame
    void Update()
    {
        if (proximity ) {
            if(player.GetComponent<playerController>().TimeLine.Count > proxdelay) {
                if (
                player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay - 1)].position.y < transform.position.y + 9f
                && player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay - 1)].position.y > transform.position.y
                && player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay - 1)].position.x > transform.position.x - proxrange
                && player.GetComponent<playerController>().TimeLine[(player.GetComponent<playerController>().TimeLine.Count - proxdelay - 1)].position.x < transform.position.x + proxrange
              ) {
                    on = true;
                } else {
                    on = false;
                }
            } else {
                on = false;
            }
            
        } else {
            if (on) {
                if(tracker <=  0) {
                    on = !on;
                    tracker = offtimer;
                } else {
                    tracker -= Time.deltaTime;
                }
            } else {
                if (tracker <= 0) {
                    on = !on;
                    tracker = ontimer;
                } else {
                    tracker -= Time.deltaTime;
                }
            }
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
