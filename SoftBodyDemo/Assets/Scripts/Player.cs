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
    private float _jampForce, _forceGround, _forceFly;
    private bool _isJamp,_isFly;

    //[HideInInspector]
    //public bool IsJamp;
    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            _currentMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
            Vector2 direction = (_currentMousePos - _startMousePos).normalized;

            if ((_currentMousePos - _startMousePos).y>0.1f)
            {
                _isJamp = true;
            }
            else
            {
                _isJamp = false;
            }

            if (direction.y>0)
            {
                direction.y = 0;
            }
            if (_isFly)
            {
                _rigidbodyMain.AddForce(direction * _forceFly);
            }
            else
            {
                _rigidbodyMain.AddForce(direction * _forceGround);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isJamp = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isJamp )
        {
            _rigidbodyMain.AddForce(Vector2.up*_jampForce);
        }
        _isFly = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _isFly = true;
    }

}
