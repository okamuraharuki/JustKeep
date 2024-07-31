using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Tree : MonoBehaviour
{
    [SerializeField] Vector3 _massPosi = Vector3.zero;
    [SerializeField] float _outRad = 75;
    [SerializeField] float _reduceWaveIndex = 0.95f;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        ChangeMassCenter();
    }

    void Update()
    {
        MeasureRad();
        _rb.angularVelocity = new Vector3(_rb.angularVelocity.x * _reduceWaveIndex, _rb.angularVelocity.y * _reduceWaveIndex, _rb.angularVelocity.z * _reduceWaveIndex);
    }

    public void ChangeMassCenter()
    {
        _rb.centerOfMass = _massPosi;
        if (_rb.IsSleeping())
        {
            _rb.WakeUp();
        }
    }
    public void MeasureRad()
    {
        float nowRad = Vector3.Angle(Vector3.up, _rb.transform.up);
        if (nowRad >= _outRad)
        {//GameManager‚Åtree‚ª“|‚ê‚½‚Ìˆ—‚ğ‘‚­
            Debug.Log("“]“|");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + transform.rotation * _massPosi, new Vector3(0.3f, 0.3f, 0.3f));
    }
}
