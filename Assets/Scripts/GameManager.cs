using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
