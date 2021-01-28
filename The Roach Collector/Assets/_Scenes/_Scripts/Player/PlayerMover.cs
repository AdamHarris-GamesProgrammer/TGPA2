using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Min(0f)][SerializeField] float _movementSpeed = 2.0f;

        
        void FixedUpdate()
        {
            //Gets the horizontal and vertical movement
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (input == Vector2.zero) return;

            Vector3 movement = Camera.main.transform.forward * input.y + Camera.main.transform.right * input.x;

            movement += transform.position;

            //Sets the new position
            transform.position = movement;
        }
    }
}

