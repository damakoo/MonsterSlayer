using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] BlackJackManager _blackJackManager;
    public void ReloadCurrentScene()
    {
        _blackJackManager.PressedReload();
        //Scene currentScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(currentScene.name);
    }
}
