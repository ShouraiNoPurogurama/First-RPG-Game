using MainCharacter;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float minDistanceToPlaySound;
    [SerializeField] private AudioSource[] bgAudioSource;
    [SerializeField] private AudioSource[] sfxAudioSource;

    public bool _playBgMusic;
    private int _bgmIndex;
    public bool _playSfx;

    //[SerializeField] private AudioClip bgAudioClip;
    //[SerializeField] private AudioClip checkPointClip;
    //[SerializeField] private AudioClip jumpClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        } 
    }

    public void Update()
    {
        if (!_playBgMusic)
        {
            StopBgMusic();
        }
        else
        {
            if (!bgAudioSource[_bgmIndex].isPlaying)
            {
                PlayBgMusic(0);
            }
        }
    }

    public void PlaySfx(int sfxIndex, Transform src)
    {
        if (sfxAudioSource[sfxIndex].isPlaying)
        {
            return;
        }

        if(src != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, src.position) > minDistanceToPlaySound)
        {
            return;
        }

        if (sfxIndex < sfxAudioSource.Length)
        {
            sfxAudioSource[sfxIndex].Play();
        }
    }
    public void StopSfx(int sfxIndex) => sfxAudioSource[sfxIndex].Stop();
    public void PlayBgMusic(int bgmIndex)
    {
        _bgmIndex = bgmIndex;

        StopBgMusic();
        bgAudioSource[bgmIndex].Play();
    }
    
    public void StopBgMusic()
    {
        for (int i = 0; i< bgAudioSource.Length; i++)
        {
            bgAudioSource[i].Stop();
        }
    }
}
