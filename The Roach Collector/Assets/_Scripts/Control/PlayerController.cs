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



        private void Awake()
        {
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
        }

        private void FixedUpdate()
        {
        }


    }
}

