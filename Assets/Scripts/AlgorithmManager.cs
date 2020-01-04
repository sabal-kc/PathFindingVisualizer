using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmManager : Singleton<AlgorithmManager>
{

    public Grid grid;
    public Transform startPosition, endPosition;
    public HashSet<Node> visitedNodes = new HashSet<Node>();


    public bool stepWiseMode = true;

    AStar astar;
    BreadthFirst breadthFirst;
    DepthFirst depthFirst;
    Dijkstra dijkstra;
    GreedyBestFirst greedyBfs;

    public enum AlgoType { ASTAR, DFS, BFS, GREEDY, DIJKSTRA};
    public Algorithm currentAlgorithm;
    public AlgoType algorithmType;


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
        //Debug.Log(algorithmType);
        switch (algorithmType) {
            case AlgoType.ASTAR:
                currentAlgorithm = astar;
                break;
            case AlgoType.BFS:
                currentAlgorithm = breadthFirst;
                break;
            case AlgoType.DFS:
                currentAlgorithm = depthFirst;
                break;
            case AlgoType.DIJKSTRA:
                currentAlgorithm = dijkstra;
                break;
            case AlgoType.GREEDY:
                currentAlgorithm = greedyBfs;
                break;
        }
        visitedNodes = currentAlgorithm.FindShortestPath(startPosition.position, endPosition.position);
        if (grid.grid != null && stepWiseMode) {
            grid.UpdateGridStep();

        } else if (grid.grid != null && !stepWiseMode) {
            grid.UpdateGridOnePass();
        }
    }

    public void RetracePath(Node first, Node last) {
        List<Node> path = new List<Node>();
        Node currentNode = last;

        while (currentNode != first) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(first);
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
