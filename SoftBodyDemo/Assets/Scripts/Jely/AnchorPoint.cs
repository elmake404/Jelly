using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public Blob Belongs, BelongsAngle;
    [SerializeField]
    private Transform _compass, _compassPencil;

    public bool IsCircle;
    [SerializeField]
    private bool _isTop, _isBottom, _isRight, _isLeft;

    [SerializeField]
    private float _sizeCollider;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sizeCollider/2);
    }
    public float GetRot()
    {
        if (_isTop)
        {
            return 90;
        }
        else if (_isRight)
        {
            return 0;
        }
        else if (_isBottom)
        {
            return 270;
        }
        else 
        {
            return 180;
        }

    }

    public Vector2 GetPositionOnACircle(float Rotation)
    {
        _compass.eulerAngles = new Vector3(0, 0, Rotation);
        return _compassPencil.position;
    }

}
