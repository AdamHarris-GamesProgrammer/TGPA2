using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace TGP.Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] Texture2D _mouseCursor;

        public GameObject _followTarget;

        public float rotationPower = 3f;
        public float rotationLerp = 0.5f;

        public Quaternion nextRotation;
        public Vector3 nextPosition;

        public float speed = 5.4f;

        public float _aimValue;

        private Vector2 _look;
        private Vector2 _move;

        public GameObject aimCam;
        public GameObject mainCam;

        void Awake()
        {
            //Cursor.SetCursor(_mouseCursor, Vector2.zero, CursorMode.Auto);
            GetComponent<NavMeshAgent>().speed = 5.4f;
        }

        public void OnAim(InputValue value)
        {
            _aimValue = value.Get<float>();
            
        }

        public void OnLook(InputValue value)
        {
            _look = value.Get<Vector2>();
        }

        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        }

        void Update()
        {
            //Debug.Log(_aimValue);

            

            if (Mouse.current.rightButton.isPressed && !aimCam.activeInHierarchy)
            {
                mainCam.SetActive(false);
                aimCam.SetActive(true);

                //TODO: Show reticle
                GetComponent<Animator>().SetBool("isAiming", true);

            }else if(!Mouse.current.rightButton.isPressed && !mainCam.activeInHierarchy)
            {
                mainCam.SetActive(true);
                aimCam.SetActive(false);

                //TODO: Disable reticle
                GetComponent<Animator>().SetBool("isAiming", false);
            }

            _followTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);


            _followTarget.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

            var angles = _followTarget.transform.localEulerAngles;
            angles.z = 0;

            var angle = _followTarget.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            _followTarget.transform.localEulerAngles = angles;

            nextRotation = Quaternion.Lerp(_followTarget.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);


            if(_move == Vector2.zero)
            {
                nextPosition = transform.position;

            }
            else
            {
                float moveSpeed = speed / 100f;
                Vector3 position = (transform.forward * _move.y * speed) + (transform.right * _move.x * speed);
                nextPosition = transform.position + position;

                transform.rotation = Quaternion.Euler(0, _followTarget.transform.rotation.eulerAngles.y, 0);

                _followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

            }


            //transform.position = nextPosition;

            GetComponent<NavMeshAgent>().destination = nextPosition;

            GetComponent<Animator>().SetFloat("movementSpeed", GetComponent<NavMeshAgent>().velocity.magnitude);
        }


    }
}

