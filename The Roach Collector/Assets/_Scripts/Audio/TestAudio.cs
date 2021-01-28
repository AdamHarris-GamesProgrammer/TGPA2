using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class TestAudio : MonoBehaviour
    {

        public AudioController audioController;

       private void Update()
        
        {
            if (Input.GetKeyUp(KeyCode.T))
            {
                audioController.PlayAudio(AudioType.ROOM_1);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                audioController.StopAudio(AudioType.ROOM_1);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                audioController.RestartAudio(AudioType.ROOM_1);
            }

        }
    }
}

