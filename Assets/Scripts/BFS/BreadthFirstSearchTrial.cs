using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearchTrial : MonoBehaviour
{   

    //root node variable
    [SerializeField] NodeBlock rootNode;

    //destination node variable
    [SerializeField] NodeBlock destNode;

    //delegate and event to be accessed by generator for setting the rootnode
    public delegate void SetRootNodeDelegate(NodeBlock _nodeBlock);
    public static SetRootNodeDelegate setRootNodeEvent = delegate { };

    //delegate and event for setting the destination node
    public delegate void SetDestinationNodeDelegate(NodeBlock _nodeBlock);
    public static SetDestinationNodeDelegate setDestinationNodeEvent = delegate { };

    public delegate void BreadthFirstSearchDelegate();
    public static BreadthFirstSearchDelegate breadthFirstSearchEvent = delegate { };

    public delegate void ClearColourFromNodesDelegate();
    public static ClearColourFromNodesDelegate clearColourFromNodesEvent = delegate { };

    //list of nodeblocks which form the route once calculateed
    [SerializeField] List<NodeBlock> nodeBlockRoute = new List<NodeBlock>();

    bool searching = false;

    List<NodeBlock> stack = new List<NodeBlock>();

    NodeBlock currentNode; //the node which this algorithm is currently checking

    //public static BreadthFirst Instance;

    bool pathfound = false;
    bool pathmade = false;

    private void OnEnable()
    {
        setDestinationNodeEvent += SetDestNode;
        setRootNodeEvent += SetRootNode;
        //breadthFirstSearchEvent += BreadthFirstSearchAlgorithm;
    }

    private void Start()
    {
        /*
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Only one BreadthFirst allowed, ya dingus");
        }*/

        //set the root node

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && searching == false)
        {
            if (destNode != null)
            {
                searching = true;
                nodeBlockRoute.Clear();
                clearColourFromNodesEvent.Invoke();
                stack.Clear();
                stack.Add(rootNode);

                //starting with the root node...
                currentNode = stack[0];

                pathfound = false;
                pathmade = false;
                Debug.Log("Starting search");
                //BreadthFirstSearchAlgorithm();
            }
        }        

        if (searching == true)
        {
            BreadthFirstSearchAlgorithm();            
        }
    }

    //method for setting destination node
    private void SetDestNode(NodeBlock _destNode)
    {
        destNode = _destNode;
    }


    //method for setting rootNode
    private void SetRootNode(NodeBlock _rootNode)
    {
        rootNode = _rootNode;
    }

    private void BreadthFirstSearchAlgorithm()
    {
        if (pathfound == false)
        {
            if (currentNode == destNode)
            {
                //start stepping back through the nodes and add them to the route
                //destNode.SetParent(currentNode);
                //currentNode = destNode;
                Debug.Log("Found the path.");
                clearColourFromNodesEvent.Invoke();
                pathfound = true;
            }
            else
            {

                //IF IT ISN'T, FIND ITS NEIGHBOURS AND ADD THEM TO STACK LIST
                for (int i = 0; i < currentNode.neighbours.Count; i++)
                {
                    if (currentNode.neighbours[i].pathNode == null)
                    {
                        currentNode.neighbours[i].SetPathNode(currentNode);
                    }
                    if (!stack.Contains(currentNode.neighbours[i]))
                    {

                        stack.Add(currentNode.neighbours[i]);
                        //Debug.Log("Added " + currentNode.name + " to stack");                        
                    }                   
                    stack.Remove(currentNode);
                        // Debug.Log("Can't add anything eelse to the stack for some reason?");
                    
                }

                //set the node that's the next in the stack from the current node and continue
                if (stack.Count > 0)
                {
                    currentNode = stack[0];
                    currentNode.SetColour(2);
                }
                else
                {
                    Debug.Log("The stack is empty");
                }
            }
        }
        else
        {

            if (pathmade == false)
            {
                if (currentNode == rootNode)
                {
                    nodeBlockRoute.Add(rootNode);
                    rootNode.SetColour(1);
                    pathmade = true;
                    searching = false;
                    Debug.Log("Finished making the path");
                }
                else
                {
                    Debug.Log("Drawing path still");
                    Debug.Log("Current node is " + currentNode.name + ", and the destination node is " + destNode.name);
                    if (!nodeBlockRoute.Contains(currentNode))
                    {
                        nodeBlockRoute.Add(currentNode);
                    }
                    currentNode.SetColour(1);
                    currentNode = currentNode.pathNode;
                }
            }
        }
    }

    private void OnDestroy()
    {
        setDestinationNodeEvent -= SetDestNode;
        setRootNodeEvent -= SetRootNode;
        //breadthFirstSearchEvent -= BreadthFirstSearchAlgorithm;
    }
}
