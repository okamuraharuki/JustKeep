using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnumChangeWheel : MonoBehaviour
{
    [SerializeField] public string[] _explains = new string[Enum.GetValues(typeof(EffectType)).Length];
    [SerializeField] float _wheelSpeedIndex = 2;
    SpawnManager _spawnManager;
    float _enumIndex = 0;
    float _wheel = 0;
    Text _enumStateText;
    void Start()
    {
        _enumStateText = GameObject.FindWithTag("EnumState").GetComponent<Text>();
        _enumStateText.text = $"íuÇ≠ÉÇÉmÇÃèÛë‘\n{_explains[(int)_enumIndex]}";
        _spawnManager = GameObject.FindWithTag("System").GetComponent<SpawnManager>();
    }
    void Update()
    {
        if (!_spawnManager._isCreating)
        {
             _wheel = Input.GetAxis("Mouse ScrollWheel");
        }
        _enumIndex += _wheel * _wheelSpeedIndex;
        if(_enumIndex < 0)
        {
            _enumIndex = Enum.GetValues(typeof(EffectType)).Length - 0.1f;
        }
        if (_enumIndex >=  Enum.GetValues(typeof(EffectType)).Length)
        {
            _enumIndex = 0;
        }
        //Debug.Log(_enumIndex + " " + wh);
        if((int)_spawnManager._effectType != _enumIndex)
        {
            _spawnManager._effectType = (EffectType)_enumIndex;
            _enumStateText.text = $"íuÇ≠ÉÇÉmÇÃèÛë‘\n{_explains[(int)_enumIndex]}";
        }
    }
}
