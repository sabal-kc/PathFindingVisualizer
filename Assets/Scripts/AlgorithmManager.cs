using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmManager : Singleton<AlgorithmManager>
{

    public Grid grid;
    public Transform startPosition, endPosition;
    public HashSet<Node> visitedNodes = new HashSet<Node>();
    AStar astar;
    BreadthFirst breadthFirst;
    DepthFirst depthFirst;
    Dijkstra dijkstra;
    GreedyBestFirst greedyBfs;
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
        if (grid.grid != null) {
            switch (algo) {
                case AlgoType.ASTAR:
                    visitedNodes = astar.FindShortestPath(startPosition.position, endPosition.position);
                    break;
                case AlgoType.BFS:
                    visitedNodes = breadthFirst.FindShortestPath(startPosition.position, endPosition.position);
                    break;
                case AlgoType.DFS:
                    visitedNodes = depthFirst.FindShortestPath(startPosition.position, endPosition.position);
                    break;
                case AlgoType.DIJKSTRA:
                    visitedNodes = dijkstra.FindShortestPath(startPosition.position, endPosition.position);
                    break;
                case AlgoType.GREEDY:
                    visitedNodes = greedyBfs.FindShortestPath(startPosition.position, endPosition.position);
                    break;
            }


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
        Debug.Log(path);


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
