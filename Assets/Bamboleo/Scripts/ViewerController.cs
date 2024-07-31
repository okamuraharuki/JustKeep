using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ViewerController : MonoBehaviour
{
    [SerializeField] float _moveSpd = 5.0f;
    [SerializeField] float _viewSensi = 5.0f;
    [SerializeField] float _rayDistance = 50f;
    [SerializeField] PrimitiveType _type;
    Rigidbody _rb;
    Rigidbody _treeRb;
    public bool _isStacking = false;
    public bool _isPlacable = true;
    public bool _isCreating = false;
    GameObject _targetGameObject;
    RaycastHit _hit;
    Transform _parentSpawnObjTra;
    Material _targetMaterial;
    void Start()
    {
        _parentSpawnObjTra = GameObject.FindWithTag("ParentSpawnObject").GetComponent<Transform>();
        _treeRb = GameObject.FindWithTag("Tree").GetComponent<Rigidbody>();
        _rb = GetComponent<Rigidbody>();
        _targetMaterial = new Material(Shader.Find("Standard"));
        _targetMaterial.name = "Test";
        _targetMaterial.SetOverrideTag("RenderingMode", "Transplant");
        _targetMaterial.renderQueue = (int)RenderQueue.Transparent;
    }

    void Update()
    {
        Move();
        if(_isPlacable)
        {
            PlaceObject();
        }
    }
    private void FixedUpdate()
    {
        RayShot();
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
        else
        {
            _moveVec = Camera.main.transform.TransformDirection(_moveVec);
            _rb.transform.forward = _moveVec;
            Vector3 velo = _rb.transform.forward * _moveSpd;
            _rb.velocity = velo;
        }
    }
    void RayShot()
    {
        Vector3 cameraFront = Camera.main.transform.forward;

        if (Physics.Raycast(_rb.transform.position, cameraFront, out _hit, _rayDistance))
        {
            Debug.DrawLine(_rb.transform.position, _rb.transform.position + Camera.main.transform.forward * _rayDistance, Color.cyan);
            if (_hit.collider.tag == "Bottom" || _hit.collider.tag == "Middle" || _hit.collider.tag == "Top")
            {
                _isPlacable = true;
            }
            else
            {
                _isPlacable = false;
            }
            if(_isCreating)
            {
                if (_targetGameObject.tag == "Placing")
                {
                    _targetGameObject.transform.position = _hit.point + _treeRb.transform.up;
                }
            }
            
        }
        else 
        { 
            _isStacking = false;
            _isPlacable = false;
        }
    }
    void PlaceObject()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Down MouseLeft");
            CreateObject(_hit);
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Down MouseRight");
            _targetGameObject.AddComponent<Rigidbody>();
            _targetGameObject.AddComponent<Collider>().tag = "Placed";
            _targetGameObject = new GameObject();
            _isCreating = false;
        }
    }
    void CreateObject(RaycastHit hit)
    {
        if(!_isCreating)
        {
            _targetGameObject = GameObject.CreatePrimitive(_type);
            _targetGameObject.AddComponent<SpawnObject>();
            _targetGameObject.tag = "Placing";
            MeshRenderer tarMaterial = _targetGameObject.GetComponent<MeshRenderer>();
            tarMaterial.material = _targetMaterial;
            _targetGameObject = GameObject.FindWithTag("Placing");
            _isCreating = true;
        }
    }
    private void OnDrawGizmos()
    {
    }
}
