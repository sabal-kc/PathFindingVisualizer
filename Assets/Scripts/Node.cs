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

    //3d cube for the node
    public GameObject cube;

    //Constructor
    public Node(bool isWalkable, Vector3 position, int gridX, int gridY, Vector3 cubeSize) {
        this.isWalkable = isWalkable;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;

        this.initializeCube(cubeSize);

    }

    public void initializeCube(Vector3 cubeSize) {

        this.cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.cube.transform.position = this.position;
        this.cube.transform.localScale = cubeSize;

    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
