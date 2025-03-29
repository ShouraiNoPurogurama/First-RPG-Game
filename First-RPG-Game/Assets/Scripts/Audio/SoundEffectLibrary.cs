using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class SoundEffectLibrary : MonoBehaviour
    {
        [SerializeField] private SoundEffectGroup[] soundEffectGroups;
        private Dictionary<string, List<AudioClip>> soundDictionary;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            soundDictionary = new Dictionary<string, List<AudioClip>>();
            foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
            {
                soundDictionary.Add(soundEffectGroup.name, soundEffectGroup.clips);
            }
        }

        public AudioClip GetRandomClip (string name)
        {
            if (soundDictionary.ContainsKey(name))
            {
                List<AudioClip> clips = soundDictionary[name];
                if (clips.Count == 0)
                {
                    return null;
                }
                return clips[UnityEngine.Random.Range(0, clips.Count)];
            }
            return null;
        }

        public AudioClip[] GetClip(string name)
        {
            if (soundDictionary.ContainsKey(name))
            {
                foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
                {
                    if (soundEffectGroup.name == name)
                    {
                        return soundEffectGroup.clips.ToArray();
                    }
                }
                //List<AudioClip> clips = soundDictionary[name];
                //if (clips.Count == 0)
                //{
                //    return null;
                //}
                //return clips;
            }
            return null;
        }

        public AudioClip GetClipWithIndex(string name, int index)
        {
            if (soundDictionary.ContainsKey(name))
            {
                List<AudioClip> clips = soundDictionary[name];
                if (clips.Count == 0)
                {
                    return null;
                }
                return clips[index];
            }
            return null;
        }
    }

    [System.Serializable]
    public struct SoundEffectGroup
    {
        public string name;
        public List<AudioClip> clips;
    }
}