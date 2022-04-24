using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

    private int circleNumber;
    private int clockWise;
    private float[] mode5Values;
    private int i = 0;
    private Transform pointPrefab;
    private Transform randomCirclePart;

    private void Awake() {
        mode5Values = new float[] { 0.8f, 0.15f};
        pointPrefab = Resources.Load<Transform>("Prefabs/pfPoint");
    }

    private void Start() {
        for (int i = 1; i < circleNumber; i++) {
            int randomNumber = Random.Range(0, 24);
            transform.GetChild(randomNumber).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(randomNumber).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            transform.GetChild(randomNumber).tag = "AlreadyColored";
        }

        int randomPointCount = Random.Range(0,3);
        for (int i = 0; i < randomPointCount; i++) {
            int randomNumber = Random.Range(0, 24);
            while (transform.GetChild(randomNumber).tag == "AlreadyColored") {
                randomNumber = Random.Range(0, 24);
            }

                randomCirclePart = transform.GetChild(randomNumber).transform;
                Transform point = Instantiate(pointPrefab);
                point.SetParent(randomCirclePart);
                point.localRotation = Quaternion.Euler(0, 0, 0);
                point.localPosition = new Vector3(13, 0, 0);
                point.localScale = new Vector3(0.01f, 0.01f, 0.02f);
        }
    } 

    public void SetCircleNumber(int circleNumber) {
        this.circleNumber = circleNumber;
        Rotate();
    }

    public void Rotate() {
        switch (circleNumber) {
            case 1:
                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1f,
                    "speed", 100f,
                    "easetype", iTween.EaseType.linear,
                    "looptype", iTween.LoopType.loop
                    ));
                break;
            case 2:
                if (Random.value < 0.5f) clockWise = 1;
                else                     clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 0.8f * clockWise,
                    "speed", 100f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.2f,
                    "OnComplete", "Rotate"
                    ));
                break;
            case 3:
                if (Random.value < 0.5f) clockWise = 1;
                else                     clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1f * clockWise,
                    "speed", 100f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.2f,
                    "OnComplete", "Rotate"
                    ));
                break;
            case 4:
                if (Random.value < 0.5f) clockWise = 1;
                else                     clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1.2f * clockWise,
                    "speed", 100f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.2f,
                    "OnComplete", "Rotate"
                    ));
                break;
            case 5:
                if (i > 1) i = 0;

                if (Random.value < 0.5f) clockWise = 1;
                else                     clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", mode5Values[i] * clockWise,
                    "speed", 100f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.2f,
                    "OnComplete", "Rotate"
                    ));
                i++;
                break;
            case 6:
                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", -1f,
                    "speed", 200f,
                    "easetype", iTween.EaseType.linear,
                    "looptype", iTween.LoopType.loop
                    ));
                break;
            case 7:
                if (Random.value < 0.5f) clockWise = 1;
                else clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1.2f * clockWise,
                    "speed", 150f,
                    "easetype", iTween.EaseType.linear,
                    "delay", 0.1f,
                    "OnComplete", "Rotate"
                    ));
                break;
            case 8:
                if (Random.value < 0.5f) clockWise = 1;
                else clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1.2f * clockWise,
                    "speed", 150f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.25f,
                    "OnComplete", "Rotate"
                    ));
                break;
            default:
                if (Random.value < 0.5f) clockWise = 1;
                else clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1.2f * clockWise,
                    "speed", 150f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.25f,
                    "OnComplete", "Rotate"
                    ));
                break;
        }

    }
}
