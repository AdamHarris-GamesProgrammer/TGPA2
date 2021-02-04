using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Movement;

namespace TGP.Player
{
    public class PlayerMover : MonoBehaviour
    {
        Mover _mover;

        [Header("Crouch Settings")]
        [SerializeField] private float _verticalOffet = -0.4f;
        [Range(0.01f, 1.0f)][SerializeField] private float _crouchSpeedMultiplier = 0.5f;


        private void Awake()
        {
            _mover = GetComponent<Mover>();
        }

        void Update()
        {
            //Gets the horizontal and vertical movement
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //Calculates the movement vector
            //Vector3 move = transform.right * input.x + transform.forward * input.y;
            Vector3 move = new Vector3(input.x, 0.0f, input.y);

            Vector3 position = transform.position + move;

            _mover.MoveTo(position);
        }
    }
}

