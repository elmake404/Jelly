using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobCircle : MonoBehaviour
{
    [SerializeField]
    private int _width = 5, _height = 5/*, _referencePointsCount = 12*/;
    [SerializeField]
    private float _sizeCollider, _step, _mappingDetail = 10, _springDampingRatio = 0, _springFrequency = 2;
    [SerializeField]
    private PhysicsMaterial2D _surfaceMaterial;
    [SerializeField]
    private MeshFilter _meshMain;
    [SerializeField]
    private Rigidbody2D _rbMain;

    private List<GameObject> _referencePoints;
    private List<Vector3> _vertices;
    private List<Vector2> _uv;
    private Vector3[,] _offsets;

    [SerializeField]
    private bool _isTop, _isBottom, _isRight, _isLeft;
    private int[] _triangles;
    private float[,] _weights;

    private void Awake()
    {
        CreateReferencePoints();
        CreateMesh();
        MapVerticesToReferencePoints();
    }
    private void Start()
    {
        Coupling(_referencePoints[0]);
        Coupling(_referencePoints[_referencePoints.Count-1]);
    }
    private void Update()
    {
        UpdateVertexPositions();
    }
    private void Coupling(GameObject game)
    {
        List<RaycastHit2D> hits2D = new List<RaycastHit2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();

        Physics2D.CircleCast(game.transform.position, _sizeCollider / 2, Vector2.zero, contactFilter2D, hits2D);
        GameObject contactPoint = null;
        float magnitude = float.PositiveInfinity;
        for (int i = 0; i < hits2D.Count; i++)
        {
            Transform refernsPointOther = hits2D[i].collider.gameObject.transform;
            if (refernsPointOther.gameObject.layer == 8 && refernsPointOther.parent != transform)
            {
                if (magnitude > (refernsPointOther.position - game.transform.position).magnitude)
                {
                    magnitude = (refernsPointOther.position - game.transform.position).magnitude;
                    contactPoint = refernsPointOther.gameObject;
                }
            }
        }
        AttachWithFixedJoint(game,
        contactPoint, _springFrequency * 100);

    }

    private void CreateReferencePoints()
    {
        if (_surfaceMaterial == null)
        {
            Debug.LogError("lack of physical material");
        }

        _referencePoints = new List<GameObject>();

        float positionColliderX = (transform.localScale.x / 2 - _sizeCollider / 2);
        float rotationZ = 0;

        if (_isTop)
        {
            rotationZ = 270;
        }
        else if (_isRight) 
        {
            rotationZ = 180;
        }
        else if (_isBottom) 
        {
            rotationZ = 90;
        }
        else if (_isLeft) 
        {
            rotationZ = 0;
        }


        int i = 0;

        while (true)
        {            
            _referencePoints.Add(new GameObject());
            _referencePoints[i].tag = gameObject.tag;

            _referencePoints[i].transform.eulerAngles = new Vector3(0,0,rotationZ);
            _referencePoints[i].transform.position = transform.position;
            _referencePoints[i].transform.Translate(Vector2.up*positionColliderX);
            _referencePoints[i].transform.parent = transform;
            _referencePoints[i].layer = 8;
            rotationZ += _step;

            AddComponents(_referencePoints[i]);

            AttachWithSpringJoint(_referencePoints[i], gameObject, _springFrequency);

            if (i > 0)
            {
                    AttachWithSpringJoint(_referencePoints[i],
                            _referencePoints[i - 1], _springFrequency * 2);
            }


            if (180/_step<=i)
            {
                break;
            }

            i++;
        }
    }
    private void AddComponents(GameObject referencePoints)
    {
        Rigidbody2D body = referencePoints.AddComponent<Rigidbody2D>();
        body.freezeRotation = true;
        body.interpolation = _rbMain.interpolation;
        body.collisionDetectionMode = _rbMain.collisionDetectionMode;

        CircleCollider2D collider = referencePoints.AddComponent<CircleCollider2D>();
        collider.radius = _sizeCollider / 2;
        collider.sharedMaterial = _surfaceMaterial;
    }
    private void AttachWithSpringJoint(GameObject referencePoint, GameObject connected, float springFrequency)
    {
        SpringJoint2D springJoint = referencePoint.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint) -
            LocalPosition(connected);

        springJoint.distance = 0;
        springJoint.dampingRatio = _springDampingRatio;
        springJoint.frequency = springFrequency;
    }
    private void AttachWithFixedJoint(GameObject referencePoint, GameObject connected, float springFrequency)
    {
        FixedJoint2D springJoint = referencePoint.AddComponent<FixedJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint) -
            LocalPosition(connected);
    }
    private void AttachWithSpringJointToTheFastener(GameObject referencePoint, float springFrequency)
    {
        bool topMount = transform.position.y < referencePoint.transform.position.y;
        bool rearMount = transform.position.x > referencePoint.transform.position.x;
        SpringJoint2D springJoint = referencePoint.AddComponent<SpringJoint2D>();

        Rigidbody2D rbConnected = Fasteners(rearMount, topMount);

        springJoint.connectedBody = rbConnected;
        springJoint.connectedAnchor = LocalPosition(referencePoint) -
            LocalPosition(rbConnected.gameObject);
        springJoint.distance = 0;
        springJoint.dampingRatio = _springDampingRatio;
        springJoint.frequency = springFrequency;
    }
    private Rigidbody2D Fasteners(bool rearMount, bool topMount)
    {
        int multiplierX = rearMount ? -1 : 1;
        int multiplierY = topMount ? 1 : -1;

        float positionColliderX = transform.position.x + ((transform.localScale.x / 2 - _sizeCollider / 2) * multiplierX);
        float positionColliderY = transform.position.y + ((transform.localScale.y / 2 - _sizeCollider / 2) * multiplierY);
        Vector2 positionCollider = new Vector2(positionColliderX, positionColliderY);

        GameObject fasteners = new GameObject();

        fasteners.transform.parent = transform;
        fasteners.transform.position =
            new Vector2(positionCollider.x + (_step * multiplierX), positionCollider.y);

        Rigidbody2D rbFasteners = fasteners.AddComponent<Rigidbody2D>();
        rbFasteners.freezeRotation = true;
        rbFasteners.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;

        return rbFasteners;
    }
    private void IgnoreCollisionsBetweenReferencePoints()
    {
        CircleCollider2D a;
        CircleCollider2D b;

        for (int i = 0; i < _referencePoints.Count; i++)
        {
            for (int j = i; j < _referencePoints.Count; j++)
            {
                a = _referencePoints[i].GetComponent<CircleCollider2D>();
                b = _referencePoints[j].GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(a, b, true);
            }
        }
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
                _offsets[i, j] = _vertices[i] - LocalPosition(_referencePoints[j]);
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
        Vector3[] vertices = new Vector3[_vertices.Count];

        for (int i = 0; i < _vertices.Count; i++)
        {
            vertices[i] = Vector3.zero;

            for (int j = 0; j < _referencePoints.Count; j++)
            {
                vertices[i] += _weights[i, j] *
                    (LocalPosition(_referencePoints[j]) + _offsets[i, j]);
            }
        }

        Mesh mesh = _meshMain.mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
    private Vector3 LocalPosition(GameObject obj)
    {
        return transform.InverseTransformPoint(obj.transform.position);
    }
}
