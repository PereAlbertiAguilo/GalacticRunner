using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] spaceCraftBuys;
    [SerializeField] GameObject[] spaceCraftSelects;

    [SerializeField] int[] spaceCraftPrices;

    [SerializeField] int spaceCraftSelected = 0;

    [Space]

    [SerializeField] GameObject[] bulletBuys;
    [SerializeField] GameObject[] bulletSelects;

    [SerializeField] int[] bulletPrices;

    [SerializeField] int bulletSelected = 0;

    [Space]

    [SerializeField] GameObject[] shieldBuys;
    [SerializeField] GameObject[] shieldSelects;

    [SerializeField] int[] shieldPrices;

    [SerializeField] int shieldSelected = 0;

    [Space]

    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] Color selectedColor;

    int money;

    private void Start()
    {
        // Gets the selected values from player prefs
        spaceCraftSelected = PlayerPrefs.GetInt("spaceSelect");
        bulletSelected = PlayerPrefs.GetInt("bulletSelect");
        shieldSelected = PlayerPrefs.GetInt("shieldSelect");

        // Updates the selected estate of the buttons
        ButtonSelectedUpdate("spaceBuy", "spaceSelect", spaceCraftSelected, spaceCraftSelects, true);
        ButtonSelectedUpdate("bulletBuy", "bulletSelect", bulletSelected, bulletSelects, true);
        ButtonSelectedUpdate("shieldBuy", "shieldSelect", shieldSelected, shieldSelects, false);

        // Updates the moeny
        money = PlayerPrefs.GetInt("pointsScore");
        moneyText.text = "Scraps: " + money;

        // Updates the prices texts
        PricesUpdate();

        // Updates the active state of the bought and selected buttons
        for (int i = 0; i < spaceCraftBuys.Length; i++)
        {
            ButtonUpdate("spaceBuy" + i, i, spaceCraftBuys, spaceCraftSelects);
        }
        for (int i = 0; i < bulletBuys.Length; i++)
        {
            ButtonUpdate("bulletBuy" + i, i, bulletBuys, bulletSelects);
        }
        for (int i = 0; i < shieldBuys.Length; i++)
        {
            ButtonUpdate("shieldBuy" + i, i, shieldBuys, shieldSelects);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }

    // Updates the active state of the bought and selected buttons
    void ButtonUpdate(string key, int index, GameObject[] buys, GameObject[] selects)
    {
        if(PlayerPrefs.HasKey(key))
        {
            if(PlayerPrefs.GetInt(key) == 1)
            {
                buys[index].SetActive(false);
                selects[index].SetActive(true);
            }
            else
            {
                buys[index].SetActive(true);
                selects[index].SetActive(false);
            }
        }
        else
        {
            buys[index].SetActive(true);
            selects[index].SetActive(false);
        }
    }

    // Updates the prices texts
    void PricesUpdate()
    {
        for (int i = 0; i < spaceCraftPrices.Length; i++)
        {
            spaceCraftBuys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = spaceCraftPrices[i] + " S";
        }
        for (int i = 0; i < bulletPrices.Length; i++)
        {
            bulletBuys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = bulletPrices[i] + " S";
        }
        for (int i = 0; i < shieldPrices.Length; i++)
        {
            shieldBuys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shieldPrices[i] + " S";
        }
    }

    // Updates the selected estate of the buttons
    void ButtonSelectedUpdate(string keyBuy, string keySelect, int selected, GameObject[] selects, bool firstSelected)
    {
        if (PlayerPrefs.HasKey(keySelect) && PlayerPrefs.GetInt(keySelect) >= 0)
        {
            for (int i = 0; i < selects.Length; i++)
            {
                if (i == selected)
                {
                    selects[i].GetComponent<Image>().color = selectedColor;
                }
                else
                {
                    selects[i].GetComponent<Image>().color = Color.white;
                }
            }
        }
        else if (firstSelected)
        {
            PlayerPrefs.SetInt(keyBuy + 0, 1);
            PlayerPrefs.SetInt(keySelect, 0);
            selects[0].GetComponent<Image>().color = selectedColor;
        }
    }

    // Buys and selects a spacecraft
    #region SpaceCraftButton
    void SelectSpaceCraft(int index)
    {
        spaceCraftSelected = index;

        PlayerPrefs.SetInt("spaceSelect", spaceCraftSelected);

        spaceCraftSelects[index].GetComponent<Image>().color = selectedColor;

        ButtonSelectedUpdate("spaceBuy", "spaceSelect", spaceCraftSelected, spaceCraftSelects, true);
    }

    public void BuySpaceCraft(int index)
    {
        if (PlayerPrefs.HasKey("spaceBuy" + index))
        {
            SelectSpaceCraft(index);
        }
        else
        {
            if (money >= spaceCraftPrices[index])
            {
                money -= spaceCraftPrices[index];
                moneyText.text = "Scraps: " + money;

                PlayerPrefs.SetInt("pointsScore", money);
                PlayerPrefs.SetInt("spaceBuy" + index, 1);

                SelectSpaceCraft(index);

                ButtonUpdate("spaceBuy" + index, index, spaceCraftBuys, spaceCraftSelects);
            }
        }
    }
    #endregion

    // Buys and selects a bullet
    #region BulletButton
    void SelectBullet(int index)
    {
        bulletSelected = index;

        PlayerPrefs.SetInt("bulletSelect", bulletSelected);

        bulletSelects[index].GetComponent<Image>().color = selectedColor;

        ButtonSelectedUpdate("bulletBuy", "bulletSelect", bulletSelected, bulletSelects, true);

        PlayerPrefs.SetInt("bulletDamage", index + 1);
    }

    public void BuyBullet(int index)
    {
        if (PlayerPrefs.HasKey("bulletBuy" + index))
        {
            SelectBullet(index);
        }
        else
        {
            if (money >= bulletPrices[index])
            {
                money -= bulletPrices[index];
                moneyText.text = "Scraps: " + money;

                PlayerPrefs.SetInt("pointsScore", money);
                PlayerPrefs.SetInt("bulletBuy" + index, 1);

                SelectBullet(index);

                ButtonUpdate("bulletBuy" + index, index, bulletBuys, bulletSelects);
            }
        }
    }
    #endregion

    // Buys and selects a health point
    #region ShieldButton
    void SelectShield(int index, bool select)
    {
        if (select)
        {
            shieldSelected = index;
            PlayerPrefs.SetInt("shieldSelect", shieldSelected);
            shieldSelects[index].GetComponent<Image>().color = selectedColor;
            ButtonSelectedUpdate("shieldBuy", "shieldSelect", shieldSelected, shieldSelects, true);
        }
        else
        {
            shieldSelected = -1;
            ButtonSelectedUpdate("shieldBuy", "shieldSelect", shieldSelected, shieldSelects, true);
            shieldSelects[index].GetComponent<Image>().color = Color.white;
        }

        PlayerPrefs.SetInt("shieldSelect", shieldSelected);
    }

    public void BuyShield(int index)
    {
        if (PlayerPrefs.HasKey("shieldBuy" + index))
        {
            if (shieldSelected >= 0 || shieldSelected == index)
            {
                SelectShield(index, false);
                return;
            }

            SelectShield(index, true);
        }
        else
        {
            if (money >= shieldPrices[index])
            {
                money -= shieldPrices[index];
                moneyText.text = "Scraps: " + money;

                PlayerPrefs.SetInt("pointsScore", money);
                PlayerPrefs.SetInt("shieldBuy" + index, 1);

                SelectShield(index, true);

                ButtonUpdate("shieldBuy" + index, index, shieldBuys, shieldSelects);
            }
        }
    }
    #endregion
}
