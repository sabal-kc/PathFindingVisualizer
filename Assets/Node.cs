using UnityEngine;
public class Node
{
    public bool isWalkable;
    public Vector3 position;

    //Position in grid
    public int gridX, gridY;

    //Actual cost and the heuristic cost
    public int gCost, hCost;
    public Node parent;

    //Constructor
    public Node(bool isWalkable, Vector3 position, int gridX, int gridY) {
        this.isWalkable = isWalkable;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;


    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
