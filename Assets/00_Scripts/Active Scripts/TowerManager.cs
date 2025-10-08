using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

[System.Serializable]
public class TowerData {
    public string name;
    public GameObject prefab;
    public TextMeshProUGUI costText;
}

public class TowerManager : MonoBehaviour {
    public static TowerManager Instance { get; private set; }
    [Header("Towers")]
    [SerializeField] private List<TowerData> towers = new List<TowerData>();

    [SerializeField] private LayerMask towerLayer;
    [SerializeField] public GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerLevel;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI towerTargetting;
    [SerializeField] public TextMeshProUGUI sellPrice;

    private GameObject selectedTower;
    public GameObject placingTower;
    private TowerPlacement tower;
    private float sellPercentage = 0.5f;
    private int sellValue = 0;
    public bool isUpgrading = false;
    [SerializeField] private Transform towerContainer;

    private void Awake() {
        tower = GetComponent<TowerPlacement>();
        UpdateCostUI();
        upgradePanel.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) || GameManager.Instance.settingsPanel.activeSelf) {
            ClearSelected();
        }

        if (placingTower != null) {
            var placement = placingTower.GetComponent<TowerPlacement>();
            if (placement == null || !placement.isPlacing) {
                placingTower = null;
            } else {
                // A tower *is* being placed, so hide the upgrade panel
                if (upgradePanel.activeSelf) {
                    ClearUpgradePanel();
                }
            }
        }



        //Select the tower with Left Click
        if (!placingTower && Input.GetMouseButtonDown(0)) {
            //raycast to the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, towerLayer);
            if (hit.collider != null) {
                isUpgrading = true;
                if (selectedTower) {
                    selectedTower.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                }
                selectedTower = hit.collider.gameObject;
                selectedTower.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

                upgradePanel.SetActive(true);
                towerName.text = selectedTower.name.Replace("(Clone)", "").Trim();
                towerLevel.text = "Tower LVL: " + (selectedTower.GetComponent<TowerUpgrades>().currentLevel + 1).ToString();
                upgradeCost.text = "$ " + selectedTower.GetComponent<TowerUpgrades>().currentCost;
                Tower tower = selectedTower.GetComponent<Tower>();
                UpdateSellValue();

                if (tower.first) towerTargetting.text = "First";
                else if (tower.last) towerTargetting.text = "Last";
                else if (tower.strong) towerTargetting.text = "Strong";
            } else if (!EventSystem.current.IsPointerOverGameObject() && selectedTower) {
                ClearUpgradePanel();
            }
        }
    }
    // Clears the currently selected tower
    public void ClearSelected() {
        if (placingTower) {
            Destroy(placingTower);
            placingTower = null;
        }
    }
    // Clears currently open upgrade panel
    public void ClearUpgradePanel() {
        upgradePanel.SetActive(false);
        selectedTower.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        selectedTower = null;
        isUpgrading = false;
    }

    public void SetTower(int towerIndex) {
        ClearSelected();
        if (towerIndex < 0 || towerIndex >= towers.Count) return;

        GameObject towerPrefab = towers[towerIndex].prefab;
        int cost = towerPrefab.GetComponent<Tower>().cost;
        //if player has enough gold, instantiate the tower
        if (GameStats.Instance.playerGold >= cost) {
            placingTower = Instantiate(towerPrefab, towerContainer);
        } else {
            Debug.Log("Not enough gold to purchase tower.");
        }
    }
    // Changes values on tower upgrade panel on upgrade
    public void UpgradeSelected() {
        if (selectedTower) {
            selectedTower.GetComponent<TowerUpgrades>().Upgrade();
            towerLevel.text = "Tower LVL: " + (selectedTower.GetComponent<TowerUpgrades>().currentLevel + 1).ToString();
            upgradeCost.text = "$ " + selectedTower.GetComponent<TowerUpgrades>().currentCost;

            UpdateSellValue();


        }
    }

    public void ChangeTargetting() {
        if (selectedTower) {
            Tower tower = selectedTower.GetComponent<Tower>();

            if (tower.first) {
                tower.first = false;
                tower.last = true;
                tower.strong = false;
                towerTargetting.text = "Last";
            } else if (tower.last) {
                tower.first = false;
                tower.last = false;
                tower.strong = true;
                towerTargetting.text = "Strong";
            } else {
                tower.first = true;
                tower.last = false;
                tower.strong = false;
                towerTargetting.text = "First";
            }
        }
    }

    public void UpdateCostUI() {
        foreach (var towerData in towers) {
            int cost = towerData.prefab.GetComponent<Tower>().cost;
            if (towerData.costText != null)
                towerData.costText.text = "$" + cost;
        }
    }

    public void SellTower() {
        if (selectedTower) {
            Tower tower = selectedTower.GetComponent<Tower>();
            GameStats.Instance.playerGold += sellValue;
            GameStats.Instance.UpdateGoldText();
            upgradePanel.SetActive(false);
            Destroy(selectedTower);
            selectedTower = null;
        }
    }

    public void UpdateSellValue() {
        Tower tower = selectedTower.GetComponent<Tower>();
        sellValue = Mathf.CeilToInt(tower.totalInvestedCost * sellPercentage);
        sellPrice.text = "Sell: $" + sellValue.ToString();
    }
}
