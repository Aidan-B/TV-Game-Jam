using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinding : MonoBehaviour
{
    /*
        public mapGenerator mapGenerator;
        private bool[,] mapNodes;
        private List<Vector2Int> nodes;

        // Start is called before the first frame update
        void Start()
        {
            mapNodes = mapGenerator.mapPaths;
        }
    */
    public List<Vector2Int> findPath(Vector2Int start, Vector2Int end, bool[,] nodes)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        List<node> open = new List<node>() { new node(start, start, end, 0) };
        List<node> closed = new List<node>();
        bool searching = true;
        while (searching)
        {
            // find node with lowest fCost
            node current = open[0];
            foreach (node node in open)
                if (node.fCost < current.fCost)
                    current = node;
            //Debug.Log("Current:  Pos:" + current.position + " hCost:" + current.hCost + " gCost:" + current.gCost + " fCost:" + current.fCost);

            //move item from open to closed list
            closed.Add(current);
            open.Remove(current);
            //Debug.Log("Closed: " + closed + ", Open: " + open);

            if (current.position != end)
            {
                //add all adjacent nodes to open list
                Vector2Int checkPos;
                if (nodes[current.position.x + 1, current.position.y])
                {
                    checkPos = new Vector2Int(current.position.x + 1, current.position.y);
                    if (open.Exists(x => x.position == checkPos))
                    {
                        //Debug.Log("A Tile to the right exists in the open list");
                        open.Find(x => x.position == checkPos).recalculateCost(current.gCost + 1);
                    }
                    else
                    {
                        //Debug.Log("A Tile to the right does not exist in the open list");
                        open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                    }
                    //Debug.Log("Dir: Right");
                }
                if (nodes[current.position.x - 1, current.position.y])
                {
                    checkPos = new Vector2Int(current.position.x - 1, current.position.y);
                    if (open.Exists(x => x.position == checkPos))
                        open.Find(x => x.position == checkPos).recalculateCost(current.gCost + 1);
                    else
                        open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                    //Debug.Log("Dir: Left");
                }
                if (nodes[current.position.x, current.position.y + 1])
                {
                    checkPos = new Vector2Int(current.position.x, current.position.y + 1);
                    if (open.Exists(x => x.position == checkPos))
                        open.Find(x => x.position == checkPos).recalculateCost(current.gCost + 1);
                    else
                        open.Add(new node(checkPos, current.position, end, current.gCost + 1));
                    //Debug.Log("Dir: Up");
                }
                if (nodes[current.position.x, current.position.y - 1])
                {
                    checkPos = new Vector2Int(current.position.x, current.position.y - 1);
                    if (open.Exists(x => x.position == checkPos))
                        open.Find(x => x.position == checkPos).recalculateCost(current.gCost + 1);
                    else
                        open.Add(new node(checkPos, current.position, end, current.gCost + 1));
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
        return path;

    }
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