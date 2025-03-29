using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager Instance;

        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Image SfxSoundOnImg;
        [SerializeField] private Image SfxSoundOffImg;
        [SerializeField] private Image BgmSoundOnImg;
        [SerializeField] private Image BgmSoundOffImg;

        private static AudioSource audioSrc;
        private static AudioSource randomPitchAudioSrc;
        private static AudioSource bgmAudioSrc;
        private static SoundEffectLibrary soundEffectLibrary;
        private static SoundBgmLibrary soundBgmLibrary;

        private bool sfxMuted = false;
        private bool bgmMuted = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                Transform effectTransform = transform.Find("SoundEffectAudioSource");
                if (effectTransform != null)
                {
                    AudioSource[] audioSources = effectTransform.GetComponents<AudioSource>();
                    audioSrc = audioSources[0];
                    randomPitchAudioSrc = audioSources[1];
                }
                else
                {
                    Debug.LogError("EffectAudioSource child object not found!");
                }

                Transform bgmTransform = transform.Find("SoundBackGroundAudioSource");
                if (bgmTransform != null)
                {
                    bgmAudioSrc = bgmTransform.GetComponent<AudioSource>();
                    bgmAudioSrc.loop = true;
                }
                else
                {
                    Debug.LogError("BgmAudioSource child object not found!");
                }

                soundEffectLibrary = GetComponentInChildren<SoundEffectLibrary>();
                soundBgmLibrary = GetComponentInChildren<SoundBgmLibrary>();

                LoadSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Play SFX
        public static void PlaySFX(string soundName, int? element, bool randomPitch = false)
        {

            if (soundEffectLibrary == null) return;

            AudioClip clip = soundEffectLibrary.GetRandomClip(soundName);

            if (element.HasValue)
            {
                clip = soundEffectLibrary.GetClipWithIndex(soundName, element.Value);
            }

        
            if (clip != null)
            {
                if (randomPitch)
                {
                    randomPitchAudioSrc.pitch = Random.Range(1f, 1.5f);
                    randomPitchAudioSrc.PlayOneShot(clip);
                }
                else
                {
                    audioSrc.PlayOneShot(clip);
                }
            }
        }

        // Stop SFX
        public static void StopSFX()
        {
            audioSrc.Stop();
        }

        // Play BGM
        public static void PlayBGM(string bgmName)
        {
            if (soundBgmLibrary == null) return;

            AudioClip bgmClip = soundBgmLibrary.GetClip(bgmName);
            if (bgmClip != null)
            {
                bgmAudioSrc.clip = bgmClip;
                bgmAudioSrc.Play();
            }
        }

        // Stop BGM
        public static void StopBGM()
        {
            bgmAudioSrc.Stop();
        }

        private void Start()
        {
            sfxSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChange(); });
            bgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChange(); });

            UpdateUI();
            PlayBGM("DarkWoods");
            OnOffBgm();
            OnOffSfx();
        }

        private void Load()
        {
            sfxMuted = PlayerPrefs.GetInt("sfxMuted") == 1;
            bgmMuted = PlayerPrefs.GetInt("bgmMuted") == 1;
        }

        public void OnSfxVolumeChange()
        {
            float volume = sfxSlider.value;
            audioSrc.volume = volume;
            randomPitchAudioSrc.volume = volume;
            PlayerPrefs.SetFloat("sfxVolume", volume);
        }

        public void OnBgmVolumeChange()
        {
            float volume = bgmSlider.value;
            bgmAudioSrc.volume = volume;
            PlayerPrefs.SetFloat("bgmVolume", volume);
        }

        public void ToggleSFX()
        {
            sfxMuted = !sfxMuted;
            OnOffSfx();
            UpdateUI();
        }

        private void OnOffSfx()
        {
            audioSrc.mute = sfxMuted;
            randomPitchAudioSrc.mute = sfxMuted;
            PlayerPrefs.SetInt("sfxMuted", sfxMuted ? 1 : 0);
        }

        public void ToggleBGM()
        {
            bgmMuted = !bgmMuted;
            OnOffBgm();
            UpdateUI();
        }

        private void OnOffBgm()
        {
            bgmAudioSrc.mute = bgmMuted;
            PlayerPrefs.SetInt("bgmMuted", bgmMuted ? 1 : 0);
        }

        private void UpdateUI()
        {
            SfxSoundOnImg.enabled = !sfxMuted;
            SfxSoundOffImg.enabled = sfxMuted;

            BgmSoundOnImg.enabled = !bgmMuted;
            BgmSoundOffImg.enabled = bgmMuted;
        }

        private void LoadSettings()
        {
            if (!PlayerPrefs.HasKey("sfxMuted"))
            {
                PlayerPrefs.SetInt("sfxMuted", 0);
                Load();
            }
            else if (!PlayerPrefs.HasKey("bgmMuted"))
            {
                PlayerPrefs.SetInt("bgmMuted", 0);
                Load();
            }
            else
            {
                Load();
            }
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
            float bgmVolume = PlayerPrefs.GetFloat("bgmVolume", 1f);

            audioSrc.volume = sfxVolume;
            randomPitchAudioSrc.volume = sfxVolume;
            bgmAudioSrc.volume = bgmVolume;

            sfxSlider.value = sfxVolume;
            bgmSlider.value = bgmVolume;
        }

        public static void PlayClick()
        {
            PlaySFX("Click", 0);
        }
    }
}
