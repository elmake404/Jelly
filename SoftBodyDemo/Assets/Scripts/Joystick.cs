using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private Vector2 _startMousePos, _currentMousePos;
    private Camera _cam;
    [SerializeField]
    private SpriteRenderer _mainSprite, _stickSprite;
    void Start()
    {
        _cam = Camera.main;
        OffStick();
    }

    void Update()
    {
        if (!LevelManager.IsGameOver && !LevelManager.IsWin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
                OnStick();
                _mainSprite.transform.position
                    = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3));
            }
            else if (Input.GetMouseButton(0))
            {
                _currentMousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
                Vector2 direction = (_currentMousePos - _startMousePos).normalized;
                if ((_currentMousePos - _startMousePos).magnitude >= 0.01f)
                {
                    _stickSprite.transform.localPosition = direction * (_currentMousePos - _startMousePos).magnitude * 4;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OffStick();
            }
        }
    }
    private void OnStick()
    {
        _mainSprite.enabled = true;
        _stickSprite.enabled = true;
    }
    private void OffStick()
    {
        _stickSprite.transform.localPosition = Vector3.zero;
        _mainSprite.enabled = false;
        _stickSprite.enabled = false;
    }
}
