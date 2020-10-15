using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuUi, _winUI, _loseUI;
    [SerializeField]
    private Text _level;
    void Start()
    {
        if (PlayerPrefs.GetInt("Level") <= 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
        _level.text = "Level " + PlayerPrefs.GetInt("Level");
        if (!LevelManager.IsStartGame)
        {
            _menuUi.SetActive(true);
        }
    }

    void Update()
    {
        if (LevelManager.IsWin && !_winUI.activeSelf)
        {
            _winUI.SetActive(true);
            PlayerPrefs.SetInt("Scenes", PlayerPrefs.GetInt("Scenes")+1);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") +1);
        }
    }
}
