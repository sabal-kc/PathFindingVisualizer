using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirst : MonoBehaviour
{
    Grid grid;
    public Transform startPosition, endPosition;
    public HashSet<Node> visitedNodes = new HashSet<Node>();

    public float animationSpeed = 30.0f;


    private void Awake() {
        grid = GetComponent<Grid>();
    }


    private void Update() {
        if (grid.grid != null) {
            visitedNodes = FindShortestPath(startPosition.position, endPosition.position);
        }


    }

    HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(endPos);

        // Create a queue for BFS 
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> closedList = new HashSet<Node>();


        // Mark the current node as visited and enqueue it 
        closedList.Add(startNode);

        startNode.gCost = 0;
        queue.Enqueue(startNode);


        while (queue.Count > 0) {
            Node currentNode = queue.Dequeue();
            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);

                return closedList;
            }
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode, Grid.Direction.FOUR) ){
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                    continue;



                neighbor.parent = currentNode;
                int newCost;
                neighbor.gCost = currentNode.gCost + CalculateDist(currentNode, neighbor);
                
                closedList.Add(neighbor);
                queue.Enqueue(neighbor);



            }
        }

        return closedList;


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
        grid.visitedNodes = this.visitedNodes;
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
