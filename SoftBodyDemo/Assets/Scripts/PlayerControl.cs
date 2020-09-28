using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    private Vector3 _startMousePos, _currentMousePos, _startPositionPlayer, _velosity;
    private Camera _cam;

    [SerializeField]
    private float _maxAddForse, _forceMultiplier;

    void Start()
    {
        _cam = Camera.main;
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
    //        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    //        _startPositionPlayer = transform.position;
    //    }
    //    else if (Input.GetMouseButton(0))
    //    {
    //        if (_startMousePos == Vector3.zero)
    //        {
    //            _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
    //            _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    //            _startPositionPlayer = transform.position;
    //        }
    //        _currentMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
    //        if (true)
    //        {

    //        }
    //        float Y = _startPositionPlayer.y + ((_currentMousePos - _startMousePos).y) * 2f;
    //        transform.position = new Vector3(transform.position.x, Y);
    //    }
    //    else if (Input.GetMouseButtonUp(0))
    //    {
    //        _rb.constraints = RigidbodyConstraints2D.None;
    //    }
    //}  
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _rb.transform.position = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            //_rb.constraints = RigidbodyConstraints2D.FreezePosition;
            //_rb.AddForce((_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10))-transform.position).normalized*300);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}
