using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blob : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _meshMain;
    [SerializeField]
    private Rigidbody2D _rbMain;
    private List<Vector3> _vertices;
    private List<Vector2> _uv;
    private Vector3[,] _offsets;

    //[HideInInspector]
    public List<PointSpring> _referencePoints;

    [SerializeField]
    private int _width = 5, _height = 5;
    [SerializeField]
    private float _mappingDetail = 10, _springDampingRatio = 0, _springFrequency = 2;
    private int[] _triangles;
    private float[,] _weights;
    [SerializeField]
    private bool _isStart = false;
    private void Update()
    {
        UpdateVertexPositions();
    }
    private void CreateReferencePoints()
    {
        for (int i = 0; i < _referencePoints.Count; i++)
        {
            AttachWithSpringJoint(_referencePoints[i], _rbMain, _springFrequency);
        }
    }
    private void AttachWithSpringJoint(PointSpring referencePoint, Rigidbody2D connected, float springFrequency)
    {
        SpringJoint2D springJoint = referencePoint.gameObject.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint.transform) -
            LocalPosition(connected.transform);

        springJoint.distance = 0;
        springJoint.dampingRatio = _springDampingRatio;
        springJoint.frequency = springFrequency;
    }
    private void CreateMesh()
    {
        //_vertexCount = (_width + 1) * (_height + 1);

        int trianglesCount = _width * _height * 6;
        _vertices = new List<Vector3>();
        _triangles = new int[trianglesCount];
        _uv = new List<Vector2>();
        int t;

        for (int y = 0; y <= _height; y++)
        {
            for (int x = 0; x <= _width; x++)
            {
                int v = (_width + 1) * y + x;
                Vector3 PosVertex = new Vector3(x / (float)_width - 0.5f,
                        y / (float)_height - 0.5f, 0);
                _vertices.Add(PosVertex);
                _uv.Add(new Vector2(x / (float)_width, y / (float)_height));

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
        Mesh mesh = _meshMain.mesh;

        mesh.vertices = _vertices.ToArray();
        mesh.uv = _uv.ToArray();
        mesh.triangles = _triangles;
    }
    private void MapVerticesToReferencePoints()
    {
        _offsets = new Vector3[_vertices.Count, _referencePoints.Count];
        _weights = new float[_vertices.Count, _referencePoints.Count];

        for (int i = 0; i < _vertices.Count; i++)
        {
            float totalWeight = 0;

            for (int j = 0; j < _referencePoints.Count; j++)
            {
                _offsets[i, j] = _vertices[i] - LocalPosition(_referencePoints[j].transform);
                _weights[i, j] = 1 / Mathf.Pow(_offsets[i, j].magnitude, _mappingDetail);
                totalWeight += _weights[i, j];
            }

            for (int j = 0; j < _referencePoints.Count; j++)
            {
                _weights[i, j] /= totalWeight;
            }
        }
    }
    private void UpdateVertexPositions()
    {
        if (_isStart )
        {
            Vector3[] vertices = new Vector3[_vertices.Count];

            for (int i = 0; i < _vertices.Count; i++)
            {
                vertices[i] = Vector3.zero;

                for (int j = 0; j < _referencePoints.Count; j++)
                {
                    vertices[i] += _weights[i, j] *
                        (LocalPosition(_referencePoints[j].transform) + _offsets[i, j]);
                }
            }

            Mesh mesh = _meshMain.mesh;
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
    }
    private Vector3 LocalPosition(Transform obj)
    {
        return transform.InverseTransformPoint(obj.position);
    }
    public void StartMethod()
    {
        CreateReferencePoints();
        CreateMesh();
        MapVerticesToReferencePoints();
        _isStart = true;
    }
}
