using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideJump : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    private List<Collider2D> _listCollision = new List<Collider2D>();

    [SerializeField]
    private bool IsRight, IsLeft;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            if (IsLeft)
            {
                _player.IsJumpLeft = true;
            }
            if (IsRight)
            {
                _player.IsJumpRight = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            if (IsLeft)
            {
                _player.IsJumpLeft = false;
            }
            if (IsRight)
            {
                _player.IsJumpRight = false;
            }
        }

    }
    public void InitializationPlayer(Player player)
    {
        _player = player;
    }

}
