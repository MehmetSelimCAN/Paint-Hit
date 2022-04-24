using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    private void Awake() {
        if (!PlayerPrefs.HasKey("RemainingExtraLifeTime")) PlayerPrefs.SetFloat("RemainingExtraLifeTime", 0);
        if (!PlayerPrefs.HasKey("TotalTime")) PlayerPrefs.SetFloat("TotalTime", 0);
        if (!PlayerPrefs.HasKey("ExtraLife")) PlayerPrefs.SetInt("ExtraLife", 0);
        if (!PlayerPrefs.HasKey("QuestLevel")) PlayerPrefs.SetInt("QuestLevel", 5);
        if (!PlayerPrefs.HasKey("LastLevel")) PlayerPrefs.SetInt("LastLevel", 1);
        if (!PlayerPrefs.HasKey("Attempts")) PlayerPrefs.SetInt("Attempts", 0);
        if (!PlayerPrefs.HasKey("Successes")) PlayerPrefs.SetInt("Successes", 0);
        if (!PlayerPrefs.HasKey("Fails")) PlayerPrefs.SetInt("Fails", 0);
        if (!PlayerPrefs.HasKey("BossesBeaten")) PlayerPrefs.SetInt("BossesBeaten", 0);
        if (!PlayerPrefs.HasKey("PaintballsFired")) PlayerPrefs.SetInt("PaintballsFired", 0);
        if (!PlayerPrefs.HasKey("LayersFilled")) PlayerPrefs.SetInt("LayersFilled", 0);
        if (!PlayerPrefs.HasKey("ChestsOpened")) PlayerPrefs.SetInt("ChestsOpened", 0);
        if (!PlayerPrefs.HasKey("ColorCardNumber")) PlayerPrefs.SetInt("ColorCardNumber", 1);
        if (!PlayerPrefs.HasKey("Card1")) PlayerPrefs.SetInt("Card1", 1);
        if (!PlayerPrefs.HasKey("Heart")) PlayerPrefs.SetInt("Heart", 1);
        if (!PlayerPrefs.HasKey("Point")) PlayerPrefs.SetInt("Point", 0);
        if (!PlayerPrefs.HasKey("RemainingSkillCount")) PlayerPrefs.SetInt("RemainingSkillCount", 5);
        if (!PlayerPrefs.HasKey("TempRemainingSkillCount")) PlayerPrefs.SetInt("TempRemainingSkillCount", 5);
        if (!PlayerPrefs.HasKey("QuitTime")) PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
    }
}
