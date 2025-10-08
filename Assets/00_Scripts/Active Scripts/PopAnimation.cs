using UnityEngine;

public class PopAnimation : MonoBehaviour {
    public float popDuration = 0.3f;
    public float popScale = 1.5f;
    private Vector3 originalScale;
    private float timer;
    private bool popping;

    private void Awake() {
        originalScale = transform.localScale; //set variable to object's original scale
    }

    private void Update() {
        if (!popping) return; //if not popping, do nothing

        timer += Time.deltaTime; //increment timer by time since last frame
        float progress = timer / popDuration;

        if (progress >= 1f) { //if progress is 100%, set progress to 100% and set popping to false
            progress = 1f;
            popping = false;
        }

        // Scale up and then back to original
        float scale = Mathf.Lerp(popScale, 1f, progress);
        transform.localScale = originalScale * scale;
    }

    public void PlayPop() {
        popping = true;
        timer = 0f;
    }
}