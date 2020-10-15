using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Static
    public static bool IsStartGame, IsWin, IsGameOver;
    #endregion
    [SerializeField]
    private Player _player;
    private void Awake()
    {
        //if (!IsStartGame)
        //{
        //    _player.gameObject.SetActive(false);
        //}

        IsGameOver = false;
    }

    void FixedUpdate()
    {
        //if (IsStartGame&& !_player.gameObject.activeSelf)
        //{
        //    _player.gameObject.SetActive(true);
        //}

        if (IsGameOver)
        {
            IsGameOver = false;
          StartCoroutine(GoToStartPosPlayer());
        }
    }
    private IEnumerator GoToStartPosPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        _player.GoToStartPos();
        _player.gameObject.SetActive(true);
    }

}
