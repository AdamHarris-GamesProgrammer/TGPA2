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

    private Animator _animator;

    bool _isAiming = false;

    public bool GetAiming() { return _isAiming; }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(_crosshairTexture, new Vector2(_crosshairTexture.width / 2, _crosshairTexture.height / 2), CursorMode.Auto);

        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
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
