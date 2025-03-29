using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class SoundBgmLibrary : MonoBehaviour
    {
        [SerializeField] private List<BgmTrack> bgmTracks;
        private Dictionary<string, AudioClip> bgmDictionary;

        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            bgmDictionary = new Dictionary<string, AudioClip>();
            foreach (BgmTrack track in bgmTracks)
            {
                bgmDictionary.Add(track.name, track.clip);
            }
        }

        public AudioClip GetClip(string name)
        {
            if (bgmDictionary.ContainsKey(name))
            {
                return bgmDictionary[name];
            }
            return null;
        }
    }

    [System.Serializable]
    public struct BgmTrack
    {
        public string name;
        public AudioClip clip;
    }
}