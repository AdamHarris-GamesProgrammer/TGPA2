using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DebugNavMeshAgent : MonoBehaviour
{
    NavMeshAgent _agent;


    [SerializeField] bool _drawVelocity;
    [SerializeField] bool _drawDesiredVelocity;
    [SerializeField] bool _drawPath;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

    }

    private void OnDrawGizmos()
    {
        if (!_agent) return;

        if (_drawVelocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _agent.velocity);
        }   

        if(_drawDesiredVelocity)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _agent.desiredVelocity);
        }

        if(_drawPath)
        {
            var agentPath = _agent.path;
            Gizmos.color = Color.red;

            Vector3 prevCorner = transform.position;
            foreach(var corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
