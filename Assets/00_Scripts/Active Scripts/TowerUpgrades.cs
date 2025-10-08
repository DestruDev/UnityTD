using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerUpgrades : MonoBehaviour
{
    [System.Serializable]
    class Level {
        public float range = 8f;
        public int damage = 25;
        public float fireRate = 1f;
        public int cost = 3;
    }
    [SerializeField] private Level[] levels = new Level[3]; //creates an array of levels
    [NonSerialized] public int currentLevel = 0;
    [NonSerialized] public string currentCost;
    private Tower tower;

    [SerializeField] private TowerRange towerRange; 
    void Awake()
    {
        tower = GetComponent<Tower>();
        //set the current cost to the cost of the first level
        currentCost = levels[0].cost.ToString();
    }
    public void Upgrade() {
        //if level isnt max, and player has enough gold, upgrade
        if (currentLevel < levels.Length && GameStats.Instance.playerGold >= levels[currentLevel].cost) {
            //upgrade the stats and range
            tower.range = levels[currentLevel].range;
            tower.damage = levels[currentLevel].damage;
            tower.fireRate = levels[currentLevel].fireRate;
            towerRange.UpdateRange();
            
            //subtract the cost of the upgrade from the player's gold
            GameStats.Instance.playerGold -= levels[currentLevel].cost;
            GameStats.Instance.UpdateGoldText();

            // Add the cost of THIS upgrade to total invested
            tower.totalInvestedCost += levels[currentLevel].cost;

            currentLevel++;
            //if level is max, set the current cost to "MAX"
            if (currentLevel >= levels.Length) {
                currentCost = "MAX";
            } else {
                //if level isnt max, set the current cost to the cost of the current level
                currentCost = levels[currentLevel].cost.ToString();
            }

        } else {
            Debug.Log("Not enough gold to upgrade.");
        }
    }
}
