using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIManager : MonoBehaviour
{
    static public string[] _explains = new string[Enum.GetValues(typeof(EffectType)).Length];
    static public int[] _effectObjCnt = new int[Enum.GetValues(typeof(EffectType)).Length];
    static public int _score = 0;
    Text _resultText;
    string _results = string.Empty;
    void Start()
    {
        Cursor.visible = true;
        _results = $"SCORE  {_score}";
        _resultText = GameObject.FindWithTag("ResultUIText").GetComponent<Text>();
        for(int i = 0; i < _effectObjCnt.Length; i++)
        {
            _results += $"\n{_explains[i]}  {_effectObjCnt[i]}";
        }
        _resultText.text = _results;
    }
}
