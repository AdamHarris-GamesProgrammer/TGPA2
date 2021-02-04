using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Camera
{
    public class CameraRenderer : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private GameObject _lastGo;

        private void Awake()
        {
            _lastGo = GameObject.FindGameObjectWithTag("Player");
        }

        void FixedUpdate()
        {
            Vector3 direction = transform.position - _target.position;
            direction.Normalize();

            RaycastHit hit;
            Physics.Raycast(transform.position, -direction, out hit, 35.0f);



            if(hit.transform.gameObject.GetInstanceID() != _lastGo.GetInstanceID())
            {
                if (!hit.transform.CompareTag("Player"))
                {
                    //Create a color variable so we can edit the material color
                    Color hitColor;

                    //Get the mesh renderer from the hit object
                    MeshRenderer renderer = hit.transform.GetComponent<MeshRenderer>();

                    //Set the hitColor variable to the hit objects colour
                    hitColor = renderer.material.color;

                    //Set the alpha value to .5
                    hitColor.a = 0.5f;

                    //Set the materials colour to our modified colour
                    renderer.material.color = hitColor;
                }

                MeshRenderer render = _lastGo.GetComponent<MeshRenderer>();

                if (render)
                {
                    Color col = render.material.color;
                    col.a = 1.0f;
                    render.material.color = col;

                }

                _lastGo = hit.transform.gameObject;
            }



        }
    }
}


