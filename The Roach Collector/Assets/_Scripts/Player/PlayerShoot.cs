using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float damage = 10.0f;
    public float range = 100.0f;

    public Camera shootCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootRayCast();
        }
    }

    public void ShootRayCast()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(shootCam.transform.position, shootCam.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
        }

        TestShoot testShoot = hitInfo.transform.GetComponent<TestShoot>();
        if (testShoot != null)
        {
            testShoot.TakeDamage(damage);
        }
    }
}
