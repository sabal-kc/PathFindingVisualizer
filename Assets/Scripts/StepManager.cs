using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : Singleton<StepManager>
{

    int step = 0;
    Grid grid;

    // Start is called before the first frame update
    void Start() {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update() {


        if (Input.GetKeyDown(KeyCode.Space)) {
            StepForward();
        }

    }

    public void StepForward() {
        var neigbors = AlgorithmManager.Instance.currentAlgorithm.stepNeighbors;
        var visited = AlgorithmManager.Instance.currentAlgorithm.stepVisited;

        //Reset the animated path
        if (grid.tempPath.Count > 0)
            grid.tempPath = new List<Node>();

        if (step == 0) {
            grid.stepWiseVisited = null;
            grid.stepWiseNeigbors = new HashSet<Node>();
            grid.stepWiseClosed = new HashSet<Node>();

        }

        //Debug.Log(neigbors);
        if (step < neigbors.Count) {
            foreach (Node node in neigbors[step]) {
                grid.stepWiseNeigbors.Add(node);
            }
            grid.stepWiseVisited = visited[step];
            grid.stepWiseClosed.Add(visited[step]);
            step += 1;
        } else {
            step = 0;
            grid.AnimateFinalPath();

        }
    }


}
