using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject aimCam;
    [SerializeField] Texture2D crosshair;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;

    bool isAiming = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(crosshair, new Vector2(crosshair.width / 2, crosshair.height/2), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && !isAiming)
        {
            mainCam.SetActive(false);
            aimCam.SetActive(true);
            Cursor.visible = true;
            GetComponent<Animator>().SetBool("isAiming", true);
            isAiming = true;
            //TODO: Aiming logic

        }
        else if(!Input.GetMouseButton(1) && isAiming)
        {
            isAiming = false;
            Cursor.visible = false;
            mainCam.SetActive(true);
            aimCam.SetActive(false);
            GetComponent<Animator>().SetBool("isAiming", false);
        }

        if (isAiming)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //TODO: Shoot projectile
                Instantiate(_bulletPrefab, _bulletSpawnLocation.position, transform.rotation);
            }
        }

    }
}
