using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _boxCollider;

    void Start()
    {
    }

    public Vector2 GetPerimeter(Vector2 PosObjContact)
    {
        Vector2 PosMain;
        if (PosObjContact.x > transform.localPosition.x)
        {
            PosMain.x = transform.localPosition.x + _boxCollider.size.x / 2;
        }
        else
        {
            PosMain.x = transform.localPosition.x - _boxCollider.size.x / 2;
        }

        if (PosObjContact.y > transform.localPosition.y)
        {
            PosMain.y = transform.localPosition.y + _boxCollider.size.y / 2;
        }
        else
        {
            PosMain.y = transform.localPosition.x - _boxCollider.size.y / 2;
        }
        return PosMain;
    }
}
