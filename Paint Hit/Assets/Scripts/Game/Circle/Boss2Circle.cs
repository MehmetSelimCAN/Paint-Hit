using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Circle : MonoBehaviour {

    private int circleNumber;
    private int clockWise;

    private void Start() {

        for (int i = 0; i < 2 * (circleNumber + 1); i++) {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            transform.GetChild(i).tag = "AlreadyColored";
        }
    }

    public void SetCircleNumber(int circleNumber) {
        this.circleNumber = circleNumber;
        Rotate();
    }

    public void Rotate() {
        if (Random.value < 0.5f) clockWise = 1;
        else clockWise = -1;

        iTween.RotateBy(transform.gameObject, iTween.Hash(
            "y", 0.5f * clockWise,
            "speed", 100f + 10 * circleNumber,
            "easetype", iTween.EaseType.easeInOutSine,
            "delay", 0.2f,
            "OnComplete", "Rotate"
        ));
    }
}
