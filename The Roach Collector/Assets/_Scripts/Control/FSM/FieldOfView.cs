using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("View Settings")]
    [Min(0f)] public float _viewRadius;
    [Range(0, 360)] public float _viewAngle;
    
    [Header("Target Layer")]
    [SerializeField] LayerMask _targetMask;

    [Header("Detection Settings")]
    [SerializeField] float _detectedStand = 1;
    [SerializeField] float _detectedCrouch = 3;
    [SerializeField] float _detectedValue = 1;

    [Header("Debug Information")]
    [SerializeField] float _detectionTimer = 0;

    [HideInInspector()]
    public List<Transform> _visibleTargets = new List<Transform>();

    bool _hasTimerStarted = false;

    private GameObject _playerGO;
    CharacterLocomotion _charLocomotion;

    //Says if the player is currently in the AI's FOV
    bool _isEnemyinFOV;
    public bool IsEnemyInFOV { get { return _isEnemyinFOV; } }

    Health _aiHealth;

    LastKnownLocation _lastKnownLocation;

    private void Start()
    {
        _playerGO = GameObject.FindGameObjectWithTag("Player");
        _charLocomotion = _playerGO.GetComponent<CharacterLocomotion>();
        _aiHealth = GetComponent<Health>();
        _lastKnownLocation = FindObjectOfType<LastKnownLocation>();
    }

    void Update()
    {
        //If the AI is dead then destroy this component
        if (_aiHealth.IsDead) Destroy(this);

        //Is the player in FOV
        FindVisibleTargets();

        //have we got a visible target
        if (_visibleTargets.Count > 0)
        {
            _playerGO.GetComponent<PlayerController>().IsDetected = true;
            _isEnemyinFOV = true;
        }
        else
        {
            _playerGO.GetComponent<PlayerController>().IsDetected = false;
            _isEnemyinFOV = false;
        }

        //Add the value to the detection timer
        if(_hasTimerStarted) _detectionTimer = Mathf.Min(_detectionTimer + Time.deltaTime, 5.0f);
        else _detectionTimer = Mathf.Max(_detectionTimer -= Time.deltaTime, 0.0f);

    }
    
    void FindVisibleTargets()
    {
        //Clear the visible targets list
        _visibleTargets.Clear();

        //Find all the colliders in the target layer
        Collider[] TargetsInRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);
        //Cycle through them all
        foreach (Collider target in TargetsInRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 targetDirection = (targetTransform.position - transform.position).normalized;
            
            //In the FOV 
            if (Vector3.Angle(transform.forward, targetDirection) < _viewAngle / 2)
            {
                float DistanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                RaycastHit hitInfo;
         
                //Check if we are visible
                if (Physics.Raycast((transform.position + Vector3.up), targetDirection, out hitInfo, _viewRadius, _targetMask))
                {
                    //Draw a line to them
                    Debug.DrawRay((transform.position + Vector3.up), (targetDirection * DistanceToTarget), Color.green);
                    if (hitInfo.collider.tag == "Player")
                    {
                        //if the player is crouching then change the detected value to the crouch value
                        if(_charLocomotion.IsCrouching) _detectedValue = _detectedCrouch;
                        else _detectedValue = _detectedStand;

                        //if the distance to the target is less than 1/8th of the radius then auto detect them
                        if(DistanceToTarget <= (_viewRadius / 8))
                        {
                            _visibleTargets.Add(targetTransform);
                            //Move the last known location 
                            _lastKnownLocation.transform.position = hitInfo.point;
                            return;
                        }

                        _hasTimerStarted = true;

                        if (_detectionTimer > _detectedValue)
                        {
                            _visibleTargets.Add(targetTransform);
                            //Move the last known location
                            _lastKnownLocation.transform.position = hitInfo.point;
                        }
                    }
                    else
                    {
                        Debug.Log(hitInfo.collider.tag);
                        _hasTimerStarted = false;
                    }
                }

            }
        }

    }

    public Vector3 DirectionFromAngle(float angleindegrees, bool angleisglobal)
    {
        if (!angleisglobal) angleindegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleindegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleindegrees * Mathf.Deg2Rad));
    }
}

