using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlocker : MonoBehaviour
{
    [SerializeField]
    private Player _target;
    private void FixedUpdate()
    {
        transform.position = _target.transform.position;
        transform.eulerAngles = Vector3.zero;
    }
}
