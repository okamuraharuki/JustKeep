using System.Collections;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnObject : MonoBehaviour
{
    public EffectType _effectType = EffectType.Normal;
    Text _placableStateText;
    Image _reticle;
    Collider _collider;
    MeshRenderer _meshRen;
    SpawnManager _spawnManager;
    ScoreManager _scoreManager;
    public int _score;
    void Start()
    {
        _scoreManager = GameObject.FindWithTag("System").GetComponent<ScoreManager>();
        _spawnManager = GameObject.FindWithTag("System").GetComponent<SpawnManager>();
        _collider = GetComponent<Collider>();
        _meshRen = GetComponentInChildren<MeshRenderer>();
        if(this.tag == "Placing")
        {
            _collider.isTrigger = true;
        }
        _placableStateText = GameObject.FindWithTag("PlaceState").GetComponent<Text>();
        _reticle = GameObject.FindWithTag("Reticle").GetComponent<Image>();
    }

    void Update()
    {
        if (_spawnManager._isCreating)
        {
            if (this.tag == "Placing")
            {
                if (_spawnManager._isStacking)
                {
                    _placableStateText.text = "重複しています";
                    _reticle.color = Color.black;
                }
                else if (!_spawnManager._isPlacable)
                {
                    _placableStateText.text = "設置不可";
                    _reticle.color = Color.clear;
                }
                else
                {
                    _placableStateText.text = "設置可能";
                    _reticle.color = Color.yellow;
                }
            }
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if(this.tag == "Placing")
        {
            _spawnManager._isStacking = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if(this.tag == "Placing")
        {
            _spawnManager._isStacking = false;
        }
    }
    public void EffectMove(Vector3 treeUp)
    {
        switch (_effectType)
        {
            case EffectType.Normal:
                break;
            case EffectType.MassChange:
                break;
            case EffectType.Ice:
                break;
            case EffectType.Jump:
                StartCoroutine(IJump(treeUp));
                break;
        }
    }
    IEnumerator IJump(Vector3 treeUp)
    {
        yield return new WaitForSeconds(3);
        Debug.Log("jump");
        GetComponent<Rigidbody>().AddForce(treeUp * 3,ForceMode.VelocityChange);
    }
    private void OnDestroy()
    {
        if (_meshRen != null)
        {
            Destroy(_meshRen);
            _meshRen = null;
        }
        _scoreManager.DecreaseeScore(_score);
        Destroy(this.gameObject);
    }
}
