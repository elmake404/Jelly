using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        FacebookManager.Instance.GameStart();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LevelManager.IsStartGame = true;
            gameObject.SetActive(false);
        }
    }
}
