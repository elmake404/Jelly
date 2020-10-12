using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    private Player _player;
    private List<Collider2D> _listCollision = new List<Collider2D>();

    [SerializeField]
    private bool IsBlokRight, IsBlokLeft, IsBlokJamp;
    private void FixedUpdate()
    {
        if (_listCollision.Count>0)
        {
            if (IsBlokJamp)
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
        else
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==8)
        {
            if (IsBlokJamp && collision.tag == "Ground")
            {
                _listCollision.Add(collision);
            }
            if (IsBlokLeft)
            {
                _listCollision.Add(collision);
            }
            if (IsBlokRight)
            {
                _listCollision.Add(collision);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (IsBlokJamp && collision.tag == "Ground")
            {
                _listCollision.Remove(collision);
            }
            if (IsBlokLeft)
            {
                _listCollision.Remove(collision);
            }
            if (IsBlokRight)
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
