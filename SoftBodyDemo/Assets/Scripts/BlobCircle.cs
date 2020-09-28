using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobCircle : MonoBehaviour
{
    [SerializeField]
    private int _width = 5, _height = 5, _referencePointsCount = 12;
    [SerializeField]
    private float _referencePointRadius = 0.25f, _mappingDetail = 10, _springDampingRatio = 0, _springFrequency = 2;
    [SerializeField]
    private PhysicsMaterial2D _surfaceMaterial;

    private GameObject[] _referencePoints;
    private int _vertexCount;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uv;
    private Vector3[,] _offsets;
    private float[,] _weights;

    void Start()
    {
        CreateReferencePoints();
        CreateMesh();
        MapVerticesToReferencePoints();
    }

    void CreateReferencePoints()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        _referencePoints = new GameObject[_referencePointsCount];
        Vector3 offsetFromCenter = ((0.5f - _referencePointRadius) * Vector3.up);
        float angle = 360.0f / _referencePointsCount;

        for (int i = 0; i < _referencePointsCount; i++)
        {
            _referencePoints[i] = new GameObject();
            _referencePoints[i].tag = gameObject.tag;
            _referencePoints[i].transform.parent = transform;
            Quaternion rotation =
                Quaternion.AngleAxis(angle * (i - 1), Vector3.back);
            _referencePoints[i].transform.localPosition =
                rotation * offsetFromCenter;

            Rigidbody2D body = _referencePoints[i].AddComponent<Rigidbody2D>();
            body.freezeRotation = true;
            body.interpolation = rigidbody.interpolation;
            body.collisionDetectionMode = rigidbody.collisionDetectionMode;
            body.mass =0.5f;

            CircleCollider2D collider =
                _referencePoints[i].AddComponent<CircleCollider2D>();
            collider.radius = _referencePointRadius * transform.localScale.x;
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
        AttachWithSpringJoint(_referencePoints[0],
                _referencePoints[_referencePointsCount - 1]);

        IgnoreCollisionsBetweenReferencePoints();
    }

    void AttachWithSpringJoint(GameObject referencePoint, GameObject connected)
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

    void IgnoreCollisionsBetweenReferencePoints()
    {
        int i;
        int j;
        CircleCollider2D a;
        CircleCollider2D b;

        for (i = 0; i < _referencePointsCount; i++)
        {
            for (j = i; j < _referencePointsCount; j++)
            {
                a = _referencePoints[i].GetComponent<CircleCollider2D>();
                b = _referencePoints[j].GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(a, b, true);
            }
        }
    }

    void CreateMesh()
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

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = _vertices;
        mesh.uv = _uv;
        mesh.triangles = _triangles;
    }

    void MapVerticesToReferencePoints()
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

    void Update()
    {
        UpdateVertexPositions();
    }

    void UpdateVertexPositions()
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

    Vector3 LocalPosition(GameObject obj)
    {
        return transform.InverseTransformPoint(obj.transform.position);
    }
}
