using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSides : MonoBehaviour
{
    [SerializeField] float sideSpeed = 10f;
    [SerializeField] float screenBorderLimit = 0f;

    private float horizontalInput;

    private void Update()
    {
        PlayerInput();

        ScreenBorderLimit();
    }

    void ScreenBorderLimit()
    {
        if(Mathf.Abs(transform.position.x) >= screenBorderLimit)
        {
            transform.position = new Vector2(transform.position.x > 0 ? screenBorderLimit : -screenBorderLimit, transform.position.y);
        }
    }

    void PlayerInput()
    {
        if(Input.touchCount > 0)
        {
            foreach(Touch t in Input.touches)
            {
                if (t.position.x > Screen.width / 2)
                {
                    transform.Translate(Vector2.right * sideSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.left * sideSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0)) 
            { 
                if (Input.mousePosition.x > Screen.width / 2)
                {
                    transform.Translate(Vector2.right * sideSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.left * sideSpeed * Time.deltaTime);
                }
            }
        }

        horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(Vector2.right * horizontalInput * sideSpeed * Time.deltaTime);
    }
}
