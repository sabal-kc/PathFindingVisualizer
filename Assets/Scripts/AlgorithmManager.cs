using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmManager : Singleton<AlgorithmManager>
{

    public Grid grid;
    public Transform startPosition, endPosition;
    public HashSet<Node> visitedNodes = new HashSet<Node>();

    public Dictionary<int, List<Node>> stepNeighbor;
    public Dictionary<int, Node> stepVisited;

    public bool stepWiseMode = true;

    AStar astar;
    BreadthFirst breadthFirst;
    DepthFirst depthFirst;
    Dijkstra dijkstra;
    GreedyBestFirst greedyBfs;
    Algorithm currentAlgo;
    public enum AlgoType { ASTAR, BFS, DFS, DIJKSTRA, GREEDY };
    public AlgoType algo;
    private void Start() {
        grid = GetComponent<Grid>();
        astar = new AStar();
        breadthFirst = new BreadthFirst();
        depthFirst = new DepthFirst();
        dijkstra = new Dijkstra();
        greedyBfs = new GreedyBestFirst();
    }



    // Update is called once per frame
    void Update() {
        if (grid.grid != null && stepWiseMode) {
            switch (algo) {
                case AlgoType.ASTAR:
                    currentAlgo = astar;
                    break;
                case AlgoType.BFS:
                    currentAlgo = breadthFirst;
                    break;
                case AlgoType.DFS:
                    currentAlgo = depthFirst;
                    break;
                case AlgoType.DIJKSTRA:
                    currentAlgo = dijkstra;
                    break;
                case AlgoType.GREEDY:
                    currentAlgo = greedyBfs;
                    break;
            }
            visitedNodes = currentAlgo.FindShortestPath(startPosition.position, endPosition.position);
            stepNeighbor = currentAlgo.stepNeighbors;
            stepVisited = currentAlgo.stepVisited;

        } else if (grid.grid != null && stepWiseMode) {

        }
    }

    public void RetracePath(Node first, Node last) {
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
    public int CalculateDist(Node first, Node second) {
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
