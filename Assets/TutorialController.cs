using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
    private int screen = 1;
    public GameObject q;
    public GameObject w;
    public GameObject p;

    private void Update()
    {
        switch (screen)
        {
            case 2:
                q.SetActive(false);
                w.SetActive(true);
                break;
            case 3:
                w.SetActive(false);
                p.SetActive(true);
                break;
            case 4:
                SceneManager.LoadScene(0);
                break;
                
            
        }
    }

    public void nextScreen()
    {
        screen++;
    }
}
