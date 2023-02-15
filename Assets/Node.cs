using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int gridX;
    public int gridY;

    public bool isNotWall;
    public bool isNotWater;
    public Vector3 position;

    public Node parentNode;

    public int igCost;
    public int ihCost;

    public int FCost { get { return igCost + ihCost; } }

    public Node(bool isWall, bool isWater, Vector3 a_vPos, int gridX, int gridY)
    {
        this.isNotWall = isWall;
        this.isNotWater = isWater;
        this.position = a_vPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

}
