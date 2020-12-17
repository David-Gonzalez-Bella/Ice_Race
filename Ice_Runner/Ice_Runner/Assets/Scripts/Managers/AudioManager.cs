using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sharedInstance { get; private set; }

    //Audio Mixers
    public AudioMixer mainThemeAudioMixer;
    public AudioMixer sfxAudioMixer;

    //Audios
    public AudioSource coinPicked;
    public AudioSource playerJump;

    //Events
    public event Action OnCoinPicked; //Observer pattern

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    private void Start()
    {
        mainThemeAudioMixer.SetFloat("MainVolume", Mathf.Log10(0.35f) * 20);
        sfxAudioMixer.SetFloat("SFX_Volume", Mathf.Log10(0.35f) * 20);

        ScreensManager.sharedInstance.mainThemeVolumeBar.value = 0.35f;
        ScreensManager.sharedInstance.sfxVolumeBar.value = 0.35f;
    }

    public void Update()
    {
        OnCoinPicked?.Invoke();
    }

    public void SetMainThemeVolume(float volume)
    {
        mainThemeAudioMixer.SetFloat("MainVolume", Mathf.Log10(volume) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }

    public void MuteMainThemeVolume()
    {
        ScreensManager.sharedInstance.mainThemeVolumeBar.value = 0.0001f;
        mainThemeAudioMixer.SetFloat("MainVolume", Mathf.Log10(0.0001f) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }
    public void SetSFX_Volume(float volume)
    {
        sfxAudioMixer.SetFloat("SFX_Volume", Mathf.Log10(volume) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }

    public void MuteSFX_Volume()
    {
        ScreensManager.sharedInstance.sfxVolumeBar.value = 0.0001f;
        sfxAudioMixer.SetFloat("SFX_Volume", Mathf.Log10(0.0001f) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }

    public void PickCoin()
    {
        coinPicked.Play();
        OnCoinPicked -= PickCoin;
    }
}
