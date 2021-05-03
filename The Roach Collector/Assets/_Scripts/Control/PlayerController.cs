using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TGP.Control
{
    public class PlayerController : MonoBehaviour
    {
        bool _canDisableAlarm = false;

        AIAgent _agentInRange = null;

        bool _inKillAnimation = false;

        public bool InKillAnimation { get { return _inKillAnimation; } }

        public AIAgent AgentInRange { get { return _agentInRange; } set { _agentInRange = value; } }

        public bool CanDisableAlarm { get { return _canDisableAlarm; } set { _canDisableAlarm = value; } }
        AlarmController _alarm;

        bool _detected = false;
        public bool IsDetected { get { return _detected; } set { _detected = value; } }

        public AlarmController Alarm { set { _alarm = value; } }

        ActionStore _actionSlots;

        Animator _animator;

        [SerializeField] Vector3 _assassinOffset = Vector3.back;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionSlots = GetComponent<ActionStore>();
        }

 

        private void Update()
        {
            if(_canDisableAlarm && _alarm)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _alarm.DisableAlarm();
                }
            }

            InteractWithActionBar();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if(_agentInRange != null && !_detected)
                {
                    if (_agentInRange.GetHealth().IsDead()) return;
                    //TODO: Somehow make the animation look better 


                    Vector3 offSetPos = _agentInRange.transform.position - _assassinOffset;

                    transform.position = offSetPos;

                    _animator.SetTrigger("stealthAssassinate");

                    _agentInRange.BeingKilled = true;

                    _inKillAnimation = true;
                    
                    
                    _agentInRange.GetComponent<Animator>().SetTrigger("stealthAssassinate");

                    _agentInRange = null;

                }
            }

        }

        private void InteractWithActionBar()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _actionSlots.Use(0, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _actionSlots.Use(1, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _actionSlots.Use(2, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _actionSlots.Use(3, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _actionSlots.Use(4, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _actionSlots.Use(5, this.gameObject);
            }
        }

        void OutOfKillAnim()
        {
            _inKillAnimation = false;
        }
    }


}

