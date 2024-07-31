using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreJSon : MonoBehaviour
{
    [System.Serializable]
    public class ScoreData
    {
        public int _score;
        public string _name;
    }
}