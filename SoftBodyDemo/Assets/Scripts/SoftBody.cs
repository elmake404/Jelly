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

    void Start()
    {
        CreateReferencePoints();
    }

    void Update()
    {

    }

    private void CreateReferencePoints()
    {
        _positionPoint = _anchorPoints[0].transform.position;

        int i = 1;
        Transform parentReferens = transform;
        while (true)
        {
            PointSpring referensPoint = Instantiate(_pointSpring, _positionPoint, Quaternion.identity);

            _listPointSprings.Add(referensPoint);

            referensPoint.transform.SetParent(parentReferens);

            _anchorPoints[i].Belongs._referencePoints.Add(referensPoint);

            if ((_positionPoint - _anchorPoints[i].transform.position).magnitude < 0.01f)
            {
                if (_anchorPoints[i].tag == "Angle")
                {
                    _anchorPoints[i].BelongsAngle._referencePoints.Add(referensPoint);
                }
                if (i < _anchorPoints.Length - 1)
                {
                    //parentReferens = transform;
                    i++;
                }
                else
                {
                    break;
                }
            }


            if (_listPointSprings.Count > 1)
            {
                AttachWithSpringJoint(_listPointSprings[_listPointSprings.Count - 1], _listPointSprings[_listPointSprings.Count - 2], _springFrequency);
            }

            _positionPoint = Vector3.MoveTowards(_positionPoint, _anchorPoints[i].transform.position, _step);
        }
        for (int g = 0; g < _blobs.Length; g++)
        {
            _blobs[g].StartMethod();
        }
    }
    private void AttachWithSpringJoint(PointSpring referencePoint, PointSpring connected, float springFrequency)
    {
        referencePoint._springJointNeighbour.connectedBody = connected._rigidbodyMain;
        referencePoint._springJointNeighbour.connectedAnchor = LocalPosition(referencePoint.transform) -
            LocalPosition(connected.transform);

        referencePoint._springJointNeighbour.distance = 0;
        referencePoint._springJointNeighbour.dampingRatio = _springDampingRatio;
        referencePoint._springJointNeighbour.frequency = springFrequency;
    }
    private Vector3 LocalPosition(Transform obj)
    {
        return transform.InverseTransformPoint(obj.position);
    }

}
