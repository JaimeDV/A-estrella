using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// @copyright © 2023+ Gaslight Studios All Rights Reserved
// @author Author: Jaime Díaz Viéitez
// pamecin@gmail.com
// @date 20/11/2023
// @brief  Using A A* star grid guides movement towards an already defined objective
public class MoveCharacter : MonoBehaviour
{
    [SerializeField]
    public Grid grid;

    [SerializeField]
    public GameObject endPoint;

    private Node cuarrentNode;
    private int nodePosition;

    [SerializeField]
    private GameObject Self;

    //private float steeps;
    //private float timer;
    private float startDelay;

    [SerializeField]
    [Tooltip("How2 should it wait for it to search a path")]
    private float startDelayMax;

    private Vector3 startPosition;
    private List<Node> finalPath;

    private float speed;

    private Vector3 cuarrentNodeDistance;
    private Vector3 velocity;


    [SerializeField]
    private GameObject model;

    [SerializeField]
    private float rotationSpeed;

    // checks if the character calls the rotate fuction or not
    [SerializeField]
    public bool rotatesWhenMoving = true;

    [SerializeField] private bool isDebug = false;

    [SerializeField] private bool chasesPlayer = true;

    public static System.Action<Vector3, Vector3> generateFinal;
    public static System.Action<GameObject, GameObject> RequestPath;

    private void Awake()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        if (endPoint == null && isDebug && chasesPlayer)
        {
            //grid = GameObject.FindObjectOfType<Grid>();
            endPoint = GameObject.Find("P_PlayerControler");
        }
    }

    private void Start()
    {
        velocity = Vector3.zero;
        startDelay = startDelayMax;
    }

    private void Update()
    {
        startDelay++;
        startDelay -= Time.deltaTime;
        if (startDelay < 0)
        {
            //grid = GetComponent<Grid>();
            RequestPath(this.gameObject, endPoint);
            CheckNode();
            //finalPath = grid.FinalPath;
            //finalPath = S_Pathfinding.
            startDelay = 0;
        }
        MoveToTarget();
        if (rotatesWhenMoving)
        {
            Rotate();
        }

        //RequestPath(this.gameObject, endPoint);
    }

    /// @brief rotates the player mesh to the positon the player is looking at or with the gamepad
    private void Rotate()
    {
        if (velocity.magnitude > 0)
        {
            Vector3 rotation = velocity * Time.deltaTime;
            float angleRadians = Mathf.Atan2(rotation.z, rotation.x);
            float angleDegrees = -angleRadians * Mathf.Rad2Deg;
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation,
            Quaternion.AngleAxis(angleDegrees + 90, Vector3.up), rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckNode()
    {
        if (finalPath != null && finalPath.Count > 0)
        {
            startPosition = Self.transform.position;
            cuarrentNodeDistance = finalPath[nodePosition].position;
        }
    }

    private void MoveToTarget()
    {
        if (finalPath != null && finalPath.Count > 0)//should change it so it continues moving
        {
            //if (Self.transform.position != cuarrentNodeDistance)
            //{
            //    Vector3 distance = (cuarrentNodeDistance - Self.transform.position);

            //    Vector3 desiredVelocity = (distance.normalized * speed);
            //    Vector3 steering = desiredVelocity - velocity;

            //    velocity += steering * Time.deltaTime;

            //    velocity.y = 0;//i don't know why it adds to the y axis

            //    //rigid.velocity = velocity;
            //    Self.transform.position += velocity * Time.deltaTime;
            //    //player.transform.position = Vector3.Lerp(startPosition, cuarrentNodeDistance, timer);
            //}

            if (Self.transform.position != cuarrentNodeDistance)
            {
                SetDirrection(cuarrentNodeDistance);
            }
            //else if (Self.transform.position != endPoint.transform.position && finalPath.Count <= 0)
            //{
            //    SetDirrection(endPoint.transform.position);
            //}
            else
            {
                if (nodePosition < finalPath.Count - 1)
                {
                    nodePosition++;
                    CheckNode();
                }
            }
        }
        else if (Self.transform.position != endPoint.transform.position)
        {
            SetDirrection(endPoint.transform.position);
            //RequestPath(this.gameObject, endPoint);
        }
    }

    private void SetDirrection(Vector3 target)
    {
        Vector3 distance = (target - Self.transform.position);

        Vector3 desiredVelocity = (distance.normalized * speed);
        Vector3 steering = desiredVelocity - velocity;

        velocity += steering * Time.deltaTime;
        velocity.y = 0;//i don't know why it adds to the y axis

        Vector3 ChracterPosition = new Vector3(Self.transform.position.x, Self.transform.position.y, Self.transform.position.z);
        Vector3 NodeDistance = new Vector3(target.x, Self.transform.position.y, target.z);
        Self.transform.position = Vector3.Lerp(ChracterPosition, NodeDistance, speed * Time.deltaTime);
    }

    //public void resetTimer()
    //{
    //    startDelay = 0;
    //}

    private void recivePath(GameObject SentendPoint, List<Node> path)
    {
        if (SentendPoint == endPoint)
        {
            finalPath = path;
        }
    }

    public void SetEndPoint(GameObject newPoint)
    {
        endPoint = newPoint;
    }

    private void OnEnable()
    {
        S_Pathfinding.SendPath += recivePath;
    }

    private void OnDisable()
    {
        S_Pathfinding.SendPath -= recivePath;
    }

    private void OnDrawGizmos()
    {
        if (finalPath != null)
        {
            Gizmos.color = Color.red;
            foreach (var node in finalPath)
            {
                Gizmos.DrawSphere(node.position, 0.2f);
            }
        }
        if (endPoint != null)
        {
            Gizmos.DrawSphere(endPoint.transform.position, 0.5f);
        }
    }
}