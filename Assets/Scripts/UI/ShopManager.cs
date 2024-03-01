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
        spaceCraftSelected = PlayerPrefs.GetInt("spaceSelect");
        bulletSelected = PlayerPrefs.GetInt("bulletSelect");
        shieldSelected = PlayerPrefs.GetInt("shieldSelect");

        ButtonSelectedUpdate("spaceBuy", "spaceSelect", spaceCraftSelected, spaceCraftSelects, true);
        ButtonSelectedUpdate("bulletBuy", "bulletSelect", bulletSelected, bulletSelects, true);
        ButtonSelectedUpdate("shieldBuy", "shieldSelect", shieldSelected, shieldSelects, false);

        money = PlayerPrefs.GetInt("pointsScore");

        moneyText.text = "" + money;

        PricesUpdate();

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void ButtonUpdate(string key, int index, GameObject[] buys, GameObject[] selects)
    {
        if(PlayerPrefs.HasKey(key))
        {
            if(PlayerPrefs.GetInt(key) == 1)
            {
                buys[index].SetActive(false);
                spaceCraftSelects[index].SetActive(true);
            }
            else
            {
                buys[index].SetActive(true);
                selects[index].SetActive(false);
            }
        }
        else
        {
            print("no key");
            buys[index].SetActive(true);
            selects[index].SetActive(false);
        }
    }

    void PricesUpdate()
    {
        for (int i = 0; i < spaceCraftBuys.Length; i++)
        {
            spaceCraftBuys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + spaceCraftPrices[i];
        }
        for (int i = 0; i < spaceCraftBuys.Length; i++)
        {
            bulletBuys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + bulletPrices[i];
        }
        for (int i = 0; i < spaceCraftBuys.Length; i++)
        {
            shieldBuys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + shieldPrices[i];
        }
    }

    void ButtonSelectedUpdate(string keyBuy, string keySelect, int selected, GameObject[] selects, bool firstSelected)
    {
        if (PlayerPrefs.HasKey(keySelect))
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
                moneyText.text = "" + money;

                PlayerPrefs.SetInt("pointsScore", money);
                PlayerPrefs.SetInt("spaceBuy" + index, 1);

                SelectSpaceCraft(index);

                ButtonUpdate("spaceBuy" + index, index, spaceCraftBuys, spaceCraftSelects);
            }
        }
    }
    #endregion

    #region BulletButton
    void SelectBullet(int index)
    {
        bulletSelected = index;

        PlayerPrefs.SetInt("bulletSelect", bulletSelected);

        bulletSelects[index].GetComponent<Image>().color = selectedColor;

        ButtonSelectedUpdate("bulletBuy", "bulletSelect", bulletSelected, bulletSelects, true);
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
                moneyText.text = "" + money;

                PlayerPrefs.SetInt("pointsScore", money);
                PlayerPrefs.SetInt("bulletBuy" + index, 1);

                SelectBullet(index);

                ButtonUpdate("bulletBuy" + index, index, bulletBuys, bulletSelects);
            }
        }
    }
    #endregion

    #region ShieldButton
    void SelectShield(int index)
    {
        shieldSelected = index;

        PlayerPrefs.SetInt("shieldSelect", shieldSelected);

        shieldSelects[index].GetComponent<Image>().color = selectedColor;

        ButtonSelectedUpdate("shieldBuy", "shieldSelect", shieldSelected, shieldSelects, true);
    }

    public void BuyShield(int index)
    {
        if (PlayerPrefs.HasKey("shieldBuy" + index))
        {
            SelectShield(index);
        }
        else
        {
            if (money >= shieldPrices[index])
            {
                money -= shieldPrices[index];
                moneyText.text = "" + money;

                PlayerPrefs.SetInt("pointsScore", money);
                PlayerPrefs.SetInt("shieldBuy" + index, 1);

                SelectShield(index);

                ButtonUpdate("shieldBuy" + index, index, shieldBuys, shieldSelects);
            }
        }
    }
    #endregion
}
