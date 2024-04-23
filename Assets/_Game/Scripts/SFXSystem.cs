using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSystem : SettingListener
{
    public static SFXSystem Instance;
    [SerializeField] private AudioSource[] audioPlayerOneShot;
    [SerializeField] private List<AudioClip> audioClips;
    private Dictionary<string, AudioClip> audioLibrary = new Dictionary<string, AudioClip>();
    private List<AudioSource> enabledAudio = new List<AudioSource>();
    private List<AudioSource> disabledAudio = new List<AudioSource>();
    private AudioSource audioPlayer;
    private bool enableSound;
    public Dictionary<string, AudioClip> AdioLibrary => audioLibrary;
    private void Awake()
    {
        Instance = this;
        Init();
        UpdateSetting();
    }

    public void Play(AudioClip audioClip) {
        if (enableSound == false) return;
        if (audioLibrary.ContainsKey(audioClip.name) == false) {
            audioLibrary.Add(audioClip.name, audioClip);
        }
        Play(audioClip.name);
    }

    public void Play(string audioName)
    {
        if (enableSound == false) return;
        var clip = audioLibrary[audioName];
        if (disabledAudio.Count != 0)
        {
            var audioSource = disabledAudio[0];
            audioSource.gameObject.SetActive(true);
            audioSource.clip = clip;
            audioSource.time = 0;
            audioSource.Play();
            enabledAudio.Add(audioSource);
            disabledAudio.Remove(audioSource);
            LeanTween.delayedCall(audioSource.clip.length, () =>
            {
                if (audioSource == null) return;
                audioSource.gameObject.SetActive(false);
                disabledAudio.Add(audioSource);
                enabledAudio.Remove(audioSource);
            });
        }
        else
        {
            var newAudio = Instantiate(audioPlayer);
            newAudio.clip = clip;
            newAudio.time = 0;
            newAudio.Play();
            enabledAudio.Add(newAudio);
            disabledAudio.Remove(newAudio);
            LeanTween.delayedCall(newAudio.clip.length, () =>
            {
                if (newAudio == null) return;
                newAudio.gameObject.SetActive(false);
                enabledAudio.Remove(newAudio);
                disabledAudio.Add(newAudio);
            });
        }
    }
    private void Init()
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            string key = audioClips[i].name;
            audioLibrary.Add(key, audioClips[i]);
        }
        enabledAudio = new List<AudioSource>();
        disabledAudio = new List<AudioSource>();
        audioPlayer = new GameObject().AddComponent<AudioSource>();
        audioPlayer.gameObject.name = "audioPlayer";
        audioPlayer.loop = false;
        //audioPlayer.gameObject.SetActive(false);
        //disabledAudio.Add(audioPlayer);
    }

    public void PlayOneShot(int chanel, string clip)
    {
        AudioClip oneShotClip = audioLibrary[clip];
        audioPlayerOneShot[chanel].PlayOneShot(oneShotClip);
    }

    public void PlayOneShot(int chanel, AudioClip clip)
    {
        audioPlayerOneShot[chanel].PlayOneShot(clip);
    }

    public override void UpdateSetting()
    {
        enableSound = PlayerPrefs.GetInt(Constants.SETTING_SOUND, Constants.DEFAULT_SOUND) == 1;
    }
}