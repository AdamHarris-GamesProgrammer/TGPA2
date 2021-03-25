using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tgpAudio;


public class AmbientSoundTrigger : MonoBehaviour
{
    public AudioController audiocontroller;
    private string currentlyPlaying;
    bool firstDoorEnter;

    private void Start()
    {
        currentlyPlaying = "RoomOutside";
        Debug.Log("Initial Outside Audio");
    }

    void OnTriggerEnter(Collider collider)
    {
        
        string collisionName = gameObject.name;
        Debug.Log(collisionName);

        if (collisionName == "PLAY_ROOM1")
        {
            Debug.Log("Playing ON Trigger: " + currentlyPlaying);

            if (currentlyPlaying == "Room1")
            {
                audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_OUTSIDE, true);

                currentlyPlaying = "RoomOutside";
                Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
            }
            else
            {
                audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_1, true);
                Debug.Log("Playing Main room SFX");
                currentlyPlaying = "Room1";
                Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
            }
        }

        else if (collisionName == "PLAY_BUZZ_ROOM")
        {
            Debug.Log("Playing ON Trigger: " + currentlyPlaying);

            if (currentlyPlaying == "Room1")
            {
                audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_BUZZ, true);
                Debug.Log("Playing Buzz room");
                currentlyPlaying = "RoomBuzz";
                Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
            }
            else
            {
                audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_1, true);
                Debug.Log("Playing Main room SFX");
                currentlyPlaying = "Room1";
                Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
            }
        }

        //switch (collisionName)
        //{

        //    //case "PLAY_ROOM_OUTSIDE":

        //    //    audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_OUTSIDE, true);
        //    //    currentlyPlaying = "RoomOutside";
        //    //    Debug.Log("Playing Outside SFX");

        //    //    break;

        //    case "PLAY_ROOM1":
        //        Debug.Log("Playing ON Trigger: " + currentlyPlaying);
        //        if (currentlyPlaying == "Room1")
        //        {
        //            audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_OUTSIDE, true);
                    
        //            currentlyPlaying = "RoomOutside";
        //            Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
        //        }
        //        else
        //        {
        //            audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_1, true);
        //            Debug.Log("Playing Main room SFX");
        //            currentlyPlaying = "Room1";
        //            Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
        //        }

        //            break;

        //    case "PLAY_BUZZ_ROOM":
        //        Debug.Log("Playing ON Trigger: " + currentlyPlaying);

        //        if(currentlyPlaying == "Room1")
        //        {
        //             audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_BUZZ, true);
        //            Debug.Log("Playing Buzz room");
        //            currentlyPlaying = "RoomBuzz";
        //            Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
        //        }
        //        else
        //        {
        //            audiocontroller.PlayAudio(tgpAudio.AudioType.ROOM_1, true);
        //            Debug.Log("Playing Main room SFX");
        //            currentlyPlaying = "Room1";
        //            Debug.Log("Playing AFTER Trigger: " + currentlyPlaying);
        //        }
        //        break;

        //    default:
        //        break;

           
        //}


    }
}
