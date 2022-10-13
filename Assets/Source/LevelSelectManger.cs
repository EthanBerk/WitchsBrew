using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManger : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Button> _buttons;
    private List<Text> _texts;
    void Start()
    {
        _buttons = gameObject.GetComponentsInChildren<Button>().ToList();
        _texts = gameObject.GetComponentsInChildren<Text>().ToList();
        
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            var button = _buttons[i];
            button.interactable = PlayerPrefs.GetInt("CompletedLevels") >= (i);
            if (PlayerPrefs.GetInt("CompletedLevels") >= (i + 1))
            {
                var TotalTimeLeft = PlayerPrefs.GetFloat("Level" + (i + 1).ToString());
                _texts[i].text = Mathf.Round(TotalTimeLeft).ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void loadLevel(int level)
    {
        SceneManager.LoadScene(level + 2);
    }
}
