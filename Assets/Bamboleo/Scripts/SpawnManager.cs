using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Vector2 _massPrefabLange = new Vector2(1f,6f);
    [SerializeField] Vector3 _minPrefabScale = new Vector3(0.8f, 0.8f, 0.8f);
    [SerializeField] Vector3 _maxPrefabScale = new Vector3(2.5f, 2.5f, 2.5f);
    [SerializeField] float _flowObjUpIndex = 0.3f;
    GameObject _prefabPrim;
    public bool _isStacking = false;
    public bool _isPlacable = true;
    public bool _isCreating = false;
    GameObject _newClone;
    public EffectType _effectType = EffectType.Normal;
    PhysicMaterial _normalPhysics;
    PhysicMaterial _icePhysics;
    Material _normalMaterial;
    Material _iceMaterial;
    [SerializeField] float _massChanged = 3;
    [SerializeField] int _newCloneScore = 100;
    Transform _treeTra;
    ScoreManager _scoreManager;
    AudioManager _audioManager;
    void Start()
    {
        _treeTra = GameObject.FindWithTag("Tree").GetComponent<Transform>();
        _prefabPrim = (GameObject)Resources.Load("testObj");
        if(_prefabPrim != null)
        {
            Debug.Log(_prefabPrim.name);
        }
        _normalPhysics = (PhysicMaterial)Resources.Load("normalPhy");
        _normalMaterial = (Material)Resources.Load("a");
        _icePhysics = (PhysicMaterial)Resources.Load("Ice");
        _iceMaterial = (Material)Resources.Load("Icem0 1");
        Debug.Log(_normalMaterial == null ? "normalMaterial is null" : _normalMaterial.name);
        Debug.Log(_normalPhysics == null ? "normalPhysics is null" : _normalPhysics.name);
        Debug.Log(_iceMaterial == null ? "iceMaterial is null" : _iceMaterial.name);
        Debug.Log(_icePhysics == null ? "icePhysics is null" : _icePhysics.name);
        _scoreManager = this.GetComponent<ScoreManager>();
        _audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
    }
    public void CreateObj(RaycastHit hit)
    {
        UnityEngine.Random.InitState(Time.frameCount);
        float prefabMassNow = UnityEngine.Random.Range(_massPrefabLange.x, _massPrefabLange.y);
        float prefabScaleNowX = UnityEngine.Random.Range(_minPrefabScale.x, _maxPrefabScale.x);
        float prefabScaleNowY = UnityEngine.Random.Range(_minPrefabScale.y, _maxPrefabScale.y);
        float prefabScaleNowZ = UnityEngine.Random.Range(_minPrefabScale.z, _maxPrefabScale.z);
        Vector3 prefabScaleNow = new Vector3(prefabScaleNowX, prefabScaleNowY, prefabScaleNowZ);
        Rigidbody prefabRb = _prefabPrim.GetComponent<Rigidbody>();
        Color prefabColorSet;
        if(prefabMassNow >= 5f * 6f / 7f)
        {
            prefabColorSet = Color.red;
        }
        else if (prefabMassNow >= 5f * 5f / 7f)
        {//orange
            prefabColorSet = new Color(255, 165, 0);
        }
        else if (prefabMassNow >= 5f * 4f / 7f)
        {//yellow
            prefabColorSet = Color.yellow;
        }
        else if (prefabMassNow >= 5f * 3f / 7f)
        {//lightgreen
            prefabColorSet = new Color(0, 255, 51);
        }
        else if (prefabMassNow >= 5f * 2f / 7f)
        {//skyblue
            prefabColorSet = new Color(0, 203, 255);
        }
        else if (prefabMassNow >= 5f/ 7f)
        {
            prefabColorSet = Color.blue;
        }
        else
        {
            prefabColorSet= new Color(255, 0, 255);
        }
        prefabRb.useGravity = false;
        prefabRb.mass = prefabMassNow;
        prefabRb.transform.localScale = prefabScaleNow;
        _prefabPrim.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        _prefabPrim.tag = "Placing";
        _audioManager.PlayClip(0);
        _newClone = Instantiate(_prefabPrim, hit.point + _treeTra.up * (_flowObjUpIndex + prefabRb.transform.localScale.y / 2), Quaternion.Euler(Camera.main.transform.forward));
        Material[] newCloneMaterials = _newClone.GetComponent<MeshRenderer>().materials;
        BoxCollider newCloneBoxCol = _newClone.GetComponent<BoxCollider>();
        switch (_effectType)
        {
            case EffectType.Normal:
                newCloneBoxCol.material = _normalPhysics;
                break;
            case EffectType.MassChange:
                _newClone.GetComponent<Rigidbody>().mass += _massChanged;
                break;
            case EffectType.Ice:
                //メモ　iceMaterialにiceShaderを加える
                //学び　materialsは配列ごと入れ替える必要あり
                //newCloneRenerer.materials[1] = _iceMaterial;
                newCloneMaterials[1] = _iceMaterial;
                _newClone.GetComponent<MeshRenderer>().materials = newCloneMaterials;
                newCloneBoxCol.material = _icePhysics;
                break;
            case EffectType.Jump:
                newCloneBoxCol.material = _normalPhysics;
                break;
        }
        SpawnObject newCloneScript = _newClone.GetComponent<SpawnObject>();
        newCloneScript._score = _scoreManager._scores[(int)_effectType];
        newCloneScript._effectType = _effectType;
        _newClone.GetComponent<MeshRenderer>().material.color = prefabColorSet;
        _newClone.layer = 3;
        _isCreating = true;
    }
    public void ObjMove(RaycastHit hit)
    {
        _newClone.transform.position = hit.point + _treeTra.up * (_flowObjUpIndex + _prefabPrim.transform.localScale.y / 2);
        _newClone.transform.rotation = _treeTra.rotation;
    }
    public void PlaceObj(RaycastHit hit)
    {
        _newClone.GetComponent<Rigidbody>().useGravity = true;
        _newClone.tag = "Placed";
        _newClone.layer = 0;
        _newClone.GetComponent<Collider>().isTrigger = false;
        _newClone.GetComponent<SpawnObject>().EffectMove(_treeTra.up);
        _audioManager.PlayClip(1);
        _isCreating = false;
    }
}
