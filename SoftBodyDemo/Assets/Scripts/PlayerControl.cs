using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Transform _cargo;
    [SerializeField]
    private CircleCollider2D colliderCircle;
    private Vector3 _startMousePos, _currentMousePos, _startPositionPlayer, _velosity;
    private Camera _cam;

    [SerializeField]
    private float _maxTension;
    private bool _isBottomDown = false;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isBottomDown = true;
            Vector3 PosFinger = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            transform.position = PosFinger;
            _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (!colliderCircle.isTrigger)
            {
                _currentMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
                float Y = _startPositionPlayer.y + ((_currentMousePos - _startMousePos).y) * 10f;
                float X = _startPositionPlayer.x + ((_currentMousePos - _startMousePos).x) * 10f;
                Vector2 cargoPos = new Vector3(X, Y);
                //Vector2 MaxPos = (_currentMousePos - _startMousePos).normalized * colliderCircle.radius + transform.position;
                if ((cargoPos-(Vector2)transform.position).magnitude> _maxTension)
                {
                    cargoPos = (_currentMousePos - _startMousePos).normalized * _maxTension + transform.position;
                }
                _cargo.position = cargoPos;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _cargo.localPosition = Vector3.zero;
            _isBottomDown = false;
            colliderCircle.isTrigger = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isBottomDown && other.tag == "Wall")
        {
            PositionCalculation(other.GetComponent<Wall>());
        }
    }

    private void PositionCalculation(Wall wall)
    {
        Vector2 extremePosition = wall.GetPerimeter(transform.localPosition);
        Vector2 PosMain = (wall.transform.localPosition - transform.localPosition).normalized * colliderCircle.radius;
        Vector2 NewPos = transform.localPosition;
        if (transform.localPosition.x < wall.transform.localPosition.x)
        {

            if (PosMain.x < extremePosition.x)
            {
                NewPos.x = extremePosition.x- colliderCircle.radius;
            }
        }
        else
        {
            if (PosMain.x > extremePosition.x)
            {
                NewPos.x = extremePosition.x + colliderCircle.radius;
            }
        }

        if (transform.localPosition.y < wall.transform.localPosition.y)
        {
            if (PosMain.y < extremePosition.y)
            {
                NewPos.y = extremePosition.y - colliderCircle.radius;
            }
        }
        else
        {
            if (PosMain.y > extremePosition.y)
            {
                NewPos.y = extremePosition.y + colliderCircle.radius;
            }
        }
        transform.localPosition = NewPos;
        _startPositionPlayer = transform.position;
        colliderCircle.isTrigger = false;
    }
}
