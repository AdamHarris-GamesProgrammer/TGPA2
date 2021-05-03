using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private GameObject Player;
    CharacterLocomotion CharLocomotion;
    [Min(0f)]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    bool enemyinFOV;

    [HideInInspector()]
    public List<Transform> visibleTargets = new List<Transform>();

    public float DetectionTimer = 0;
    public float DetectedStand = 1;
    public float DetectedCrouch = 3;
    public float DetectedValue = 1;
    bool StartTimer = false;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CharLocomotion = Player.GetComponent<CharacterLocomotion>();
    }

    void Update()
    {
        findVisibleTargets();

        if (visibleTargets.Count > 0)
        {
            Player.GetComponent<PlayerController>().IsDetected = true;
            enemyinFOV = true;
        }
        else
        {
            enemyinFOV = false;
        }

        if(StartTimer && DetectionTimer <= DetectedValue)
        {
            DetectionTimer += Time.deltaTime;
        }
        else
        {
            DetectionTimer = 0;
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
         
                if (Physics.Raycast((transform.position + Vector3.up), targetDirection, out hitInfo, DistanceToTarget))
                {
                    Debug.DrawRay((transform.position + Vector3.up), (targetDirection * DistanceToTarget), Color.green);
                    if (hitInfo.collider.tag == "Player")
                    {
                        if(CharLocomotion.GetisCrouching())
                        {
                            DetectedValue = DetectedCrouch;
                        }
                        else
                        {
                            DetectedValue = DetectedStand;
                        }


                        if(DistanceToTarget <= (viewRadius / 2))
                        {
                            //Debug.Log("Player is too close");
                            visibleTargets.Add(targetTransform);
                            return;
                        }

                        StartTimer = true;

                        if (DetectionTimer > DetectedValue)
                        {
                            //Debug.Log("Player is detected through time");
                            visibleTargets.Add(targetTransform);
                        }
                    }
                    else
                    {
                        StartTimer = false;
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

