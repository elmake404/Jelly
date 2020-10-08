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
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    _target.IsNotJamp = true;
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    _target.IsNotJamp = false;
    //}
}
