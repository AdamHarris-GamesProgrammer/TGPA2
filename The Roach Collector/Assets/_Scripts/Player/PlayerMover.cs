using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Movement;

namespace TGP.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Crouch Settings")]
        [SerializeField] private float _verticalOffet = -0.4f;
        [Range(0.01f, 1.0f)][SerializeField] private float _crouchSpeedMultiplier = 0.5f;


        private void Awake()
        {
        }

        void FixedUpdate()
        {
            //Gets the horizontal and vertical movement
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input == Vector2.zero)
            {
                GetComponent<Animator>().SetFloat("movementSpeed", 0.0f);
            }

            //Calculates the movement vector
            //Vector3 move = transform.right * input.x + transform.forward * input.y;
            Vector3 move = new Vector3(input.x, 0.0f, input.y);



            Vector3 position = transform.position + (move * 5.4f * Time.deltaTime);

            transform.position = position;

            

            GetComponent<Animator>().SetFloat("movementSpeed", move.magnitude * 6.0f);

        }
    }
}

