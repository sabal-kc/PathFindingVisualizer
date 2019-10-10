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


    void Start() {
        gridNodes = new Vector2Int();
        gridNodes.x = Mathf.RoundToInt(totalGridSize.x / (nodeRadius * 2));
        gridNodes.y = Mathf.RoundToInt(totalGridSize.y / (nodeRadius * 2));
        CreateGridNodes();
    }

    private void Update() {
        
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

    public List<Node> GetNeighboringNodes(Node currentNode) {
        List<Node> neighbors = new List<Node>();

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

        return neighbors;
    }


    void CreateGridNodes() {
        grid = new Node[gridNodes.x, gridNodes.y];
        Vector3 worldBottomLeft = transform.position - Vector3.right * totalGridSize.x / 2 - Vector3.forward * totalGridSize.y / 2;


        for (int i = 0; i < gridNodes.x; i++) {
            for (int j = 0; j < gridNodes.y; j++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeRadius * 2 + nodeRadius) + Vector3.forward * (j * nodeRadius * 2 + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(totalGridSize.x, 1, totalGridSize.y));
        if (grid != null) {
            Node playerNode = GetNodeFromWorldPoint(player.position);
            Node targetNode = GetNodeFromWorldPoint(target.position);
            foreach (Node n in grid) {
                Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                if (playerNode == n || targetNode == n) {
                    Gizmos.color = Color.cyan;
                }
                if (finalPath.Contains(n)) {
                    Gizmos.color = Color.yellow;
                }

                Gizmos.DrawCube(n.position, Vector3.one * (nodeRadius * 2 - .1f));
            }
        }
    }


}
