using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private GameObject _playerGO;
    CharacterLocomotion _charLocomotion;
    [Min(0f)] public float _viewRadius;
    [Range(0, 360)] public float _viewAngle;

    [SerializeField] LayerMask _targetMask;
    bool _isEnemyinFOV;

    public bool IsEnemyInFOV { get { return _isEnemyinFOV; } }

    [HideInInspector()]
    public List<Transform> _visibleTargets = new List<Transform>();

    [SerializeField] float _detectionTimer = 0;
    [SerializeField] float _detectedStand = 1;
    [SerializeField] float _detectedCrouch = 3;
    [SerializeField] float _detectedValue = 1;
    bool _hasTimerStarted = false;

    private void Start()
    {
        _playerGO = GameObject.FindGameObjectWithTag("Player");
        _charLocomotion = _playerGO.GetComponent<CharacterLocomotion>();
    }

    void Update()
    {
        FindVisibleTargets();

        if (_visibleTargets.Count > 0)
        {
            _playerGO.GetComponent<PlayerController>().IsDetected = true;
            //Debug.Log("Enemy in fov");
            _isEnemyinFOV = true;
        }
        else
        {
            _playerGO.GetComponent<PlayerController>().IsDetected = false;
            //Debug.Log("Enemy not in fov");
            _isEnemyinFOV = false;
        }

        if(_hasTimerStarted)
        {
            //caps the timer to 5 seconds
            _detectionTimer = Mathf.Min(_detectionTimer + Time.deltaTime, 5.0f);
        }
        else
        {
            _detectionTimer = Mathf.Max(_detectionTimer -= Time.deltaTime, 0.0f);
        }

    }
    
    void FindVisibleTargets()
    {
        _visibleTargets.Clear();
        Collider[] TargetsInRadius = Physics.OverlapSphere(transform.position, _viewRadius / 2, _targetMask);
        foreach (Collider target in TargetsInRadius)
        {
            //Debug.Log(target.transform.name);
            Transform targetTransform = target.transform;
            Vector3 targetDirection = (targetTransform.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, targetDirection) < _viewAngle / 2)
            {
                float DistanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                RaycastHit hitInfo;
         
                if (Physics.Raycast((transform.position + Vector3.up), targetDirection, out hitInfo, _viewRadius / 2, _targetMask))
                {
                    Debug.DrawRay((transform.position + Vector3.up), (targetDirection * DistanceToTarget), Color.green);
                    if (hitInfo.collider.tag == "Player")
                    {
                        if(_charLocomotion.IsCrouching)
                        {
                            _detectedValue = _detectedCrouch;
                        }
                        else
                        {
                            _detectedValue = _detectedStand;
                        }


                        if(DistanceToTarget <= (_viewRadius / 8))
                        {
                            //Debug.Log("Player is too close");
                            _visibleTargets.Add(targetTransform);
                            FindObjectOfType<LastKnownLocation>().transform.position = hitInfo.point;
                            return;
                        }

                        _hasTimerStarted = true;

                        if (_detectionTimer > _detectedValue)
                        {
                            //Debug.Log("Player is detected through time");
                            _visibleTargets.Add(targetTransform);

                            FindObjectOfType<LastKnownLocation>().transform.position = hitInfo.point;
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
        if (!angleisglobal)
        {
            angleindegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleindegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleindegrees * Mathf.Deg2Rad));
    }
}

