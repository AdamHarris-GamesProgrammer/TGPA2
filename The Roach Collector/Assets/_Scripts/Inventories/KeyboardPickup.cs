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

        public void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
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

        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Interact();
            }
        }
    }
}

