using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerAim : MonoBehaviour
{
    [Header("Camera Properties")]
    [Tooltip("The follow camera for the player")]
    [SerializeField] GameObject _mainCam;
    [Tooltip("The aiming camera for the player")]
    [SerializeField] GameObject _aimCam;
    [Header("GUI Properties")]
    [Tooltip("The Crosshair Gameobject from the HUD that will be displayed in the center of the screen")]
    [SerializeField] GameObject _crosshairTexture;

    [Header("General Aim Settings")]
    [Tooltip("This stores the layer that the player will look for in the direction checks.")]
    [SerializeField] private LayerMask _aimLayerMask;

    [Header("Bullet Properties")]
    [Tooltip("The bullet object that will spawn from the gun")]
    [SerializeField] private GameObject _bulletPrefab;
    [Tooltip("The spawn location for bullets, this should be the end of the gun")]
    [SerializeField] private Transform _bulletSpawnLocation;

    [Header("Follow Properties")]
    [SerializeField] private Transform _follow;

    [Header("Aiming Properties")]
    [SerializeField] private float _aimRotationSpeedFactor = 0.2f;

    //Stores whether the player is aiming or not
    bool _isAiming = false;

    //Stores a reference to the animator controller for the player
    private Animator _animator;

    //Stores a reference to the free look follow camera from the player
    CinemachineFreeLook _freeLook;

    /// <summary>
    /// Returns if the player is aiming or not
    /// </summary>
    public bool GetAiming() { return _isAiming; }

    private Camera _camera;

    void Awake()
    {
        //Sets the cursor up
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Sets the crosshair texture to not show
        _crosshairTexture.SetActive(false);

        //gets the animator component
        _animator = GetComponent<Animator>();

        //Stores a reference to the main camera 
        _camera = Camera.main;

        //Gets the freelook component from the main camera
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
            _crosshairTexture.SetActive(true);

        }
        //Off Aim
        else if (!Input.GetMouseButton(1) && _isAiming)
        {
            _isAiming = false;
            _mainCam.SetActive(true);
            _aimCam.SetActive(false);
            GetComponent<Animator>().SetBool("isAiming", false);
            _crosshairTexture.SetActive(false);

            //Set the follow cam to a suitable angle
            _freeLook.m_XAxis.Value = -1.25f;
            _freeLook.m_YAxis.Value = 0.4f;
        }

        //Aim camera controls
        if (_isAiming)
        {
            //Get the amount the mouse has changed in the last frame
            Vector2 mouseDelta = Vector2.zero;
            mouseDelta.x = Input.GetAxisRaw("Mouse X");
            mouseDelta.y = Input.GetAxisRaw("Mouse Y");

            //invert y axis
            mouseDelta.y = -mouseDelta.y;

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


            //Sets the follow targets euler angles
            _follow.localEulerAngles = angles;

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
            //if we are not aiming then adjust our facing direction as aiming needs to be done differently
            if (!_isAiming)
            {
                //Sends a ray from the center of the screen
                Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                //if we have hit something
                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
                {
                    //Gets the hit location
                    var destination = hitInfo.point;
                    destination.y = transform.position.y;

                    //calculates the direction vector
                    var direction = destination - transform.position;
                    direction.y = 0f;
                    direction.Normalize();
                    transform.rotation = Quaternion.LookRotation(direction, transform.up);
                }
            }
        }
    }
}