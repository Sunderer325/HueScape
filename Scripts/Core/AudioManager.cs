using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource click;
    AudioSource paintWall;
    AudioSource stack;
    AudioSource stackReverb;
    AudioSource death;
    AudioSource music;
    AudioSource bwMode;
    AudioSource whoosh;
    AudioSource whooshReverse;
    AudioSource buy;
    public AudioClip clickClip;
    public AudioClip paintWallClip;
    public AudioClip stackClip;
    public AudioClip stackReverbClip;
    public AudioClip deathClip;
    public AudioClip musicClip;
    public AudioClip bwModeClip;
    public AudioClip whooshClip;
    public AudioClip whooshReverseClip;
    public AudioClip buyClip;

    SettingsManager settings;

    void Awake(){
        settings = FindObjectOfType<SettingsManager>();

        click = gameObject.AddComponent<AudioSource>();
        click.volume = 0.5f;
        click.clip = clickClip;

        paintWall = gameObject.AddComponent<AudioSource>();
        paintWall.volume =0.15f;
        paintWall.clip = paintWallClip;

        stack = gameObject.AddComponent<AudioSource>();
        stack.volume =0.15f;
        stack.clip = stackClip;

        stackReverb = gameObject.AddComponent<AudioSource>();
        stackReverb.volume =0.25f;
        stackReverb.clip = stackReverbClip;

        death = gameObject.AddComponent<AudioSource>();
        death.volume = 0.2f;
        death.clip = deathClip;

        music = gameObject.AddComponent<AudioSource>();
        music.loop = true; 
        music.volume = 0.1f;
        music.clip = musicClip;

        bwMode = gameObject.AddComponent<AudioSource>();
        bwMode.loop = true;
        bwMode.volume = 0.1f;
        bwMode.clip = bwModeClip;

        whoosh = gameObject.AddComponent<AudioSource>();
        whoosh.volume= 1.0f;
        whoosh.clip = whooshClip;

        whooshReverse = gameObject.AddComponent<AudioSource>();
        whooshReverse.volume= 1.0f;
        whooshReverse.clip = whooshReverseClip;

        buy = gameObject.AddComponent<AudioSource>();
        buy.volume =0.2f;
        buy.clip = buyClip;
    }

    public void Reload()
    {
        if (!settings.soundFlag)
            return;

        click.volume = 0.5f;
        click.pitch = 1;

        paintWall.volume = 0.15f;
        paintWall.pitch = 1;

        stack.volume = 0.15f;
        stack.pitch = 1;

        stackReverb.volume = 0.25f;
        stackReverb.pitch = 1;

        death.volume = 0.2f;
        death.pitch = 1;

        music.loop = true;
        music.volume = 0.1f;
        music.pitch = 1;

        bwMode.loop = true;
        bwMode.volume = 0.1f;
        bwMode.pitch = 1;

        whoosh.volume = 1.0f;
        whoosh.pitch = 1;

        whooshReverse.volume = 1.0f;
        whooshReverse.pitch = 1;

        buy.volume = 0.2f;
        buy.pitch = 1;
    }
    
    public void Play(ClipType type){
        if (!settings.soundFlag)
            return;

        switch (type){
            case ClipType.Click:
                click.Play();
            break;
            case ClipType.PaintWall:
                paintWall.Play();
            break;
            case ClipType.Stack:
                stack.pitch = Mathf.Pow(1.05946f, Random.Range(-6, 5));
                stack.Play();
            break;
            case ClipType.StackReverb:
                stackReverb.pitch = Mathf.Pow(1.05946f, Random.Range(-6, 5));
                stackReverb.Play();
            break;
            case ClipType.Music:
                bwMode.Stop();
                music.Play();
            break;
            case ClipType.Death:
                death.Play();
            break;
            case ClipType.BWMode:
                music.Stop();
                bwMode.Play();
            break;
            case ClipType.Whoosh:
                whoosh.PlayDelayed(0.5f);
            break;
            case ClipType.WhooshReverse:
                whoosh�PNG

   IHDR         ��h6  �IDAT(%��/�aǗ;Ú�M*eJ���XZ����j�8��7uԩӼS34�Z&�lFkb��1��3�������/���󭭭555���
���玎�D"188����L&���F��ٙR���b����d2���h4����������\.'�JWVVz{{��׹^__~~�\.���8�H$����):>>nll���b ����UWW///Kfgg���t:]gg�\.�����뫩���`0��럞��{www�@��
�=�agg���������M�ն�������R)��x�{qqqxxx{{�(}�����饥���R�R[����/V444�ROO��ƆJ��f�������������p(�ŀA�����htbbb�???��"��=�0��榤����kmm-E333p�qdddsss