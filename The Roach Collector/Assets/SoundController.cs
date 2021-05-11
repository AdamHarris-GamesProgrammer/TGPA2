using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] float _walkSoundRadius = 2.5f;
    [SerializeField] float _crouchSoundRadius = 1.5f;
    [SerializeField] float _gunSoundRadius = 20.0f;

    PlayerController _player;
    SphereCollider _collider;

    private float _defaultRadius;
    float _currentRadius;

    bool _isStanding = false;
    bool _isShooting = false;
    public bool IsShooting { get { return _isShooting; } }
    public bool IsStanding { get { return _isStanding; } }

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _defaultRadius = _collider.radius;
        _currentRadius = _defaultRadius;
        _player = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        _currentRadius = _collider.radius;
        _isShooting = _player.IsShooting;
        _isStanding = _player.IsStanding;

        if (_isShooting)
        {
            _currentRadius = Mathf.Lerp(_currentRadius, _gunSoundRadius, Time.deltaTime * 5.0f);
        }
        else if (_isStanding)
        {
            _currentRadius = Mathf.Lerp(_currentRadius, _walkSoundRadius, Time.deltaTime);
        }
        else if(!_isStanding)
        {
            _currentRadius = Mathf.Lerp(_currentRadius, _crouchSoundRadius, Time.deltaTime);
        }

        _collider.radius = _currentRadius;
    }
}
