using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterLocomotion : MonoBehaviour
{
    

    [Min(0f)][SerializeField] private float _jumpHeight;
    [Min(0f)][SerializeField] private float _gravity;
    [SerializeField] private float _stepDown;
    [SerializeField] private float _jumpDamping;
    [SerializeField] private float _playerSpeed = 1.0f;
    [SerializeField] private float _pushPower = 2.0F;
    [SerializeField] private float _airControl;
    [SerializeField] Rig _kneeLayer;


    bool _isJumping;
    bool _isCrouching = false;

    Vector2 _input;
    Vector3 _velocity;
    Vector3 _rootMotion;

    CharacterController _controller;
    Animator _animator;


    #region UNITY MESSAGES
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Disables crouching if we are crouching enables crouching if I am standing
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouching = !_isCrouching;

            _controller.height = _isCrouching ? 0.9f : 1.6f;
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

        //Handles our root motion
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        //Sets our animator values
        _animator.SetFloat("InputX", _input.x);
        _animator.SetFloat("InputY", _input.y);

        //Checks to see if we are jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        if (_isJumping) //In Air state
        {
            UpdateInAir();

        }
        else //Is grounded state
        {
            UpdateOnGround();

        }
    }

    private void OnAnimatorMove()
    {
        //Accumulates our root motion this frame
        _rootMotion += _animator.deltaPosition;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * _pushPower;
    }

    #endregion

    #region PRIVATE METHODS

    private void UpdateInAir()
    {
        //Calculate gravity's factor on our velocity
        _velocity.y -= _gravity * Time.fixedDeltaTime;

        //Calculate displacement 
        Vector3 displacement = _velocity * Time.fixedDeltaTime;

        //Add in our air control (if any)
        displacement += CalculateAirControl();

        //Move our character by the displacement
        _controller.Move(displacement);

        //Checks if we are jumping by seeing if we are grounded
        _isJumping = !_controller.isGrounded;

        //Sets the anim value
        _animator.SetBool("isJumping", _isJumping);

        //Resets our root motion
        _rootMotion = Vector3.zero;
    }

    private void UpdateOnGround()
    {
        //Moving along X and Z
        Vector3 stepForward = _rootMotion * _playerSpeed;
        Vector3 stepDown = Vector3.down * _stepDown;

        _controller.Move(stepForward + stepDown);


        //To remove the one frame glitch check if we are no longer grounded here and then step back up
        if (!_controller.isGrounded)
        {
            _controller.Move(Vector3.up * _stepDown);
        }



        _rootMotion = Vector3.zero;

        //Stepped off edge
        if (!_controller.isGrounded)
        {
            InheritVelocity(0.0f);
        }
    }



    private void InheritVelocity(float jumpVelocity)
    {
        _isJumping = true;
        _animator.SetBool("isJumping", true);
        _velocity = _animator.velocity * _jumpDamping * _playerSpeed;
        _velocity.y = jumpVelocity;
    }

    void Jump()
    {
        if(!_isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * _gravity * _jumpHeight);
            InheritVelocity(jumpVelocity);
        }
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * _input.y) + (transform.right * _input.x)) *(_airControl / 100);
    }

    #endregion


}
