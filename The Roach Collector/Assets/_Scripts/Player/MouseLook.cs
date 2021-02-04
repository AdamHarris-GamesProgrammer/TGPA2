using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Player
{
    public class MouseLook : MonoBehaviour
    {
        [Header("Mouse Settings")]
        [SerializeField] float _mouseSensitivity = 100.0f;

        float _xRotation = 0.0f;

        private Transform _playerBody;

        private void Awake()
        {
            _playerBody = transform.parent.GetComponent<Transform>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            _playerBody.Rotate(Vector3.up * mouseX);

            _xRotation -= mouseY;

            _xRotation = Mathf.Clamp(_xRotation, -89.9f, 89.9f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0.0f, 0.0f);
        }

    }
}

