using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private Vector3 _offSet, _cameraPos;
    [SerializeField]
    private Vector3 _limitDown, _limitUp;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private bool _isMove = true, _isMoveStatUp;
    [SerializeField]
    private float _topWindow = 5, _bottomWindow = 10;
    private void Awake()
    {
        _limitDown = transform.position;
        if (_isMoveStatUp)
        {
            transform.position = _limitUp;
        }
    }
    void Start()
    {
        _offSet = _target.position - transform.position;
        _cameraPos = new Vector3(transform.position.x, _target.position.y - _offSet.y, transform.position.z);
    }

    void FixedUpdate()
    {
        if (!LevelManager.IsGameOver && _isMove)
        {
            if (_target.position.y > transform.position.y + _topWindow)
            {
                _cameraPos = new Vector3(transform.position.x, _target.position.y - _topWindow, transform.position.z);
            }
            else if (_target.position.y < transform.position.y - _bottomWindow)
            {
                _cameraPos = new Vector3(transform.position.x, _target.position.y + _bottomWindow, transform.position.z);
            }

            CorectionPositioncamera();

            transform.position = Vector3.SmoothDamp(transform.position, _cameraPos, ref velocity, 0.07f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            collision.GetComponent<ActivationSoftBody>().OnBlob();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            collision.GetComponent<ActivationSoftBody>().OffBlob();
        }
    }
    private void CorectionPositioncamera()
    {
        if (_cameraPos.y > _limitUp.y)
        {
            _cameraPos.y = _limitUp.y;
        }
        if (_cameraPos.y < _limitDown.y)
        {
            _cameraPos.y = _limitDown.y;
        }
    }
    [ContextMenu("UpperLimitEntry")]
    private void UpperLimitEntry()
    {
        _limitUp = transform.position;
    }
}
