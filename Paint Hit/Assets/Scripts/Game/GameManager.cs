using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public static bool isTimeSlow;
    public static Transform background;
    private static Transform gameOverScreen;

    private static Button restartButton;
    private static Button menuButton;

    private static Transform startLevelScreen;
    private static Transform levelCompleteScreen;

    private static bool died;
    public static bool finishedCurrentLevel;

    private void Awake() {
        Instance = this;

        died = false;
        finishedCurrentLevel = false;
        Time.timeScale = 1f;

        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(() => {
            Restart();
        });

        menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
        menuButton.onClick.AddListener(() => {
            BackToMenu();
        });

        gameOverScreen = GameObject.Find("GameOverScreen").transform;
        gameOverScreen.gameObject.SetActive(false);

        levelCompleteScreen = GameObject.Find("LevelCompleteScreen").transform;
        levelCompleteScreen.gameObject.SetActive(false);


        background = GameObject.Find("Background").transform;

        PlayerPrefs.SetInt("TempRemainingSkillCount", PlayerPrefs.GetInt("RemainingSkillCount"));

        startLevelScreen = GameObject.Find("StartLevelScreen").transform;
        if (PlayerPrefs.GetInt("LastLevel") % 5 != 0) {
            startLevelScreen.Find("LevelText").GetComponent<Text>().text = "Level " + PlayerPrefs.GetInt("LastLevel");
        }
        else {
            startLevelScreen.Find("LevelText").GetComponent<Text>().text = "BOSS";
        }
        Destroy(startLevelScreen.gameObject, 1.3f); //Delete animation after complete.
    }

    private void Start() {
        UIManager.RefreshSkillUI();
    }

    public static void Skill() {
        if (isTimeSlow) {       //If user has selected the skill but clicked second time to quit skill.
            background.GetComponent<Animator>().Play("SkillFadeOut");
            GameObject.Find("ballSpawnPoint").GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            isTimeSlow = false;
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("TempRemainingSkillCount", PlayerPrefs.GetInt("TempRemainingSkillCount") + 1);
            UIManager.RefreshSkillUI();
        }
        else {       //If user has selected the skill but not has not yet used
            background.GetComponent<Animator>().Play("SkillFadeIn");
            GameObject.Find("ballSpawnPoint").GetComponent<MeshRenderer>().material.color = new Color32(0, 77, 57, 255);
            isTimeSlow = true;
            Time.timeScale = 0.5f;
            PlayerPrefs.SetInt("TempRemainingSkillCount", PlayerPrefs.GetInt("TempRemainingSkillCount") - 1);
            UIManager.RefreshSkillUI();
        }
    }

    public static void UseSkill() {
        background.GetComponent<Animator>().Play("SkillFadeOut");
        GameObject.Find("ballSpawnPoint").GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
        isTimeSlow = false;
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("RemainingSkillCount", PlayerPrefs.GetInt("RemainingSkillCount") - 1);
        UIManager.RefreshSkillUI();
    }

    public static void SetRemainingSkillCount(int remainingSkillCount) {
        PlayerPrefs.SetInt("RemainingSkillCount", remainingSkillCount);
        PlayerPrefs.SetInt("TempRemainingSkillCount", remainingSkillCount);
    }

    public static int GetRemainingSkillCount() {
        return PlayerPrefs.GetInt("RemainingSkillCount");
    }

    public static int GetTempRemainingSkillCount() {
        return PlayerPrefs.GetInt("TempRemainingSkillCount");
    }

    public static void GetPoint() {
        PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") + 5);
    }

    public static void Continue() {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt("Heart", 1);
        UIManager.RefreshHeartsUI();

        BallHandler.SetCurrentBallsCount(BallHandler.GetCurrentBallsCount() + 1);
        UIManager.RefreshBallsUI();

        BallHandler.GetLastPaintedArea().GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();

        gameOverScreen.gameObject.SetActive(false);
    }

    public static void LoseHeart() {
        if (PlayerPrefs.GetInt("Heart") == 2) {
            PlayerPrefs.SetInt("Heart", 1);
            UIManager.RefreshHeartsUI();

            BallHandler.SetCurrentBallsCount(BallHandler.GetCurrentBallsCount() + 1);
            UIManager.RefreshBallsUI();

            BallHandler.GetLastPaintedArea().GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
        }
        else {
            PlayerPrefs.SetInt("Heart", 0);
            UIManager.RefreshHeartsUI();
            GameOver();
        }
    }

    public static void GameOver() {
        Instance.StartCoroutine(Instance.GameOverDelay());
    }

    private IEnumerator GameOverDelay() {
        BallHandler.canShoot = false;
        yield return new WaitForSeconds(0.5f);
        if (!died) {
            gameOverScreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
            died = true;
            BallHandler.canShoot = true;
        }
        else {
            gameOverScreen.gameObject.SetActive(true);
            gameOverScreen.Find("Countdown").gameObject.SetActive(false);
            gameOverScreen.Find("GameOverText").gameObject.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    public static void Restart() {
        PlayerPrefs.SetInt("Attempts", PlayerPrefs.GetInt("Attempts") + 1);
        PlayerPrefs.SetInt("Fails", PlayerPrefs.GetInt("Fails") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void NextLevel() {
        finishedCurrentLevel = true;
        Instance.StartCoroutine(Instance.NextLevelDelay());
    }

    private IEnumerator NextLevelDelay() {
        PlayerPrefs.SetInt("Attempts", PlayerPrefs.GetInt("Attempts") + 1);
        PlayerPrefs.SetInt("Successes", PlayerPrefs.GetInt("Successes") + 1);
        if (PlayerPrefs.GetInt("LastLevel") % 5 == 0) {
            PlayerPrefs.SetInt("BossesBeaten", PlayerPrefs.GetInt("BossesBeaten") + 1);
        }
        levelCompleteScreen.gameObject.SetActive(true);
        levelCompleteScreen.GetComponent<Animator>().Play("LevelComplete", -1, 0f);
        levelCompleteScreen.Find("LevelText").GetComponent<Text>().text = "Level " + PlayerPrefs.GetInt("LastLevel");
        PlayerPrefs.SetInt("LastLevel", PlayerPrefs.GetInt("LastLevel") + 1);
        yield return new WaitForSeconds(2f);

        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
        SceneManager.LoadScene("Menu");
    }

    public static void BackToMenu() {
        PlayerPrefs.SetInt("Attempts", PlayerPrefs.GetInt("Attempts") + 1);
        PlayerPrefs.SetInt("Fails", PlayerPrefs.GetInt("Fails") + 1);

        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
        SceneManager.LoadScene("Menu");
    }
}
