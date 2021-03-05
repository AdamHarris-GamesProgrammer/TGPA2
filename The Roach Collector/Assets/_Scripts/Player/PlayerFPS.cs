using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Resources;
using TGP.Combat;

public class PlayerFPS : MonoBehaviour
{
    [Header("GUI Properties")]
    [Tooltip("The Crosshair Gameobject from the HUD that will be displayed in the center of the screen")]
    [SerializeField] GameObject _crosshairTexture;

    [Header("Aiming Sensitivity")]
    [SerializeField] float _mouseXSensitivity = 25.0f;
    [SerializeField] float _mouseYSensitivity = 25.0f;

    //Stores whether the player is aiming or not
    bool _isAiming = false;

    //Stores a reference to the animator controller for the player
    private Animator _animator;

    private Health _health;

    float xRotation = 0f;

    [SerializeField] Transform _gun;

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
        _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        //Stores a reference to the main camera 
        _camera = Camera.main;

        _health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        GameObject.FindGameObjectWithTag("Player").GetComponent<MeshSockets>().Attach(_gun, MeshSockets.SocketId.Spine);
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.IsDead())
        {
            Debug.Log("Player Dead");
            return;
        }

        //On Aim
        if (Input.GetMouseButton(1) && !_isAiming)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<MeshSockets>().Attach(_gun, MeshSockets.SocketId.RightHand);
            _animator.SetBool("isAiming", true);
            _isAiming = true;
            _crosshairTexture.SetActive(true);

        }
        //Off Aim
        else if (!Input.GetMouseButton(1) && _isAiming)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<MeshSockets>().Attach(_gun, MeshSockets.SocketId.Spine);
            _isAiming = false;
            _animator.SetBool("isAiming", false);
            _crosshairTexture.SetActive(false);

        }


        float mouseX = Input.GetAxis("Mouse X") * _mouseXSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseYSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 50f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        GameObject.FindGameObjectWithTag("Player").transform.Rotate(Vector3.up * mouseX);


        //shoot
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().Shoot();
        }

    }
}
