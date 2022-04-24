using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallHandler : MonoBehaviour {

    private static Transform ballSpawnPoint;
    private Transform ballPrefab;
    private float ballForce = 200f;
    public static bool canShoot;

    private static Button shootButton;
    private static Transform shootText;

    private static int maximumBallsCount;
    private static int currentBallsCount;

    private static Collider lastPaintedArea;

    private void Awake() {
        canShoot = true;


        ballPrefab = Resources.Load<Transform>("Prefabs/pfBall");
        ballSpawnPoint = GameObject.Find("ballSpawnPoint").transform;
        PaintBallSpawnPoint();


        shootText = GameObject.Find("ShootText").transform;
        shootButton = GameObject.Find("ShootButton").GetComponent<Button>();
        shootButton.onClick.AddListener( () => {
            if (canShoot && currentBallsCount > 0) {
                Shoot();
            }
        });
    }

    private void Shoot() {
        PlayerPrefs.SetInt("PaintballsFired", PlayerPrefs.GetInt("PaintballsFired") + 1);

        if (shootText.gameObject.activeInHierarchy) {
            shootText.gameObject.SetActive(false);
        }

        if (GameManager.isTimeSlow) {
            GameManager.UseSkill();
        }

        Transform ball = Instantiate(ballPrefab);
        ball.GetComponent<Rigidbody>().AddForce(Vector3.forward * ballForce, ForceMode.Impulse);

        SetCurrentBallsCount(GetCurrentBallsCount() - 1);
        UIManager.RefreshBallsUI();
        StartCoroutine(CheckBallsCountWithDelay());

    }

    private IEnumerator CheckBallsCountWithDelay() {
        yield return new WaitForSeconds(0.1f);
        if (GetCurrentBallsCount() < 1) {
            LevelGenerator.NextCircle();
        }
    }

    public static void PaintBallSpawnPoint() {
        ballSpawnPoint.GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
    }

    public static void SetCurrentBallsCount(int ballsCount) {
        currentBallsCount = ballsCount;
    }

    public static void SetMaximumBallsCount(int ballsCount) {
        maximumBallsCount = ballsCount;
    }

    public static int GetCurrentBallsCount() {
        return currentBallsCount;
    }

    public static int GetMaximumBallsCount() {
        return maximumBallsCount;
    }

    public static void SetLastPaintedArea(Collider _lastPaintedArea) {
        lastPaintedArea = _lastPaintedArea;
    }

    public static Collider GetLastPaintedArea() {
        return lastPaintedArea;
    }
}
