using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject exitUi;
    public GameObject newHighScoreUi;
    public Text NewHighScoreText;
    public GameObject looseUi;
    public GameObject winUi;
    public int requiredAmount;
    public int totalAmount { get; set; }

    private float startTime;

    private float time;

    public GameObject text;
    private Text _text;
    public List<Text> curent = new List<Text>();
    public List<Text> Needed = new List<Text>();
    public List<Text> EndTimes = new List<Text>();

    public float TotalTime;

    public int level;
    public bool canExit = true;

    private void Start()
    {

        TotalTime *= 60;
        exitUi.SetActive(false);
        looseUi.SetActive(false);
        newHighScoreUi.SetActive(false);
        winUi.SetActive(false);
        startTime = Time.time;
        _text = text.GetComponent<Text>();
        foreach (var text in Needed)
        {
            text.text = requiredAmount.ToString();
        }


    }

    // Update is called once per frame
    void Update()
    {
        foreach (var text in curent)
        {
            text.text = totalAmount.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canExit)
        {
            if (exitUi.activeSelf)
            {
                closeExit();
            }
            else
            {
                OpenExit();
            }
        }

        time = Time.time - startTime;

        _text.text = Mathf.Floor(Mathf.Round(TotalTime - time) / 60).ToString() + ":" +
                     (((Mathf.Round(TotalTime - time) - Mathf.Floor(Mathf.Round(TotalTime - time) / 60) * 60) < 10)
                         ? "0"
                         : "") +
                     (Mathf.Round(TotalTime - time) - Mathf.Floor(Mathf.Round(TotalTime - time) / 60) * 60)
                     .ToString();
        
        
        if (TotalTime - time <= 0 || totalAmount >= requiredAmount)
        {
            Time.timeScale = 0;
            canExit = false;
            
            var timeLeft = TotalTime - time;
            foreach (var endTime in EndTimes)
            {
                endTime.text = Mathf.Round(timeLeft).ToString();

            }
            if (totalAmount < requiredAmount)
            {
                Time.timeScale = 0;
                looseUi.SetActive(true);
            }

            if (level > PlayerPrefs.GetInt("CompletedLevels")) PlayerPrefs.SetInt("CompletedLevels", level);
            if (PlayerPrefs.GetFloat("Level" + (level).ToString() ) < timeLeft)
            {
                PlayerPrefs.SetFloat("Level" + level.ToString(), timeLeft);
                newHighScoreUi.SetActive(true);
                
            }
            else
            {
                winUi.SetActive(true);
            }

        }

    }





    public void OpenExit()
    {
        Time.timeScale = 0;
        exitUi.SetActive(true);
    }

    public void closeExit()
    {
        Time.timeScale = 1;
        exitUi.SetActive(false);
    }

    public void resetScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void home()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
  
