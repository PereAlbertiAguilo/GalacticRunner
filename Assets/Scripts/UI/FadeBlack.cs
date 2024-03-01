using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlack : MonoBehaviour
{
    public static FadeBlack instance;

    Animator _animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _animator = GetComponent<Animator>();
    }

    public void FadeToBlack()
    {
        Time.timeScale = 1;

        _animator.Play("FadeToBlack");
    }

    public void FadeFromBlack()
    {
        Time.timeScale = 1;

        _animator.Play("FadeFromBlack");
    }
}
