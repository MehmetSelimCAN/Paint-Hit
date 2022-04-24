using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Circle : MonoBehaviour {

    private int circleNumber;
    private int clockWise;

    private void Start() {
        int randomNumber = Random.Range(0, 24);

        for (int i = 0; i < 24; i++) {
            if (i != randomNumber) {
                transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                transform.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
                transform.GetChild(i).tag = "AlreadyColored";
            }
        }
    }

    public void SetCircleNumber(int circleNumber) {
        this.circleNumber = circleNumber;
        Rotate();
    }

    public void Rotate() {
        switch (circleNumber) {
            case 0:
                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1f,
                    "speed", 100f,
                    "easetype", iTween.EaseType.linear,
                    "looptype", iTween.LoopType.loop
                    ));
                break;
            case 1:
                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", -1f,
                    "speed", 100f,
                    "easetype", iTween.EaseType.linear,
                    "looptype", iTween.LoopType.loop
                    ));
                break;
            case 2:
                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1f,
                    "speed", 150f,
                    "easetype", iTween.EaseType.linear,
                    "looptype", iTween.LoopType.loop
                    ));
                break;
            case 3:
                if (Random.value < 0.5f) clockWise = 1;
                else clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 1.2f * clockWise,
                    "speed", 100f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.2f,
                    "OnComplete", "Rotate"
                    ));
                break;
            case 4:
                if (Random.value < 0.5f) clockWise = 1;
                else clockWise = -1;

                iTween.RotateBy(transform.gameObject, iTween.Hash(
                    "y", 0.8f * clockWise,
                    "speed", 150f,
                    "easetype", iTween.EaseType.easeInOutSine,
                    "delay", 0.1f,
                    "OnComplete", "Rotate"
                    ));
                break;
        }
    }
}
