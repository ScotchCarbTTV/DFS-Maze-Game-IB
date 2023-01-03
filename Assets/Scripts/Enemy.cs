using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        InitializeAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null)
            {
                //If within 0.25 units of the current node.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                else
                {
                    //Implement path finding here
                    currentNode = DFSAlgorithm();
                }

            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Implement DFS algorithm method here

    //declare and clear temporary list of nodes
    private Node DFSAlgorithm()
    {
        //current node of the player
        Node _playerNode = GameManager.Instance.Player.CurrentNode;
        
        //variable for the node to set as the new destination (the method will return this)
        Node _targetNode = null;

        //bool for checking if we have actually found the target node
        bool _found = false;        

        //list of nodes to be checked, which will be updated in the while loop
        List<Node> _nodeList = new List<Node>();

        //add the root node to the _nodeList
        _nodeList.Add(GameManager.Instance.Nodes[0]);

        while (!_found)
        {
            //make sure the list isn't empty for whatever reason
            if(_nodeList != null)
            {
                Debug.Log("Checking" + _nodeList[0]);
                //check if the node at the 'top' of the list is the one we want
                if(_nodeList[0] == _playerNode)
                {
                    //if it is then we return our target node
                    _targetNode = _nodeList[0];
                    Debug.Log("Found the target!");
                    _found = true;
                }
                else
                {
                    //if it isn't the target, we check if it has children and add the children to the list
                    if(_nodeList[0].Children != null)
                    {
                        foreach(Node n in _nodeList[0].Children)
                        {
                            //insert each child into the list at the second position
                            //once the node being checked is removed one of the child objects will then be at position 0 and take priority
                            _nodeList.Insert(1, n);
                        }
                        //we remove the current node from the list and the loop continues
                        _nodeList.Remove(_nodeList[0]);
                    }
                }
            }
            else
            {
                Debug.Log("The list of Nodes was exhausted without finding the target/the list was not populated");
                break;
            }
        }

        //return the target node once the for loop is done
        currentDir = _targetNode.transform.position + new Vector3(0, 0.5f, 0) - transform.position;
        currentDir = currentDir.normalized;
        return _targetNode;
    }

    
}
