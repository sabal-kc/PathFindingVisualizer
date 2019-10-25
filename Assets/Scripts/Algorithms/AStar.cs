using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{

    Grid grid = AlgorithmManager.Instance.grid;


    public HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
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

            //Step2: Remove currentNode from openlist
            openList.Remove(currentNode);

            //Step3 : add currentNode to the closedlist
            closedList.Add(currentNode);

            //Step4 : if target then retrace path and return
            if (currentNode == targetNode) {
                AlgorithmManager.Instance.RetracePath(startNode, targetNode);
                return closedList;
            }

            //Step5:  foreach neighbour of the current node
            foreach (Node neighbor in grid.GetNeighboringNodes(currentNode, Grid.Direction.FOUR)) {
                //if neighbour is not traversable or 
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                    continue;


                //if new path to neighbour is shorter OR neighbour is not in OPEN
                //        set f_cost of neighbour
                //        set parent of neighbour to current
                //        if neighbour is not in OPEN
                //                add neighbour to OPEN

                int newCost = currentNode.gCost + AlgorithmManager.Instance.CalculateDist(currentNode, neighbor);
                if (newCost < neighbor.gCost || !openList.Contains(neighbor)) {
                    //gCost = distance travelled so far
                    neighbor.gCost = newCost;
                    //hCost = actual between node and target
                    neighbor.hCost = AlgorithmManager.Instance.CalculateDist(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }

                }
            }

        }
        return closedList;
    }



}
