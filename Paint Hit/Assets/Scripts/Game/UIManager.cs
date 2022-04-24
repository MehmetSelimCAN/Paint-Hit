using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager Instance { get; private set; }

    private static Transform skillUI;
    private static Transform circleCountUI;
    private static Transform heartsUI;
    private static Transform pointsUI;
    private static Transform ballsUI;
    private static Button pauseGameButton;
    private static Button quitButton;
    private static Button resumeButton;
    private static Button resumeButtonArea1;
    private static Button resumeButtonArea2;
    private static Transform pauseGameMenu;
    private static Button skillButton;
    private static Button openSkillBuyMenuButton;
    private static Button buySkillButton;
    private static Transform skillBuyMenu;

    private static bool gameIsPaused;

    private void Awake() {
        Instance = this;

        skillUI = GameObject.Find("Skill").transform;
        circleCountUI = GameObject.Find("CircleCount").transform;
        heartsUI = GameObject.Find("Hearts").transform;
        pointsUI = GameObject.Find("Points").transform;
        ballsUI = GameObject.Find("Balls").transform;


        skillButton = GameObject.Find("SkillButton").GetComponent<Button>();
        skillButton.onClick.AddListener(() => {
            GameManager.Skill();
        });

        skillBuyMenu = GameObject.Find("SkillBuyMenu").transform;
        openSkillBuyMenuButton = GameObject.Find("OpenSkillBuyMenuButton").GetComponent<Button>();
        openSkillBuyMenuButton.onClick.AddListener(() => {
            OpenSkillBuyMenu();
        });

        buySkillButton = GameObject.Find("BuySkillButton").GetComponent<Button>();
        buySkillButton.onClick.AddListener(() => {
            BuySkill();
        });

        pauseGameMenu = GameObject.Find("PauseGameMenu").transform;
        pauseGameButton = GameObject.Find("PauseButton").GetComponent<Button>();
        pauseGameButton.onClick.AddListener(() => {
            PauseGame();    
        });

        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.AddListener(() => {
            BackToMenu();
        });

        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(() => {
            ResumeGame();
        });

        resumeButtonArea1 = skillBuyMenu.Find("ResumeButtonArea").GetComponent<Button>();
        resumeButtonArea1.onClick.AddListener(() => {
            ResumeGame();
        });

        resumeButtonArea2 = pauseGameMenu.Find("ResumeButtonArea").GetComponent<Button>();
        resumeButtonArea2.onClick.AddListener(() => {
            ResumeGame();
        });

        openSkillBuyMenuButton.gameObject.SetActive(false);
        skillBuyMenu.gameObject.SetActive(false);
        pauseGameMenu.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("ExtraLife") == 1) {
            PlayerPrefs.SetInt("Heart", 2);
        }
        else {
            PlayerPrefs.SetInt("Heart", 1);
        }
    }

    private void Start() {
        gameIsPaused = false;
        RefreshHeartsUI();
        RefreshPointUI();
        ResetBallsUI();
        RefreshSkillUI();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public static void RefreshHeartsUI() {
        for (int i = 0; i < 2; i++) {
            heartsUI.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("Heart"); i++) {
            heartsUI.GetChild(i).gameObject.SetActive(true);
        }
    }

    public static void RefreshBallsUI() {
        for (int i = BallHandler.GetMaximumBallsCount(); i > BallHandler.GetCurrentBallsCount(); i--) {
            if (PlayerPrefs.GetInt("ColorCardNumber") == 12) {          //If the color card is black
                ballsUI.GetChild(i-1).GetComponent<Image>().color = new Color(1,1,1,0.6f);
            }
            else {
                ballsUI.GetChild(i-1).GetComponent<Image>().color = new Color(0,0,0,0.6f);
            }
        }

        for (int i = BallHandler.GetCurrentBallsCount(); i > 0; i--) {
            ballsUI.GetChild(i - 1).GetComponent<Image>().color = ColorManager.Instance.GetCurrentColor();
        }
    }

    public static void ResetBallsUI() {
        Instance.StartCoroutine(BallsUIAnimation());
    }

    public static IEnumerator BallsUIAnimation() {
        for (int i = BallHandler.GetMaximumBallsCount(); i < ballsUI.childCount; i++) {
            ballsUI.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < BallHandler.GetMaximumBallsCount(); i++) {
            ballsUI.GetChild(i).gameObject.SetActive(true);
            ballsUI.GetChild(i).GetComponent<Image>().color = ColorManager.Instance.GetCurrentColor();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public static void RefreshPointUI() {
        pointsUI.GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("Point").ToString();
    }

    public static void RefreshCircleCountUI() {
        circleCountUI.Find("currentCircleText").GetComponent<Text>().text = CircleCounter.GetCurrentCircleNumber().ToString();
        circleCountUI.Find("totalCircleText").GetComponent<Text>().text = "/ " + CircleCounter.GetMaximumCircleCount();
    }

    public static void RefreshSkillUI() {
        skillUI.Find("SkillButton").Find("text").GetComponent<Text>().text = GameManager.GetTempRemainingSkillCount().ToString();
        if (GameManager.GetRemainingSkillCount() == 0) {
            skillUI.Find("SkillButton").gameObject.SetActive(false);
            skillUI.Find("OpenSkillBuyMenuButton").gameObject.SetActive(true);
        }
        else {
            skillUI.Find("SkillButton").gameObject.SetActive(true);
            skillUI.Find("OpenSkillBuyMenuButton").gameObject.SetActive(false);
        }
    }

    private static void OpenSkillBuyMenu() {
        skillBuyMenu.gameObject.SetActive(true);
    }

    private static void BuySkill() {
        if (PlayerPrefs.GetInt("Point") > 5) {
            PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") - 5);
            GameManager.SetRemainingSkillCount(3);
            skillUI.Find("SkillButton").gameObject.SetActive(true);
            skillUI.Find("OpenSkillBuyMenuButton").gameObject.SetActive(false);

            RefreshSkillUI();
            RefreshPointUI();
            ResumeGame();
        }
    }

    private static void PauseGame() {
        skillUI.gameObject.SetActive(false);
        ballsUI.gameObject.SetActive(false);
        heartsUI.gameObject.SetActive(false);
        circleCountUI.gameObject.SetActive(false);
        pauseGameButton.gameObject.SetActive(false);
        skillBuyMenu.gameObject.SetActive(false);
        pauseGameMenu.gameObject.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    private static void ResumeGame() {
        skillUI.gameObject.SetActive(true);
        ballsUI.gameObject.SetActive(true);
        heartsUI.gameObject.SetActive(true);
        circleCountUI.gameObject.SetActive(true);
        pauseGameButton.gameObject.SetActive(true);
        skillBuyMenu.gameObject.SetActive(false);
        pauseGameMenu.gameObject.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    private static void BackToMenu() {
        PlayerPrefs.SetInt("Attempts", PlayerPrefs.GetInt("Attempts") + 1);
        PlayerPrefs.SetInt("Fails", PlayerPrefs.GetInt("Fails") + 1);
        SceneManager.LoadScene("Menu");
    }

}
