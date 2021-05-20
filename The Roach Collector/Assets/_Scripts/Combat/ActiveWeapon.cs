using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;
using TGP.Control;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
public class ActiveWeapon : MonoBehaviour
{

    public Transform _crosshairTarget;
    [Header("Animation Settings")]

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weaponParent = null;
    [SerializeField] private Transform _weaponParentMelee = null;
    
    [SerializeField] private Transform _weaponLeftGrip = null;
    [SerializeField] private Transform _weaponRightGrip = null;
    [SerializeField] private RaycastWeapon _startingWeapon = null;
    //PlayerUI _PlayerUI = null;
    [SerializeField] public PlayerUI _PlayerUI = null;
    public RaycastWeapon _MeleeWeapon;

    Inventory _inventory;
    PlayerController _controller;

    Animator _anim;
    [SerializeField] private RaycastWeapon _weapon;

    [SerializeField] private bool _isMelee = true;

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
        
        if(_weapon)
        {
            if (_inventory)
            {

                if (!_weapon.IsMelee() && _inventory.HasItem(_weapon.Config.AmmoType))
                {
                    
                    int index = _inventory.FindItem(_weapon.Config.AmmoType);

                    _weapon.TotalAmmo = _inventory.GetNumberInSlot(index);

                    _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);

                }

                if(_weapon.IsMelee())
                {
                    _isMelee = true;
                }
                else
                {
                    _isMelee = false;
                }
            
                WeaponLogic();
            }
            
        }
        else
        {

            _PlayerUI.UpdateAmmoUI(0,0,0);
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
            if (_weapon.IsMelee())
            {
                _weapon.transform.parent = _weaponParentMelee;
            }
            else
            {
                _weapon.transform.parent = _weaponParent;
            }
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;

            newWeapon.Setup();
        }

        if (_inventory)
        {
            if (!_weapon.IsMelee() && _inventory.HasItem(_weapon.Config.AmmoType))
            {
                int index = _inventory.FindItem(_weapon.Config.AmmoType);

                _weapon.TotalAmmo = _inventory.GetNumberInSlot(index);

                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
            }
            else
            {
                _weapon.TotalAmmo = 0;
                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
            }
        }
    }

    void GunLogic()
    {
        if (_weapon.Config.IsAutomatic)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    _weapon.StartFiring();
                    _controller.IsShooting = true;
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    _weapon.StopFiring();
                    _controller.IsShooting = false;
                }
                if (_weapon.IsFiring)
                {
                    _weapon.UpdateWeapon(_crosshairTarget.position);
                    _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _weapon.StartFiring();
                    _controller.IsShooting = true;
                    _weapon.UpdateWeapon(_crosshairTarget.position);
                }
                else
                {
                    _controller.IsShooting = false;
                    _weapon.StopFiring();
                }
                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
            }


            _weapon.UpdateBullets();

            

            if (Input.GetKeyDown(KeyCode.R) && _weapon.TotalAmmo > 0)
            {
                Reload();
                _controller.IsShooting = false;

            }

            if (_weapon.NeedToReload)
            {
                _controller.IsShooting = false;
            }

            if (_weapon.IsReloading == false)
            {
                _anim.SetBool("isReloading", false);
            }
            else
            {
                _controller.IsShooting = false;
                _PlayerUI.UpdateAmmoUI(_weapon.ClipAmmo, _weapon.Config.ClipSize, _weapon.TotalAmmo);
            }
    }

    void MeleeLogic()
    {

        if(Input.GetButtonDown("Fire1"))
        {
            _anim.SetTrigger("Stab");
        }

        if(_anim.GetCurrentAnimatorStateInfo(0).IsName("Stabbing") && _anim.GetCurrentAnimatorStateInfo(0).length > _anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            GetComponent<WeaponStabCheck>().SetStabbing(true);
        }
        else
        {
            GetComponent<WeaponStabCheck>().SetStabbing(false);
        }

    }

    void WeaponLogic()
    {
        if(_isMelee)
        {
            MeleeLogic();
        }
        else
        {
            GunLogic();
        }
    }

}
