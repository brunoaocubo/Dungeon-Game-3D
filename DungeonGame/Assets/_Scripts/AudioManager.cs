using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup audioFxMixerGroup;
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(Instance);
        }

        foreach (Sound s in sounds) 
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.loop = s.isLoop;

            switch (s.audioType) 
            {
                case Sound.AudioTypes.music:
                    s.audioSource.outputAudioMixerGroup = musicMixerGroup; 
                    break;

                case Sound.AudioTypes.audioFX:
                    s.audioSource.outputAudioMixerGroup = audioFxMixerGroup;
                    break;
            }

            if(s.playOnAwake) 
            {
                s.audioSource.Play();
            }
        }
    }

    public void PlayMusic(string clipName) 
    {
        Sound s = Array.Find(sounds, x => x.clipName == clipName);
        if(s == null) 
        {
            Debug.Log("Sound: " + clipName + "not exist");
            return;
        }
        s.audioSource.Play();       
    }

    public void StopMusic(string clipName)
    {
        Sound s = Array.Find(sounds, x => x.clipName == clipName);

        if (s == null)
        {
            Debug.Log("Sound: " + clipName + "not exist");
            return;
        }
        s.audioSource.Stop();
    }

    public void SetVolumeMusic(float value) 
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 40);
    }

    public void SetVolumeAudioFX(float value) 
    {
        audioFxMixerGroup.audioMixer.SetFloat("AudioFxVolume", Mathf.Log10(value) * 40);
    }
}
