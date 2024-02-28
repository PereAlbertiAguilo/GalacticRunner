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

    bool right = false;

    void PlayerInput()
    {
        

        if (Input.touchCount > 0)
        {
            foreach(Touch t in Input.touches)
            {
                if (t.position.x > Screen.width / 2)
                {
                    MovePlayer(0, 1);
                    right = true;
                }
                else
                {
                    MovePlayer(-1, 0);
                    right = false;
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0)) 
            {
                if (Input.mousePosition.x > Screen.width / 2)
                {
                    MovePlayer(0, 1);
                    right = true;
                }
                else
                {
                    MovePlayer(-1, 0);
                    right = false;
                }
            }
            else
            {
                if (!right)
                {
                    inputValue += 7 * Time.deltaTime;
                    inputValue = Mathf.Clamp(inputValue, -1, 0);
                }
                else
                {
                    inputValue -= 7 * Time.deltaTime;
                    inputValue = Mathf.Clamp(inputValue, 0, 1);
                }
            }
        }

        transform.Translate(Vector2.right * inputValue * sideSpeed * Time.deltaTime);

        //horizontalInput = Input.GetAxis("Horizontal");
        //transform.Translate(Vector2.right * horizontalInput * sideSpeed * Time.deltaTime);
    }

    float inputValue = 0;

    void MovePlayer(int min, int max)
    {
        if (min >= 0)
        {
            inputValue += 4 * Time.deltaTime;
        }
        else
        {
            inputValue -= 4 * Time.deltaTime;
        }

        inputValue = Mathf.Clamp(inputValue, min, max);

    }
}
