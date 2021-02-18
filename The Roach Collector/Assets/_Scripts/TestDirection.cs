using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDirection : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Vector3 targetDir = target.position - transform.position;

        float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        //Debug.Log(angle);

        if(angle < 45.0f && angle > -45.0f)
        {
            Debug.Log("Forwards");
        }
        else if(angle > 45.0f && angle < 135.0f)
        {
            Debug.Log("Right");
        }
        else if(angle > 135.0f || angle < -135.0f)
        {
            Debug.Log("Backwards");
        }
        else
        {
            Debug.Log("Left");
        }


        Debug.DrawRay(transform.position, transform.forward * 3.0f);
    }

    
}
