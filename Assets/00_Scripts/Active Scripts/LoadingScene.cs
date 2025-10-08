using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    public void LoadScene(int sceneID) {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID); //begins loading the scene in background
        LoadingScreen.SetActive(true);
        while (!operation.isDone) {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f); // Convert Unity's 0-0.9 progress to 0-1 range
            LoadingBarFill.fillAmount = progressValue; // Update the progress bar visual
            yield return null;
        }
    }
}
