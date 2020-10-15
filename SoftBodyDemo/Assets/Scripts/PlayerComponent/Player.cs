using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _startMousePos, _currentMousePos, _startPosPLayer;
    [SerializeField]
    private Rigidbody2D _rigidbodyMain;
    private Camera _cam;

    [SerializeField]
    private float _jampForce;
    [SerializeField]
    private float _lateralSpeedLimit, _lateralFlySpeedLimit, _sensitivityDirection;
    private float _speed;
    [SerializeField]
    private bool _isJamp, _isCantMove;

    //[HideInInspector]
    public bool IsTouchingTheGround, IsJumpRight, IsJumpLeft, IsNotRight, IsNotLeft;
    private void Awake()
    {
        _startPosPLayer = transform.position;
    }
    private void Start()
    {
        _cam = Camera.main;
    }
    private void Update()
    {
        if (!LevelManager.IsGameOver && !LevelManager.IsWin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _rigidbodyMain.constraints = RigidbodyConstraints2D.None;
                _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                _currentMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
                if (!_isCantMove)
                {
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

                    if (Mathf.Abs((_currentMousePos - _startMousePos).x) > 0.01f)
                    {
                        if (((direction > 0 && !IsNotRight) || (direction < 0 && !IsNotLeft)))
                        {
                            _rigidbodyMain.velocity = new Vector2(_speed, _rigidbodyMain.velocity.y);
                        }
                        else
                        {
                            _rigidbodyMain.velocity = new Vector2(direction * 4, _rigidbodyMain.velocity.y);
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _rigidbodyMain.constraints = RigidbodyConstraints2D.FreezeRotation;

                _isJamp = false;
            }
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
            StartCoroutine(CantMove());
            _rigidbodyMain.AddForce(new Vector2(-1f, 1.5f) * _jampForce * 7);
        }
        if (!IsTouchingTheGround && _isJamp && IsJumpLeft && _rigidbodyMain.velocity.y < 0.1f)
        {
            StartCoroutine(CantMove());
            _rigidbodyMain.AddForce(new Vector2(1f, 1.5f) * _jampForce * 7);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Abyss")
        {
            LevelManager.IsGameOver = true;
            gameObject.SetActive(false);
        }
        if (collision.tag == "Finish")
        {
            LevelManager.IsWin = true;
            _rigidbodyMain.velocity = Vector2.zero;
            _rigidbodyMain.isKinematic=true;
            StartCoroutine(Win(collision.transform));
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
        _rigidbodyMain.velocity = Vector2.zero;
        _isCantMove = true;
        yield return new WaitForSeconds(0.5f);
        _speed = _rigidbodyMain.velocity.x;
        _isCantMove = false;
    }
    private IEnumerator Win(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.05f);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.01f);
        yield return new WaitForSeconds(0.02f);
    }
    public void GoToStartPos()
    {
        _rigidbodyMain.constraints = RigidbodyConstraints2D.FreezeRotation;
        _isJamp = false;
        _isCantMove = false;

        transform.position = _startPosPLayer;
    }
}
