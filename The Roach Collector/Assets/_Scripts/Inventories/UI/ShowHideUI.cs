using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harris.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode _toggleKey = KeyCode.Escape;
        [SerializeField] GameObject _uiContainer = null;

        public GameObject UIContainer {  get { return _uiContainer; } }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                _uiContainer.SetActive(!_uiContainer.activeSelf);

                if (!_uiContainer.activeSelf)
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }

            }
        }
    }
}