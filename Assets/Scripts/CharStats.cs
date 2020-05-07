using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{

    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseExp = 1000;

    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLevelBonus;
    public int strength;
    public int defence;
    public int weaponPower;
    public int armorPwr;
    public string equippedWeapon;
    public string equippedArmor;
    public Sprite charImage;
    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseExp;
        int currExp = baseExp;
        for (int i = 1; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = currExp;
            currExp = Mathf.FloorToInt(currExp * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
       // tsting leveling up
        if (Input.GetKeyDown(KeyCode.K))
        {
            addExp(500);
        }
    }

    public void addExp(int expToAdd)
    {
        currentEXP += expToAdd;
        if ( playerLevel < maxLevel && currentEXP > expToNextLevel[playerLevel])
        {
            currentEXP -= expToNextLevel[playerLevel];
            playerLevel++;

            //add to str or defense based on odd or even level
            if (playerLevel % 2 == 0)
            {
                strength++;
            } else
            {
                defence++;
            }
            maxHP = Mathf.FloorToInt(maxHP * 1.05f);
            currentHP = maxHP;
            maxMP += mpLevelBonus[playerLevel];
            currentMP = maxMP;
        }
        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
