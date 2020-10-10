using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _startMousePos, _currentMousePos;
    [SerializeField]
    private Rigidbody2D _rigidbodyMain;
    private Camera _cam;

    [SerializeField]
    private float _jampForce;
    [SerializeField]
    private float _upperSpeedLimit, _lowerSpeedLimit, _lateralSpeedLimit,_lateralFlySpeedLimit;
    private float _speed;
    [SerializeField]
    private bool _isJamp/*, _isFly*/;

    //[HideInInspector]
    public bool IsTouchingTheGround, IsNotRight, IsNotLeft;
    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rigidbodyMain.constraints = RigidbodyConstraints2D.None;
            _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            _currentMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);

            float direction = (_currentMousePos - _startMousePos).normalized.x > 0 ? 1 : -1;
            if (IsTouchingTheGround)
            {
                _speed = Mathf.Lerp(_speed, direction * _lateralSpeedLimit, 0.3f);
            }
            else
            {
                _speed = Mathf.Lerp(_speed, direction * _lateralFlySpeedLimit, 0.3f);
            }

            if ((_currentMousePos - _startMousePos).y > 0.1f)
            {
                _isJamp = true;
            }
            else
            {
                _isJamp = false;
            }

            if (Mathf.Abs((_currentMousePos - _startMousePos).x) > 0.15f)
            {
                if ((direction>0&& !IsNotRight)|| (direction < 0 && !IsNotLeft))
                {
                    _rigidbodyMain.velocity = new Vector2(_speed, _rigidbodyMain.velocity.y);
                }
                else
                {
                    _rigidbodyMain.velocity = new Vector2(direction , _rigidbodyMain.velocity.y);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _rigidbodyMain.constraints = RigidbodyConstraints2D.FreezeRotation;

            _isJamp = false;
        }
    }
    private void FixedUpdate()
    {
        if (IsTouchingTheGround && _isJamp)
        {
            _rigidbodyMain.AddForce(Vector2.up * _jampForce);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Abyss")
        {
            LevelManager.IsGameOver = true;
        }
    }
}
