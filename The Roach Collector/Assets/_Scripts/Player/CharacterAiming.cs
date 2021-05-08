using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float _turnSpeed = 15.0f;
    public float _aimDuration = 0.3f;

    Camera _mainCamera;

    PlayerController _controller;

    [SerializeField] GameObject _followCam;
    [SerializeField] GameObject _aimCam;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;

        if (_controller.InKillAnimation) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), _turnSpeed * Time.fixedDeltaTime);

        if (Input.GetMouseButton(1))
        {
            _aimCam.SetActive(true);
            _followCam.SetActive(false);
        }
        else
        {
            _aimCam.SetActive(false);
            _followCam.SetActive(true);
        }
    }

    private void LateUpdate()
    {
    }
}
