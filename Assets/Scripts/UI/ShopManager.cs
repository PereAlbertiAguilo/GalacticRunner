using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;

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

        public bool isFirstSelected = true;
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("pointsScore", 2000000);
        PlayerPrefs.SetInt("stagesEnded", 4);

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
    void ButtonActiveStateUpdate(string key, int index, GameObject[] buys, GameObject[] selects)
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

    // Updates the UI for each shop item
    void UIUpdate()
    {
        foreach (ShopItem item in shopItems)
        {
            item.selectedIndex = PlayerPrefs.GetInt(item.itemName + "Select");
            ButtonSelectedUpdate(item.itemName + "Buy", item.itemName + "Select", item.selectedIndex,
                    item.selects, item.isFirstSelected, !item.isFirstSelected);

            for (int i = 0; i < item.prices.Length; i++)
            {
                item.buys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.prices[i] + " S";
                ButtonActiveStateUpdate(item.itemName + "Buy" + i, i, item.buys, item.selects);
                
            }
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
            else
            {
                selects[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";
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
                    else
                    {
                        selects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";
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
                    ButtonActiveStateUpdate(buyKey + index, index, buys, selects);
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
        BuyItem(index, shopItems[0].itemName + "Buy", shopItems[0].itemName + "LastBought", shopItems[0].itemName + "Select", 
            ref shopItems[0].selectedIndex, shopItems[0].buys, shopItems[0].selects, shopItems[0].prices, false, true);
    }
    #endregion

    // Buys and selects a bullet
    #region BulletButton
    public void BuyBullet(int index)
    {
        BuyItem(index, shopItems[1].itemName + "Buy", shopItems[1].itemName + "LastBought", shopItems[1].itemName + "Select", 
            ref shopItems[1].selectedIndex, shopItems[1].buys, shopItems[1].selects, shopItems[1].prices, false, true);
    }
    #endregion

    // Buys and selects an engine
    #region EngineButton
    public void BuyEngine(int index)
    {
        BuyItem(index, shopItems[2].itemName + "Buy", shopItems[2].itemName + "LastBought", shopItems[2].itemName + "Select", 
            ref shopItems[2].selectedIndex, shopItems[2].buys, shopItems[2].selects, shopItems[2].prices, true, false);
    }
    #endregion

    // Buys and selects a health point
    #region ShieldButton
    public void BuyShield(int index)
    {
        BuyItem(index, shopItems[3].itemName + "Buy", shopItems[3].itemName + "LastBought", shopItems[3].itemName + "Select", 
            ref shopItems[3].selectedIndex, shopItems[3].buys, shopItems[3].selects, shopItems[3].prices, true, false);
    }
    #endregion

    // Buys and selects the bullet shot speed
    #region ShieldButton
    public void BuyBulletShotSpeed(int index)
    {
        BuyItem(index, shopItems[4].itemName + "Buy", shopItems[4].itemName + "LastBought", shopItems[4].itemName + "Select", 
            ref shopItems[4].selectedIndex, shopItems[4].buys, shopItems[4].selects, shopItems[4].prices, true, false);
    }
    #endregion

    // Buys and selects the bullet speed
    #region ShieldButton
    public void BuyBulletSpeed(int index)
    {
        BuyItem(index, shopItems[5].itemName + "Buy", shopItems[5].itemName + "LastBought", shopItems[5].itemName + "Select", 
            ref shopItems[5].selectedIndex, shopItems[5].buys, shopItems[5].selects, shopItems[5].prices, true, false);
    }
    #endregion

    // Buys and selects the scraps amount
    #region ShieldButton
    public void BuyScrapsAmount(int index)
    {
        BuyItem(index, shopItems[6].itemName + "Buy", shopItems[6].itemName + "LastBought", shopItems[6].itemName + "Select",
            ref shopItems[6].selectedIndex, shopItems[6].buys, shopItems[6].selects, shopItems[6].prices, true, false);
    }
    #endregion
}
