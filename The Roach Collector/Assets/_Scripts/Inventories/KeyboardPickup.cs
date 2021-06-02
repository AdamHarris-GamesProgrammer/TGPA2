using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harris.Inventories
{
    [RequireComponent(typeof(Pickup))]
    public class KeyboardPickup : MonoBehaviour
    {
        Pickup pickup;

        [SerializeField] KeyCode _pickupKey = KeyCode.E;

        bool _inRange = false;

        public void Interact()
        {
            //Check if we are pressing the pickup key
            if (Input.GetKeyDown(_pickupKey))
            {
                if (pickup.CanBePickedUp()) pickup.PickupItem();
            }
        }

        void Awake()
        {
            //Get the pickup component
            pickup = GetComponent<Pickup>();
        }

        private void Update()
        {
            if (_inRange) Interact();
        }

        private void OnTriggerEnter(Collider other)
        {
            //The player is in range
            if (other.CompareTag("Player")) _inRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            //the player is no longer in range
            if (other.CompareTag("Player")) _inRange = false;
        }
    }
}

