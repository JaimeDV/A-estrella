using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //#region oldCode
    //public Transform startPosition;
    //public LayerMask wallMask;
    //public LayerMask waterMask;
    //public Vector2 worldSize;
    //public float nodeRadius;
    //public float distanceBetweenNodes;

    //Node[,] nodeArray;
    //public List<Node> FinalPath;


    //float nodeDiameter;
    //int gridSizeX;
    //int gridSizeY;


    //private void Start()
    //{
    //    nodeDiameter = nodeRadius * 2;
    //    gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
    //    gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
    //    CreateGrid();
    //}

    //void CreateGrid()
    //{
    //    nodeArray = new Node[gridSizeX, gridSizeY];
    //    Vector3 bottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;//Get the real world position of the bottom left of the grid.
    //    for (int x = 0; x < gridSizeX; x++)
    //    {
    //        for (int y = 0; y < gridSizeY; y++)
    //        {
    //            Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the bottom left of the graph
    //            bool Wall = true; //it is backwards but it works, don't touch it

    //            if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
    //            {
    //                Wall = false;
    //            }

    //            bool Water = true;

    //            if (Physics.CheckSphere(worldPoint, nodeRadius, waterMask))
    //            {
    //                Water = false;
    //            }

    //            nodeArray[x, y] = new Node(Wall,Water, worldPoint, x, y);
    //        }
    //    }
    //}


    //public List<Node> GetNeighboringNodes(Node node)
    //{
    //    List<Node> pals = new List<Node>();
    //    int icheckX;
    //    int icheckY;

    //    icheckX = node.gridX + 1;
    //    icheckY = node.gridY;
        
    //    for (int x = -1; x <= 1; x++)
    //    {
    //        for (int y = -1; y <= 1; y++)
    //        {
    //            if (x == 0 && y == 0)
    //            {
    //                continue;
    //            }

    //            int checkX = node.gridX + x;
    //            int checkY = node.gridY + y;

    //            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
    //            {
    //                pals.Add(nodeArray[checkX, checkY]);
    //            }

    //        }
    //    }
    //    return pals;
    //}

    //public Node NodeFromWorldPoint(Vector3 node)
    //{
    //    float xPos = ((node.x + worldSize.x / 2) / worldSize.x);
    //    float yPos = ((node.z + worldSize.y / 2) / worldSize.y);

    //    xPos = Mathf.Clamp01(xPos);
    //    yPos = Mathf.Clamp01(yPos);

    //    int ix = Mathf.RoundToInt((gridSizeX - 1) * xPos);
    //    int iy = Mathf.RoundToInt((gridSizeY - 1) * yPos);

    //    return nodeArray[ix, iy];
    //}


    //private void OnDrawGizmos()
    //{

    //    Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 1, worldSize.y));

    //    if (nodeArray != null)
    //    {
    //        foreach (Node n in nodeArray)
    //        {
    //            if (n.isNotWall)
    //            {
    //                Gizmos.color = Color.white;
    //            }
    //            if (!n.isNotWater)
    //            {
    //                Gizmos.color = Color.blue;
    //            }
    //            if(!n.isNotWall)
    //            {
    //                Gizmos.color = Color.yellow;
    //            }


    //            if (FinalPath != null)
    //            {
    //                if (FinalPath.Contains(n))
    //                {
    //                    Gizmos.color = Color.red;
    //                }

    //            }

    //            Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distanceBetweenNodes));
    //        }
    //    }
    //}
    //#endregion
    #region newCode
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private LayerMask wallMask;
    [SerializeField]
    private LayerMask aditionalWallMask;
    [SerializeField]
    private LayerMask waterMask;
    [SerializeField]
    private Vector2 worldSize;
    [SerializeField]
    private float nodeRadius;
    [SerializeField]
    private float distanceBetweenNodes;

    public Node[,] nodeArray;
    public List<Node> FinalPath;


    float nodeDiameter;
    [SerializeField]
    int gridSizeX;
    [SerializeField]
    int gridSizeY;

    public bool alternate;

    public static event System.Action<Node[,]> GetGrid;

    [SerializeField]
    private bool showGrid;

    //public static event System.Action<WorldCell[,]> PassGrid;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        if (alternate)
        {
            CreateGridAlternate();
        }
        else
        {
            CreateGrid();

        }
    }
    private void RegenerateGrid()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        if (alternate)
        {
            CreateGridAlternate();
            GetGrid(nodeArray);
        }
        else
        {
            CreateGrid();

        }
    }

    void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Wall = true; //it is backwards but it works, don't touch it

                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                {
                    Wall = false;
                }

                if (Physics.CheckSphere(worldPoint, nodeRadius, aditionalWallMask))
                {
                    Wall = false;
                }

                bool Water = true;

                if (Physics.CheckSphere(worldPoint, nodeRadius, waterMask))
                {
                    Water = false;
                }

                nodeArray[x, y] = new Node(Wall, Water, worldPoint, x, y);
            }
        }
    }

    void CreateGridAlternate() //flips the wall detention so wall counts as empty and empty as walls
    {
        nodeArray = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Wall = false; //it is backwards but it works, don't touch it

                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                {
                    Wall = true;
                }

                if (Physics.CheckSphere(worldPoint, nodeRadius, aditionalWallMask))
                {
                    Wall = true;
                }

                bool Water = true;

                if (Physics.CheckSphere(worldPoint, nodeRadius, waterMask))
                {
                    Water = false;
                }

                nodeArray[x, y] = new Node(Wall, Water, worldPoint, x, y);
            }
        }
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("PathFinding");
        foreach (GameObject go in cubes)
        {
            Destroy(go);
        }
    }
    public List<Node> GetNeighboringNodes(Node node)
    {
        List<Node> pals = new List<Node>();
        int icheckX;
        int icheckY;

        icheckX = node.gridX + 1;
        icheckY = node.gridY;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    pals.Add(nodeArray[checkX, checkY]);
                }

            }
        }
        return pals;
    }

    public Node NodeFromWorldPoint(Vector3 node)
    {
        float xPos = ((node.x + worldSize.x / 2) / worldSize.x);
        float yPos = ((node.z + worldSize.y / 2) / worldSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int ix = Mathf.RoundToInt((gridSizeX - 1) * xPos);
        int iy = Mathf.RoundToInt((gridSizeY - 1) * yPos);

        return nodeArray[ix, iy];
    }
    public Node[,] getNodeArray()
    {
        return nodeArray;
    }


    private void OnDrawGizmos() //it drains a lot when you have 20 of then
    {
        if (showGrid)
        {

            Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 1, worldSize.y));

            if (nodeArray != null)
            {
                foreach (Node n in nodeArray)
                {
                    if (n.isNotWall)
                    {
                        Gizmos.color = Color.white;
                    }
                    if (!n.isNotWater)
                    {
                        Gizmos.color = Color.blue;
                    }
                    if (!n.isNotWall)
                    {
                        Gizmos.color = Color.yellow;
                    }


                    if (FinalPath != null)
                    {
                        if (FinalPath.Contains(n))
                        {
                            Gizmos.color = Color.red;
                        }

                    }

                    Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distanceBetweenNodes));
                }
            }
        }
    }
    #endregion
}
