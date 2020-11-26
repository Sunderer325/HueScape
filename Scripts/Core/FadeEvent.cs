using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEvent : MonoBehaviour
{
    public Action OnFadeIsFull, OnFadeIsEnd;
    public void FadeIsFull()
    {
        OnFadeIsFull();
    }
    public void FadeIsEnd()
    {
        OnFadeIsEnd();
    }
}
