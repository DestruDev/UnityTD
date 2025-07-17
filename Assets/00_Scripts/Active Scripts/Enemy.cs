using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float moveSpeed = 2f;
    public int maxHealth = 100;
    public int value = 10;
    private int currentHealth;
    private Transform[] waypoints;
    public Image healthBar;
    public float spawnTime;
    private static int enemyCount = 0;
    public int EnemyID { get; private set; }
    public GameObject spriteBody;
    [NonSerialized] public int index = 0;
    [NonSerialized] public float distance = 0;
    public Animator animator;
    private string currentAnimation;
    private bool isDead = false;

    void Start() {
        currentHealth = maxHealth;
        UpdateHealthBar();
        spawnTime = Time.time;
        EnemyID = enemyCount++;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0 && !isDead) {
            // Trigger death animation only once
            isDead = true;
            AnimateDeath();
            StartCoroutine(WaitForDeathAnimation());
            GameStats.Instance.AddGold(value);
        }
    }

    private IEnumerator WaitForDeathAnimation() {
        // Wait for the duration of the "Death" animation before destroying the object
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // Now destroy the GameObject after the animation has played
        Destroy(gameObject);
    }

    public void SetWaypoints(Transform[] newWaypoints) {
        if (newWaypoints == null || newWaypoints.Length == 0) return;

        waypoints = newWaypoints;
        transform.position = waypoints[0].position;
        index = 1;

    }

    void Update() {
        // Prevent movement if the enemy is dead
        if (isDead || waypoints == null || waypoints.Length == 0 || index >= waypoints.Length) return;
        Transform targetWaypoint = waypoints[index];
        distance = Vector2.Distance(transform.position, targetWaypoint.position);


        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f) {
            index++;

            // Check if this was the last waypoint
            if (index >= waypoints.Length) {
                ReachedLastWaypoint();
            }
        }


    }

    private void ReachedLastWaypoint() {


        GameStats.Instance.LoseLife(value);

        Destroy(gameObject);
    }

    private void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
        
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }


    private void AnimateDeath() {
        // Directly play the "Death" animation clip
        animator.Play("Death");
    }

    void OnEnable() {
        // Reset static counter when scene reloads (optional, but helps prevent ID issues)
        if (EnemyID == 0) {  // Only reset if this is the first enemy
            enemyCount = 0;
        }

    }

}