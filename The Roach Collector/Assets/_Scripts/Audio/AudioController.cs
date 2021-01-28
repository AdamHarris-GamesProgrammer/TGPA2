//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        //Members and data types
        public static AudioController instance;


        public AudioTrack[] audioTracks;


        //Creating hashtables
        private Hashtable m_AudioTable; // Relationship between audio types (key) and audio tracks (value)
        private Hashtable m_JobTable; // Relationship between audio types (key) and jobs (value)

        [System.Serializable]
        public class AudioObject
        {
            public AudioType type;
            public AudioClip clip;
        }

        [System.Serializable] 
        public class AudioTrack
        {
            public AudioSource source;
            public AudioObject[] audio;
        }

        #region Public Functions

        public void PlayAudio(AudioType _type)
        {
            AddJob(new AudioJob());
        }
        public void StopAudio(AudioType _type)
        {

        }
        public void RestartAudio(AudioType _type)
        {

        }

        #endregion


        #region Unity Functions
        private void Awake()
        {
            //Create instance
            if (!instance)
            {
                Configure();
            }
           
        }
        private void OnDisable()
        {
            Dispose();
        }

        #endregion

        #region Private Functions
        private void Configure()
        {
            instance = this;
            m_AudioTable = new Hashtable();
            m_JobTable = new Hashtable();
            GenerateAudioTable();
            

        }
        private void GenerateAudioTable()
        {
            foreach (AudioTracks _track in tracks)
            {
                foreach(AudioObject _obj in _track.audio)
                {
                    //Do not want to duplicate keys
                    if (m_AudioTable.ContainsKey(_obj.type))
                    {
                        ///error
                    }
                    else
                    {
                        //Create the audio table
                        m_AudioTable.Add(_obj.type, _track);

                    }
                }
            }
        }
        
        private void Dispose()
        {

        }


        #endregion



    }

}

