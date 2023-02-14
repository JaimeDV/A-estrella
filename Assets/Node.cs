using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int gridX;
    public int gridY;

    public bool isWall;
    public Vector3 position;

    public Node parentNode;

    public int igCost;
    public int ihCost;

    public int FCost { get { return igCost + ihCost; } }

    public Node(bool isWall, Vector3 a_vPos, int gridX, int gridY)
    {
        this.isWall = isWall;
        this.position = a_vPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

}
