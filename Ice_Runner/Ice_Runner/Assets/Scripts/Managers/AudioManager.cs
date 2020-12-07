using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(0.35f) * 20);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(volume) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }

    public void MuteVolume()
    {
        UI_Manager.sharedInstance.volumeBar.value = 0.0001f;
        audioMixer.SetFloat("MainVolume", Mathf.Log10(0.0001f) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }
}
