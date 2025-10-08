using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    [Header("Tower Stats")]
    public float range = 8f;
    public int damage = 25;
    public float fireRate = 1f;
    public int cost = 3;

    [Header("Targeting Mode")]
    public bool first = true;
    public bool last = false;
    public bool strong = false;

    [Header("Effects")]
    // [SerializeField] GameObject fireEffect;

    [NonSerialized] public GameObject target;

    private float cooldown = 0f;
    public int totalInvestedCost;

    public Animator animator;
    public AudioClip attackSound;
    private AudioSource audioSource;

    void Awake() {
        //start with the cost of the tower
        totalInvestedCost = cost;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (target) {
            if (cooldown >= fireRate) {
                // Rotate sprite to face target
                transform.up = target.transform.position - transform.position;
                AnimateAttack();
                cooldown = 0f;
                // StartCoroutine(FireEffect());
            }
        }

        cooldown += Time.deltaTime;
    }

    /*
    IEnumerator FireEffect() {
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fireEffect.SetActive(false);
    }
    */

    private void AnimateAttack() {
        float attackSpeedMultiplier = 1f / fireRate; // e.g., 1 / 0.25 = 4x speed

        animator.speed = attackSpeedMultiplier; // speed up animation
        animator.SetTrigger("Attack");

        if (attackSound != null && audioSource != null) {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void TakeDamage() {
        if (target != null) {
            target.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
}