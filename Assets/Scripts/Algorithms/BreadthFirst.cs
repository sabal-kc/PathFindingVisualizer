using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirst: Algorithm
{
    public override HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(endPos);

        // Create a queue for BFS 
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>();


        // Mark the current node as visited and enqueue it 
        visitedNodes.Add(startNode);
        queue.Enqueue(startNode);


         int counter = 0;
        stepVisited = new Dictionary<int, Node>();
        stepNeighbors = new Dictionary<int, List<Node>>();

        while (queue.Count > 0) {
            Node currentNode = queue.Dequeue();
            stepVisited.Add(counter, currentNode);
            if (currentNode == targetNode) {
                AlgorithmManager.Instance.RetracePath(startNode, targetNode);

                return visitedNodes;
            }

            List<Node> stepIndices = new List<Node>();
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode, Grid.Direction.FOUR)) {
                if (!neighbor.isWalkable || visitedNodes.Contains(neighbor))
                    continue;



                neighbor.parent = currentNode;
                visitedNodes.Add(neighbor);
                queue.Enqueue(neighbor);
                stepIndices.Add(neighbor);



            }
            stepNeighbors.Add(counter, stepIndices);
            counter++;
        }

        return visitedNodes;


    }


}
