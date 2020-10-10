using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private bool IsBlokRight, IsBlokLeft, IsBlokJamp;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer==8)
        {
            if (IsBlokJamp && collision.tag == "Ground")
            {
                _player.IsTouchingTheGround = true;
            }
            if (IsBlokLeft)
            {
                _player.IsNotLeft = true;
            }
            if (IsBlokRight)
            {
                _player.IsNotRight = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (IsBlokJamp)
            {
                _player.IsTouchingTheGround = false;
            }
            if (IsBlokLeft)
            {
                _player.IsNotLeft = false;
            }
            if (IsBlokRight)
            {
                _player.IsNotRight = false;
            }

        }

    }
    public void InitializationPlayer(Player player)
    {
        _player = player;
    }
}
