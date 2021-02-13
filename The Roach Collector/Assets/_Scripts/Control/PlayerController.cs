using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_bulletPrefab, _bulletSpawnLocation.position, Quaternion.LookRotation(transform.forward));
        }
    }
}
