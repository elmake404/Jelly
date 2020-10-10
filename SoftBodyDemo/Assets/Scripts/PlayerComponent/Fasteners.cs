using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fasteners : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    private Transform _contactPoint;
    private Vector3 _offSet;
    private float _magnitudeMain;
    void Start()
    {

    }
    private void FixedUpdate()
    {
        if (_contactPoint!=null)
        {
            _player.transform.position =
                new Vector3 ( _player.transform.position.x,(_contactPoint.position - _offSet).y, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Contact(collision.transform);
        }
    }
    private void Contact(Transform point)
    {
        float Magnitude = (point.position - transform.position).magnitude;
        if (_contactPoint == null
            || Magnitude < _magnitudeMain)
        {
            _contactPoint = point;
            _magnitudeMain = Magnitude;
        }
        else
            return;

        _offSet = _contactPoint.position - transform.position;

    }
}
