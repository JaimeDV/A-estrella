using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Pathfinding : MonoBehaviour {

    //#region oldCode
    //Grid gridReference;
    //public Transform startPosition;
    //public Transform targetPosition;

    //private void Awake()
    //{
    //    gridReference = GetComponent<Grid>();
    //}

    //private void Update()
    //{
    //    FindPath(startPosition.position, targetPosition.position);
    //}

    //void FindPath(Vector3 starter, Vector3 target)
    //{
    //    Node startNode = gridReference.NodeFromWorldPoint(starter);
    //    Node targetNode = gridReference.NodeFromWorldPoint(target);

    //    List<Node> OpenList = new List<Node>();
    //    HashSet<Node> ClosedList = new HashSet<Node>();

    //    OpenList.Add(startNode);

    //    while(OpenList.Count > 0)
    //    {
    //        Node CurrentNode = OpenList[0];
    //        for(int i = 1; i < OpenList.Count; i++)
    //        {
    //            if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
    //            {
    //                CurrentNode = OpenList[i];
    //            }
    //        }
    //        OpenList.Remove(CurrentNode);
    //        ClosedList.Add(CurrentNode);

    //        if (CurrentNode == targetNode)
    //        {
    //            GetFinalPath(startNode, targetNode);
    //        }

    //        foreach (Node NeighborNode in gridReference.GetNeighboringNodes(CurrentNode))
    //        {
    //            //if i add water reconigcion it should be here
    //            if (!NeighborNode.isNotWall || ClosedList.Contains(NeighborNode))
    //            {
    //                continue;
    //            }
    //            int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);
    //            if (!NeighborNode.isNotWater)//water is more expensive
    //            {
    //                MoveCost *= 2;
    //            }
    //            if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
    //            {
    //                NeighborNode.igCost = MoveCost;
    //                NeighborNode.ihCost = GetManhattenDistance(NeighborNode, targetNode);
    //                NeighborNode.parentNode = CurrentNode;

    //                if(!OpenList.Contains(NeighborNode))
    //                {
    //                    OpenList.Add(NeighborNode);
    //                }
    //            }
    //        }

    //    }
    //}



    //void GetFinalPath(Node start, Node target)
    //{
    //    List<Node> finalPath = new List<Node>();
    //    Node cuarrent = target;

    //    while(cuarrent != start)
    //    {
    //        finalPath.Add(cuarrent);
    //        cuarrent = cuarrent.parentNode;
    //    }

    //    finalPath.Reverse();

    //    gridReference.FinalPath = finalPath;

    //}

    //int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    //{
    //    int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
    //    int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

    //    return ix + iy;
    //}
    //#endregion
    #region newCode
    [SerializeField]
    public Grid gridReference;

    public GameObject startPosition;

    public GameObject targetPosition;

    //if walls should be walkable and everything else not
    [SerializeField]
    private bool alternate;

    [SerializeField]
    private GameObject MovingTarget;

    [SerializeField]
    private int waterMoveCost;

    public static System.Action<GameObject, List<Node>> SendPath;

    private List<Node> gizmoPath;

    [SerializeField] private bool isDebug = false;

    private void Awake()
    {
        if (isDebug)
        {
            gridReference = GameObject.FindObjectOfType<Grid>();
            if (targetPosition == null)
            {
                targetPosition = GameObject.Find("P_PlayerControler");
            }

        }
    }

    private void Update()
    {
        if (!alternate)
        {
            if (startPosition.transform.position != null && targetPosition.transform.position != null)
            {
                FindPath(startPosition.transform.position, targetPosition.transform.position);
            }
        }
        else
        {
            if (startPosition.transform.position != null && targetPosition.transform.position != null)
            {
                FindPathAlt(startPosition.transform.position, targetPosition.transform.position);
            }
        }
    }

    public void FindPath(Vector3 starter, Vector3 target)
    {
        Node startNode = gridReference.NodeFromWorldPoint(starter);
        Node targetNode = gridReference.NodeFromWorldPoint(target);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
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
                    MoveCost *= waterMoveCost;
                }
                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.parentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    public void FindPathAlt(Vector3 starter, Vector3 target)
    {
        Node startNode = gridReference.NodeFromWorldPoint(starter);
        Node targetNode = gridReference.NodeFromWorldPoint(target);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
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
                if (NeighborNode.isNotWall || ClosedList.Contains(NeighborNode))
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

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    public void GetFinalPath(Node start, Node target)
    {
        List<Node> finalPath = new List<Node>();
        Node cuarrent = target;

        while (cuarrent != start)
        {
            finalPath.Add(cuarrent);
            cuarrent = cuarrent.parentNode;
        }

        finalPath.Reverse();

        gridReference.FinalPath = finalPath;
    }

    public List<Node> ReturnFinalPath(Node start, Node target)
    {
        List<Node> finalPath = new List<Node>();
        Node cuarrent = target;

        while (cuarrent != start)
        {
            finalPath.Add(cuarrent);
            cuarrent = cuarrent.parentNode;
        }

        finalPath.Reverse();

        return finalPath;
    }

    private void FindPathReturner(GameObject user, GameObject target)
    {
        Debug.Log("Finding Path");
        //if (user == this.gameObject)
        //{

        List<Node> pathToReturn = null;
        Node startNode = gridReference.NodeFromWorldPoint(user.transform.position);
        Node targetNode = gridReference.NodeFromWorldPoint(target.transform.position);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
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
                pathToReturn = ReturnFinalPath(startNode, targetNode);
                gizmoPath = pathToReturn;
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

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
        SendPath(target, pathToReturn);
        //}
    }

    private int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

        return ix + iy;
    }

    private void OnEnable()//sends a final path with the asociated endpoint
    {
        MoveCharacter.RequestPath += FindPathReturner;
    }

    private void OnDisable()
    {
        MoveCharacter.RequestPath -= FindPathReturner;
    }
    private void OnDrawGizmos()
    {
        if (gizmoPath != null)
        {
            Gizmos.color = Color.green;
            foreach (var node in gizmoPath)
            {
                Gizmos.DrawSphere(node.position, 0.4f);
            }

        }

    }
    #endregion
}
