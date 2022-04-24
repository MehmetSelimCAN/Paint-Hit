using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour {

    private Transform menuCanvas;
    private Transform scrollbar;
    private Transform pageButtons;
    private float scrollPosition = 0;
    private float[] pagePositions;
    private float distance;
    private float time;
    private int buttonNumber;
    private bool isSwiping = false;

    private void Awake() {
        pagePositions = new float[3];
        distance = 1f / (pagePositions.Length - 1f);

        for (int i = 0; i < pagePositions.Length; i++) {
            pagePositions[i] = distance * i;
        }

        scrollbar = GameObject.Find("MenuScrollbar").transform;
        pageButtons = GameObject.Find("PageButtons").transform;

        scrollPosition = 0.5f;
        scrollbar.GetComponent<Scrollbar>().value = scrollPosition;

        menuCanvas = GameObject.Find("Canvas").transform;
    }

    private void Start() {
        GetComponent<HorizontalLayoutGroup>().padding.left = (int)menuCanvas.GetComponent<RectTransform>().rect.width / 2;
        GetComponent<HorizontalLayoutGroup>().padding.right = (int)menuCanvas.GetComponent<RectTransform>().rect.width / 2;
        GetComponent<HorizontalLayoutGroup>().spacing = (int)menuCanvas.GetComponent<RectTransform>().rect.width;
    }

    void Update() {
        if (isSwiping) {
            Transition();
            time += Time.deltaTime;

            if (time > 0.5f) {
                time = 0;
                isSwiping = false;
            }
        }

        if (Input.GetMouseButton(0)) {
            scrollPosition = scrollbar.GetComponent<Scrollbar>().value;
        }
        else {
            for (int i = 0; i < pagePositions.Length; i++) {
                if (scrollPosition < pagePositions[i] + (distance / 2) && scrollPosition > pagePositions[i] - (distance / 2)) {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pagePositions[i], 0.2f);
                    buttonNumber = i;
                }
            }
            RefreshButtons();
        }
    }

    private void Transition() {
        for (int i = 0; i < pagePositions.Length; i++) {
            if (scrollPosition < pagePositions[i] + (distance / 2) && scrollPosition > pagePositions[i] - (distance / 2)) {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pagePositions[buttonNumber], 1.5f * Time.deltaTime);
            }
        }
    }

    public void WhichBtnClicked(Button btn) {
        buttonNumber = btn.transform.GetSiblingIndex();
        scrollPosition = (pagePositions[buttonNumber]);
        time = 0;
        isSwiping = true;
        RefreshButtons();
    }

    private void RefreshButtons() {
        for (int i = 0; i < pagePositions.Length; i++) {
            pageButtons.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 170);
            pageButtons.transform.GetChild(i).Find("outline").gameObject.SetActive(false);
        }

        pageButtons.transform.GetChild(buttonNumber).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        pageButtons.transform.GetChild(buttonNumber).Find("outline").gameObject.SetActive(true);
    }
}