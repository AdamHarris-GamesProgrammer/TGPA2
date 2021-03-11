using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tgpAudio
{
    public class TestAudio : MonoBehaviour
    {

        public AudioController audioController;

       private void Update()
        
        {
            //Test if audio works
            if (Input.GetKeyUp(KeyCode.T))
            {
                audioController.PlayAudio(AudioType.ROOM_1, true);
            }
            if (Input.GetKeyUp(KeyCode.Y))
            {
                audioController.StopAudio(AudioType.ROOM_1, true);
            }
            if (Input.GetKeyUp(KeyCode.U))
            {
                audioController.RestartAudio(AudioType.ROOM_1, true);
            }

        }
    }
}

