using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public bool soundFlag = true;
    public bool vibrationFlag = true;

    private void Awake()
    {
        Load();
    }
    public bool ChangeSoundFlag()
    {
        soundFlag ^= true;
        return soundFlag;
    }

    public bool ChangeVibrationFlag()
    {
        vibrationFlag ^= true;
        return vibrationFlag;
    }

    public void Load()
    {
        soundFlag = PlayerPrefs.GetInt("SoundFlag", 1) == 1 ? true : false;
        vibrationFlag = PlayerPrefs.GetInt("VibrationFlag", 1) == 1 ? true : false;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("SoundFlag", soundFlag ? 1:0);
        PlayerPrefs.SetInt("VibrationFlag", vibrationFlag ? 1:0);
    }
}