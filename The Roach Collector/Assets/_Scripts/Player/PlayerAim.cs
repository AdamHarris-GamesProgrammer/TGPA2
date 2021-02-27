using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public bool GetAiming() { return _isAiming; }

    private Camera _camera;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        Debug.Log(mouseDelta);

        _follow.rotation *= Quaternion.AngleAxis(mouseDelta.x * _aimRotationSpeedFactor, Vector3.up);

        _follow.rotation *= Quaternion.AngleAxis(mouseDelta.y * _aimRotationSpeedFactor, Vector3.right);

        var angles = _follow.localEulerAngles;
        angles.z = 0;

        var angle = _follow.localEulerAngles.x;

        if(angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40)
        {
            angles.x = 40.0f;
        }

        _follow.localEulerAngles = angles;

        //Debug.Log("Angles: " + angles);


        if (Input.GetMouseButton(1) && !_isAiming)
        {
            _mainCam.SetActive(false);
            _aimCam.SetActive(true);
            Cursor.visible = true;
            GetComponent<Animator>().SetBool("isAiming", true);
            _isAiming = true;
            //TODO: Aiming logic

        }
        else if (!Input.GetMouseButton(1) && _isAiming)
        {
            _isAiming = false;
            Cursor.visible = false;
            _mainCam.SetActive(true);
            _aimCam.SetActive(false);
            GetComponent<Animator>().SetBool("isAiming", false);
        }

        if (_isAiming)
        {
            _follow.localEulerAngles = new Vector3(angles.x, angles.y, 0);

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
        if(_animator.GetFloat("velocityX") != 0.0f || _animator.GetFloat("velocityZ") != 0.0f)
        {
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