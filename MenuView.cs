using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    public Button[] buttons;

    public void Enabled(bool set) { gameObject.SetActive(set); }
    public void Interactable(bool set)
    {
        foreach (Button b in buttons)
            b.interactable = set;
    }
}
