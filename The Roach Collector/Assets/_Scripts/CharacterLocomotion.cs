using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterLocomotion : MonoBehaviour
{
    Animator _animator;
    Vector2 _input;

    bool _isCrouching = false;

    [SerializeField] Rig _kneeLayer;

    CapsuleCollider _playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Disables crouching if we are crouching enables crouching if I am standing
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouching = !_isCrouching;

            _playerCollider.height = _isCrouching ? 1.1f : 1.8f;
        }

        //Checks we are crouching
        if (_isCrouching)
        {
            _kneeLayer.weight += 0.2f;
        }
        else
        {
            _kneeLayer.weight -= 0.2f;
        }

        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        _animator.SetFloat("InputX", _input.x);
        _animator.SetFloat("InputY", _input.y);
    }
}
