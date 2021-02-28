using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerAim : MonoBehaviour
{
    [SerializeField] GameObject _mainCam;
    [SerializeField] GameObject _aimCam;
    [SerializeField] Texture2D _crosshairTexture;

    [SerializeField] private LayerMask _aimLayerMask;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;

    [SerializeField] private Transform _follow;

    private Animator _animator;


    [SerializeField] private float _aimRotationSpeedFactor = 0.2f;

    bool _isAiming = false;


    CinemachineFreeLook _freeLook;

    public bool GetAiming() { return _isAiming; }

    private Camera _camera;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        _animator = GetComponent<Animator>();
        _camera = Camera.main;

        _freeLook = _mainCam.GetComponent<CinemachineFreeLook>();

    }

    // Update is called once per frame
    void Update()
    {
        //On Aim
        if (Input.GetMouseButton(1) && !_isAiming)
        {
            _mainCam.SetActive(false);
            _aimCam.SetActive(true);
            GetComponent<Animator>().SetBool("isAiming", true);
            _isAiming = true;
            //TODO: Aiming logic

        }
        //Off Aim
        else if (!Input.GetMouseButton(1) && _isAiming)
        {
            _isAiming = false;
            _mainCam.SetActive(true);
            _aimCam.SetActive(false);
            GetComponent<Animator>().SetBool("isAiming", false);

            //Set the follow cam to a suitable angle
            _freeLook.m_XAxis.Value = -1.25f;
            _freeLook.m_YAxis.Value = 0.4f;
        }

        if (_isAiming)
        {
            //Get the amount the mouse has changed in the last frame
            Vector2 mouseDelta = Vector2.zero;
            mouseDelta.x = Input.GetAxisRaw("Mouse X");
            mouseDelta.y = Input.GetAxisRaw("Mouse Y");

            //invert y axis
            mouseDelta.y = -mouseDelta.y;


            //Rotate the X axis

                //Rotate the Y axis

            if (mouseDelta.x < .2f && mouseDelta.x > -0.2f)
            {
                mouseDelta.x = 0.0f;
            }

            Debug.Log("Mouse Delta: " + mouseDelta);

           

            if (mouseDelta.x != 0.0f)
            {
                //Rotate Vertically
                _follow.rotation *= Quaternion.AngleAxis(mouseDelta.y * _aimRotationSpeedFactor, Vector3.right);
                
                //Rotate Horizontally
                _follow.rotation *= Quaternion.AngleAxis(mouseDelta.x * _aimRotationSpeedFactor, Vector3.up);

                

                //Get the angles of the follow target
                var angles = _follow.localEulerAngles;
                angles.z = 0;

                //get the x angle
                var angle = _follow.localEulerAngles.x;

                //Clamp it
                if (angle > 180 && angle < 340)
                {
                    angles.x = 340;
                }
                else if (angle < 180 && angle > 40)
                {
                    angles.x = 40.0f;
                }


                _follow.localEulerAngles = angles;
            }

            //Aim the player in the direction of the follow target
            transform.rotation = Quaternion.Euler(0, _follow.rotation.eulerAngles.y, 0);
            _follow.localEulerAngles = new Vector3(_follow.localEulerAngles.x, 0, 0);


            //shoot
            if (Input.GetMouseButtonDown(0))
            {
                //TODO: Shoot projectile from correct position
                Instantiate(_bulletPrefab, _bulletSpawnLocation.position, Camera.main.transform.rotation);
            }
        }

    }

    private void FixedUpdate()
    {
        //Checks we are moving in some way
        if (_animator.GetFloat("velocityX") != 0.0f || _animator.GetFloat("velocityZ") != 0.0f)
        {
            //if we are not aiming then adjust our facing direction
            if (!_isAiming)
            {
                Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
                {
                    var destination = hitInfo.point;
                    destination.y = transform.position.y;

                    var _direction = destination - transform.position;
                    _direction.y = 0f;
                    _direction.Normalize();
                    transform.rotation = Quaternion.LookRotation(_direction, transform.up);
                }
            }
        }
    }
}