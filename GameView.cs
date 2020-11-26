using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Button[] buttons;
    public Text[] texts;
    public Image[] images;

    public void Enabled(bool set) { gameObject.SetActive(set); }
    public void Interactable(bool set)
    {
        foreach (Button b in buttons)
            b.interactable = set;
    }
}
