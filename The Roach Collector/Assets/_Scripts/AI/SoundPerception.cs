using System.Collections;
using System.Collections.Generic;
using TGP.Control;
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

    AIHealth _aiHealth;

    Transform _player;


    void Awake()
    {
        _aiHealth = GetComponentInParent<AIHealth>();
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        if (_aiHealth.IsDead) Destroy(this);

        if(_detectionTimer >= _detectionDuration)
        {
            _isHeard = true;
            FindObjectOfType<LastKnownLocation>().transform.position = _player.position;
        }
        else _isHeard = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Start the timer
        if (other.CompareTag("PlayerSound")) _detectionTimer = 0.0f;
    }

    private void OnTriggerExit(Collider other)
    {
        //Decrement the timer
        if (other.CompareTag("PlayerSound")) _detectionTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("PlayerSound"))
        {
            //Gets the sound controller of the player
            SoundController soundController = other.GetComponent<SoundController>();

            //Adds the appropriate value to the timer based on what the player is doing
            if (soundController.IsShooting) 
                _detectionTimer += Time.deltaTime * _shootingMultiplier;
            else if (soundController.IsStanding)
                _detectionTimer += Time.deltaTime * _standingMultiplier;
            else if (!soundController.IsStanding)
                _detectionTimer += Time.deltaTime * _crouchingMultiplier;
        }
    }

}
