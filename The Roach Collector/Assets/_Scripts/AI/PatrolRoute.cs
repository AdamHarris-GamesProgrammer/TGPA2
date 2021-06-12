using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    Transform[] _patrolPoints;

    [Tooltip("If looping is set to true then the AI will go from the last to the first waypoint. If it is set to false the AI will go back to the previous point.")]
    [SerializeField] bool _loop = false;

    int _incremeneter = 1;

    private void Awake()
    {
        //Gets the patrol points
        _patrolPoints = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            _patrolPoints[i] = transform.GetChild(i);
        }
    }

    public Vector3 GetNextPoint(int index)
    {
        //Returns the position of the passed in index
        return _patrolPoints[index].position;
    }

    public int CycleIndex(int index)
    {
        //if looping 
        if (_loop)
        {
            //Cycle the index 
            return (index + 1) % _patrolPoints.Length;
        }
        else
        {
            //if we are not looping, we have to change the incremementer to -1 or 1 based on if they are at element 0 or last element
            if(index == transform.childCount - 1) _incremeneter = -1;
            else if(index == 0) _incremeneter = 1;

            index += _incremeneter;
            return index;
        }
    }

    private void OnDrawGizmos()
    {
        //Gets the position of the first point
        Vector3 previousPos = transform.GetChild(0).position;

        //Cycles through all points in the transform
        foreach(Transform trans in transform)
        {
            //Sets the color to green
            Gizmos.color = Color.green;
            //Draws a sphere at the points location
            Gizmos.DrawWireSphere(trans.position, 0.5f);

            //Sets the color to white
            Gizmos.color = Color.white;
            //Draws a line between the previous point and this point
            Gizmos.DrawLine(previousPos, trans.position);
            previousPos = trans.position;
        }

        //If looping draw a point from the last point to the first point
        if (_loop)
        {
            Gizmos.DrawLine(previousPos, transform.GetChild(0).position);
        }
    }
}
