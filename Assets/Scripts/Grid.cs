using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 totalGridSize;
    public LayerMask unwalkableMask;
    public Transform player, target;

    public float nodeRadius;

    //No of nodes in x and y of the grid
    Vector2Int gridNodes;
    //2d array of nodes
    public Node[,] grid;



    public List<Node> finalPath;
    public HashSet<Node> visitedNodes;


    public enum Direction { FOUR, EIGHT };
    const Direction DEFAULT_DIRECTION = Direction.EIGHT;


    void Start() {
        gridNodes = new Vector2Int();
        gridNodes.x = Mathf.RoundToInt(totalGridSize.x / (nodeRadius * 2));
        gridNodes.y = Mathf.RoundToInt(totalGridSize.y / (nodeRadius * 2));
        CreateGridNodes();

    }

    private void Update() {
        if (grid != null && finalPath != null) {
            Node playerNode = GetNodeFromWorldPoint(player.position);
            Node targetNode = GetNodeFromWorldPoint(target.position);
            foreach (Node n in grid) {
                var cube = n.cube.transform;
                var cubeRenderer = cube.GetComponent<MeshRenderer>();
                cubeRenderer.material.SetColor("_Color", (n.isWalkable) ? Color.white : Color.red);

                if (visitedNodes.Contains(n)) {
                    cubeRenderer.material.SetColor("_Color", Color.grey);
                }
                if (finalPath.Contains(n)) {
                    cubeRenderer.material.SetColor("_Color", Color.yellow);
                }
                if (playerNode == n || targetNode == n) {
                    cubeRenderer.material.SetColor("_Color", Color.cyan);
                }


            }
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed
                if (hit.transform != null && hit.collider.name.Equals("Cube")) {
                    Node current = GetNodeFromWorldPoint(hit.transform.position);
                    current.isWalkable = !current.isWalkable;
                }
            }

        }
    }


    public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
        float posX = ((worldPosition.x - transform.position.x) + totalGridSize.x * 0.5f) / (nodeRadius * 2);
        float posY = ((worldPosition.z - transform.position.z) + totalGridSize.y * 0.5f) / (nodeRadius * 2);

        posX = Mathf.Clamp(posX, 0, totalGridSize.x - 1);
        posY = Mathf.Clamp(posY, 0, totalGridSize.y - 1);

        int x = Mathf.FloorToInt(posX);
        int y = Mathf.FloorToInt(posY);

        return grid[x, y];
    }

    public List<Node> GetNeighboringNodes(Node currentNode, Direction direction = DEFAULT_DIRECTION) {
        List<Node> neighbors = new List<Node>();


        switch (direction) {
            case Direction.FOUR:
                //int[] x = { 1, -1, 0, 0 };
                //int[] y = { 0, 0, 1, -1 };
                //for (int i = 0; i < 4; i++) {
                //    int checkX = currentNode.gridX + x[i];
                //    int checkY = currentNode.gridX + y[i];
                //    if (checkX >= 0 && checkX < gridNodes.x && checkY >= 0 && checkY < gridNodes.y) {
                //        neighbors.Add(grid[checkX, checkY]);
                //    }

                //}

                for (int i=-1; i<=1; i++) {
                    for (int j=-1; j<=1; j++) {
                        if (Mathf.Abs(i) == Mathf.Abs(j)) continue;


                        int checkX = currentNode.gridX + i;
                        int checkY = currentNode.gridY + j;
                        if (checkX >= 0 && checkX < gridNodes.x && checkY >= 0 && checkY < gridNodes.y) {
                            neighbors.Add(grid[checkX, checkY]);
                        }

                    }
                }
                break;
            case Direction.EIGHT:
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        if (i == 0 && j == 0) continue;


                        int checkX = currentNode.gridX + i;
                        int checkY = currentNode.gridY + j;
                        if (checkX >= 0 && checkX < gridNodes.x && checkY >= 0 && checkY < gridNodes.y) {
                            neighbors.Add(grid[checkX, checkY]);
                        }
                    }
                }

                break;
        }

        return neighbors;
    }


    void CreateGridNodes() {
        grid = new Node[gridNodes.x, gridNodes.y];
        Vector3 worldBottomLeft = transform.position - Vector3.right * totalGridSize.x / 2 - Vector3.forward * totalGridSize.y / 2;


        for (int i = 0; i < gridNodes.x; i++) {
            for (int j = 0; j < gridNodes.y; j++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeRadius * 2 + nodeRadius) + Vector3.forward * (j * nodeRadius * 2 + nodeRadius);
                Vector3 cubeSize = Vector3.one * (nodeRadius * 2 - .1f);
                grid[i, j] = new Node(true, worldPoint, i, j, cubeSize);
            }
        }
    }

    private void OnDrawGizmos() {
        //Gizmos.DrawWireCube(transform.position, new Vector3(totalGridSize.x, 1, totalGridSize.y));
        //if (grid != null) {
        //    Node playerNode = GetNodeFromWorldPoint(player.position);
        //    Node targetNode = GetNodeFromWorldPoint(target.position);
        //    foreach (Node n in grid) {
        //        Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
        //        if (playerNode == n || targetNode == n) {
        //            Gizmos.color = Color.cyan;
        //        }
        //        if (finalPath.Contains(n)) {
        //            Gizmos.color = Color.yellow;
        //        }

        //        Gizmos.DrawCube(n.position, Vector3.one * (nodeRadius * 2 - .1f));
        //    }
        //}
    }


}
