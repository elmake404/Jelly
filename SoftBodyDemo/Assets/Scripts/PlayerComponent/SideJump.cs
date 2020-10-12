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
    private void FixedUpdate()
    {
        if (_listCollision.Count > 0)
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
        else
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (IsLeft)
            {
                _listCollision.Add(collision);
            }
            if (IsRight)
            {
                _listCollision.Add(collision);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (IsLeft)
            {
                _listCollision.Remove(collision);
            }
            if (IsRight)
            {
                _listCollision.Remove(collision);
            }
        }

    }
    public void InitializationPlayer(Player player)
    {
        _player = player;
    }

}
