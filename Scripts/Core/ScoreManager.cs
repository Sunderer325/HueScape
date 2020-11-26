using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int score;
    int longestRide;

    public int GetLongestRide(){
        return longestRide;
    }
    public void SetLongestRide(int longestRide){
        this.longestRide = longestRide;
        PlayerPrefs.SetInt("LongestRide", longestRide);
    }

    public int GetScore() 
    {
        Load();
        return score; 
    }
    public void SetScore(int _score) { 
        score = _score; 
        PlayerPrefs.SetInt("ScoreToUpdate", score);
    }

    public void Save(){
        PlayerPrefs.Save();
    }
    public void Load(){
        if(PlayerPrefs.HasKey("ScoreToUpdate"))
            score = PlayerPrefs.GetInt("ScoreToUpdate",0);
        if(PlayerPrefs.HasKey("LongestRide"))
            longestRide = PlayerPrefs.GetInt("LongestRide", 0);
    }
}
