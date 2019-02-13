using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flametrap : MonoBehaviour
{

    public GameObject particlesystem;
    private ParticleSystem fire;
    public bool on;
    public float  speed;
    private float deletewait;
    // Start is called before the first frame update
    void Start()
    {
        fire = particlesystem.GetComponent<ParticleSystem>();
        fire.playbackSpeed = speed;
        //fire.lifet
    }

    // Update is called once per frame
    void Update()
    {
        if (on) {
            fire.Emit(1);
            fire.emissionRate = 1000;
            deletewait = 0.5f;
        } else {
            fire.emissionRate = 0;
            if (deletewait > 0) {
                deletewait -= Time.deltaTime;
            } else {
                fire.Clear();
            }
        }
    }
}
