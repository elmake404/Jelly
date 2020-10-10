using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spy : MonoBehaviour
{
    [SerializeField]
    private Player _target;
    [SerializeField]
    private Blocker[] blockers;
    private void Awake()
    {
        for (int i = 0; i < blockers.Length; i++)
        {
           blockers[i].InitializationPlayer(_target);
        }
    }
    private void LateUpdate()
    {
        transform.position = _target.transform.position;
    }
}
