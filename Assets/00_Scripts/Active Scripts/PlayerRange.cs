using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRange : MonoBehaviour {
    [SerializeField] private Player Player;
    private CircleCollider2D detectionCollider;
    private SpriteRenderer rangeSprite;
    public bool isVisible = false;

    void Start() {
        detectionCollider = GetComponent<CircleCollider2D>();
        rangeSprite = GetComponent<SpriteRenderer>();

        UpdateRange();
        SetTransparency(0f); // Start hidden
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            isVisible = !isVisible;
            SetTransparency(isVisible ? 0.5f : 0f);
        }
    }

    void SetTransparency(float alpha) {
        if (rangeSprite != null) {
            Color color = rangeSprite.color;
            color.a = alpha;
            rangeSprite.color = color;
        }
    }

    public void UpdateRange() {
        transform.localScale = new Vector3(Player.interactRange, Player.interactRange, Player.interactRange);
    }
}