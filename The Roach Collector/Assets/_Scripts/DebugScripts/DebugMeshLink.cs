using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMeshLink : MonoBehaviour
{
    Transform[] _transforms;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        _transforms = GetComponentsInChildren<Transform>();
        foreach (Transform transform in _transforms)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.2f); 
        }
    }
}
