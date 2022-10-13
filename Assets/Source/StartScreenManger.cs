using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManger : MonoBehaviour
{
    // Start is called before the first frame updates
    public void StartGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CompletedLevels") + 3);
    }

    public void Totorial()
    {
        SceneManager.LoadScene(2);
    }

    public void LevelScreen()
    {
        SceneManager.LoadScene(1);
    }
}
