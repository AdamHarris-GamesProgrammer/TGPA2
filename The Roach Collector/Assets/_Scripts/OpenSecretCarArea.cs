using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Control;

public class OpenSecretCarArea : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.name == "Core")
        {
            if (Player.GetComponent<PlayerController>().IsDancing)
            {
                transform.Rotate(new Vector3(transform.rotation.eulerAngles.x, (transform.rotation.eulerAngles.y + 90), transform.rotation.eulerAngles.z));
            }
        }
    }
}
