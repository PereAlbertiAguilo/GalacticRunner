using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopItem[] shopItems;
    [Space]

    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] Color selectedColor;

    [Space]

    [SerializeField] GameObject warningPanel;

    int money;

    [Serializable]
    public class ShopItem
    {
        public string itemName;

        public GameObject[] selects;
        public GameObject[] buys;

        public int[] prices;

        public int selectedIndex = 0;
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("pointsScore", 1000000);

        // Gets the selected values from player prefs
        shopItems[0].selectedIndex = PlayerPrefs.GetInt("spaceSelect");
        shopItems[1].selectedIndex = PlayerPrefs.GetInt("bulletSelect");
        shopItems[2].selectedIndex = PlayerPrefs.GetInt("engineSelect");
        shopItems[3].selectedIndex = PlayerPrefs.GetInt("shieldSelect");
        shopItems[4].selectedIndex = PlayerPrefs.GetInt("bulletShotSpeedSelect");
        shopItems[5].selectedIndex = PlayerPrefs.GetInt("bulletSpeedSelect");

        // Updates the selected estate of the buttons
        ButtonSelectedUpdate("spaceBuy", "spaceSelect", shopItems[0].selectedIndex, shopItems[0].selects, true, false);
        ButtonSelectedUpdate("bulletBuy", "bulletSelect", shopItems[1].selectedIndex, shopItems[1].selects, true, false);
        ButtonSelectedUpdate("engineBuy", "engineSelect", shopItems[2].selectedIndex, shopItems[2].selects, false, true);
        ButtonSelectedUpdate("shieldBuy", "shieldSelect", shopItems[3].selectedIndex, shopItems[3].selects, false, true);
        ButtonSelectedUpdate("bulletShotSpeedBuy", "bulletShotSpeedSelect", shopItems[4].selectedIndex, shopItems[4].selects, false, true);
        ButtonSelectedUpdate("bulletSpeedBuy", "bulletSpeedSelect", shopItems[5].selectedIndex, shopItems[5].selects, false, true);

        // Updates the moeny
        money = PlayerPrefs.GetInt("pointsScore");
        moneyText.text = "Scraps:\n" + money;

        // Updates the prices texts
        UIUpdate();
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

    // Updates the prices texts and the buttons of the UI for each shop item
    void UIUpdate()
    {
        foreach (ShopItem item in shopItems)
        {
            for (int i = 0; i < item.prices.Length; i++)
            {
                item.buys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.prices[i] + " S";
            }
        }

        for (int i = 0; i < shopItems[0].prices.Length; i++)
        {
            ButtonUpdate("spaceBuy" + i, i, shopItems[0].buys, shopItems[0].selects);
        }
        for (int i = 0; i < shopItems[1].prices.Length; i++)
        {
            ButtonUpdate("bulletBuy" + i, i, shopItems[1].buys, shopItems[1].selects);
        }
        for (int i = 0; i < shopItems[2].prices.Length; i++)
        {
            ButtonUpdate("engineBuy" + i, i, shopItems[2].buys, shopItems[2].selects);
        }
        for (int i = 0; i < shopItems[3].prices.Length; i++)
        {
            ButtonUpdate("shieldBuy" + i, i, shopItems[3].buys, shopItems[3].selects);
        }
        for (int i = 0; i < shopItems[4].prices.Length; i++)
        {
            ButtonUpdate("bulletShotSpeedBuy" + i, i, shopItems[4].buys, shopItems[4].selects);
        }
        for (int i = 0; i < shopItems[5].prices.Length; i++)
        {
            ButtonUpdate("bulletSpeedBuy" + i, i, shopItems[5].buys, shopItems[5].selects);
        }
    }

    // Updates the selected estate of the buttons
    void ButtonSelectedUpdate(string keyBuy, string keySelect, int selectedIndex, GameObject[] selects, bool firstSelected, bool canBeUnselected)
    {
        if (!PlayerPrefs.HasKey(keySelect) && firstSelected)
        {
            PlayerPrefs.SetInt(keyBuy + 0, 1);
            PlayerPrefs.SetInt(keySelect, 0);
            selects[0].GetComponent<Image>().color = selectedColor;
            if (canBeUnselected)
            {
                selects[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Deselect";
            }
        }
        else
        {
            for (int i = 0; i < selects.Length; i++)
            {
                if (i == selectedIndex)
                {
                    selects[i].GetComponent<Image>().color = selectedColor;

                    if (canBeUnselected)
                    {
                        selects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Deselect";
                    }
                }
                else
                {
                    selects[i].GetComponent<Image>().color = Color.white;
                    selects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select";
                }
            }
        }
    }

    void SelectItem(int index, string buyKey, string selectKey, ref int selectedIndex, 
        GameObject[] selects, bool canBeUnselected, bool select)
    {
        if (canBeUnselected)
        {
            if (select)
            {
                UpdateSelectedItem(index, buyKey, selectKey, ref selectedIndex, selects, canBeUnselected);
            }
            else
            {
                selectedIndex = -1;
                ButtonSelectedUpdate(buyKey, selectKey, selectedIndex, selects, false, canBeUnselected);
                PlayerPrefs.SetInt(selectKey, selectedIndex);
                selects[index].GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            UpdateSelectedItem(index, buyKey, selectKey, ref selectedIndex, selects, canBeUnselected);
        }
    }
    void UpdateSelectedItem(int index, string buyKey, string selectKey, ref int selectedIndex, GameObject[] selects, bool canBeUnselected)
    {
        selectedIndex = index;
        PlayerPrefs.SetInt(selectKey, selectedIndex);
        selects[index].GetComponent<Image>().color = selectedColor;
        ButtonSelectedUpdate(buyKey, selectKey, selectedIndex, selects, false, canBeUnselected);
    }
    void BuyItem(int index, string buyKey, string lastBoughtKey, string selectKey, ref int selectedIndex, 
        GameObject[] buys, GameObject[] selects, int[] prices, bool canBeUnselected, bool isFirstBought)
    {
        if (PlayerPrefs.HasKey(buyKey + index))
        {
            if (canBeUnselected)
            {
                if (index == selectedIndex)
                {
                    SelectItem(index, buyKey, selectKey, ref selectedIndex, selects, canBeUnselected, false);
                    return;
                }
                else
                {
                    SelectItem(index, buyKey, selectKey, ref selectedIndex, selects, canBeUnselected, true);
                }
            }
            else
            {
                SelectItem(index, buyKey, selectKey, ref selectedIndex, selects, canBeUnselected, true);
            }
        }
        else
        {
            if (!PlayerPrefs.HasKey(lastBoughtKey) && !isFirstBought)
            {
                PlayerPrefs.SetInt(lastBoughtKey, -1);
            }
            if (index <= PlayerPrefs.GetInt(lastBoughtKey) + 1)
            {
                if (money >= prices[index])
                {
                    money -= prices[index];
                    moneyText.text = "Scraps:\n" + money;
                    PlayerPrefs.SetInt("pointsScore", money);
                    PlayerPrefs.SetInt(buyKey + index, 1);
                    PlayerPrefs.SetInt(lastBoughtKey, index);
                    SelectItem(index, buyKey, selectKey, ref selectedIndex, selects, canBeUnselected, true);
                    ButtonUpdate(buyKey + index, index, buys, selects);
                }
            }
            else
            {
                warningPanel.SetActive(true);
            }
        }
    }
    

    // Buys and selects a spacecraft
    #region SpaceCraftButton
    public void BuySpaceCraft(int index)
    {
        BuyItem(index, "spaceBuy", "lastSpaceBought", "spaceSelect", ref shopItems[0].selectedIndex, 
            shopItems[0].buys, shopItems[0].selects, shopItems[0].prices, false, true);
    }
    #endregion

    // Buys and selects a bullet
    #region BulletButton
    public void BuyBullet(int index)
    {
        BuyItem(index, "bulletBuy", "lastBulletBought", "bulletSelect", ref shopItems[1].selectedIndex, 
            shopItems[1].buys, shopItems[1].selects, shopItems[1].prices, false, true);
    }
    #endregion

    // Buys and selects an engine
    #region EngineButton
    public void BuyEngine(int index)
    {
        BuyItem(index, "engineBuy", "lastEngineBought", "engineSelect", ref shopItems[2].selectedIndex, 
            shopItems[2].buys, shopItems[2].selects, shopItems[2].prices, true, false);
    }
    #endregion

    // Buys and selects a health point
    #region ShieldButton
    public void BuyShield(int index)
    {
        BuyItem(index, "shieldBuy", "lastShieldBought", "shieldSelect", ref shopItems[3].selectedIndex, 
            shopItems[3].buys, shopItems[3].selects, shopItems[3].prices, true, false);
    }
    #endregion

    // Buys and selects the bullet shot speed
    #region ShieldButton
    public void BuyBulletShotSpeed(int index)
    {
        BuyItem(index, "bulletShotSpeedBuy", "lastBulletShotSpeedBought", "bulletShotSpeedSelect", 
            ref shopItems[4].selectedIndex, shopItems[4].buys, shopItems[4].selects, shopItems[4].prices, true, false);
    }
    #endregion

    // Buys and selects the bullet speed
    #region ShieldButton
    public void BuyBulletSpeed(int index)
    {
        BuyItem(index, "bulletSpeedBuy", "lastBulletSpeedBought", "bulletSpeedSelect", 
            ref shopItems[5].selectedIndex, shopItems[5].buys, shopItems[5].selects, shopItems[5].prices, true, false);
    }
    #endregion
}
