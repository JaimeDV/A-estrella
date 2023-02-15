using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Pathfinding : MonoBehaviour {

    Grid gridReference;
    public Transform startPosition;
    public Transform targetPosition;

    private void Awake()
    {
        gridReference = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(startPosition.position, targetPosition.position);
    }

    void FindPath(Vector3 starter, Vector3 target)
    {
        Node startNode = gridReference.NodeFromWorldPoint(starter);
        Node targetNode = gridReference.NodeFromWorldPoint(target);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while(OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach (Node NeighborNode in gridReference.GetNeighboringNodes(CurrentNode))
            {
                //if i add water reconigcion it should be here
                if (!NeighborNode.isNotWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);
                if (!NeighborNode.isNotWater)//water is more expensive
                {
                    MoveCost *= 2;
                }
                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.parentNode = CurrentNode;

                    if(!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }

        }
    }



    void GetFinalPath(Node start, Node target)
    {
        List<Node> finalPath = new List<Node>();
        Node cuarrent = target;

        while(cuarrent != start)
        {
            finalPath.Add(cuarrent);
            cuarrent = cuarrent.parentNode;
        }

        finalPath.Reverse();

        gridReference.FinalPath = finalPath;

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

        return ix + iy;
    }
}
