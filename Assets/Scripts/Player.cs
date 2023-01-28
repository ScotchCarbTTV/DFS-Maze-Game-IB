using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;

    //variables for storing nodes to the north, south, east and west
    [SerializeField] private Node northNode;
    [SerializeField] private Node southNode;
    [SerializeField] private Node eastNode;
    [SerializeField] private Node westNode;

    //variable for determing which direction to move
    private int movementValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if(node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
        FindNodes();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            //Implement inputs and event-callbacks here
            MouseInteraction();
            KeyboardMovement();
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                FindNodes();
                CurrentNode = TargetNode;
            }
        }
    }

    //Implement mouse interaction method here
    private void MouseInteraction()
    {
        NavButton _navButton;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //replace with trygetcomponent for easier?
                if(hit.collider.TryGetComponent<NavButton>(out _navButton))
                {
                    //use the navbutton's direction value to send to the update target node method
                    UpdateTargetNode(_navButton.direction);
                }
            }
        }
    }

    //keyboard movement
    private void KeyboardMovement()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpdateTargetNode(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UpdateTargetNode(2);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateTargetNode(3);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UpdateTargetNode(4);
        }
    }

    //method for finding neighbouring nodes
    private void FindNodes()
    {
        RaycastHit hit;

        //send a raycast to the north, south, east and west, and see if it collides with a node
        //north
        if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hit, Mathf.Infinity))
        {
            hit.collider.TryGetComponent<Node>(out northNode);            
        }
        else
        {
            northNode = null;
        }
        //south
        if (Physics.Raycast(transform.position, new Vector3(0, 0, -1), out hit, Mathf.Infinity))
        {
            hit.collider.TryGetComponent<Node>(out southNode);            
        }
        else
        {
            southNode = null;
        }
        //west
        if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out hit, Mathf.Infinity))
        {
            hit.collider.TryGetComponent<Node>(out westNode);
        }
        else
        {
            westNode = null;
        }
        //east
        if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out hit, Mathf.Infinity))
        {
            hit.collider.TryGetComponent<Node>(out eastNode);
        }
        else
        {
            eastNode = null;
        }
    }

    public void UpdateTargetNode(int _targetNode)
    {
        switch ( _targetNode)
        {
            case 1:
                if (northNode != null)
                {
                    MoveToNode(northNode);
                }
                break;
            case 2:
                if (southNode != null)
                {
                    MoveToNode(southNode);
                }
                break;
            case 3:
                if (westNode != null)
                {
                    MoveToNode(westNode);
                }
                break;
            case 4:
                if (eastNode != null)
                {
                    MoveToNode(eastNode);
                }
                break;
        }
    }

    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }
}
