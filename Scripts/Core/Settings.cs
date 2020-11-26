using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    SettingsManager manager;
    public Button vibration;
    public Button sound;
    public Sprite check;
    public Sprite uncheck;
    private void Awake()
    {
        manager = FindObjectOfType<SettingsManager>();

        if (manager.vibrationFlag)
            vibration.GetComponent<Image>().sprite = check;
        else vibration.GetComponent<Image>().sprite = uncheck;
        if (manager.soundFlag)
            sound.GetComponent<Image>().sprite = check;
        else sound.GetComponent<Image>().sprite = uncheck;
    }

    public void ChangeSoundFlag()
    {
        bool soundFlag = manager.ChangeSoundFlag();
        if (soundFlag)
            sound.GetComponent<Image>().sprite = check;
        else sound.GetComponent<Image>().sprite = uncheck;
    }

    public void ChangeVibrationFlag()
    {
        bool vibrationFlag = manager.ChangeVibrationFlag();
        if (vibrationFlag)
            vibration.GetComponent<Image>().sprite = check;
        else vibration.GetComponent<Image>().sprite = uncheck;
    }
}
