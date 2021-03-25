//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace tgpAudio
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

        private class AudioJob
        {
            public AudioAction action;
            public AudioType type;
            public bool fade;

            public AudioJob(AudioAction _action, AudioType _type, bool _fade)
            {
                action = _action;
                type = _type;
                fade = _fade;

            }

        }

        private enum AudioAction
        {
            START,
            STOP,
            RESTART
        }

        #region Public Functions

        public void PlayAudio(AudioType _type, bool _fade = false)
        {
            AddJob(new AudioJob(AudioAction.START, _type, _fade));

        }
        public void StopAudio(AudioType _type, bool _fade = false)
        {
            AddJob(new AudioJob(AudioAction.STOP, _type, _fade));
        }
        public void RestartAudio(AudioType _type, bool _fade = false)
        {
            AddJob(new AudioJob(AudioAction.RESTART, _type, _fade));
        }


        public AudioClip GetAudioClipFromAudioTrack(AudioType _type, AudioTrack _track)
        {
            foreach(AudioObject _obj in _track.audio){
                if (_obj.type == _type)
                {
                    return _obj.clip;
                }
                
            }

            return null;
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
            Debug.Log("Configuring Audio Controller");
            instance = this;
            m_AudioTable = new Hashtable();
            m_JobTable = new Hashtable();
            GenerateAudioTable();
            

        }
        private void GenerateAudioTable()
        {
            foreach (AudioTrack _track in audioTracks)
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

        private IEnumerator RunAudioJob(AudioJob _job)
        {
            

            AudioTrack _track = (AudioTrack)m_AudioTable[ _job.type];
            _track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);

            switch (_job.action)
            {
                case AudioAction.START:
                    _track.source.Play();

                    break;
                case AudioAction.STOP:
                    //If fade is false, stop the track immediately
                    if (!_job.fade)
                    {
                        _track.source.Stop();
                    }
                    
                    

                    break;
                case AudioAction.RESTART:
                    _track.source.Stop();
                    _track.source.Play();

                    break;

            }


            if (_job.fade)
            {
                //If the job action is start or restart the initial volume will be 0, otherwise 1 as it will be fading out
                float _initialVolumeValue = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? 0.0f : 1.0f;

                //Sets the target value of the volume lerp
                float _target = _initialVolumeValue == 0 ? 1 : 0;
                float _duration = 1.0f;
                float _timer = 0.0f;

                //lerp the volume parameter
                while(_timer < _duration)
                {
                    _track.source.volume = Mathf.Lerp(_initialVolumeValue, _target, _timer / _duration);
                    _timer += Time.deltaTime;
                    yield return null;


                }

                if (_job.action == AudioAction.STOP)
                {
                    _track.source.Stop();
                }

            }

            m_JobTable.Remove(_job.type);
            yield return null;
            
        }



        private void RemoveConflictingJobs(AudioType _type)
        {
            if (m_JobTable.ContainsKey(_type))
            {
                Debug.Log("Remove Conflicting Jobs Call");
                RemoveConflictingJobs(_type);
            }

            //Stop two jobs running at the same time
            AudioType _conflictAudio = AudioType.None;
            foreach(DictionaryEntry _entry in m_AudioTable)
            {
                //Look through each audio type in the job table
                AudioType _audioType = (AudioType)_entry.Key;

                //Assign the track currently being used 
                AudioTrack _trackInUse = (AudioTrack)m_AudioTable[_audioType];

                AudioTrack _audioTrackNeeded = (AudioTrack)m_AudioTable[_type];
                if(_audioTrackNeeded.source == _trackInUse.source)
                {
                    //audio conflict
                    _conflictAudio = _audioType;
                }

            }
            if (_conflictAudio != AudioType.None)
            {
                RemoveJob(_conflictAudio);
            }

            
        }

        private void AddJob(AudioJob _job)
        {
            //Remove any conflicting jobs
            RemoveConflictingJobs(_job.type);

            //Add the job and start it 
            IEnumerator _jobRunner = RunAudioJob(_job);
            m_JobTable.Add(_job.type, _jobRunner);
            StartCoroutine(_jobRunner);
           
        }

        private void RemoveJob(AudioType _type)
        {
            if (!m_JobTable.ContainsKey(_type)){
                return;
            }

            IEnumerator _runningJob = (IEnumerator)m_JobTable[_type];
            StopCoroutine(_runningJob);
            m_JobTable.Remove(_type);
        }
        
        private void Dispose()
        {
            foreach (DictionaryEntry _entry in m_JobTable)
            {
                IEnumerator _job = (IEnumerator)_entry.Value;
                StopCoroutine(_job);
            }
        }


        #endregion



    }

}

