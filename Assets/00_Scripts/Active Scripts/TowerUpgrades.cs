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
    [SerializeField] private Level[] levels = new Level[3];
    [NonSerialized] public int currentLevel = 0;
    [NonSerialized] public string currentCost;
    private Tower tower;

    [SerializeField] private TowerRange towerRange; 
    // Start is called before the first frame update
    void Awake()
    {
        tower = GetComponent<Tower>();
        currentCost = levels[0].cost.ToString();
    }
    public void Upgrade() {
        if (currentLevel < levels.Length && GameStats.Instance.playerGold >= levels[currentLevel].cost) {
            tower.range = levels[currentLevel].range;
            tower.damage = levels[currentLevel].damage;
            tower.fireRate = levels[currentLevel].fireRate;
            towerRange.UpdateRange();

            GameStats.Instance.playerGold -= levels[currentLevel].cost;
            GameStats.Instance.UpdateGoldText();

            // Add the cost of THIS upgrade to total invested
            tower.totalInvestedCost += levels[currentLevel].cost;

            currentLevel++;

            if (currentLevel >= levels.Length) {
                currentCost = "MAX";
            } else {
                currentCost = levels[currentLevel].cost.ToString();
            }

        } else {
            Debug.Log("Not enough gold to upgrade.");
        }
    }
}
