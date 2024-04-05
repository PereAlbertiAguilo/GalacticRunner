using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileController : MonoBehaviour
{
    float borderBottom;

    Vector2 ScreenBorderLimit()
    {
        Vector2 screenRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));
        Vector2 screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));

        float x = screenRight.x;
        float y = screenTop.y;

        return new Vector2(x, y);
    }

    void Update()
    {
        borderBottom = -ScreenBorderLimit().y - transform.localScale.x / 2;

        if (transform.position.y < borderBottom)
        {
            gameObject.SetActive(false);
        }
    }
}
