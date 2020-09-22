using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour
{
    ////private class PropagateCollisions : MonoBehaviour
    ////{
    ////    void OnCollisionEnter2D(Collision2D collision)
    ////    {
    ////        transform.parent.SendMessage("OnCollisionEnter2D", collision);
    ////    }
    ////}
    [SerializeField]
    private int _width = 5, _height = 5,_referencePointsCount = 12;
    [SerializeField]
    private float _sizeCollider, _step, _mappingDetail = 10, _springDampingRatio = 0, _springFrequency = 2;
    [SerializeField]
    private PhysicsMaterial2D _surfaceMaterial;

    private GameObject[] _referencePoints;
    private Vector3[] _vertices;
    private Vector2[] _uv;
    private Vector3[,] _offsets;

    private int _vertexCount;
    private int[] _triangles;
    private float[,] _weights;

    private void Start()
    {
        CreateReferencePoints();
        CreateMesh();
        MapVerticesToReferencePoints();
    }
    private void Update()
    {
        UpdateVertexPositions();
    }
    private void CreateReferencePoints()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        _referencePointsCount = Mathf.RoundToInt(transform.localScale.x / _step);

        _referencePoints = new GameObject[_referencePointsCount];

        float positionCollider = -(_sizeCollider / 2 + (_sizeCollider * (Mathf.Round(transform.localScale.x) - 1f)));

        for (int i = 0; i < _referencePointsCount; i++)
        {
            _referencePoints[i] = new GameObject();
            _referencePoints[i].tag = gameObject.tag;
            //_referencePoints[i].AddComponent<PropagateCollisions>();
            _referencePoints[i].transform.parent = transform;

            #region Old
            //переработать
            //Quaternion rotation =
            //    Quaternion.AngleAxis(angle * (i - 1), Vector3.back);
            //referencePoints[i].transform.localPosition =
            //    rotation * offsetFromCenter; 
            #endregion

            _referencePoints[i].transform.position = new Vector2(transform.position.x + positionCollider, transform.position.y + 0.25f);

            positionCollider += _step;

            Rigidbody2D body = _referencePoints[i].AddComponent<Rigidbody2D>();
            body.freezeRotation = true;
            body.interpolation = rigidbody.interpolation;
            body.collisionDetectionMode = rigidbody.collisionDetectionMode;

            BoxCollider2D collider =
                _referencePoints[i].AddComponent<BoxCollider2D>();

            collider.size = new Vector2(_sizeCollider, _sizeCollider);

            if (_surfaceMaterial != null)
            {
                collider.sharedMaterial = _surfaceMaterial;
            }
            AttachWithSpringJoint(_referencePoints[i], gameObject);
            if (i > 0)
            {
                AttachWithSpringJoint(_referencePoints[i],
                        _referencePoints[i - 1]);
            }
        }
        AttachWithSpringJoint(_referencePoints[0], true);
        AttachWithSpringJoint(_referencePoints[_referencePoints.Length-1], false);
        IgnoreCollisionsBetweenReferencePoints();
    }
    private void AttachWithSpringJoint(GameObject referencePoint, GameObject connected)
    {
        SpringJoint2D springJoint =
            referencePoint.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint) -
            LocalPosition(connected);

        springJoint.distance = 0;
        springJoint.dampingRatio = _springDampingRatio;
        springJoint.frequency = _springFrequency;
    }
    private void AttachWithSpringJoint(GameObject referencePoint, bool fastenersFromTheBack)
    {
        SpringJoint2D springJoint = referencePoint.AddComponent<SpringJoint2D>();

        Rigidbody2D rbConnected = Fasteners(fastenersFromTheBack);
        springJoint.connectedBody = rbConnected;

        springJoint.connectedAnchor = LocalPosition(referencePoint) -
            LocalPosition(rbConnected.gameObject);
        springJoint.distance = 0;
        springJoint.dampingRatio = _springDampingRatio;
        springJoint.frequency = _springFrequency;
    }
    private Rigidbody2D Fasteners(bool multiplierGreaterThanZero)
    {
        int multiplier = multiplierGreaterThanZero ? -1 : 1;
        float positionCollider = multiplier*(_sizeCollider / 2 + (_sizeCollider * (Mathf.Round(transform.localScale.x) - 1f)));
        GameObject fasteners = new GameObject();

        fasteners.transform.parent = transform;
        fasteners.transform.position = new Vector2(transform.position.x + positionCollider+(_step*multiplier), transform.position.y + 0.25f);

        Rigidbody2D rbFasteners = fasteners.AddComponent<Rigidbody2D>();
        rbFasteners.freezeRotation = true;
        rbFasteners.constraints =RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezePositionX;
        return rbFasteners;
    }
    private void IgnoreCollisionsBetweenReferencePoints()
    {
        BoxCollider2D a;
        BoxCollider2D b;

        for (int i = 0; i < _referencePointsCount; i++)
        {
            for (int j = i; j < _referencePointsCount; j++)
            {
                a = _referencePoints[i].GetComponent<BoxCollider2D>();
                b = _referencePoints[j].GetComponent<BoxCollider2D>();
                Physics2D.IgnoreCollision(a, b, true);
            }
        }
    }
    private void CreateMesh()
    {
        _vertexCount = (_width + 1) * (_height + 1);

        int trianglesCount = _width * _height * 6;
        _vertices = new Vector3[_vertexCount];
        _triangles = new int[trianglesCount];
        _uv = new Vector2[_vertexCount];
        int t;

        for (int y = 0; y <= _height; y++)
        {
            for (int x = 0; x <= _width; x++)
            {
                int v = (_width + 1) * y + x;
                _vertices[v] = new Vector3(x / (float)_width - 0.5f,
                        y / (float)_height - 0.5f, 0);
                _uv[v] = new Vector2(x / (float)_width, y / (float)_height);

                if (x < _width && y < _height)
                {
                    t = 3 * (2 * _width * y + 2 * x);

                    _triangles[t] = v;
                    _triangles[++t] = v + _width + 1;
                    _triangles[++t] = v + _width + 2;
                    _triangles[++t] = v;
                    _triangles[++t] = v + _width + 2;
                    _triangles[++t] = v + 1;
                }
            }
        }
        //тут
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.vertices = _vertices;
        mesh.uv = _uv;
        mesh.triangles = _triangles;
    }
    private void MapVerticesToReferencePoints()
    {
        _offsets = new Vector3[_vertexCount, _referencePointsCount];
        _weights = new float[_vertexCount, _referencePointsCount];

        for (int i = 0; i < _vertexCount; i++)
        {
            float totalWeight = 0;

            for (int j = 0; j < _referencePointsCount; j++)
            {
                _offsets[i, j] = _vertices[i] - LocalPosition(_referencePoints[j]);
                _weights[i, j] =
                    1 / Mathf.Pow(_offsets[i, j].magnitude, _mappingDetail);
                totalWeight += _weights[i, j];
            }

            for (int j = 0; j < _referencePointsCount; j++)
            {
                _weights[i, j] /= totalWeight;
            }
        }
    }
    private void UpdateVertexPositions()
    {
        Vector3[] vertices = new Vector3[_vertexCount];

        for (int i = 0; i < _vertexCount; i++)
        {
            vertices[i] = Vector3.zero;

            for (int j = 0; j < _referencePointsCount; j++)
            {
                vertices[i] += _weights[i, j] *
                    (LocalPosition(_referencePoints[j]) + _offsets[i, j]);
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
    private Vector3 LocalPosition(GameObject obj)
    {
        return transform.InverseTransformPoint(obj.transform.position);
    }
}
