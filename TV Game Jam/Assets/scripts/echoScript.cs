using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoScript : MonoBehaviour
{
    public int start, count, iter;
    public GameObject player,me;
    // Start is called before the first frame update
    void Start()
    {
        me = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<playerController>().TimeLine[player.GetComponent<playerController>().TimeLine.Count - start].position;

    }
}
