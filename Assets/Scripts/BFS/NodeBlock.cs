using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBlock : MonoBehaviour
{
    //variable for the meshrenderer
    private MeshRenderer meshRenderer;

    //serialized field for the red and green materials
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;
    [SerializeField] Material blueMat;


    //list of NodeBlocks for neighbouring blocks
    public List<NodeBlock> neighbours = new List<NodeBlock>();

    //variable for the 'parent' (eg, the node which was checked before this one during pathfinding) node
    public NodeBlock pathNode { get; private set; }

    //serialized list of vector 3 which the neighbour detection will use for finding neighbours.
    [SerializeField] List<Vector3> neighbourDirections = new List<Vector3>();

    private bool activeBlock;    

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        activeBlock = false;
        //set the material to green if it isn't already
        //assign the meshRenderer
        if (!TryGetComponent<MeshRenderer>(out meshRenderer))
        {
            Debug.LogError("This object needs a MeshRenderer attached to it");
        }

    }

    // Start is called before the first frame update
    void Start()
    {   
        meshRenderer.material = greenMat;
        //find neighbours method
        FindNeighbours();

        //subscribe the 'clear parent' method to the BreadthFirstSearch event
        BreadthFirstSearchTrial.breadthFirstSearchEvent += ClearPathNode;
        BreadthFirstSearchTrial.clearColourFromNodesEvent += ResetColour;
    }

    // Update is called once per frame
    void Update()
    {
        //if this block is active and the player presses soem button, set it as the target destiantion for BreadthFirst
        if (activeBlock)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                BreadthFirstSearchTrial.setDestinationNodeEvent(this);
            }
        }
    }

    //method for clearing parents.
    private void ClearPathNode()
    {
        pathNode = null;
    }

    public void SetPathNode(NodeBlock _parent)
    {
        pathNode = _parent;
    }

    private void ResetColour()
    {
        meshRenderer.material = greenMat;
    }

    public void SetColour(int choice)
    {
        if (choice == 0)
        {
            meshRenderer.material = greenMat;
        }
        else if(choice == 1)
        {
            meshRenderer.material = redMat;
        }
        else if(choice == 2)
        {
            meshRenderer.material = blueMat;
        }
    }

    //find neighbours method
    private void FindNeighbours()
    {
        //local variables for thhe raycast hit and checking if we've founda neighbouring block
        RaycastHit hit;
        NodeBlock nodeBlock;

        //iterate through the directions we want to check and see if there are neighbours
        for(int i = 0; i < neighbourDirections.Count; i++)
        {
           if(Physics.Raycast(transform.position, neighbourDirections[i], out hit, 2f))
            {
                if (hit.collider.TryGetComponent<NodeBlock>(out nodeBlock))
                {
                    neighbours.Add(nodeBlock);
                    //Debug.Log("Added " + nodeBlock + " to " + name + "'s neighbourlist");
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        meshRenderer.material = blueMat;
        activeBlock = true;
    }

    private void OnMouseExit()
    {
        meshRenderer.material = greenMat;
        activeBlock = false;
    }

    private void OnDestroy()
    {
        BreadthFirstSearchTrial.breadthFirstSearchEvent -= ClearPathNode;
        BreadthFirstSearchTrial.clearColourFromNodesEvent -= ResetColour;
    }
}
