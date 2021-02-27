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


    Vector2 _look;

    bool _isAiming = false;


    public bool GetAiming() { return _isAiming; }

    private Camera _camera;

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.SetCursor(_crosshairTexture, new Vector2(_crosshairTexture.width / 2, _crosshairTexture.height / 2), CursorMode.Auto);

        _animator = GetComponent<Animator>();
        _camera = Camera.main;

        _look = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _look;
        _look = Input.mousePosition;

        Debug.Log(mouseDelta);

        _follow.rotation *= Quaternion.AngleAxis(mouseDelta.x * 1.0f, Vector3.up);

        _follow.rotation *= Quaternion.AngleAxis(mouseDelta.y * 1.0f, Vector3.right);

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
            _follow.localEulerAngles = new Vector3(angles.x, 0, 0);

            if (Input.GetMouseButtonDown(0))
            {
                //TODO: Shoot projectile from correct position
                Instantiate(_bulletPrefab, _bulletSpawnLocation.position, transform.rotation);
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
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
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
