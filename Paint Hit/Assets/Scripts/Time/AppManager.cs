using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AppManager : MonoBehaviour {

    private DateTime currentDate;
    private DateTime oldDate;
    private TimeSpan difference;

    private void OnApplicationFocus(bool focus) {
        if(!focus && !Advertisement.isShowing) {
            PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
        }
        else if (focus) {
            currentDate = DateTime.Now;
            long temp = Convert.ToInt64(PlayerPrefs.GetString("QuitTime"));
            oldDate = DateTime.FromBinary(temp);
            difference = currentDate.Subtract(oldDate);
            PlayerPrefs.SetFloat("RemainingExtraLifeTime", PlayerPrefs.GetFloat("RemainingExtraLifeTime") - (float)difference.TotalSeconds);
        }
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
        }
    }


    void OnApplicationQuit() {
        //Save the current system time as a string in the player prefs class
        PlayerPrefs.SetString("QuitTime", System.DateTime.Now.ToBinary().ToString());
    }
}
