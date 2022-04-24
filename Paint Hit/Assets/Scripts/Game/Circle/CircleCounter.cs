using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCounter : MonoBehaviour {

    private static int circleNumber;
    private static int maximumCircleCount;
    private static int remainingCircleCount;

    public static void SetCurrentCircleNumber(int _circleNumber) {
        circleNumber = _circleNumber;
    }

    public static int GetCurrentCircleNumber() {
        return circleNumber;
    }

    public static void SetMaximumCircleCount(int _maximumCircleCount) {
        maximumCircleCount = _maximumCircleCount;
    }

    public static int GetMaximumCircleCount() {
        return maximumCircleCount;
    }

    public static void SetRemainingCircleCount(int _remainingCircleCount) {
        remainingCircleCount = _remainingCircleCount;
    }

    public static int GetRemainingCircleCount() {
        return remainingCircleCount;
    }

}
