using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenBorderLimit
{
    public static float X()
    {
        Vector2 screenRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));

        float x = screenRight.x;

        return x;
    }

    public static float Y()
    {
        Vector2 screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));

        float y = screenTop.y;

        return y;
    }
}
