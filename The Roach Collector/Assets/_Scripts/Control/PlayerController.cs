using System.Collections;
using System.Collections.Generic;
using TGP.Movement;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;


    private Mover _mover;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _mover.SetCrouched(!_mover.GetCrouched());
        }

    }
}
