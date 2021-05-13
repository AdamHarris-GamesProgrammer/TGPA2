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
                    //Unfreeze time
                    Time.timeScale = 1;
                    //Locks cursor to center of screen
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    //freezes time
                    Time.timeScale = 0;
                    //confines the cursor to the screen
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }

            }
        }
    }
}