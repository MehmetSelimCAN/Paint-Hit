using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour {

    private Text questText;
    private Button claimButton;
    private Sprite blackBackground;
    private Sprite blueBackground;

    private void Awake() {
        questText = GetComponentInChildren<Text>();
        claimButton = GetComponentInChildren<Button>();

        blackBackground = Resources.Load<Sprite>("Sprites/BlackBackground");
        blueBackground = Resources.Load<Sprite>("Sprites/spEmptyBlue");

        claimButton.onClick.AddListener(() => {
            ClaimChest();
        });

        //PlayerPrefs.SetInt("LastLevel", 1);
        //PlayerPrefs.SetInt("QuestLevel", 0);

        if (PlayerPrefs.GetInt("QuestLevel") == 0) {
            PlayerPrefs.SetInt("QuestLevel", 5);
        }

        questText.text = "COMPLETE LEVEL " + PlayerPrefs.GetInt("QuestLevel");

        CheckQuest();
    }

    private void ClaimChest() {
        PlayerPrefs.SetInt("QuestLevel", PlayerPrefs.GetInt("QuestLevel") + 5);
        questText.text = "COMPLETE LEVEL " + PlayerPrefs.GetInt("QuestLevel");
        ChestManager.OpenChest();
        CheckQuest();
    }

    private void CheckQuest() {
        if (PlayerPrefs.GetInt("LastLevel") > PlayerPrefs.GetInt("QuestLevel")) {
            questText.text = "CLAIM";
            transform.Find("Background").GetComponent<Image>().sprite = blueBackground;
            claimButton.interactable = true;
        }
        else {
            questText.text = "COMPLETE LEVEL " + PlayerPrefs.GetInt("QuestLevel");
            transform.Find("Background").GetComponent<Image>().sprite = blackBackground;
            claimButton.interactable = false;
        }
    }
}
