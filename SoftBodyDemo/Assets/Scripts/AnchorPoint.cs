using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public Blob Belongs, BelongsAngle;

    [SerializeField]
    private float _sizeCollider;

    private void Start()
    {

    }
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sizeCollider/2);
    }

}
