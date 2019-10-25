using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirst
{
    Grid grid = AlgorithmManager.Instance.grid;

    public HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(endPos);

        // Create a queue for BFS 
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>();


        // Mark the current node as visited and enqueue it 
        visitedNodes.Add(startNode);
        queue.Enqueue(startNode);


        while (queue.Count > 0) {
            Node currentNode = queue.Dequeue();
            if (currentNode == targetNode) {
                AlgorithmManager.Instance.RetracePath(startNode, targetNode);

                return visitedNodes;
            }
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode, Grid.Direction.FOUR)) {
                if (!neighbor.isWalkable || visitedNodes.Contains(neighbor))
                    continue;



                neighbor.parent = currentNode;
                visitedNodes.Add(neighbor);
                queue.Enqueue(neighbor);



            }
        }

        return visitedNodes;


    }


}
