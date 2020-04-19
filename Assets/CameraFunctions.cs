using UnityEngine;

public static class CameraFunctions{
    public static bool IsOnScreen(Vector3 pos)
    {
        var viewportPoint = Camera.main.WorldToViewportPoint(pos);
        const float flexAmount = .3f;
        bool onScreen = viewportPoint.x >= -flexAmount && viewportPoint.x <= 1 + flexAmount && viewportPoint.y >= -flexAmount && viewportPoint.y <= 1 + flexAmount;
        return onScreen;
    }
    
    public static bool IsWayOffScreen(Vector3 pos)
    {
        var viewportPoint = Camera.main.WorldToViewportPoint(pos);
        const float flexAmount = .6f;
        bool onScreen = viewportPoint.x >= -flexAmount && viewportPoint.x <= 1 + flexAmount && viewportPoint.y >= -flexAmount && viewportPoint.y <= 1 + flexAmount;
        return !onScreen;
    }


}