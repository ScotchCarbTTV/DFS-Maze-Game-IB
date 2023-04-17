using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBlockGenerator : MonoBehaviour
{
    //VARIABLES
    [SerializeField] GameObject nodeBlockPref;
    
    [SerializeField] int gridWidth;
    
    [SerializeField] int gridHeight;

    [SerializeField] Vector3 gridStartPoint;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //spawn in width * height number of block objects
        SpawnBlockNodes(gridWidth, gridHeight);
    }

    //method for spawning in blocks
    private void SpawnBlockNodes(int _width, int _height)
    {
        GameObject newNode;

        NodeBlock rootNodeBlock;

        bool rootNodeSet = false;
        //take the width and iterate through it from 0 to width
        for (int x = 0; x < _width; x++)
        {
            //iterate through height from 0 to height
            for (int y = 0; y < _height; y++)
            {                
                //spawn in a block at each coordinate from 0,0, to width, height
                newNode = Instantiate(nodeBlockPref, new Vector3(x + gridStartPoint.x, 2, y + gridStartPoint.z), gameObject.transform.rotation, gameObject.transform);
                newNode.name = "node " + x.ToString() + "," + y.ToString();
                if (rootNodeSet == false)
                {
                    if (newNode.TryGetComponent<NodeBlock>(out rootNodeBlock))
                    {
                        BreadthFirstSearchTrial.setRootNodeEvent(rootNodeBlock);
                        rootNodeSet = true;
                    }
                    else
                    {
                        Debug.Log("Ain't found the component");
                    }
                }
                else
                {
                    
                    rootNodeSet = true;
                }
            }
        }

    }


}
