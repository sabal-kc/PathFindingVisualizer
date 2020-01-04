using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm
{

    protected Grid grid = AlgorithmManager.Instance.grid;
    //Nodes visited at every step
    public Dictionary<int, Node> stepVisited = new Dictionary<int, Node>();
    //List of neighbor nodes at every step
    public Dictionary<int, List<Node>> stepNeighbors = new Dictionary<int, List<Node>>();

    public virtual HashSet<Node> FindShortestPath(Vector3 startPos, Vector3 endPos) {
        return null;
    }

}
