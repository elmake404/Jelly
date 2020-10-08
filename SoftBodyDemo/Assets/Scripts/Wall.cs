using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private bool _isBlokY, _isBlokX;

    void Start()
    {
    }

    public Vector2 GetPerimeter(Vector2 PosObjContact, float Radius)
    {
        Vector2 PosMain = Vector2.zero;
        if (PosObjContact.x > transform.position.x)
        {

            PosMain.x = transform.position.x + _boxCollider.size.x / 2;
        }
        else
        {

            PosMain.x = transform.position.x - _boxCollider.size.x / 2;
        }

        if (PosObjContact.y > transform.position.y)
        {

            PosMain.y = transform.position.y + _boxCollider.size.y / 2;
        }
        else
        {

            PosMain.y = transform.position.x - _boxCollider.size.y / 2;
        }
        return PosMain;
    }
    public void GetSize(out float SizeX, out float SizeY)
    {

        SizeX = _isBlokX ? 0 : _boxCollider.size.x / 2;
        SizeY = _isBlokY ? 0 : _boxCollider.size.y / 2;
    }
}
