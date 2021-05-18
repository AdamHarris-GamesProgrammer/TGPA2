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
        _patrolPoints = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            _patrolPoints[i] = transform.GetChild(i);
        }
    }

    public Vector3 GetNextPoint(int index)
    {
        return _patrolPoints[index].position;
    }

    public int CycleIndex(int index)
    {
        if (_loop)
        {
            return (index + 1) % _patrolPoints.Length;
        }
        else
        {
            if(index == transform.childCount - 1) _incremeneter = -1;
            else if(index == 0) _incremeneter = 1;

            index += _incremeneter;
            return index;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 previousPos = transform.GetChild(0).position;


        foreach(Transform trans in transform)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(trans.position, 0.5f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(previousPos, trans.position);
            previousPos = trans.position;
        }

        if (_loop)
        {
            Gizmos.DrawLine(previousPos, transform.GetChild(0).position);
        }
    }
}
