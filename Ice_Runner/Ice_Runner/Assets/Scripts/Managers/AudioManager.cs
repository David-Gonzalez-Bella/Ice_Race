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

    //Audio - Themes
    public AudioSource[] mainThemes;
    //public AudioSource menus_MUSIC;
    //public AudioSource levels_0_to_3_MUSIC;
    //public AudioSource levels_4_to_7_MUSIC;
    //public AudioSource levels_8_to_9_MUSIC;
    //public AudioSource level_10_MUSIC;

    //Audio - SFX
    public AudioSource coinPicked_SFX;
    public AudioSource playerJump_SFX;
    public AudioSource checkPoint_SFX;
    public AudioSource winCheers_SFX;
    public AudioSource enemyDie_SFX;
    public AudioSource gameOver_SFX;
    public AudioSource buttonPressed_SFX;
    public AudioSource playerHurt_SFX;

    //Events
    public event Action OnCoinPicked_snd; //Observer pattern
    public event Action OnCheckPoint_snd;
    public event Action OnEnemyDie_snd;
    public event Action OnButtonPressed_snd;

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
        OnCoinPicked_snd?.Invoke();
        OnCheckPoint_snd?.Invoke();
        OnEnemyDie_snd?.Invoke();
        OnButtonPressed_snd?.Invoke();
    }

    //Volume bars functions
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

    //SFX events add functions
    public void ButtonPressedEvent()
    {
        OnButtonPressed_snd += ButtonPressedSND;
    }

    //Themes play functions
    public void PlayMainTheme(int themeIndex)
    {
        for(int i = 0; i < mainThemes.Length; i++)
        {
            if (i == themeIndex)
                mainThemes[i].Play();
            else
                mainThemes[i].Stop();
        }
    }

    //SFX play functions
    public void PickCoinSND()
    {
        coinPicked_SFX.Play();
        OnCoinPicked_snd -= PickCoinSND;
    }

    public void CheckPointSND()
    {
        checkPoint_SFX.Play();
        OnCheckPoint_snd -= CheckPointSND;
    }

    public void EnemyDieSND()
    {
        enemyDie_SFX.Play();
        OnEnemyDie_snd -= EnemyDieSND;
    }

    public void ButtonPressedSND()
    {
        buttonPressed_SFX.Play();
        OnButtonPressed_snd -= ButtonPressedSND;
    }
}
