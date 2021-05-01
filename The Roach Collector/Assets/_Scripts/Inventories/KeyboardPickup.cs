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
            if (Input.GetKeyDown(_pickupKey))
            {
                if (pickup.CanBePickedUp())
                {
                    pickup.PickupItem();
                }
            }
        }

        void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        private void Update()
        {
            if(_inRange)
            {
                Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _inRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _inRange = false;
            }
        }
    }
}

