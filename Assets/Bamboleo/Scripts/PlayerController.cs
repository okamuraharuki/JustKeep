using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpd = 5.0f;
    [SerializeField] float _viewSensi = 5.0f;
    [SerializeField] float _rayDistance = 50f;
    [SerializeField] bool _shiftPushParallelMove = false;
    Rigidbody _rb;
    public bool _isStacking = false;
    public bool _isPlacable = true;
    public bool _isCreating = false;
    RaycastHit _hit;
    Collider _limitCollider;
    static bool _shiftPushParallelMoveStatic = false;
    SpawnManager _spawnManager;
    PlayingManager _playingManager;
    void Start()
    {
        _playingManager = GameObject.FindWithTag("Playing").GetComponent<PlayingManager>();
        _spawnManager = GameObject.FindWithTag("System").GetComponent<SpawnManager>();
        _rb = GetComponent<Rigidbody>();
        _limitCollider = GameObject.FindWithTag("Limit").GetComponent<Collider>();
    }

    void Update()
    {
        if(_playingManager._isPlaying)
        {
            RayShot();
            if (_isPlacable)
            {
                if (_spawnManager._isCreating)
                {
                    _spawnManager.ObjMove(_hit);
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        Debug.Log("Down MouseRight");
                        if (_spawnManager._isStacking)
                        {
                            Debug.Log("_isStacking is true");
                        }
                        else
                        {
                            _spawnManager.PlaceObj(_hit);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        Debug.Log("Down MouseLeft");
                        _spawnManager.CreateObj(_hit);
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if(_shiftPushParallelMove != _shiftPushParallelMoveStatic)
        {
            _shiftPushParallelMoveStatic = _shiftPushParallelMove;
        }
        Move();
    }
    void Move()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        Vector3 _moveVec = Vector3.right * hori + Vector3.forward * vert;
        if (_moveVec == Vector3.zero)
        {
            _rb.velocity = Vector3.zero;
        }
        else if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            _moveVec = Camera.main.transform.TransformDirection(_moveVec);
            if (_shiftPushParallelMoveStatic)
            {
                _moveVec = Vector3.ProjectOnPlane(_moveVec, Vector3.up).normalized;
            }
            _rb.transform.forward = _moveVec;
            Vector3 velo = _rb.transform.forward * _moveSpd;
            _rb.velocity = velo;
        }
        else
        {
            _moveVec = Camera.main.transform.TransformDirection(_moveVec);
            if (!_shiftPushParallelMoveStatic)
            {
                _moveVec = Vector3.ProjectOnPlane(_moveVec, _rb.transform.forward);
            }
            _rb.transform.forward = _moveVec;
            Vector3 velo = _rb.transform.forward * _moveSpd;
            _rb.velocity = velo;
        }
        Vector3 limitScale = _limitCollider.transform.localScale;
        Vector3 limitPos = _limitCollider.transform.position;
        Vector3 clamped = new Vector3(Mathf.Clamp(transform.position.x, limitPos.x - limitScale.x / 2, limitPos.x + limitScale.x / 2),
            Mathf.Clamp(transform.position.y, limitPos.y - limitScale.y / 2, limitPos.y + limitScale.y / 2),
            Mathf.Clamp(transform.position.z, limitPos.z - limitScale.z / 2, limitPos.z + limitScale.z / 2));
        transform.position = clamped;
    }
    void RayShot()
    {
        Vector3 cameraFront = Camera.main.transform.forward;
        Debug.DrawLine(_rb.transform.position, _rb.transform.position + Camera.main.transform.forward * _rayDistance, Color.cyan);
        if (Physics.Raycast(_rb.transform.position, cameraFront,out _hit, _rayDistance, 3))
        {
            _isPlacable = true;
        }
        else
        {
            _isStacking = false;
            _isPlacable = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_hit.point, new Vector3(1, 1, 1));
    }
}
