using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitAndPrint(3f));
    }    

        IEnumerator WaitAndPrint(float waitTime)
    {
        Debug.Log("First message. No time has passed.");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Second messsage. It's been " + waitTime + " seconds.");
        yield return new WaitForSeconds(waitTime * 2);
        Debug.Log("Third messsage. It's been " + waitTime * 2 + " seconds.");
    }
}
