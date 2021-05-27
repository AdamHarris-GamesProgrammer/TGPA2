using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPerception : MonoBehaviour
{
    bool _isHeard = false;
    public bool IsHeard { get { return _isHeard; } }

    [SerializeField] float _detectionDuration = 2.0f;
    [SerializeField] float _standingMultiplier = 1.0f;
    [SerializeField] float _shootingMultiplier = 1000.0f;
    [SerializeField] float _crouchingMultiplier = 0.5f;

    float _detectionTimer;

    Vector3 _contactPoint;

    private void FixedUpdate()
    {
        if(_detectionTimer >= _detectionDuration)
        {
            _isHeard = true;
            FindObjectOfType<LastKnownLocation>().transform.position = _contactPoint;
             Debug.Log("Detected through sound");
        }
        else
        {
            _isHeard = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerSound"))
        {
            _detectionTimer = 0.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerSound"))
        {
            _detectionTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("PlayerSound"))
        {
            SoundController soundController = other.GetComponent<SoundController>();

            if (soundController.IsShooting) 
            {
                _detectionTimer += Time.deltaTime * _shootingMultiplier;
            }
            else if (soundController.IsStanding)
            {
                _detectionTimer += Time.deltaTime * _standingMultiplier;
            }
            else if (!soundController.IsStanding)
            {
                _detectionTimer += Time.deltaTime * _crouchingMultiplier;
            }

            _contactPoint = other.ClosestPoint(transform.position);
        }
    }

}
