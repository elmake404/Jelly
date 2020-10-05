using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private Transform _refernsPoint;
    private Vector3 _offSet;
    private void Start()
    {
        List<RaycastHit2D> hits2D = new List<RaycastHit2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();

        Physics2D.CircleCast(transform.position, 0.5f, Vector2.zero, contactFilter2D, hits2D);
        Transform contactPoint = null;
        float magnitude = float.PositiveInfinity;
        for (int i = 0; i < hits2D.Count; i++)
        {
            Transform refernsPointOther = hits2D[i].collider.gameObject.transform;
            if (refernsPointOther.gameObject.layer == 8 && refernsPointOther.parent != transform)
            {
                if (magnitude > (refernsPointOther.position - transform.position).magnitude)
                {
                    magnitude = (refernsPointOther.position - transform.position).magnitude;
                    contactPoint = refernsPointOther;
                }
            }
        }
        transform.SetParent(contactPoint);
    }
}
