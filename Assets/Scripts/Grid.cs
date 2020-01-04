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



    public List<Node> finalPath; //Final path taken
    public HashSet<Node> visitedNodes;

    //Stepwise animation neigbors, closed and visited
    public List<Node> tempPath = new List<Node>();
    public HashSet<Node> stepWiseNeigbors = new HashSet<Node>();
    public HashSet<Node> stepWiseClosed = new HashSet<Node>();
    public Node stepWiseVisited;


    public enum Direction { FOUR, EIGHT };
    public Direction noOfDirections = Direction.FOUR;
    public float nodeSeparationUnits = .2f;

    List<GameObject> connectors = new List<GameObject>();


    void Start() {
        gridNodes = new Vector2Int();
        gridNodes.x = Mathf.RoundToInt(totalGridSize.x / (nodeRadius * 2));
        gridNodes.y = Mathf.RoundToInt(totalGridSize.y / (nodeRadius * 2));
        CreateGridNodes();

    }


    private void Update() {
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

    void CreateGridNodes() {
        grid = new Node[gridNodes.x, gridNodes.y];
        Vector3 worldBottomLeft = transform.position - Vector3.right * totalGridSize.x / 2 - Vector3.forward * totalGridSize.y / 2;


        for (int i = 0; i < gridNodes.x; i++) {
            for (int j = 0; j < gridNodes.y; j++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeRadius * 2 + nodeRadius) + Vector3.forward * (j * nodeRadius * 2 + nodeRadius);
                Vector3 cubeSize = Vector3.one * (nodeRadius * 2 - nodeSeparationUnits);
                grid[i, j] = new Node(true, worldPoint, i, j, cubeSize);



            }
        }
        CreateConnectors();




    }

    public void CreateConnectors() {
        foreach(GameObject o in connectors) {
            Destroy(o);
        }
        connectors.Clear();
        foreach (Node n in grid) {
            List<Node> neighbors = GetNeighboringNodes(n, noOfDirections);
            foreach (Node nOther in neighbors) {
                var connector = GameObject.CreatePrimitive(PrimitiveType.Cube);
                connector.transform.position = n.position + new Vector3(nOther.position.x / 2 - n.position.x / 2, 0, nOther.position.z / 2 - n.position.z / 2);
                connector.transform.localScale = new Vector3(nodeSeparationUnits, 2.0f, nodeSeparationUnits);
                connector.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                connectors.Add(connector);
            }
        }
    }


    //Color the grid in one pass //Must be called by the update of algorithm manager in non step mode
    public void UpdateGridOnePass() {
        Node playerNode = GetNodeFromWorldPoint(player.position);
        Node targetNode = GetNodeFromWorldPoint(target.position);
        foreach (Node n in grid) {
            var cube = n.cube.transform;
            var cubeRenderer = cube.GetComponent<MeshRenderer>();
            cubeRenderer.material.SetColor("_Color", (n.isWalkable) ? Color.white : Color.red);
            if (playerNode == n || targetNode == n) {
                cubeRenderer.material.SetColor("_Color", Color.cyan);
            }

            //Main coloring
            if (finalPath != null && visitedNodes.Contains(n)) {
                cubeRenderer.material.SetColor("_Color", Color.grey);
            }
            if (finalPath != null && finalPath.Contains(n)) {
                cubeRenderer.material.SetColor("_Color", Color.yellow);
            }
        }
    }







    //Called by the step manager during animation
    public void AnimateFinalPath() {
        StartCoroutine(ColorNode());
    }

    IEnumerator ColorNode() {
        foreach (Node current in finalPath) {

            foreach (Node n in grid) {
                var cube = n.cube.transform;
                var cubeRenderer = cube.GetComponent<MeshRenderer>();

                if (n == current) {
                    tempPath.Add(current);

                }
            }
            yield return new WaitForSeconds(0.2f);

        }

    }

    //Called by algorithm manager in each update pass in stepwise mode
    //Stepwise update of the color of the grid
    public void UpdateGridStep() {
        if (grid != null) {
            Node playerNode = GetNodeFromWorldPoint(player.position);
            Node targetNode = GetNodeFromWorldPoint(target.position);
            foreach (Node n in grid) {
                var cube = n.cube.transform;
                var cubeRenderer = cube.GetComponent<MeshRenderer>();
                cubeRenderer.material.SetColor("_Color", (n.isWalkable) ? Color.white : Color.red);
                if (playerNode == n || targetNode == n) {
                    cubeRenderer.material.SetColor("_Color", Color.cyan);
                }

                //Stepwise coloring

                //Neighbors expanded
                if (stepWiseNeigbors.Contains(n)) {
                    cubeRenderer.material.SetColor("_Color", Color.grey);
                }

                //Closed nodes
                if (stepWiseClosed.Contains(n)) {
                    cubeRenderer.material.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
                }

                //Current visited
                if (n == stepWiseVisited) {
                    cubeRenderer.material.SetColor("_Color", Color.green);

                }



                //Animation of path
                if (tempPath.Contains(n)) {
                    cubeRenderer.material.SetColor("_Color", Color.yellow);
                }


                //if (GetComponent<StepManager>().closedList.Contains(n)) {
                //    cubeRenderer.material.SetColor("_Color", Color.yellow);
                //}


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

    public List<Node> GetNeighboringNodes(Node currentNode, Direction direction) {
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

                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
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






}
