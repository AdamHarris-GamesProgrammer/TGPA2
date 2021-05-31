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

        [SerializeField] AudioClip _openBackpackSound;
        [SerializeField] AudioClip _closeBackpackSound;

        PlayerHealth _playerHealth;

        bool _isDisabled = false;
        public bool Disabled { get { return _isDisabled; } set { _isDisabled = value; } }

        public GameObject UIContainer {  get { return _uiContainer; } }
        public GameObject UIGuideContainer;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (_isDisabled) return;

            if (Input.GetKeyDown(_toggleKey))
            {
                _uiContainer.SetActive(!_uiContainer.activeSelf);
                if(UIGuideContainer != null)
                {
                    UIGuideContainer.SetActive(!_uiContainer.activeSelf);
                }
                

                if (!_uiContainer.activeSelf)
                {
                    //Unfreeze time
                    Time.timeScale = 1;
                    //Locks cursor to center of screen
                    Cursor.lockState = CursorLockMode.Locked;
                    //Debug.Log("Show Hide UI cursor");
                    Cursor.visible = false;

                    GetComponentInParent<AudioSource>().PlayOneShot(_closeBackpackSound);
                }
                else
                {
                    //freezes time
                    Time.timeScale = 0;
                    //confines the cursor to the screen
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;

                    GetComponentInParent<AudioSource>().PlayOneShot(_openBackpackSound);
                }

            }
        }
    }
}