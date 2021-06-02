using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class WeaponStab : MonoBehaviour
{
    private Health _attackedHealth;
    private GameObject _parent;
    private GameObject _player;

    private WeaponStabCheck _wsCheck;
    private bool _isStabbing = false;
    [SerializeField] WeaponConfig _meleeConfig;

    bool _attacked = false;

    float _attackDuration = 1.5f;
    float _attackTimer = 0.0f;


    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;

        if (transform.root.name == "Core") _wsCheck = _player.GetComponent<WeaponStabCheck>();
        else _wsCheck = GetComponentInParent<WeaponStabCheck>();

    }

    private void Update()
    {
        if (_attacked)
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer > _attackDuration)
            {
                _attackTimer = 0.0f;
                _attacked = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.root == transform.root) return;
        if (_attacked) return;

        //are we stabbing?
        _isStabbing = _wsCheck.GetStabbing();


        //See if we are stabbing an agent
        AIAgent agent = collision.transform.GetComponentInParent<AIAgent>();

        //if we are then the parent is the agent
        if (agent)
        {
            _parent = agent.gameObject;
            _attackedHealth = _parent.GetComponent<Health>();
        }
        else
        {
            _parent = _player;
            _attackedHealth = _player.GetComponent<Health>();
        }

        //Get the health of the attacked object and deal damage
        if (_isStabbing)
        {
            _attacked = true;
            _attackedHealth.TakeDamage(_meleeConfig.DamageType, _meleeConfig.Damage);
        }

    }
}
