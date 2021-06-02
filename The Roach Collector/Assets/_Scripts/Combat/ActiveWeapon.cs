using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;
using TGP.Control;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

[RequireComponent(typeof(Inventory))]
public class ActiveWeapon : MonoBehaviour
{

    [SerializeField] Transform _crosshairTarget;

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weaponParent = null;
    [SerializeField] private Transform _weaponParentMelee = null;

    [SerializeField] private Transform _weaponLeftGrip = null;
    [SerializeField] private Transform _weaponRightGrip = null;
    [SerializeField] private RaycastWeapon _startingWeapon = null;

    [SerializeField] private GameObject InventoryCanvas = null;

    [SerializeField] public PlayerUI _PlayerUI = null;
    [SerializeField] RaycastWeapon _MeleeWeapon;

    Inventory _inventory;
    PlayerController _controller;

    Animator _anim;
    private RaycastWeapon _weapon = null;

    [SerializeField] private bool _isMelee = true;
    bool _isStabbing = false;

    private void Awake()
    {
        _PlayerUI = GetComponent<PlayerUI>();
        _controller = GetComponent<PlayerController>();
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

            _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
        }
    }

    private void Reload()
    {
        //Stops the player from reloading with a full mag
        if (_weapon.ClipAmmo == _weapon.Config.ClipSize) return;

        _weapon.Reload();

        _anim.SetBool("isReloading", true);
        _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        //if we have a weapon
        if (_weapon)
        {
            //If the weapon is not melee and we have the type of ammo we need
            if (!_weapon.IsMelee && _inventory.HasItem(_weapon.Config.AmmoType))
            {
                //Find the slot of the ammo
                int index = _inventory.FindItem(_weapon.Config.AmmoType);
                //Set the ammo to the number in the slot
                _weapon.TotalAmmo = _inventory.GetNumberInSlot(index);
                //Update the UI
                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);

            }

            //If the weapon is a melee weapon use the melee logic method if not use the gun logic
            if (_isMelee) MeleeLogic();
            else GunLogic();

        }
        //if we don't have a weapon
        else _PlayerUI.UpdateAmmoUI(0, 0, 0);
    }

    public void DropWeapon()
    {
        //Drop the current weapon onto the floor
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
        //if we currently have a weapon
        if (_weapon)
        {
            if (!_weapon.IsMelee)
            {
                //Check if we have the ammo
                if (_inventory.HasItem(_weapon.Config.AmmoType))
                {
                    //Find the slot that the weapon is in, add the weapon to the inventory
                    int index = _inventory.FindItem(_weapon.Config.AmmoType);
                    _inventory.AddItemToSlot(index, _weapon.Config.AmmoType, _weapon.ClipAmmo);
                }
                //if we do not have the ammo in our inventory then add it to the first empty slot
                else _inventory.AddToFirstEmptySlot(_weapon.Config.AmmoType, _weapon.ClipAmmo);

                //Destroy the weapon we have
            }
            Destroy(_weapon.gameObject);
        }

        //Set our new weapon
        _weapon = newWeapon;

        if (_weapon)
        {
            if (_weapon.IsMelee) _weapon.transform.parent = _weaponParentMelee;
            else _weapon.transform.parent = _weaponParent;

            //Sets it to origin
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;

            _weapon.Setup();
        }

        //if the weapon is not melee and we have the ammo item
        if (!_weapon.IsMelee && _inventory.HasItem(_weapon.Config.AmmoType))
        {
            //Display the total ammo of the weapon
            int index = _inventory.FindItem(_weapon.Config.AmmoType);
            _weapon.TotalAmmo = _inventory.GetNumberInSlot(index);
            _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
        }
        else
        {
            //We have no ammo, so set the UI accordingly
            _weapon.TotalAmmo = 0;
            _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
        }

        //Set our melee bool
        _isMelee = _weapon.IsMelee;
    }

    void GunLogic()
    {
        //Weapon logic for automatic vs non automatic weapons is different
        if (_weapon.Config.IsAutomatic)
        {
            //Starts and stops firing based on the FIre1 button
            if (Input.GetButtonDown("Fire1"))
            {
                _weapon.StartFiring();
                _controller.IsShooting = true;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                _weapon.StopFiring();
                _controller.IsShooting = false;
            }

            //Updates the weapon and the ammo UI if we are shooting
            if (_weapon.IsFiring)
            {
                _weapon.UpdateWeapon(_crosshairTarget.position);
                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
            }
        }
        else
        {
            //Shoot
            if (Input.GetButtonDown("Fire1"))
            {
                //Start firing
                _weapon.StartFiring();
                _controller.IsShooting = true;
                //Update the weapon (shoot)
                _weapon.UpdateWeapon(_crosshairTarget.position);
                //Updates the player UI
                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
            }
            else
            {
                //Stop shooting
                _controller.IsShooting = false;
                if(_weapon.IsFiring) _weapon.StopFiring();
            }

        }

        //Update any bullets from the weapon
        _weapon.UpdateBullets();

        //Reload the gun if we can
        if (Input.GetKeyDown(KeyCode.R) && _weapon.TotalAmmo > 0)
        {
            Reload();
            _controller.IsShooting = false;
        }

        ///If we have no more bullets in the mag, then stop shooting
        if (_weapon.NeedToReload) _controller.IsShooting = false;

        //If reloading, update the UI
        if (_weapon.IsReloading) _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
    }

    void MeleeLogic()
    {
        //if LMB is down, and the inventory is not active, and we are not stabbing, then stab
        if (Input.GetButtonDown("Fire1") && !InventoryCanvas.activeSelf && _isStabbing == false)
        {
            _isStabbing = true;
            _anim.SetTrigger("Stab");
        }
    }

    void Stabbing()
    {
        _isStabbing = true;
        GetComponent<WeaponStabCheck>().SetStabbing(true);
    }

    void NotStabbing()
    {
        _isStabbing = false;
        GetComponent<WeaponStabCheck>().SetStabbing(false);
    }
}
