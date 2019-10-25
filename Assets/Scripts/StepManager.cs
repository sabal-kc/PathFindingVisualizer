//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StepManager : Singleton<StepManager>
//{
//    Node startNode, targetNode;
//    Stack<Node> stack = new Stack<Node>();
//    public HashSet<Node> closedList = new HashSet<Node>();

//    // Start is called before the first frame update
//    void Start() {

//    }

//    // Update is called once per frame
//    void Update() {
//        if (Input.GetKeyDown(KeyCode.R)) {
//            startNode = GetComponent<Grid>().GetNodeFromWorldPoint(GetComponent<DepthFirst>().startPosition.position);
//            targetNode = GetComponent<Grid>().GetNodeFromWorldPoint(GetComponent<DepthFirst>().endPosition.position);

//            // Mark the current node as visited and enqueue it 
//            closedList.Add(startNode);
//            stack.Push(startNode);
//        }

        
//        if (Input.GetKeyDown(KeyCode.Space)) {
//            if (stack.Count > 0) {
//                step();
//            }
//        }

//    }


//    void step() {
//        Debug.Log("Step");
//        Node currentNode = stack.Pop();
//        if (currentNode == targetNode) {
//            //RetracePath(startNode, targetNode);

//            //return closedList;
//            return;
//        }
//        foreach (Node neighbor in GetComponent<Grid>().GetNeighboringNodes(currentNode, Grid.Direction.FOUR)) {
//            Debug.Log(closedList.Count);
//            if (!neighbor.isWalkable || closedList.Contains(neighbor))
//                continue;



//            neighbor.parent = currentNode;
//            closedList.Add(neighbor);
//            stack.Push(neighbor);



//        }




//    }
//}
