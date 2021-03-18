using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tgpAudio;

public class AmbientSoundTrigger : MonoBehaviour
{
    public AudioController audiocontroller;
    string currentlyPlaying;

    void OnTriggerEnter(Collider collider)
    {
        
        string collisionName = gameObject.name;
        Debug.Log(collisionName);

       

        switch (collisionName)
        {

            case "PLAY_ROOM_OUTSIDE":

                audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_OUTSIDE, true);
                Debug.Log("Playing Outside SFX");
                break;

            case "PLAY_ROOM1":
                Debug.Log("Currently Playing: " + currentlyPlaying);
                if (currentlyPlaying == "PLAY_ROOM1")
                {
                    audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_OUTSIDE, true);
                    Debug.Log("Playing Outside SFX");
                    currentlyPlaying = "RoomOutside";
                }
                else
                {
                    audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_1, true);
                    Debug.Log("Playing Main room SFX");
                    currentlyPlaying = "Room1";
                }

       
                    break;

            case "PLAY_BUZZ_ROOM":
                Debug.Log("Currently Playing: " + currentlyPlaying);

                if(currentlyPlaying == "Room1")
                {
                     audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_BUZZ, true);
                    Debug.Log("Playing Buzz room");
                    currentlyPlaying = "RoomBuzz";
                }
                else
                {
                    audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_1, true);
                    Debug.Log("Playing Main room SFX");
                    currentlyPlaying = "Room1";
                }
                break;

            default:
                break;

           
        }


    }
}
