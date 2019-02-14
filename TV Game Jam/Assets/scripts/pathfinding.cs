using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public struct pathVals
{
    public readonly Vector2Int start;
    public readonly Vector2Int end;
    public readonly bool[,] nodes;
    public pathVals(Vector2Int start, Vector2Int end, bool[,] nodes)
    {
        this.start = start;
        this.end = end;
        this.nodes = nodes;
    }
}

public class pathfinding : MonoBehaviour
{
    
    //public mapGenerator mapGenerator;
    //public bool[,] mapNodes;
    //public bool[,][,] zombieNodes;
    //public Vector2Int roomSize;

    // Start is called before the first frame update
    void Start()
    {
        //mapNodes = mapGenerator.mapPaths;
        //zombieNodes = mapGenerator.zombiePaths;
        //roomSize = mapGenerator.roomSize;
    }

    public List<Vector2Int> findPath(Vector2Int start, Vector2Int end, bool[,] nodes, int maxAttempts)
    {
        /*
        Debug.Log("Start of array:");
        for (int y = 9; y >= 0; y--)
        {
            string val = "";
            for (int x = 0; x < 10; x++)
            {
                val += "(" + x +"," + y + ")" + nodes[x, y] + ", ";
            }
            Debug.Log(val);
        }
        Debug.Log("End of array.");
        */
        List<node> open = new List<node>() { new node(start, start, end, 0) };
        List<node> closed = new List<node>();
        bool searching = true;
        int count = 0;
        Vector2Int lastPos = start;
        List<Vector2Int> path = new List<Vector2Int>();
        while (searching)
        {
            if (count > maxAttempts)
            {
                Debug.Log("breaking from loop");
                return null;
            }
            count++;

            //Debug.Log(open.Count);
            // find node with lowest fCost
            node current = open[0];
            foreach (node node in open)
                if (node.fCost < current.fCost)
                    current = node;
            //Debug.DrawLine(new Vector3(lastPos.x - 249.5f, lastPos.y - 249.5f, 0) * 10, new Vector3(current.position.x - 249.5f, current.position.y - 249.5f, 0) * 10, Color.green, 10000f);
            lastPos = current.position;
            //Debug.Log("Current:  Pos:" + current.position + " hCost:" + current.hCost + " gCost:" + current.gCost + " fCost:" + current.fCost);

            //move item from open to closed list
            closed.Add(current);
            open.Remove(current);
            //Debug.Log("Closed: " + closed.Count + ", Open: " + open.Count);

            if (current.position != end)
            {
                //add all adjacent nodes to open list
                Vector2Int checkPos;
                int index;


                if (current.position.x + 1 < nodes.GetLength(0) && nodes[current.position.x + 1, current.position.y])
                {
                    checkPos = new Vector2Int(current.position.x + 1, current.position.y);
                    if (open.Exists(x => x.position == checkPos))
                    {
                        index = open.FindIndex(x => x.position == checkPos);
                        if (current.gCost + 1 < open[index].gCost)
                        {
                            //Debug.Log("Updating Right");
                            open[index].recalculateCost(current.gCost + 1);
                        }
                    }
                    else
                    {
                        if (!closed.Exists(x => x.position == checkPos))
                        {
                            //Debug.Log("Adding Right");
                            open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                        }
                            
                    }
                    //Debug.Log("Dir: Right");
                }
                if (current.position.x - 1 >= 0 && nodes[current.position.x - 1, current.position.y])
                {
                    checkPos = new Vector2Int(current.position.x - 1, current.position.y);
                    if (open.Exists(x => x.position == checkPos))
                    {
                        index = open.FindIndex(x => x.position == checkPos);
                        if (current.gCost + 1 < open[index].gCost)
                        {
                            //Debug.Log("Updating Left");
                            open[index].recalculateCost(current.gCost + 1);
                        }
                            
                    }
                    else
                    {
                        if (!closed.Exists(x => x.position == checkPos))
                        {
                            //Debug.Log("Adding Left");
                            open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                        }
                    }
                        
                    //Debug.Log("Dir: Left");
                }
                if (current.position.y + 1 < nodes.GetLength(1) && nodes[current.position.x, current.position.y + 1])
                {
                    checkPos = new Vector2Int(current.position.x, current.position.y + 1);
                    if (open.Exists(x => x.position == checkPos))
                    {
                        index = open.FindIndex(x => x.position == checkPos);
                        if (current.gCost + 1 < open[index].gCost)
                        {
                            //Debug.Log("Updating Up");
                            open[index].recalculateCost(current.gCost + 1);
                        }
                    }
                    else
                    {
                        if (!closed.Exists(x => x.position == checkPos))
                        {
                            //Debug.Log("Adding Up");
                            open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                        }
                    }
                        
                    //Debug.Log("Dir: Up");
                }
                if (current.position.y - 1 >= 0 && nodes[current.position.x, current.position.y - 1])
                {
                    checkPos = new Vector2Int(current.position.x, current.position.y - 1);
                    if (open.Exists(x => x.position == checkPos))
                    {
                        index = open.FindIndex(x => x.position == checkPos);
                        if (current.gCost + 1 < open[index].gCost)
                        {
                            //Debug.Log("Updating Down");
                            open[index].recalculateCost(current.gCost + 1);
                        }
                    }
                    else
                    {
                        if (!closed.Exists(x => x.position == checkPos))
                        {
                            //Debug.Log("Adding Down");
                            open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                        }
                    }
                        
                    //Debug.Log("Dir: Down");
                }
            }
            else
            {
                //Found the path
                if (open.Count == 0)
                {
                    //Debug.Log("Could not find path");
                    searching = false;
                    return null;
                }
                else
                {
                    path.Add(current.position);
                    while (current.position != start)
                    {
                        current = closed.Find(x => x.position == current.parentPosition);
                        path.Add(current.position);
                    }
                    searching = false;
                }
            }
        }
        path.Reverse();
        return path;
    }
    /*
    void OnDrawGizmos()
    {
        foreach (Vector2Int pos in path)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(pos.x-249.5f, pos.y-249.5f)*10, 1f);
        }

    }
    */
}

public class node
{
    public Vector2Int position;
    public Vector2Int parentPosition;
    public float fCost;
    public float gCost;
    public float hCost;
    public node(Vector2Int position, Vector2Int parentPosition, Vector2Int end, float gCost)
    {
        this.position = position;
        this.parentPosition = parentPosition;
        this.hCost = Mathf.Pow(end.x - position.x, 2) + Mathf.Pow(end.y - position.y, 2);
        this.gCost = gCost;
        this.fCost = this.gCost + this.hCost;
    }
    public void recalculateCost(float gCost)
    {
        this.gCost = gCost;
        this.fCost = this.gCost + this.hCost;
    }

}