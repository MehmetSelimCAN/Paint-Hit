using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

    public static ColorManager Instance { get; private set; }

    private List<Color> colors;
    private Color currentColor;

    public ColorsList colorsList = new ColorsList();

    private void Awake() {
        Instance = this;
        colors = colorsList.list[PlayerPrefs.GetInt("ColorCardNumber") - 1].colors;
    }

    public Color GetCurrentColor() {
        currentColor = colors[CircleCounter.GetCurrentCircleNumber()];
        return currentColor;
    }
}
