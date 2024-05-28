using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using BubbleShooter.Scripts.Common.Databases;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.PlayDatas;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private SoundEffectDatabase soundEffects;

    public static event Action<float> OnMasterChange;
    public static event Action<float> OnMusicChange;
    public static event Action<float> OnSoundChange;

    private const string MasterVolumeMixer = "MasterVolume";
    private const string MusicVolumeMixer = "MusicVolume";
    private const string SoundVolumeMixer = "SoundVolume";

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    public float MasterVolume
    {
        get => PlayerPrefs.GetFloat(MasterVolumeKey, 1);
        set
        {
            PlayerPrefs.SetFloat(MasterVolumeKey, value);
            OnMasterChange.Invoke(value);
        }
    }

    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicVolumeKey, 1);
        set
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            OnMusicChange.Invoke(value);
        }
    }

    public float SoundVolume
    {
        get => PlayerPrefs.GetFloat(SoundVolumeKey, 1);
        set
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
            OnSoundChange.Invoke(value);
        }
    }

    private void Start()
    {
        OnMasterChange += AdjustMasterVolume;
        OnMusicChange += AdjustMusicVolume;
        OnSoundChange += AdjustSoundVolume;

        MasterVolume += 0;
        MusicVolume += 0;
        SoundVolume += 0;
    }

    public void PlayMusic(AudioClip musicClip, bool loop = false)
    {
        if (musicClip == null)
            return;

        musicSource.Stop();
        musicSource.loop = loop;
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySoundEffect(SoundEffectEnum soundEffect, float volumeScale = 1, bool loop = false)
    {
        SoundEffect sound = soundEffects.GetSoundEffect(soundEffect);

        if (sound != null)
            PlaySoundEffect(sound.EffectClip, volumeScale, loop);
    }

    public void PlaySoundEffect(AudioClip soundClip, float volumeScale = 1, bool loop = false)
    {
        if (soundClip == null || sfxSource == null)
            return;

        sfxSource.loop = loop;
        sfxSource.PlayOneShot(soundClip, volumeScale);
    }

    public void SetBackGroundMusic(AudioClip music, bool loop = true, float volume = 1)
    {
        if (music == null || musicSource == null)
            return;

        musicSource.Stop();
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.clip = music;
        musicSource.Play();
    }

    private void AdjustMasterVolume(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(MasterVolumeMixer, volume);
    }

    private void AdjustMusicVolume(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(MusicVolumeMixer, volume);
    }

    private void AdjustSoundVolume(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(SoundVolumeMixer, volume);
    }

    private void OnDestroy()
    {
        OnMasterChange -= AdjustMasterVolume;
        OnMusicChange -= AdjustMusicVolume;
        OnSoundChange -= AdjustSoundVolume;
    }
}
