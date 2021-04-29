using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    bool enemyinFOV;

    [HideInInspector()]
    public List<Transform> visibleTargets = new List<Transform>();

    public float DetectionTimer = 0;

    private void Update()
    {
        findVisibleTargets();

        if (visibleTargets.Count > 0)
        {
            enemyinFOV = true;
        }
        else
        {
            enemyinFOV = false;
        }

    }

    void findVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] TargetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (Collider target in TargetsInRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 targetDirection = (targetTransform.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, targetDirection) < viewAngle / 2)
            {
                float DistanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                RaycastHit hitInfo;
         
                if (Physics.Raycast((transform.position + new Vector3(0, 1, 0)), targetDirection, out hitInfo, DistanceToTarget))
                {
                    Debug.DrawRay((transform.position + new Vector3(0,1,0)), (targetDirection * DistanceToTarget), Color.green);
                    if (hitInfo.collider.tag == "Player")
                    {
                        DetectionTimer = Mathf.Lerp(0, 1, 0.1f);
                        visibleTargets.Add(targetTransform);
                    }
                }

            }
        }

    }

    public Vector3 DirectionFromAngle(float angleindegrees, bool angleisglobal)
    {
        if (!angleisglobal)
        {
            angleindegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleindegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleindegrees * Mathf.Deg2Rad));
    }

    public bool IsEnemyInFOV()
    {
        return enemyinFOV;
    }

}

