﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echoscript : MonoBehaviour
{
    public int start, count, iter;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<playerController>().TimeLine[player.GetComponent<playerController>().TimeLine.Count - start].position;

    }
}
