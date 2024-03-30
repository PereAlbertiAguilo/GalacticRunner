using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject[] selectButtons;
    [SerializeField] GameObject[] lockedTexts;

    private void Start()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            if (PlayerPrefs.HasKey("stagesEnded") && PlayerPrefs.GetInt("stagesEnded") >= i)
            {
                UpdateUI(i, true, "Select");
            }
            else
            {
                UpdateUI(i, false, "Locked");
            }
        }

        UpdateUI(0, true, "Select");
    }

    void UpdateUI(int index, bool interactable, string slectText)
    {
        GetButton(selectButtons[index]).interactable = interactable;
        GetText(selectButtons[index]).text = slectText;
        lockedTexts[index].SetActive(!interactable);
    }

    Button GetButton(GameObject buttonGameObject)
    {
        return buttonGameObject.GetComponent<Button>();
    }

    TextMeshProUGUI GetText(GameObject buttonGameObject)
    {
        return buttonGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
