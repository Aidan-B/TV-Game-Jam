using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombiescript : MonoBehaviour
{
    public int Causeofdeath;
    public GameObject player;
    Rigidbody2D rb;
    public pathfinding pathfinding;
    public mapGenerator mapGenerator;
    List<Vector2Int> pathToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(LookForPaths());

    }

    IEnumerator LookForPaths()
    {
        while (true)
        {
            Vector2Int zombieRoomPos = new Vector2Int(Mathf.FloorToInt(transform.position.x / mapGenerator.roomSize.x) + 250, Mathf.FloorToInt(transform.position.y / mapGenerator.roomSize.y) + 250);
            Vector2Int playerRoomPos = new Vector2Int(Mathf.FloorToInt(player.transform.position.x / mapGenerator.roomSize.x) + 250, Mathf.FloorToInt(player.transform.position.y / mapGenerator.roomSize.y) + 250);

            pathToPlayer = pathfinding.findPath(zombieRoomPos, playerRoomPos, mapGenerator.mapPaths, 100);

            Vector2Int position = zombieRoomPos;
            //roomPathToPlayer.Reverse();
            foreach (Vector2Int point in pathToPlayer)
            {
                //Debug.Log(point);
                Debug.DrawLine(new Vector3(position.x - 249.5f, position.y - 249.5f) * 10, new Vector3(point.x - 249.5f, point.y - 249.5f) * 10, Color.red, 2f);
                position = point;
            }


            Vector2Int zombiePos = new Vector2Int(Mathf.FloorToInt(transform.position.x / mapGenerator.roomSize.x), Mathf.FloorToInt(transform.position.y / mapGenerator.roomSize.y) + 250);


            yield return new WaitForSeconds(2f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        //rb.velocity = new Vector3((player.transform.position.x-transform.position.x)/3f,rb.velocity.y,0);
    }
}
