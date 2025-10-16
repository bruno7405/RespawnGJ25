using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; // each sound has its own source
    private AudioSource mainAudioSource;

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        //DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        mainAudioSource = GetComponent<AudioSource>();
    }

    public void PlayBackgroundMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + "' not found!");
            return;
        }

        mainAudioSource.clip = s.audioClip;
        mainAudioSource.volume = s.volume;
        mainAudioSource.Play();
    }

    public void PlayOneShot(string name, float pitchRandomization = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + "' not found!");
            return;
        }

        if (pitchRandomization != 0) s.source.pitch = UnityEngine.Random.Range(1 - pitchRandomization, 1 + pitchRandomization); ;
        s.source.PlayOneShot(s.audioClip);
    }
}