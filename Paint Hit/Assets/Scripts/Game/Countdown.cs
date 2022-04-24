using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    private float timer = 10f;
    private Image circleOutline;
    private Transform gameOverText;
    private Text timeText;

    private void Awake() {
        circleOutline = transform.Find("CircleOutline").GetComponent<Image>();
        timeText = transform.Find("time").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").transform;
        gameOverText.gameObject.SetActive(false);

    }

    private void Update() {
        timer -= Time.unscaledDeltaTime;
        circleOutline.fillAmount = timer / 10f;
        timeText.text = Mathf.Ceil(timer).ToString();

        if (timer < 0) {
            gameOverText.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}
