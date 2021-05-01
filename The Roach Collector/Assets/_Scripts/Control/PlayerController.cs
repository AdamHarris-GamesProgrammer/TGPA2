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



        public bool CanDisableAlarm { get { return _canDisableAlarm; } set { _canDisableAlarm = value; } }
        AlarmController _alarm;

        public AlarmController Alarm { set { _alarm = value; } }

        ActionStore _actionSlots;


        private void Awake()
        {
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
    }
}

