using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harris.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;

        public GameObject UIContainer {  get { return uiContainer; } }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                uiContainer.SetActive(!uiContainer.activeSelf);

                if (!uiContainer.activeSelf)
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