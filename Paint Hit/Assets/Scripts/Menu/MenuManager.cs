using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    #region Statistics
    private static Text attempts;
    private static Text successes;
    private static Text fails;
    private static Text bossesBeaten;
    private static Text paintballsFired;
    private static Text layersFilled;
    private static Text chestsOpened;

    private static Text timeInGame;
    private static TimeSpan timeSpan;
    #endregion

    private static Transform extraLifed;
    private static Transform notExtraLifed;
    private static Text remainingExtraLifeTime;

    private static Text point;

    private static Button playButton;
    private static Text playButtonText;

    private static Sprite blueBackground;
    private static Sprite redBackground;

    private void Awake() {
        #region Statistics
        attempts = GameObject.Find("StatisticsPage").transform.Find("Attempts").Find("text").GetComponent<Text>();
        successes = GameObject.Find("StatisticsPage").transform.Find("Successes").Find("text").GetComponent<Text>();
        fails = GameObject.Find("StatisticsPage").transform.Find("Fails").Find("text").GetComponent<Text>();
        bossesBeaten = GameObject.Find("StatisticsPage").transform.Find("BossesBeaten").Find("text").GetComponent<Text>();
        paintballsFired = GameObject.Find("StatisticsPage").transform.Find("PaintballsFired").Find("text").GetComponent<Text>();
        layersFilled = GameObject.Find("StatisticsPage").transform.Find("LayersFilled").Find("text").GetComponent<Text>();
        chestsOpened = GameObject.Find("StatisticsPage").transform.Find("ChestsOpened").Find("text").GetComponent<Text>();
        timeInGame = GameObject.Find("StatisticsPage").transform.Find("TimeInGame").Find("text").GetComponent<Text>();
        #endregion

        extraLifed = GameObject.Find("HomePage").transform.Find("ExtraLife").Find("ExtraLifed");
        notExtraLifed = GameObject.Find("HomePage").transform.Find("ExtraLife").Find("NotExtraLifed");
        remainingExtraLifeTime = extraLifed.Find("time").GetComponent<Text>();


        point = GameObject.Find("Point").transform.Find("text").GetComponent<Text>();

        playButton = GameObject.Find("HomePage").transform.Find("PlayButton").GetComponent<Button>();
        playButtonText = GameObject.Find("HomePage").transform.Find("PlayButton").Find("text").GetComponent<Text>();

        blueBackground = Resources.Load<Sprite>("Sprites/spEmptyBlue");
        redBackground = Resources.Load<Sprite>("Sprites/spEmptyRed");

        playButton.onClick.AddListener(() => {
            Play();
        });
    }

    private void Start() {
        if (PlayerPrefs.GetInt("ExtraLife") == 1) {
            notExtraLifed.gameObject.SetActive(false);
            extraLifed.gameObject.SetActive(true);
        }
        else {
            notExtraLifed.gameObject.SetActive(true);
            extraLifed.gameObject.SetActive(false);
        }

        RefreshStatisticsMenuUI();
        RefreshExtraLifeTimeUI();
        RefreshPlayButtonUI();
        RefreshPointUI();
    }

    private void Update() {
        if (PlayerPrefs.GetInt("ExtraLife") == 1) { //If user has an extra life, count down always refreshing.
            RefreshExtraLifeTimeUI();
        }
    }
    
    private static void Play() {
        if (PlayerPrefs.GetInt("LastLevel") % 5 != 0) {
            SceneManager.LoadScene("NormalLevel");
        }
        else {
            SceneManager.LoadScene("Boss" + (PlayerPrefs.GetInt("LastLevel") / 5) + "Level");
        }
    }

    private static void RefreshPlayButtonUI() {
        if (PlayerPrefs.GetInt("LastLevel") % 5 != 0) {
            playButtonText.text = "Level " + PlayerPrefs.GetInt("LastLevel");
            playButton.GetComponent<Image>().sprite = blueBackground;
        }
        else {
            playButtonText.text = "BOSS";
            playButton.GetComponent<Image>().sprite = redBackground;
        }
    }

    public static void RefreshPointUI() {
        point.text = PlayerPrefs.GetInt("Point").ToString();
    }

    public static void RefreshStatisticsMenuUI() {
        attempts.text = PlayerPrefs.GetInt("Attempts").ToString();
        successes.text = PlayerPrefs.GetInt("Successes").ToString();
        fails.text = PlayerPrefs.GetInt("Fails").ToString();
        bossesBeaten.text = PlayerPrefs.GetInt("BossesBeaten").ToString();
        paintballsFired.text = PlayerPrefs.GetInt("PaintballsFired").ToString();
        layersFilled.text = PlayerPrefs.GetInt("LayersFilled").ToString();
        chestsOpened.text = PlayerPrefs.GetInt("ChestsOpened").ToString();

        timeSpan = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("TotalTime"));
        timeInGame.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private static void RefreshExtraLifeTimeUI() {
        timeSpan = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("RemainingExtraLifeTime"));
        remainingExtraLifeTime.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        if (PlayerPrefs.GetFloat("RemainingExtraLifeTime") <= 0f) {
            ExtraLifeEnd();
        }
    }

    public static void GetExtraLife() {
        notExtraLifed.gameObject.SetActive(false);
        extraLifed.gameObject.SetActive(true);
        PlayerPrefs.SetInt("ExtraLife", 1);
        PlayerPrefs.SetFloat("RemainingExtraLifeTime", 600);
        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
    }

    public static void ExtraLifeEnd() {
        notExtraLifed.gameObject.SetActive(true);
        extraLifed.gameObject.SetActive(false);
        PlayerPrefs.SetInt("ExtraLife", 0);

    }
}
