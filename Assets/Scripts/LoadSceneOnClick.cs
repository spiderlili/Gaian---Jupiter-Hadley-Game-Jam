using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//load the correct scene based on level unlocked
public class LoadSceneOnClick : MonoBehaviour {
    public void LoadByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.Out, sceneIndex));
    }
}
