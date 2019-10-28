using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirst: Algorithm
{

    public override HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(endPos);
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        int counter = 0;
        stepVisited = new Dictionary<int, Node>();
        stepNeighbors = new Dictionary<int, List<Node>>();

        while (openList.Count > 0) {
            //Step1: Find the lowest hcost in the open list
            //current = node in OPEN with the lowest heurestic
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].hCost < currentNode.hCost) {
                    currentNode = openList[i];
                }
            }

            stepVisited.Add(counter, currentNode);

            //Step2: Remove currentNode from openlist
            openList.Remove(currentNode);

            //Step3 : add currentNode to the closedlist
            closedList.Add(currentNode);

            //Step4 : if target then retrace path and return
            if (currentNode == targetNode) {
                AlgorithmManager.Instance.RetracePath(startNode, targetNode);
                return closedList;
            }


            List<Node> stepIndices = new List<Node>();
            //Step5:  foreach neighbour of the current node
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode, Grid.Direction.FOUR)) {
                //if neighbour is not traversable or 
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                    continue;

                neighbor.hCost = AlgorithmManager.Instance.CalculateDist(neighbor, targetNode);

        
                if (!openList.Contains(neighbor)) {
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                        stepIndices.Add(neighbor);
                    }

                }
            }

            stepNeighbors.Add(counter, stepIndices);
            counter++;

        }
        return closedList;
    }

}
