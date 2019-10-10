using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{

    Grid grid;
    public Transform startPosition, endPosition;


    private void Awake() {
        grid = GetComponent<Grid>();
    }


    private void Update() {
        if (grid.grid != null) {
            FindShortestPath(startPosition.position, endPosition.position);
        }
    }

    void FindShortestPath(Vector3 startPos, Vector3 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(endPos);
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        while (openList.Count > 0) {
            //Step1: Find the lowest fcost in the open list
            //current = node in OPEN with the lowest f_cost
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost < currentNode.fCost || (openList[i].fCost < currentNode.fCost && openList[i].hCost < currentNode.hCost)) {
                    currentNode = openList[i];
                }
            }

            //Step2: Remove current from OPEN
            openList.Remove(currentNode);

            //Step3 : add current to CLOSED
            closedList.Add(currentNode);

            //Step4 : if current is the target node //path has been found
            //           return
            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }

            //Step5:  foreach neighbour of the current node
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode)) {
                //if neighbour is not traversable or neighbour is in CLOSED
                //       skip to the next neighbour
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                    continue;


                //if new path to neighbour is shorter OR neighbour is not in OPEN
                //        set f_cost of neighbour
                //        set parent of neighbour to current
                //        if neighbour is not in OPEN
                //                add neighbour to OPEN

                int newCost = currentNode.gCost + CalculateDist(currentNode, neighbor);
                if (newCost < neighbor.gCost || !openList.Contains(neighbor)) {
                    //gCost = distance travelled so far; hCost = actual between node and target
                    neighbor.gCost = newCost;
                    neighbor.hCost = CalculateDist(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }

                }
            }

        }
    }

    void RetracePath(Node first, Node last) {
        List<Node> path = new List<Node>();
        Node currentNode = last;

        while (currentNode != first) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();


        grid.finalPath = path;
    }


    //Diagonal move            -> 14 unit cost
    //Horizontal/vertical move -> 10 unit cost
    int CalculateDist(Node first, Node second) {
        int dist = 0;

        int distXAxis = Mathf.Abs(first.gridX - second.gridX);
        int distYAxis = Mathf.Abs(first.gridY - second.gridY);

        if (distYAxis < distXAxis) {
            //Minimize diagonal traversal
            dist = 14 * distYAxis + 10 * (distXAxis - distYAxis);
        } else {
            dist = 14 * distXAxis + 10 * (distYAxis - distXAxis);
        }
        return dist;
    }

}
