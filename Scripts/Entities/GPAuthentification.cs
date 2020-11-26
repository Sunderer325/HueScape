using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPAuthentification : MonoBehaviour
{
    public static PlayGamesPlatform platform;
    void Start()
    {
        if(platform == null){
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success =>{
            if(success){
                Debug.Log("logged");
            }
            else{
                Debug.Log("login is failed");
            }
        });
    }
}
