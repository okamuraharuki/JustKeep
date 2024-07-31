using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] float _timeLimitSec = 60f;
    int _limitCountDownSec = 3;
    float _nowLimitSec;
    Text _timeText;
    GameObject _countDownTextObj;
    Text _countDownText;
    public bool _isPlaying = false;
    bool _isStarting = false;
    bool _isFinish = false;
    bool _isSendData = false;
    AudioManager _audioManager;
    ScoreManager _scoreManager;
    SceneChanger _sceneChanger;
    Image _reticle;
    void Start()
    {
        _reticle = GameObject.FindWithTag("Reticle").GetComponent<Image>();
        _reticle.enabled = false;
        Cursor.visible = false;
        _sceneChanger = GameObject.FindWithTag("System").GetComponent<SceneChanger>();
        _scoreManager = GameObject.FindWithTag("System").GetComponent<ScoreManager>();
        _audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        _nowLimitSec = _timeLimitSec;
        _timeText = GameObject.FindWithTag("Timer").GetComponent<Text>();
        _countDownTextObj = GameObject.FindWithTag("CountDown");
        _countDownText = GameObject.FindWithTag("CountDown").GetComponentInChildren<Text>();
        _timeText.text = $"Žc‚è{String.Format("{0:000}", _nowLimitSec.ToString("F1"))}•b";
    }
    void Update()
    {
        if(_scoreManager._scoreCalculated && !_isSendData)
        {
            StartCoroutine(IFinish());
        }
        if (!_isPlaying && !_isStarting  && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {//StartAnim
            _isStarting = true;
            _countDownTextObj.GetComponent<Animator>().SetBool("isStart", true);
            StartCoroutine(ICountDown());
        }
        if (_isPlaying)
        {//startGame
            _isStarting = false;
            _nowLimitSec -= Time.deltaTime;
            _timeText.text = $"Žc‚è{String.Format("{0:000}", _nowLimitSec.ToString("F1"))}•b";
            if(_nowLimitSec <= 0 && !_isFinish)
            {
                _isFinish = true;
                _countDownText.text = "Finish";
                _nowLimitSec = 0;
                _isPlaying = false;
                _audioManager.PlayClip(2);
                _countDownTextObj.GetComponent<Animator>().SetBool("isFinish", true);
                _scoreManager.SumScore();
            }
            if(_nowLimitSec < _limitCountDownSec)
            {
                _limitCountDownSec--;
                _audioManager.PlayClip(5);
            }
        }
    }
    IEnumerator IFinish()
    {
        _isSendData = true;
        yield return new WaitForSeconds(1);
        _scoreManager.SendData();
        _sceneChanger.ChangeScene(2);
    }
    IEnumerator ICountDown()
    {
        _countDownText.DOCounter(3, 1, 2f).SetEase(Ease.Linear).SetDelay(0.5f);
        yield return new WaitForSeconds(3);
        _countDownText.text = "GO!";
        _reticle.enabled = true;
        yield return new WaitForSeconds(1);
        _isPlaying = true;
    }
}
