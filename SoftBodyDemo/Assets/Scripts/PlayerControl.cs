using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Transform _cargo;
    [SerializeField]
    private CircleCollider2D colliderCircle;
    [SerializeField]
    private LayerMask _layerMask;
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
                if ((cargoPos - (Vector2)transform.position).magnitude > _maxTension)
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
        Vector2 direction = Vector2.zero;
        List<RaycastHit2D> hit2D = new List<RaycastHit2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        float SizeX;
        float SizeY;
        wall.GetSize(out SizeX, out SizeY);

        if (transform.position.x <= wall.transform.position.x + SizeX
            && transform.position.x >= wall.transform.position.x - SizeX)
        {
            if (transform.position.y > wall.transform.position.y)
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.up;
            }
        }
        else if (transform.position.y <= wall.transform.position.y + SizeY
           && transform.position.y >= wall.transform.position.y - SizeY)
        {
            if (transform.position.x > wall.transform.position.x)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }

        Physics2D.Raycast(transform.position, direction, contactFilter, hit2D);

        for (int i = 0; i < hit2D.Count; i++)
        {
            if (hit2D[i].collider.gameObject.layer == 11)
            {
                transform.position = hit2D[i].point - direction * colliderCircle.radius;
                break;
            }
        }
        colliderCircle.isTrigger = false;
        _startPositionPlayer = transform.position;
    }
}
