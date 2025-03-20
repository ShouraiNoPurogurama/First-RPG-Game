//using System;
//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager instance;
    
//    [SerializeField] private AudioSource[] _sfx;
//    [SerializeField] private AudioSource[] _backgroundMusic;

//    public bool playBackgroundMusic;
//    private bool playedFirstBackgroundMusic;
//    private int currentBackgroundMusicIndex;
    
//    private void Awake()
//    {
//        if(instance != null)
//        {
//            Destroy(instance.gameObject);
//        }
//        else
//        {
//            instance = this;
//        }
//    }

//    private void Update()
//    {
//        if(!playBackgroundMusic) 
            
//            StopAllBackgroundMusics();
//        else
//        {
//            if(!_backgroundMusic[currentBackgroundMusicIndex].isPlaying)
//            {
//                PlayAndCycleBackgroundMusic();
//            }
//        }
//    }

//    private void PlayAndCycleBackgroundMusic()
//    {
//        if(!playedFirstBackgroundMusic)
//        {
//            PlayBackgroundMusic(0);
//            playedFirstBackgroundMusic = true;
//        }
//        else
//        {
//            currentBackgroundMusicIndex++;
//            if(currentBackgroundMusicIndex >= _backgroundMusic.Length)
//            {
//                currentBackgroundMusicIndex = 1;
//            }
//            PlayBackgroundMusic(currentBackgroundMusicIndex);
//        }
//    }
//    //
//    // public void PlayRandomBackgroundMusic()
//    // {
//    //     currentBackgroundMusicIndex = UnityEngine.Random.Range(0, _backgroundMusic.Length);
//    //     PlayBackgroundMusic(currentBackgroundMusicIndex);
//    // }

//    public void PlaySFX(int index)
//    {
//        if(index < _sfx.Length)
//        {
//            _sfx[index].Play();
//        }
//    }
    
//    public void PlayBackgroundMusic(int index)
//    {
//        StopAllBackgroundMusics();
        
//        _backgroundMusic[index].Play();
//    }
    
//    public void StopSFX(int index) => _sfx[index].Stop();

//    public void StopAllBackgroundMusics()
//    {
//        for (int i = 0; i < _backgroundMusic.Length; i++)
//        {
//            _backgroundMusic[i].Stop();
//        }
//    }
//}
