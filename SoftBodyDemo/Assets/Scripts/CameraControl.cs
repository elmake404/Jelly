using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private Vector3 _offSet, _CameraPos;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        _offSet = _target.position - transform.position;
        _CameraPos = new Vector3(transform.position.x, _target.position.y- _offSet.y, transform.position.z);
    }

    void FixedUpdate()
    {
        if (!LevelManager.IsGameOver)
        {
            if (_target.position.y> transform.position.y)
            {
                _CameraPos = new Vector3(transform.position.x, _target.position.y, transform.position.z);
            }
            transform.position =  Vector3.SmoothDamp(transform.position, _CameraPos, ref velocity, 0.07f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer ==14)
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
}
