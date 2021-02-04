using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Camera
{
    public class CameraRenderer : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        void FixedUpdate()
        {
            Vector3 targetPosition = _target.position;
            Vector3 cameraPosition = transform.position;

            Vector3 direction = cameraPosition - targetPosition;
            direction.Normalize();

            RaycastHit hit;
            Physics.Raycast(transform.position, -direction, out hit, 35.0f);

            if(!hit.transform.CompareTag("Player"))
            {
                Color hitColor;

                MeshRenderer renderer = hit.transform.GetComponent<MeshRenderer>();

                hitColor = renderer.material.color;

                hitColor.a = 0.5f;
                renderer.material.color = hitColor;
            }

        }
    }
}


