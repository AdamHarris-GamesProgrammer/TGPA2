using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] Texture2D _mouseCursor;

        void Awake()
        {
            Cursor.SetCursor(_mouseCursor, Vector2.zero, CursorMode.Auto);
        }

        void Update()
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            float hitDist = 0.0f;

            if(playerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);

                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
            }
        }

    }
}

