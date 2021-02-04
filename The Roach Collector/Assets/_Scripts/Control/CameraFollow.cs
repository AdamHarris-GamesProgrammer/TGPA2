using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    Vector3 _offset;


    private void Awake()
    {
        //Calculate the offset
        _offset = transform.position - _target.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate the new position
        Vector3 newPosition = _target.position + _offset;

        //Move the camera
        transform.position = newPosition;
    }
}
