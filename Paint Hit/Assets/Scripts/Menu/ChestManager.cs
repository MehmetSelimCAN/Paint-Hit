using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour {

    public static ChestManager Instance { get; private set; }

    private GameObject[] cards;
    private Button buyChestButton;
    private Image buyButtonImage;
    private Sprite emptyRed;
    private Sprite emptyBlue;
    private Sprite lastOpenedCardSprite;


    private void Awake() {
        Instance = this;

        cards = GameObject.FindGameObjectsWithTag("Card");

        buyChestButton = GameObject.Find("CardPage").transform.Find("OpenChest").Find("BuyButton").GetComponent<Button>();
        buyButtonImage = GameObject.Find("CardPage").transform.Find("OpenChest").Find("BuyButton").GetComponent<Image>();
        emptyRed = Resources.Load<Sprite>("Sprites/spEmptyRed");
        emptyBlue = Resources.Load<Sprite>("Sprites/spEmptyBlue");

        buyChestButton.onClick.AddListener(() => {
            StartCoroutine(BuyChest());
        });

        Time.timeScale = 1f;
    }

    private void Start() {
        for (int i = 0; i < cards.Length; i++) {
            cards[i].transform.Find("outline").GetComponent<Image>().color = new Color32(98, 101, 168, 255);        //Unhighlight the color cards.
        }

        GameObject.Find("Card" + PlayerPrefs.GetInt("ColorCardNumber")).transform.Find("outline").GetComponent<Image>().color = new Color32(255, 255, 255, 255);    //Highlight the last selected color card.

        RefreshBuyMenuUI();
    }

    private void UnlockRandomColorCard() {
        cards = GameObject.FindGameObjectsWithTag("Card");
        int randomNumber = Random.Range(0, cards.Length);
        cards[randomNumber].GetComponent<Card>().Unlock();
        lastOpenedCardSprite = cards[randomNumber].transform.Find("cardImage").GetComponent<Image>().sprite;
    }

    private IEnumerator BuyChest() {
        if (buyButtonImage.sprite == emptyBlue) {
            PlayerPrefs.SetInt("ChestsOpened", PlayerPrefs.GetInt("ChestsOpened") + 1);
            MenuManager.RefreshStatisticsMenuUI();

            PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") - 250);
            MenuManager.RefreshPointUI();

            PlayerPrefs.SetInt("RemainingSkillCount", PlayerPrefs.GetInt("RemainingSkillCount") + 3);

            UnlockRandomColorCard();

            GameObject.Find("CardPage").transform.Find("OpenChest").Find("Animation").Find("ColorCard").Find("image").GetComponent<Image>().sprite = lastOpenedCardSprite;
            GameObject.Find("CardPage").transform.Find("OpenChest").Find("Animation").GetComponent<Animator>().Play("BuyChestOpen", -1, 0f);
            yield return new WaitForSeconds(1.5f);

            PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") + 50);
            MenuManager.RefreshPointUI();

            yield return new WaitForSeconds(1.5f);  //Delay for animation
            RefreshBuyMenuUI();
        }
    }

    public static void OpenChest() {
        Instance.StartCoroutine(Instance.OpeningChest());
    }

    private IEnumerator OpeningChest() {
        PlayerPrefs.SetInt("ChestsOpened", PlayerPrefs.GetInt("ChestsOpened") + 1);
        MenuManager.RefreshStatisticsMenuUI();

        cards = GameObject.FindGameObjectsWithTag("Card");
        if (cards.Length != 0) {
            UnlockRandomColorCard();
            GameObject.Find("HomePage").transform.Find("Chest").Find("Animation").Find("ColorCard").Find("image").GetComponent<Image>().sprite = lastOpenedCardSprite;
            GameObject.Find("HomePage").transform.Find("Chest").Find("Animation").GetComponent<Animator>().Play("ChestOpen", -1, 0f);
            yield return new WaitForSeconds(1.5f);
        }
        else {  //If all cards is unlocked then user gain only skill and gem.
            GameObject.Find("HomePage").transform.Find("Chest").Find("AnimationWithoutCard").GetComponent<Animator>().Play("ChestOpenWithoutCard", -1, 0f);
            yield return new WaitForSeconds(1f);
        }

        PlayerPrefs.SetInt("RemainingSkillCount", PlayerPrefs.GetInt("RemainingSkillCount") + 3);
        PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") + 50);
        MenuManager.RefreshPointUI();
    }

    private void RefreshBuyMenuUI() {
        if (PlayerPrefs.GetInt("Point") < 250) {        //If users can't afford the chest price, button will be red. Otherwise green.
            buyButtonImage.sprite = emptyRed;
        }
        else {
            buyButtonImage.sprite = emptyBlue;
        }

        cards = GameObject.FindGameObjectsWithTag("Card");
        if (cards.Length == 0) {        //If all color cards is unlocked, remove buy menu.
            GameObject.Find("CardPage").transform.Find("OpenChest").gameObject.SetActive(false);
            RectTransform rectTransform = GameObject.Find("CardPage").transform.Find("Cards").Find("Panel").GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 350f);
        }
    }

}
