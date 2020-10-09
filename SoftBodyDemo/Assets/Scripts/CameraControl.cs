using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private Vector3 _offSet;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        _offSet = _target.position - transform.position;
    }

    void FixedUpdate()
    {
        if (!LevelManager.IsGameOver)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position - _offSet, ref velocity, 0.03f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer ==14)
        {
            collision.GetComponent<SoftBody>().OnBlob();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            collision.GetComponent<SoftBody>().OffBlob();
        }

    }
}
