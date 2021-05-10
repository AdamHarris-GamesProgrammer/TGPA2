using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
public class ActiveWeapon : MonoBehaviour
{

    public Transform _crosshairTarget;
    [Header("Animation Settings")]

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weaponParent = null;
    [SerializeField] private Transform _weaponLeftGrip = null;
    [SerializeField] private Transform _weaponRightGrip = null;
    [SerializeField] private RaycastWeapon _startingWeapon = null;
    PlayerUI _PlayerUI = null;

    Inventory _inventory;

    Animator _anim;
    RaycastWeapon _weapon;

    private void Awake()
    {
        _PlayerUI = GetComponent<PlayerUI>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _inventory = GetComponent<Inventory>();


        if (_startingWeapon)
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon);

            Equip(weapon);

            _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
        }
    }

    private void Reload()
    {
        //Stops the player from reloading with a full mag
        if (_weapon._clipAmmo == _weapon.Config.ClipSize) return;

        _weapon._isReloading = true;

        _anim.SetBool("isReloading", true);
        _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        if(_weapon)
        {
            if (_inventory)
            {
                if (_inventory.HasItem(_weapon._config.AmmoType))
                {
                    int index = _inventory.FindItem(_weapon._config.AmmoType);

                    _weapon._totalAmmo = _inventory.GetNumberInSlot(index);

                    _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
                }
            }


            if (Input.GetButtonDown("Fire1"))
            {
                _weapon.StartFiring();
            }

            if (_weapon.IsFiring())
            {
                _weapon.UpdateWeapon(Time.deltaTime, _crosshairTarget.position);
                _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
            }

            _weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                _weapon.StopFiring();
            }

            if (Input.GetKeyDown(KeyCode.R) && _weapon._totalAmmo > 0)
            {
                Reload();

            }


            if (_weapon._isReloading == false)
            {
                _anim.SetBool("isReloading", false);
            }
            else
            {
                _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
            }
        }
        else
        {
        }
    }

    public void DropWeapon()
    {
        if (_weapon)
        {
            _weapon.transform.SetParent(null);
            _weapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            _weapon.gameObject.AddComponent<Rigidbody>();
            _weapon = null;
        }
    }
    public void Equip(RaycastWeapon newWeapon)
    {
        if(_weapon)
        {
            Destroy(_weapon.gameObject);
        }

        _weapon = newWeapon;

        if (newWeapon)
        {
            _weapon.transform.parent = _weaponParent;
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;

            newWeapon.Setup();
        }

        if (_inventory)
        {
            if (_inventory.HasItem(_weapon._config.AmmoType))
            {
                int index = _inventory.FindItem(_weapon._config.AmmoType);

                _weapon._totalAmmo = _inventory.GetNumberInSlot(index);

                _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
            }
            else
            {
                _weapon._totalAmmo = 0;
                _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
            }
        }
    }
}
