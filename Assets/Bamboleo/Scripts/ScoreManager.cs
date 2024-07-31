using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class ScoreManager : MonoBehaviour
{
    public int _nowScore = 0;
    static int _resultScoreStatic;
    [SerializeField] public int[] _scores = new int[Enum.GetValues(typeof(EffectType)).Length];
    [SerializeField] int[] _effectCnts = new int[Enum.GetValues(typeof(EffectType)).Length];
    int[] _effectCntsStatic;
    public bool _scoreCalculated = false;
    public void SumScore()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Placed"); 
        foreach(var ga in gameObjects)
        {
            SpawnObject so = ga.GetComponent<SpawnObject>();
            _nowScore += so._score;
            _effectCnts[(int)so._effectType]++;
            Debug.Log("aaaa" + _effectCnts[(int)so._effectType]) ;
        }
        Debug.Log("calculated");
        _effectCntsStatic = _effectCnts;
        _resultScoreStatic = _nowScore;
        _scoreCalculated = true;
    }
    public void SendData()
    {
        ResultUIManager._explains = GameObject.FindWithTag("System").GetComponent<EnumChangeWheel>()._explains;
        ResultUIManager._effectObjCnt = _effectCntsStatic;
        ResultUIManager._score = _resultScoreStatic;
    }

    public void DecreaseeScore(int scoreObj)
    {
        _nowScore -= scoreObj;
        Debug.Log(_nowScore);
    }
}
