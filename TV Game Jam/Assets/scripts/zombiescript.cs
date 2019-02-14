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
    public float walkSpeed = 2f;
    public float jumpSpeed = 2f;
    public bool crouched = false;
    Vector2Int zombieRoomPos;
    Vector2Int playerRoomPos;

    public Transform groundCheck;
    private bool onGround;
    private Vector2 groundNormal = Vector2.zero;
    public int groundLayer = 10;

    Vector3 travelDir = Vector3.zero;

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
            zombieRoomPos = new Vector2Int(Mathf.FloorToInt(transform.position.x / mapGenerator.roomSize.x) + 250, Mathf.FloorToInt(transform.position.y / mapGenerator.roomSize.y) + 250);
            playerRoomPos = new Vector2Int(Mathf.FloorToInt(player.transform.position.x / mapGenerator.roomSize.x) + 250, Mathf.FloorToInt(player.transform.position.y / mapGenerator.roomSize.y) + 250);
            bool goToPlayer = true;
            if (zombieRoomPos != playerRoomPos)
            {
                goToPlayer = false;
                //Well, we made it boys. Now we gotta grab the player and run
                pathToPlayer = pathfinding.findPath(zombieRoomPos, playerRoomPos, mapGenerator.mapPaths, 2000);

            }
            

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
            Debug.Log(goToPlayer);
            if (!goToPlayer)
            {
                Vector2Int direction = new Vector2Int(pathToPlayer[1].x - zombieRoomPos.x, pathToPlayer[1].y - zombieRoomPos.y);
                //Debug.Log(direction);
                if (direction == Vector2Int.up)
                {
                    int minX = 0;
                    int minDistance = mapGenerator.roomSize.x;
                    for (int x = 0; x < mapGenerator.roomSize.x; x++) //check whole top row
                    {
                        if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][x, mapGenerator.roomSize.y - 1]) //there is a node on the top row
                        {
                            if (Mathf.Abs(zombiePos.x - x) < minDistance)
                            {
                                minDistance = Mathf.Abs(zombiePos.x - x);
                                minX = x;
                            }
                        }
                    }
                    roomExit = new Vector2Int(minX, mapGenerator.roomSize.y - 1);
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
            }
            else
            {
                Vector2Int playerPos = new Vector2Int(
                    Mathf.FloorToInt(((player.transform.position.x / mapGenerator.roomSize.x) - Mathf.Floor(player.transform.position.x / mapGenerator.roomSize.x)) * mapGenerator.roomSize.x),
                    Mathf.FloorToInt(((player.transform.position.y / mapGenerator.roomSize.y) - Mathf.Floor(player.transform.position.y / mapGenerator.roomSize.y)) * mapGenerator.roomSize.y)
                    );

                roomExit = new Vector2Int(Mathf.FloorToInt(mapGenerator.roomSize.x * 0.5f), Mathf.FloorToInt(mapGenerator.roomSize.y * 0.5f));
                float minPlayerDistance = mapGenerator.roomSize.x * mapGenerator.roomSize.y;
                for (int x = 0; x < mapGenerator.roomSize.x; x++)
                {
                    for (int y = 0; y < mapGenerator.roomSize.y; y++)
                    {

                        if (mapGenerator.zombiePaths[zombieRoomPos.x, zombieRoomPos.y][x, y]) //there is a node on the left side
                        {
                            if (Mathf.Pow(playerPos.y - y, 2) + Mathf.Pow(playerPos.x - x, 2) < minPlayerDistance)
                            {
                                minPlayerDistance = Mathf.Pow(playerPos.y - y, 2) + Mathf.Pow(playerPos.x - x, 2);
                                roomExit = new Vector2Int(x, y);
                            }
                        }
                    }
                }
            }
            
            //Debug.Log("Room exit: " + roomExit);
            //Debug.Log(zombieRoomPos);

            Vector2Int nearestZombiePos = new Vector2Int(Mathf.FloorToInt(mapGenerator.roomSize.x * 0.5f), Mathf.FloorToInt(mapGenerator.roomSize.y * 0.5f));
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
                }
            }
            //nearestZombiePos = new Vector2Int(250, 250);


            //Debug.Log("Now doing room calcs...");
            Debug.Log("zombie's coordinates in room: " + nearestZombiePos);
            Debug.Log("player's coordinates in room: " + roomExit);
            pathToRoom = pathfinding.findPath(nearestZombiePos, roomExit, mapGenerator.zombiePaths[zombieRoomPos.x,zombieRoomPos.y], 100);

            if (pathToRoom != null)
            {
                position = nearestZombiePos;
                //roomPathToPlayer.Reverse();
                foreach (Vector2Int point in pathToRoom)
                {
                    //Debug.Log(point + (zombieRoomPos-new Vector2Int(250,250)*10));
                    Debug.DrawLine(new Vector3(position.x + (zombieRoomPos.x-250) * 10 +0.5f, position.y + (zombieRoomPos.y - 250) * 10 +0.5f), new Vector3(point.x + (zombieRoomPos.x - 250) * 10+0.5f, point.y + (zombieRoomPos.y - 250) * 10+0.5f), Color.magenta, 2f);
                    position = point;
                }
            }
            else
            {
                Debug.Log("The zombie is unable to get to the required location");
            }
            

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //rb.velocity = new Vector3((player.transform.position.x-transform.position.x)/3f,rb.velocity.y,0);

        //Vector3 travelDir = Vector3.zero;
        if (pathToRoom != null && pathToRoom.Count > 1)
        {
            travelDir = (new Vector3(pathToRoom[1].x + (zombieRoomPos.x - mapGenerator.Width * 0.5f) * mapGenerator.roomSize.x + 0.5f, pathToRoom[1].y + (zombieRoomPos.y - mapGenerator.Height * 0.5f) * mapGenerator.roomSize.y + 0.5f) - transform.position);
            if (travelDir.magnitude < 1.2f && Mathf.Abs(travelDir.x) < 0.5f)
            {
                pathToRoom.RemoveAt(0);
            }
        }
        else if (pathToPlayer.Count > 1)
        {
            travelDir = new Vector3(pathToPlayer[1].x - pathToPlayer[0].x, pathToPlayer[1].y - pathToPlayer[0].y);
        }
        Debug.DrawLine(transform.position, transform.position + travelDir);
        travelDir = travelDir.normalized;
        
        //Debug.Log(direction);
        if (onGround && travelDir.y > 0.5f)
        {
            //jumping
            rb.velocity = new Vector2(rb.velocity.x, 0f);// , rb.velocity.y);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            crouched = false;
            onGround = false;
        }
        else if (travelDir.y < groundCheck.localPosition.y)
        {
            crouched = true;
        }
        else
        {
            //walking
            rb.velocity = new Vector2(travelDir.x * walkSpeed, rb.velocity.y - groundNormal.x);
            crouched = false;
        }



    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == groundLayer)
        {

            foreach (ContactPoint2D point in other.contacts)
            {
                allContact = point.point;

                Debug.DrawLine(point.point, point.normal + point.point, Color.red);
                Debug.DrawLine(point.point, new Vector2(point.normal.y + point.point.x, -point.normal.x + point.point.y), Color.blue);
                //Debug.Log(groundCheck.position + "vs. " + point.point);
                if (point.normal.normalized.y > 0.5f && point.point.y < groundCheck.position.y) //angle is less than 60 degrees and the contact is at the feet
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + point.normal * 2, Color.green);
                    contactpoint = point.point;
                    //Debug.Log("Grounded");
                    onGround = true;
                    groundNormal = point.normal.normalized;
                }
            }
        }
    }

    //Debug
    private Vector3 contactpoint;
    private Vector3 allContact;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(contactpoint, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(allContact, 0.1f);
    }

    void OnCollisionExit2D(Collision2D other) // when jumping
    {
        if (other.gameObject.layer == groundLayer)
        {
            //Debug.Log("left");
            onGround = false;
            groundNormal = Vector2.up;
        }
    }

    void OnTriggerExit2D(Collider2D other) //when crouch jumping on platforms
    {
        if (other.gameObject.layer == groundLayer)
        {
            //Debug.Log("Left");
            onGround = false;
            groundNormal = Vector2.up;
        }
    }

}
