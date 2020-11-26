using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameViewManager : MonoBehaviour
{
    public GameView playUI;
    public GameView menuUI;
    public GameView gameOverUI;

    public Action OnViewChange, OnInvertColors;
    GameView activeUI, nextUI;

    public GameObject fader;
    public GameObject whiteFader;
    FadeEvent fadeEvent;
    FadeEvent whiteFadeEvent;
    bool invertColors;

    public AudioManager audioManager;

    public void ChangeGameView(GameStates state)
    {
        switch (state)
        {
            case GameStates.Play:
                nextUI = playUI;
                break;
            case GameStates.MainMenu:
                nextUI = menuUI;
                break;
            case GameStates.GameOver:
                nextUI = gameOverUI;
                break;
        }

        if (!activeUI)
        {
            activeUI = nextUI;
            activeUI.Interactable(true);
            return;
        }

        fader.SetActive(true);
        fader.GetComponent<Animator>().SetTrigger("FadeToBlack");
        activeUI.Interactable(false);
    }
    public void InvertColorUI()
    {
        invertColors = true;
        whiteFader.SetActive(true);
        whiteFader.GetComponent<Animator>().SetTrigger("FadeToWhite");
    }
    private void Invert()
    {
        audioManager.Play(ClipType.WhooshReverse);
        foreach (Button b in activeUI.buttons)
        {
            ColorBlock colors = b.GetComponent<Button>().colors;
            colors.normalColor = new Color(1.0f - colors.normalColor.r, 1.0f - colors.normalColor.g, 1.0f - colors.normalColor.b, colors.normalColor.a);
            b.GetComponent<Button>().colors = colors;
        }
        foreach (Text t in activeUI.texts)
        {
            t.color = new Color(1.0f - t.color.r, 1.0f - t.color.g, 1.0f - t.color.b, t.color.a);
        }
        foreach (Image i in activeUI.images)
        {
            i.color = new Color(1.0f - i.color.r, 1.0f - i.color.g, 1.0f - i.color.b, i.color.a);
        }
        invertColors = false;
        OnInvertColors();
    }
    private void Awake()
    {
        fadeEvent = fader.GetComponent<FadeEvent>();
        fadeEvent.OnFadeIsFull += FadeFull;
        fadeEvent.OnFadeIsEnd += FadeEnd;
        whiteFadeEvent = whiteFader.GetComponent<FadeEvent>();
        whiteFadeEvent.OnFadeIsFull += FadeFull;
        whiteFadeEvent.OnFadeIsEnd += FadeEnd;
        fader.GetComponent<Animator>().Play("AnimScreenOut");
    }
    private void SetActiveUI(GameView view)
    {
        activeUI = view;
        OnViewChange();
    }
    private void FadeFull()
    {
        if (invertColors)
        {
            Invert();
            return;
        }
        activeUI.Interactable(false);
        activeUI.Enabled(false);
        nextUI?.Enabled(true);
        nextUI?.Interactable(true);
        SetActiveUI(nextUI);
    }
    private void FadeEnd()
    {
        //activeUI.Interactable(true);
        fader.SetActive(false);
        whiteFader.SetActive(false);
    }
}
