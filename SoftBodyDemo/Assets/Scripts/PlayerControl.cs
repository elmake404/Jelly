using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private CircleCollider2D colliderCircle;
    private Camera _cam;

    [SerializeField]
    private float _maxAddForse, _forceMultiplier;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            colliderCircle.isTrigger = false;
            Vector3 PosFinger = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            _rb.transform.position = PosFinger;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            colliderCircle.isTrigger = true;
        }
    }
}
