using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoScript : MonoBehaviour
{
    public int start, count, iter;
    private bool faceRight = true;
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
        if (faceRight != player.GetComponent<playerController>().TimeLine[player.GetComponent<playerController>().TimeLine.Count - start].right){
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
