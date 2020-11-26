using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void Save(){
        PlayerPrefs.SetString("savestring", "");
    }

    public void Load(){
        if(PlayerPrefs.HasKey("savestring")){
            
        }
    }
}
