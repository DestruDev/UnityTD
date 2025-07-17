using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {
    [SerializeField] private Tower Tower;
    private List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        UpdateRange();
    }

    // Update is called once per frame
    void Update() {
        // Remove any dead enemies from the targets list
        for (int i = targets.Count - 1; i >= 0; i--) {
            if (targets[i] == null || targets[i].GetComponent<Enemy>().GetCurrentHealth() <= 0) {
                targets.RemoveAt(i);
            }
        }

        if (targets.Count > 0) {
            if (Tower.first) {
                float minDistance = Mathf.Infinity;
                int maxIndex = 0;
                GameObject firstTarget = null;

                foreach (GameObject target in targets) {
                    int index = target.GetComponent<Enemy>().index;
                    float distance = target.GetComponent<Enemy>().distance;

                    if (index > maxIndex || (index == maxIndex && distance < minDistance)) {
                        maxIndex = index;
                        minDistance = distance;
                        firstTarget = target;
                    }
                }

                Tower.target = firstTarget;
            } else if (Tower.last) {
                float maxDistance = -Mathf.Infinity;
                int minIndex = int.MaxValue;
                GameObject lastTarget = null;

                foreach (GameObject target in targets) {
                    int index = target.GetComponent<Enemy>().index;
                    float distance = target.GetComponent<Enemy>().distance;

                    if (index < minIndex || (index == minIndex && distance > maxDistance)) {
                        minIndex = index;
                        maxDistance = distance;
                        lastTarget = target;
                    }
                }

                Tower.target = lastTarget;
            } else if (Tower.strong) {
                GameObject strongestTarget = null;
                float maxHealth = 0;
                foreach (GameObject target in targets) {
                    float health = target.GetComponent<Enemy>().maxHealth;

                    if (health > maxHealth) {
                        maxHealth = health;
                        strongestTarget = target;
                    }
                }
                Tower.target = strongestTarget;
            } else {
                Tower.target = targets[0];
            }
        } else {
            Tower.target = null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            // Add the enemy if it's alive
            if (collision.gameObject.GetComponent<Enemy>().GetCurrentHealth() > 0) {
                targets.Add(collision.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            targets.Remove(collision.gameObject);
        }
    }

    public void UpdateRange() {
        transform.localScale = new Vector3(Tower.range, Tower.range, Tower.range);
    }
}
