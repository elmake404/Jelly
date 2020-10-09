using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Static
    public static bool IsGameOver;
    #endregion
    private void Awake()
    {
        IsGameOver = false;
    }

    void FixedUpdate()
    {
        if (IsGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
}
