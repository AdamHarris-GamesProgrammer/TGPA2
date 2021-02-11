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

                    StartCoroutine(FadeOut(renderer));
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

        IEnumerator FadeOut(MeshRenderer renderer)
        {
            for(float ft = 1f; ft > 0.5f; ft -= 0.1f)
            {
                Color c = renderer.material.color;
                c.a = ft;
                renderer.material.color = c;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}


