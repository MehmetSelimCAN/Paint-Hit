using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4Circle : MonoBehaviour {

    private int circleNumber;
    private int clockWise;

    private void Start() {
        for (int i = 0; i < 3; i++) {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            transform.GetChild(i).tag = "AlreadyColored";
        }

        for (int i = 8; i < 11; i++) {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            transform.GetChild(i).tag = "AlreadyColored";
        }

        for (int i = 16; i < 19; i++) {
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
            "y", Random.Range(0.1f, 0.5f) * clockWise,
            "speed", 200f + 10 * circleNumber,
            "easetype", iTween.EaseType.linear,
            "delay", 0.15f,
            "OnComplete", "Rotate"
        ));
    }
}
