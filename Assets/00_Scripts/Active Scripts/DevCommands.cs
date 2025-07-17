using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCommands : MonoBehaviour
{

    private bool isPaused = false;
    private bool isSlowedDown = false;
    private bool isSpedUp = false;

    public Player player;

    
    // Update is called once per frame
    void Update()
    {
        DevButtons();
    }

    private void Start() {
        
    }

    #region Dev cheats
    private void DevButtons() {
        //if (Input.GetKeyDown(KeyCode.Q)) ToggleSlowMotion();
        //if (Input.GetKeyDown(KeyCode.E)) TogglePause();
        if (Input.GetKeyDown(KeyCode.T)) ToggleSpeedUp();
        
    }

    private void ToggleTimeEffect(ref bool effectState, float timeScale) {
        effectState = !effectState;
        Time.timeScale = effectState ? timeScale : 1f;
    }

    public void ToggleSlowMotion() => ToggleTimeEffect(ref isSlowedDown, 0.5f);
    public void TogglePause() => ToggleTimeEffect(ref isPaused, 0f);
    public void ToggleSpeedUp() {

        ToggleTimeEffect(ref isSpedUp, 8f);
    } 
    #endregion
}
