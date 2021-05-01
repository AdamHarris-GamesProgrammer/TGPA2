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

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
        }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                uiContainer.SetActive(!uiContainer.activeSelf);

                if (!uiContainer.active)
                {
                    //showUI.Invoke();
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    //closeUI.Invoke();
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }

            }
        }
    }
}