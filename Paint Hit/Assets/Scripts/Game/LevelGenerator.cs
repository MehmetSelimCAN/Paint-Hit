using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator Instance { get; private set; }

    private static Transform circlePrefab;
    private static Transform currentCircle;
    private static GameObject[] circlesOnTheScene;
    private static GameObject[] pointOnTheScene;

    private static List<Vector3> tempVectors;
    private static Vector3 temp;
    private static float transitionSpeed = 20f;
    private static bool newCircleComing = false;


    private void Awake() {
        Instance = this;

        newCircleComing = false;
        tempVectors = new List<Vector3>();

        CircleCounter.SetCurrentCircleNumber(0);

        if (SceneManager.GetActiveScene().name == "NormalLevel") {
            CircleCounter.SetMaximumCircleCount((PlayerPrefs.GetInt("LastLevel") / 10) + 5);
            BallHandler.SetMaximumBallsCount(CircleCounter.GetMaximumCircleCount() + 1);

            circlePrefab = Resources.Load<Transform>("Prefabs/pfCircle");
            Transform firstCircle = Instantiate(circlePrefab);
            firstCircle.GetComponent<Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
        }

        else if (SceneManager.GetActiveScene().name == "Boss1Level") {
            CircleCounter.SetMaximumCircleCount(5);
            BallHandler.SetMaximumBallsCount(1);

            circlePrefab = Resources.Load<Transform>("Prefabs/pfBoss1Circle");
            Transform firstCircle = Instantiate(circlePrefab);
            firstCircle.GetComponent<Boss1Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
        }
        else if (SceneManager.GetActiveScene().name == "Boss2Level") {
            CircleCounter.SetMaximumCircleCount(8);
            BallHandler.SetMaximumBallsCount(9);

            circlePrefab = Resources.Load<Transform>("Prefabs/pfBoss2Circle");
            Transform firstCircle = Instantiate(circlePrefab);
            firstCircle.GetComponent<Boss2Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
        }
        else if (SceneManager.GetActiveScene().name == "Boss3Level") {
            CircleCounter.SetMaximumCircleCount(5);
            BallHandler.SetMaximumBallsCount(6);

            circlePrefab = Resources.Load<Transform>("Prefabs/pfBoss3Circle");
            Transform firstCircle = Instantiate(circlePrefab);
            firstCircle.GetComponent<Boss3Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
        }
        else if (SceneManager.GetActiveScene().name == "Boss4Level") {
            BallHandler.SetMaximumBallsCount(10);
            CircleCounter.SetMaximumCircleCount(10);

            circlePrefab = Resources.Load<Transform>("Prefabs/pfBoss4Circle");
            Transform firstCircle = Instantiate(circlePrefab);
            firstCircle.GetComponent<Boss4Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
        }

        CircleCounter.SetRemainingCircleCount(CircleCounter.GetMaximumCircleCount());
        UIManager.RefreshCircleCountUI();

        BallHandler.SetCurrentBallsCount(BallHandler.GetMaximumBallsCount());
    }

    private void Update() {
        if (newCircleComing) {
            for (int i = 0; i < circlesOnTheScene.Length; i++) {
                circlesOnTheScene[i].transform.position = Vector3.Lerp(circlesOnTheScene[i].transform.position, tempVectors[i], Time.deltaTime * transitionSpeed);
            }
        }
    }

    public static void NextCircle() {
        Instance.StartCoroutine(Instance.NextCircleDelayed());
    }

    private IEnumerator NextCircleDelayed() {
        CircleCounter.SetRemainingCircleCount(CircleCounter.GetRemainingCircleCount() - 1);
        yield return new WaitForSeconds(0.2f);

        if (CircleCounter.GetRemainingCircleCount() > 0 && PlayerPrefs.GetInt("Heart") != 0) {
            StartCoroutine(WaitForShootingBall());
            PaintWholeCircle();
            yield return new WaitForSeconds(0.4f);
            OneStepDownwards();
            CreateNewCircle();
            yield return new WaitForSeconds(0.15f);
            GameObject.Find("Ring").GetComponent<Animator>().Play("Ring", -1, 0f);
            yield return new WaitForSeconds(0.1f);
            newCircleComing = true;
            yield return new WaitForSeconds(0.5f);
            newCircleComing = false;
        }
        else if (CircleCounter.GetRemainingCircleCount() < 1 && PlayerPrefs.GetInt("Heart") != 0) {
            yield return new WaitForSeconds(0.4f);
            PaintWholeCircle();

            CircleCounter.SetCurrentCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
            UIManager.RefreshCircleCountUI();

            BallHandler.SetMaximumBallsCount(0);
            UIManager.ResetBallsUI();
            BallHandler.canShoot = false;

            GameManager.NextLevel();
        }
    }

    private void PaintWholeCircle() {
        PlayerPrefs.SetInt("LayersFilled", PlayerPrefs.GetInt("LayersFilled") + 1);
        currentCircle = GameObject.FindGameObjectWithTag("CurrentCircle").transform;
        currentCircle.GetComponent<iTween>().enabled = false;
        currentCircle.tag = "Circle";

        StartCoroutine(PaintCircleParts());
    }

    private IEnumerator PaintCircleParts() {
        for (int i = 0; i < currentCircle.childCount - 1; i++) {
            currentCircle.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            currentCircle.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < currentCircle.childCount - 1; i++) {
            currentCircle.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }

        currentCircle.Find("wholeCircle").GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();        //Paint whole circle after the completing
    }

    private void CreateNewCircle() {
        CircleCounter.SetCurrentCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
        BallHandler.PaintBallSpawnPoint();

        Transform circle = Instantiate(circlePrefab);
        circle.GetComponent<Animator>().Play("CircleMovement");

        if (SceneManager.GetActiveScene().name == "NormalLevel") {
            circle.GetComponent<Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);

        }
        else if (SceneManager.GetActiveScene().name == "Boss1Level") {
            circle.GetComponent<Boss1Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
            BallHandler.SetCurrentBallsCount(BallHandler.GetMaximumBallsCount());
        }
        else if (SceneManager.GetActiveScene().name == "Boss2Level") {
            circle.GetComponent<Boss2Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
            BallHandler.SetMaximumBallsCount(BallHandler.GetMaximumBallsCount() - 1);
            BallHandler.SetCurrentBallsCount(BallHandler.GetMaximumBallsCount());

        }
        else if (SceneManager.GetActiveScene().name == "Boss3Level") {
            circle.GetComponent<Boss3Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
            BallHandler.SetMaximumBallsCount(BallHandler.GetMaximumBallsCount() + 1);
            BallHandler.SetCurrentBallsCount(BallHandler.GetMaximumBallsCount());

        }
        else if (SceneManager.GetActiveScene().name == "Boss4Level") {
            circle.GetComponent<Boss4Circle>().SetCircleNumber(CircleCounter.GetCurrentCircleNumber() + 1);
            BallHandler.SetMaximumBallsCount(BallHandler.GetMaximumBallsCount() + 1);
            BallHandler.SetCurrentBallsCount(BallHandler.GetMaximumBallsCount());

        }


        UIManager.RefreshCircleCountUI();
    }

    private IEnumerator WaitForShootingBall() {
        BallHandler.canShoot = false;
        yield return new WaitForSeconds(0.8f);
        BallHandler.canShoot = true;

        if (SceneManager.GetActiveScene().name == "NormalLevel") {
            BallHandler.SetMaximumBallsCount(BallHandler.GetMaximumBallsCount() + 1);
            BallHandler.SetCurrentBallsCount(BallHandler.GetMaximumBallsCount());
        }
        UIManager.ResetBallsUI();
    }

    private static void OneStepDownwards() {
        circlesOnTheScene = GameObject.FindGameObjectsWithTag("Circle");
        pointOnTheScene = GameObject.FindGameObjectsWithTag("Point");

        tempVectors.Clear();

        foreach (GameObject point in pointOnTheScene) { Destroy(point); }

        for (int i = 0; i < circlesOnTheScene.Length; i++) {
            circlesOnTheScene[i].GetComponent<Animator>().enabled = false;

            temp = circlesOnTheScene[i].transform.position;
            temp.y -= 2.88f;
            tempVectors.Add(temp);
        }
    }
}
