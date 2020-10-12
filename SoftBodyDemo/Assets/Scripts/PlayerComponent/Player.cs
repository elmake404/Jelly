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
    private float _lateralSpeedLimit, _lateralFlySpeedLimit, _sensitivityDirection;
    private float _speed;
    private bool _isJamp, _isCantMove;

    [HideInInspector]
    public bool IsTouchingTheGround, IsJumpRight, IsJumpLeft, IsNotRight, IsNotLeft;
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

            float direction = GetDirection((_currentMousePos - _startMousePos).x);

            if (IsTouchingTheGround)
            {
                _speed = Mathf.Lerp(_speed, direction * _lateralSpeedLimit, 0.3f);
            }
            else
            {
                _speed = Mathf.Lerp(_speed, direction * _lateralFlySpeedLimit, 0.3f);
            }

            _isJamp = true;

            if (Mathf.Abs((_currentMousePos - _startMousePos).x) > 0.01f && !_isCantMove)
            {
                if (((direction > 0 && !IsNotRight) || (direction < 0 && !IsNotLeft)))
                {
                    _rigidbodyMain.velocity = new Vector2(_speed, _rigidbodyMain.velocity.y);
                }
                else
                {
                    _rigidbodyMain.velocity = new Vector2(direction, _rigidbodyMain.velocity.y);
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
        if (!IsTouchingTheGround && _isJamp && IsJumpRight && _rigidbodyMain.velocity.y < 0.1f)
        {
            _rigidbodyMain.AddForce(new Vector2(-1, 1) * _jampForce * 10);
            StartCoroutine(CantMove());
        }
        if (!IsTouchingTheGround && _isJamp && IsJumpLeft && _rigidbodyMain.velocity.y < 0.1f)
        {
            _rigidbodyMain.AddForce(new Vector2(1, 1) * _jampForce * 10);
            StartCoroutine(CantMove());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Abyss")
        {
            LevelManager.IsGameOver = true;
        }
    }
    private float GetDirection(float X)
    {
        X *= _sensitivityDirection;
        if (X > 1)
        {
            X = 1;
        }
        else if (X < -1)
        {
            X = -1;
        }
        return X;
    }
    private IEnumerator CantMove()
    {
        _isCantMove = true;
        yield return new WaitForSeconds(0.5f);
        Debug.Log(0);
        _isCantMove = false;
    }
}
