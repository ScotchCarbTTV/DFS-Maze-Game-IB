using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndPrint(1f));

        InvokeRepeating("UpdateAIPathFinding", 1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateAIPathFinding()
    {
        //check if my path is valid
        Debug.Log("Checking AI paths");
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        Debug.Log("First message. No time has passed.");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Second message. " + waitTime + " second has passed.");
        yield return new WaitForSeconds(waitTime + 4);
        Debug.Log("Third message. " + waitTime + 1f + " seconds have passed.");
    }

}

