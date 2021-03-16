using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float _turnSpeed = 15.0f;
    public float _aimDuration = 0.3f;
    public Rig aimLayer;

    Camera _mainCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), _turnSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        //if (Input.GetMouseButton(1))
        //{
        //    aimLayer.weight += Time.deltaTime / _aimDuration;
        //}
        //else
        //{
        //    aimLayer.weight -= Time.deltaTime / _aimDuration;
        //}
        aimLayer.weight = 1.0f;
    }
}
