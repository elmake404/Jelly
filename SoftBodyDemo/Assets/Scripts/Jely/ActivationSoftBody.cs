using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationSoftBody : MonoBehaviour
{
    [SerializeField]
    private Blob _blob;
    public void OnBlob()
    {
        _blob.enabled = true;
    }
    public void OffBlob()
    {
        _blob.enabled = false;
    }
}
