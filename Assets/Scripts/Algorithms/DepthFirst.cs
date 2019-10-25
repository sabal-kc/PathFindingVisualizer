using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirst
{
    Grid grid = AlgorithmManager.Instance.grid;



    public HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(endPos);

        // Create a stack for DFS
        Stack<Node> stack = new Stack<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>(); //Visited list


        // Mark the current node as visited and enqueue it 
        visitedNodes.Add(startNode);
        stack.Push(startNode);


        while (stack.Count > 0) {
            Node currentNode = stack.Pop();
            if (currentNode == targetNode) {
                AlgorithmManager.Instance.RetracePath(startNode, targetNode);

                return visitedNodes;
            }

            if (!visitedNodes.Contains(currentNode)) {
                visitedNodes.Add(currentNode);
            }
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode, Grid.Direction.FOUR)) {
                if (!neighbor.isWalkable || visitedNodes.Contains(neighbor))
                    continue;

                neighbor.parent = currentNode;
                stack.Push(neighbor);



            }
        }

        return visitedNodes;


    }


}
