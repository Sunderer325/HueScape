using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuViewManager : MonoBehaviour
{
    public MenuView mainMenu;
    public MenuView store;
    public MenuView leaderboard;
    public MenuView play;
    public MenuView settings;
    MenuView activeUI, nextUI;

    public GameObject fader;
    FadeEvent fadeEvent;

    public int gameSceneIndex;
    MenuState state;

    AudioManager audioManager;
    SettingsManager settingsManager;
    Ads ads;

    public void ChangeMenuView(MenuState nextState)
    {
        audioManager.Play(ClipType.Click);
        switch (nextState)
        {
            case MenuState.MainMenu:
                nextUI = mainMenu;
                break;
            case MenuState.Store:
                nextUI = store;
                break;
            case MenuState.Leaderboard:
                nextUI = leaderboard;
                break;
            case MenuState.Settings:
                nextUI = settings;
                break;
        }

        if (!activeUI)
        {
            activeUI = nextUI;
            activeUI.Interactable(false);
            return;
        }

        fader.SetActive(true);
        fader.GetComponent<Animator>().SetTrigger("FadeToBlack");
        activeUI.Interactable(false);
    }
    public void OnPlay()
    {
        audioManager.Play(ClipType.Whoosh);
        ChangeMenuView(MenuState.Play);
        state = MenuState.Play;
    }
    public void OnLeaderboard(){
        GPLeaderboard.OpenLeaderboard();
    }
    public void OnShop()
    {
        audioManager.Play(ClipType.Whoosh);
        ChangeMenuView(MenuState.Store);
        state = MenuState.Store;

        ads.RequestBanner();
    }
    public void OnMenu()
    {
        if(state == MenuState.Settings)
        {
            settingsManager.Save();
            if (!settingsManager.soundFlag)
                audioManager.StopMusic();
            else audioManager.Play(ClipType.Music);
        }
        audioManager.Play(ClipType.Whoosh);
        ChangeMenuView(MenuState.MainMenu);
        state = MenuState.MainMenu;

        ads.DestroyBannerAd();
    }
    public void OnSettings()
    {
        audioManager.Play(ClipType.Whoosh);
        ChangeMenuView(MenuState.Settings);
        state = MenuState.Settings;


        ads.RequestBanner();
    }
    private void Awake()
    {
        Time.timeScale = 1;
        activeUI = mainMenu;
        state = MenuState.MainMenu;
        fadeEvent = fader.GetComponent<FadeEvent>();
        fadeEvent.OnFadeIsFull += FadeFull;
        fadeEvent.OnFadeIsEnd += FadeEnd;
        fader.GetComponent<Animator>().Play("AnimScreenOut");

        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play(ClipType.WhooshReverse);
        settingsManager = FindObjectOfType<SettingsManager>();
        if (settingsManager.soundFlag)
            audioManager.Play(ClipType.Music);

        ads = FindObjectOfType<Ads>();
    }
    private void SetActiveUI(MenuView view)
    {
        activeUI = view;
        ViewChanged();
    }
    private void FadeFull()
    {
        activeUI.Interactable(false);
        activeUI.Enabled(false);
        nextUI?.Enabled(true);
        nextUI?.Interactable(true);
        SetActiveUI(nextUI);
        audioManager.Play(ClipType.WhooshReverse);
    }
    private void FadeEnd()
    {
        activeUI?.Interactable(true);
        fader.SetActive(false);
        
    }

    private void ViewChanged()
    {
        switch (state)
        {
            case MenuState.Play:
                SceneManager.LoadScene(gameSceneIndex);
                break;
            case MenuState.Store:
                store.GetComponent<Shop>();
                break;
        }
    }
}

public enum MenuState
{
    MainMenu,
    Store,
    Leaderboard,
    Play,
    Settings
}