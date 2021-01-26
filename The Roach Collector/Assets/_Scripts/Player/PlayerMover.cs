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
            float xMovement = Input.GetAxis("Horizontal");
            float yMovement = Input.GetAxis("Vertical");

            //Gets the current position
            Vector3 currentPosition = transform.position;

            //Adds the movement to the current position
            currentPosition += new Vector3(xMovement,0.0f, yMovement) * Time.deltaTime * _movementSpeed;

            Debug.Log(string.Format("Movement ({0},{1})", xMovement, yMovement));

            //Sets the new position
            transform.position = currentPosition;
        }
    }
}

