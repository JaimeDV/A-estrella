using System.Collections.Generic;
using UnityEngine;

public class S_ModePlayer : MonoBehaviour
{
    private Grid grid;
    private Node cuarrentNode;
    private int nodePosition;

    [SerializeField]
    private GameObject player;

    private float steeps;
    private float timer;
    private float startDelay;
    private Vector3 start;
    private List<Node> finalPath;
    [SerializeField]
    private float velocity;

    private Vector3 cuarrentPosition;

    private void Start()
    {
        grid = GetComponent<Grid>();
        finalPath=grid.FinalPath;
        CheckNode();
    }

    private void Update()
    {
        startDelay++;
        startDelay += Time.deltaTime;
        if (startDelay > 5) {
            grid = GetComponent<Grid>();
            finalPath = grid.FinalPath;
            CheckNode();
        }
        MoveToTarget();
    }

    private void CheckNode()
    {
        if (finalPath != null && finalPath.Count > 0)
        {
            timer = 0;
            start = player.transform.position;
            cuarrentPosition = finalPath[nodePosition].position;
        }    
    }

    private void MoveToTarget()
    {
        if (finalPath != null && finalPath.Count > 0)
        {
            timer += Time.deltaTime * velocity;

            if (player.transform.position != cuarrentPosition)
            {
                player.transform.position = Vector3.Lerp(start, cuarrentPosition, timer);
            }
            else
            {
                if (nodePosition < finalPath.Count - 1)
                {
                    nodePosition++;
                    CheckNode();
                }
            }
        }
    }
    public void resetTimer()
    {
        startDelay=0;
    }
    private void OnEnable()
    {
        MoveTarget.MoveCube += resetTimer;
    }
    private void OnDisable()
    {
        MoveTarget.MoveCube -= resetTimer;
    }
}