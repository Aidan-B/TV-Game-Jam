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
    List<Vector2Int> pathToRoom;

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

            pathToPlayer = pathfinding.findPath(zombieRoomPos, playerRoomPos, mapGenerator.mapPaths, 2000);

            Vector2Int position = zombieRoomPos;
            //roomPathToPlayer.Reverse();
            foreach (Vector2Int point in pathToPlayer)
            {
                //Debug.Log(point);
                Debug.DrawLine(new Vector3(position.x - 249.5f, position.y - 249.5f) * 10, new Vector3(point.x - 249.5f, point.y - 249.5f) * 10, Color.red, 2f);
                position = point;
            }


            Vector2Int zombiePos = new Vector2Int(
                Mathf.FloorToInt(((transform.position.x / mapGenerator.roomSize.x) - Mathf.Floor(transform.position.x / mapGenerator.roomSize.x)) * mapGenerator.roomSize.x),
                Mathf.FloorToInt(((transform.position.y / mapGenerator.roomSize.y) - Mathf.Floor(transform.position.y / mapGenerator.roomSize.y)) * mapGenerator.roomSize.y)
                );
            Vector2Int roomExit = Vector2Int.zero;
            Vector2Int direction = new Vector2Int(pathToPlayer[1].x - zombieRoomPos.x, pathToPlayer[1].y - zombieRoomPos.y);
            Debug.Log(direction);
            if (direction == Vector2Int.up)
            {
                int minX = 0;
                int minDistance = mapGenerator.roomSize.x;
                for (int x = 0; x < mapGenerator.roomSize.x; x++) //check whole top row
                {
                    if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][x, mapGenerator.roomSize.y-1]) //there is a node on the top row
                    {
                        if (Mathf.Abs(zombiePos.x - x) < minDistance)
                        {
                            minDistance = Mathf.Abs(zombiePos.x - x);
                            minX = x;
                        }
                    }
                }
                roomExit = new Vector2Int(minX, mapGenerator.roomSize.y-1);
            }
            else if (direction == Vector2Int.down)
            {
                int minX = 0;
                int minDistance = mapGenerator.roomSize.x;
                for (int x = 0; x < mapGenerator.roomSize.x; x++) //check whole top row
                {
                    if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][x, 0]) //there is a node on the bottom row
                    {
                        if (Mathf.Abs(zombiePos.x - x) < minDistance)
                        {
                            minDistance = Mathf.Abs(zombiePos.x - x);
                            minX = x;
                        }
                    }
                }
                roomExit = new Vector2Int(minX, 0);
            }
            else if (direction == Vector2Int.right)
            {
                int minY = 0;
                int minDistance = mapGenerator.roomSize.y;
                for (int y = 0; y < mapGenerator.roomSize.y; y++) //check whole right side
                {
                    if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][mapGenerator.roomSize.x - 1, y]) //there is a node on the right
                    {
                        if (Mathf.Abs(zombiePos.y - y) < minDistance)
                        {
                            minDistance = Mathf.Abs(zombiePos.y - y);
                            minY = y;
                        }
                    }
                }
                roomExit = new Vector2Int(mapGenerator.roomSize.x - 1, minY);
            }
            else if (direction == Vector2Int.left)
            {
                int minY = 0;
                int minDistance = mapGenerator.roomSize.y;
                for (int y = 0; y < mapGenerator.roomSize.y; y++) //check whole left side
                {
                    if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][0, y]) //there is a node on the left side
                    {
                        if (Mathf.Abs(zombiePos.y - y) < minDistance)
                        {
                            minDistance = Mathf.Abs(zombiePos.y - y);
                            minY = y;
                        }
                    }
                }
                roomExit = new Vector2Int(0, minY);
            }
            else
            {
                Debug.Log("An error occured With determining direction for zombie pathing");
            }
            Debug.Log("Room exit: " + roomExit);
            Debug.Log(zombieRoomPos);

            Vector2Int nearestZombiePos = Vector2Int.zero;
            float minRoomDistance = mapGenerator.roomSize.x * mapGenerator.roomSize.y;
            for (int x = 0; x < mapGenerator.roomSize.x; x++)
            {
                for (int y = 0; y < mapGenerator.roomSize.y; y++)
                {

                    if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][x, y]) //there is a node on the left side
                    {
                        if (Mathf.Pow(zombiePos.y - y, 2) + Mathf.Pow(zombiePos.x - x, 2) < minRoomDistance)
                        {
                            minRoomDistance = Mathf.Pow(zombiePos.y - y, 2) + Mathf.Pow(zombiePos.x - x, 2);
                            nearestZombiePos = new Vector2Int(x,y);
                        }
                    }
                    /*
                    if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][x, y])
                    {
                        Vector2Int tempPos = new Vector2Int(x, y);
                        if (Vector2Int.Distance(nearestZombiePos, tempPos) < minRoomDistance)
                        {
                            nearestZombiePos = tempPos;
                            minRoomDistance = Vector2Int.Distance(nearestZombiePos, tempPos);
                            Debug.Log("min room dist: " + minRoomDistance);
                        }
                    }
                    */
                }
            }
            //nearestZombiePos = new Vector2Int(250, 250);
            

            Debug.Log("Now doing room calcs...");
            Debug.Log("zombie's coordinates in room: " + nearestZombiePos);
            pathToRoom = pathfinding.findPath(nearestZombiePos, roomExit, mapGenerator.zombiePaths[zombieRoomPos.x,zombieRoomPos.y], 100);

            if (pathToRoom != null)
            {
                position = zombiePos;
                //roomPathToPlayer.Reverse();
                foreach (Vector2Int point in pathToPlayer)
                {
                    //Debug.Log(point);
                    Debug.DrawLine(new Vector3(position.x + zombieRoomPos.x * mapGenerator.roomSize.x, position.y + zombieRoomPos.y * mapGenerator.roomSize.y), new Vector3(point.x + zombieRoomPos.x * mapGenerator.roomSize.x, point.y + zombieRoomPos.y * mapGenerator.roomSize.y) * 10, Color.red, 2f);
                    position = point;
                }
            }
            else
            {
                Debug.Log("The zombie is unable to get to the required location");
            }
            

            yield return new WaitForSeconds(2f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        //rb.velocity = new Vector3((player.transform.position.x-transform.position.x)/3f,rb.velocity.y,0);
    }
}
