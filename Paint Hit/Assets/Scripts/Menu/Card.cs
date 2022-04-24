using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    private Button button;
    private Image outline;
    private GameObject[] cards;

    private void Awake() {
        cards = GameObject.FindGameObjectsWithTag("Card");

        outline = transform.Find("outline").GetComponent<Image>();

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => {
            ButtonClicked();
        });
    }

    private void Start() {
        if (PlayerPrefs.GetInt(transform.name) == 1) {      //If the card has already been opened, PlayerPrefs.GetInt("Card1") should be 1.
            Unlock();
        }
    }

    private void ButtonClicked() {
        if (PlayerPrefs.GetInt(transform.name) == 1) {     //If card is unlocked...
            for (int i = 0; i < cards.Length; i++) {
                cards[i].transform.Find("outline").GetComponent<Image>().color = new Color32(98, 101, 168, 255);
            }

            outline.color = new Color32(255, 255, 255, 255);
            PlayerPrefs.SetInt("ColorCardNumber", int.Parse(transform.name.Substring(4)));      //...select this color card as a game color card.
        }
    }

    public void Unlock() {
        transform.tag = "UnlockedCard";
        PlayerPrefs.SetInt(transform.name, 1);
        transform.Find("locked").gameObject.SetActive(false);
    }
}
