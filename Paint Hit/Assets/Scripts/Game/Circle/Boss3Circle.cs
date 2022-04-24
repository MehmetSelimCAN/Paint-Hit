using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Circle : MonoBehaviour {

    private int circleNumber;

    private void Start() {
        for (int i = 0; i < 4; i++) {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            transform.GetChild(i).tag = "AlreadyColored";
        }

        for (int i = 8; i < 12; i++) {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();
            transform.GetChild(i).tag = "AlreadyColored";
        }

        for (int i = 16; i < 20; i++) {
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
        iTween.RotateBy(transform.gameObject, iTween.Hash(
            "y", 0.2f,
            "speed", 200f + 10 * circleNumber,
            "easetype", iTween.EaseType.linear,
            "delay", 0.3f - (0.05 * circleNumber),
            "OnComplete", "Rotate"
        ));
    }
}
