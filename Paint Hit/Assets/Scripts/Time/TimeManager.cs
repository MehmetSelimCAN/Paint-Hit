using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private void Update() {
        if (PlayerPrefs.GetInt("ExtraLife") == 1) {
            PlayerPrefs.SetFloat("RemainingExtraLifeTime", PlayerPrefs.GetFloat("RemainingExtraLifeTime") - Time.deltaTime);
            if (PlayerPrefs.GetFloat("RemainingExtraLifeTime") < 0f) {
                MenuManager.ExtraLifeEnd();
            }
        }

        PlayerPrefs.SetFloat("TotalTime", PlayerPrefs.GetFloat("TotalTime") + Time.deltaTime);
    }
}
