using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPlacement : MonoBehaviour
{
    public static TowerPlacement Instance { get; private set; }
    [SerializeField] private SpriteRenderer rangeSprite;
    [SerializeField] private CircleCollider2D rangeCollider;
    [SerializeField] private Color gray;
    [SerializeField] private Color red;
    public List<Image> uiBlockerImages;
    
    [NonSerialized] public bool isPlacing = true;
    [HideInInspector] public bool isRestricted = false;
    private Tower tower;
    private PlayerRange playerRange;
    [HideInInspector] public bool inPlayerInteractRange = false;
    private int restrictedOverlapCount = 0;
    public GameManager gameManager;


    void Awake()
    {   
        tower = GetComponent<Tower>();
        
        rangeCollider.enabled = false;

        uiBlockerImages = GameManager.Instance.uiBlockerImages;
        /*
        if (restrictedMessage == null) {
            restrictedMessage = GameObject.FindWithTag("RestrictedMessage");
        }

        if (restrictedMessage != null) {
            restrictedMessage.SetActive(false); // Ensure it starts hidden, just in case
        }
        */

        gameManager = GameManager.Instance;
    }

  


    void Update()
    {
        if (isPlacing) {
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = mousePosition;

            UpdateRestrictionState();
            
        }
        /*
        if (isPlacing && restrictedMessage != null) {
            restrictedMessage.SetActive(isRestricted);
        }
        */

        if (isPlacing && gameManager.restrictedMessage != null) {
            gameManager.restrictedMessage.SetActive(isRestricted);
        }
        // Places the tower
        if (Input.GetMouseButtonDown(0) && !isRestricted && GameStats.Instance.playerGold >= tower.cost) {
           
            rangeCollider.enabled = true;
            isPlacing = false;
            rangeSprite.enabled = false;
            GetComponent<TowerPlacement>().enabled = false;
            GameStats.Instance.playerGold -= tower.cost;
            GameStats.Instance.UpdateGoldText();
        }

        rangeSprite.color = isRestricted ? red : gray;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((collision.CompareTag("Restricted") || collision.CompareTag("Tower")) && isPlacing) {
            restrictedOverlapCount++;
            UpdateRestrictionState();
        }

        if (collision.CompareTag("PlayerRange") && isPlacing) {
            inPlayerInteractRange = true;
            UpdateRestrictionState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if ((collision.CompareTag("Restricted") || collision.CompareTag("Tower")) && isPlacing) {
            restrictedOverlapCount = Mathf.Max(0, restrictedOverlapCount - 1);
            UpdateRestrictionState();
        }

        if (collision.CompareTag("PlayerRange") && isPlacing) {
            inPlayerInteractRange = false;
            UpdateRestrictionState();
        }
    }

    private void UpdateRestrictionState() {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        bool isUnderUI = false;

        foreach (Image img in uiBlockerImages) {
            if (img == null) continue; // safety check

            RectTransform rect = img.rectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, null)) {
                isUnderUI = true;
                break;
            }
        }

        isRestricted = restrictedOverlapCount > 0 || !inPlayerInteractRange || isUnderUI;
    }

}
