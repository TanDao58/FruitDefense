using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames {
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour {
        public static AudioSystem Instance;

        public const int CHANEL_AMOUNT = 5;
        public const float FADE_BACKGROUND_SONG_TIME = 2f;

        public List<AudioClip> clips;
        public AudioClip buttonSound;

        private Dictionary<string, AudioClip> dicClips;
        private List<AudioSource> enabledAudio = new List<AudioSource>();
        private List<AudioSource> disabledAudio = new List<AudioSource>();
        private AudioSource[] chanels;
        private AudioSource chanelBgSong;
        private AudioSource chanelBgSong2;

        int backgroundChanel = 0;

        void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            dicClips = new Dictionary<string, AudioClip>();
            chanels = new AudioSource[CHANEL_AMOUNT];

            for (int i = 0; i < CHANEL_AMOUNT; i++) {
                chanels[i] = CreateAudioSource(false);
            }
            chanelBgSong = CreateAudioSource(true);
            chanelBgSong2 = CreateAudioSource(true);
        }

        AudioSource CreateAudioSource(bool loop) {
            var audioSource = new GameObject().AddComponent<AudioSource>();
            audioSource.transform.SetParent(gameObject.transform);
            audioSource.loop = loop;
            return audioSource;
        }

        public void PlayChanel(string path, int chanel_id) {
            if (!dicClips.ContainsKey(path)) {
                AudioClip clip = Resources.Load<AudioClip>(path);
                dicClips.Add(path, clip);
            }

            chanels[chanel_id].clip = dicClips[path];
            chanels[chanel_id].Play();
        }

        public void PlayChanel(AudioClip clip, int chanel_id) {
            chanels[chanel_id].clip = clip;
            chanels[chanel_id].Play();
        }

        public void FadeBackgroundSong(AudioClip clip) {
            backgroundChanel = backgroundChanel == 0 ? 1 : 0;
            AudioSource fadeOut = backgroundChanel == 0 ? chanelBgSong : chanelBgSong2;
            AudioSource fadeIn = backgroundChanel == 1 ? chanelBgSong : chanelBgSong2;

            fadeIn.clip = clip;
            fadeIn.Play();

            LeanTween.value(1f, 0f, FADE_BACKGROUND_SONG_TIME).setOnUpdate((float f) => {
                fadeOut.volume = f;
            }).setOnComplete(() => {
                fadeOut.Stop();
            });

            LeanTween.value(0f, 1f, FADE_BACKGROUND_SONG_TIME).setOnUpdate((float f) => {
                fadeIn.volume = f;
            });
        }
    }
}