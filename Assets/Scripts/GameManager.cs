﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Iron Armor");
            RemoveItem("Health Potion");
            RemoveItem("Bleep");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        //could be done faster by making itemsHeld a map of string to Item
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }

        }


        return null;
    }

    //class version but mine is a lot faster O(N) vs O(N^2)ish
    //public void SortItems()
    //{
    //    while
    //    for(int i = 0; i < itemsHeld.Length; i++)
    //    {
    //        if(itemsHeld[i] == "" && i + 1 < itemsHeld.Length)
    //        {
    //            itemsHeld[i] = itemsHeld[i + 1];
    //            itemsHeld[i + 1] = "";

    //            numberOfItems[i] = numberOfItems[i + 1];
    //            numberOfItems[i + 1] = 0;
    //        }
    //    }
    //}
    public void SortItems()
    {
        string[] newItemsHeld = new string[40];
        int[] newNumberOfItems = new int[40];
        int currIndex = 0;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] != "")
            {
                newItemsHeld[currIndex] = itemsHeld[i];
                newNumberOfItems[currIndex] = numberOfItems[i];
                currIndex++;
            }
        }
        for (int i = currIndex; i < newNumberOfItems.Length; i++)
        {
            newItemsHeld[i] = "";
            newNumberOfItems[i] = 0;
        }
        itemsHeld = newItemsHeld;
        numberOfItems = newNumberOfItems;
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if ((itemsHeld[i] == "") || (itemsHeld[i] == itemToAdd))
            {
                newItemPosition = i;
                foundSpace = true;
                break;
            }
        }
        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {

                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    break;
                }
            }
            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist");
            }
        }
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;
                break;
            }
        }
        if (foundItem)
        {
            numberOfItems[itemPosition]--;
            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
        }
        else
        {
            Debug.LogError("Couldnt find: " + itemToRemove);
        }

        GameMenu.instance.ShowItems();

    }

    public void SaveData()
    {
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        //save character info
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHp", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WeaponPower", playerStats[i].weaponPower);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmorPower", playerStats[i].armorPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWeapon", playerStats[i].equippedWeapon);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmor", playerStats[i].equippedArmor);

        }

        //Store Inventory Data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);

        }
    }

    public void LoadData()
    {
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));
        
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }
            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHp");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP" );
            playerStats[i].strength =  PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence =  PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].weaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WeaponPower");
            playerStats[i].armorPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmorPower");
            playerStats[i].equippedWeapon = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWeapon");
            playerStats[i].equippedArmor = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmor");
        }
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);

        }
    }


}
