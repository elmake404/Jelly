using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody : MonoBehaviour
{
    [SerializeField]
    private AnchorPoint[] _anchorPoints;
    [SerializeField]
    private Blob[] _blobs;
    [SerializeField]
    private List<PointSpring> _listPointSprings;
    [SerializeField]
    private PointSpring _pointSpring;
    private Vector3 _positionPoint;

    [SerializeField]
    private float _sizeCollider, _step, _springDampingRatio, _springFrequency;

    void Awake()
    {
        CreateReferencePoints();
        OffBlob();
    }

    void Update()
    {

    }

    private void CreateReferencePoints()
    {
        _positionPoint = _anchorPoints[0].transform.position;

        int i = 1;
        Transform parentReferens = transform;
        float rotationZ = 0;

        while (true)
        {
            PointSpring referensPoint = Instantiate(_pointSpring, _positionPoint, Quaternion.identity);

            _listPointSprings.Add(referensPoint);

            if (_listPointSprings.Count > 1)
            {
                AttachWithSpringJoint(_listPointSprings[_listPointSprings.Count - 1], _listPointSprings[_listPointSprings.Count - 2], _springFrequency);
            }

            referensPoint.transform.SetParent(parentReferens);
            if (_anchorPoints[i].IsGround)
            {
                referensPoint.tag = "Ground";
            }

            if (_anchorPoints[i].tag == "Angle" && _anchorPoints[i].gameObject.layer==9)
            {
                _anchorPoints[i].Belongs._referencePoints.Add(referensPoint);
                _anchorPoints[i].BelongsAngle._referencePoints.Add(referensPoint);
            }
            else
            {
                _anchorPoints[i].Belongs._referencePoints.Add(referensPoint);
            }

            if ((_positionPoint - _anchorPoints[i].transform.position).magnitude < 0.01f)
            {
                if (_anchorPoints[i].tag == "Angle")
                {
                    if (_anchorPoints[i].BelongsAngle!=null && _anchorPoints[i].gameObject.layer != 9)
                    {
                        _anchorPoints[i].BelongsAngle._referencePoints.Add(referensPoint);
                    }
                    else
                    {
                        Debug.Log("_anchorPoints[i].BelongsAngle==null i="+i);
                    }
                }

                if (i < _anchorPoints.Length - 1)
                {
                    i++;
                }
                else
                {
                    break;
                }

                rotationZ = _anchorPoints[i].GetRot();
            }

            if (!_anchorPoints[i].IsCircle)
            {
                _positionPoint = Vector3.MoveTowards(_positionPoint, _anchorPoints[i].transform.position, _step);
            }
            else
            {
                _positionPoint = _anchorPoints[i].GetPositionOnACircle(rotationZ);
                rotationZ -= (_step*10);
            }
        }
        for (int g = 0; g < _blobs.Length; g++)
        {
            _blobs[g].StartMethod();
        }
    }
    private void AttachWithSpringJoint(PointSpring referencePoint, PointSpring connected, float springFrequency)
    {
        referencePoint.SpringJointNeighbour.connectedBody = connected.RigidbodyMain;
        referencePoint.SpringJointNeighbour.connectedAnchor = LocalPosition(referencePoint.transform) -
            LocalPosition(connected.transform);

        referencePoint.SpringJointNeighbour.distance = 0;
        referencePoint.SpringJointNeighbour.dampingRatio = _springDampingRatio;
        referencePoint.SpringJointNeighbour.frequency = springFrequency;
    }
    private Vector3 LocalPosition(Transform obj)
    {
        return transform.InverseTransformPoint(obj.position);
    }

    public void OnBlob()
    {
        for (int i = 0; i < _blobs.Length; i++)
        {
            _blobs[i].enabled = true;
        }
    }
    public void OffBlob()
    {
        for (int i = 0; i < _blobs.Length; i++)
        {
            _blobs[i].enabled = false;
        }
    }
}
