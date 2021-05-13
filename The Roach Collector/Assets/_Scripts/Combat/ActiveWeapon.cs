using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] public PlayerUI _PlayerUI = null;
    public RaycastWeapon _MeleeWeapon;

    public Cinemachine.CinemachineFreeLook _camera;

    Animator _anim;
    [SerializeField] private RaycastWeapon _weapon;

    [SerializeField] private bool _isMelee = false;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();

        if (_startingWeapon)
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon);

            Equip(weapon);

            _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if(_weapon)
        {
           
            if(_weapon._totalAmmo == 0 && _weapon._clipAmmo == 0 && _isMelee == false)
            {
                _isMelee = true;
                RaycastWeapon Melee = Instantiate(_MeleeWeapon);
                Equip(Melee);
            }
            
            WeaponLogic();

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

            _weapon.Recoil._camera = _camera;
        }
    }

    void GunLogic()
    {
        if (Input.GetButtonDown("Fire1") && _weapon._clipAmmo > 0 && _weapon._isReloading == false)
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            Equip(null);
        }
        if (Input.GetKeyDown(KeyCode.R) && _weapon._totalAmmo > 0)
        {
            _weapon._isReloading = true;
            _weapon._totalAmmo += _weapon._clipAmmo;

            if (_weapon._totalAmmo < _weapon.Config.ClipSize)
            {
                _weapon._clipAmmo = _weapon._totalAmmo;
                _weapon._totalAmmo = 0;
            }
            else
            {
                _weapon._clipAmmo = _weapon.Config.ClipSize;
                _weapon._totalAmmo -= _weapon.Config.ClipSize;
            }

            _anim.SetBool("isReloading", true);
            _PlayerUI.UpdateAmmoUI(_weapon._clipAmmo, _weapon._config.ClipSize, _weapon._totalAmmo);
        }
        if (_weapon._isReloading == false)
        {
            _anim.SetBool("isReloading", false);
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
