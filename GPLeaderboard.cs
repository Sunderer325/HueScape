using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPLeaderboard
{
    public static  void OpenLeaderboard(){
        if(Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
    }

    public static void UpdateLeaderboardScore(){
        if(!Social.localUser.authenticated)
            return;
        int score = PlayerPrefs.GetInt("LongestRide", 0);
        if(score == 0)
            return;
        
        Social.ReportScore(score, GPGSIds.leaderboard_leaderboard, (bool success) =>{
            if(success)
                Debug.Log("Score is stored");
        });
    }
}
