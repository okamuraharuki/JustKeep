using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> _audioClip = new List<AudioClip>();

    AudioSource _audioSource;
    [SerializeField] float _delayAudioSec = 1.0f;
    [SerializeField] float _volumeSE = 0.1f;
    [SerializeField ]float _volumeBGM = 0.1f;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(IStart(_delayAudioSec));
    }
    public void PlayClip(int clipNum)
    {
        if(_audioClip[clipNum] != null)
        {
            _audioSource.PlayOneShot(_audioClip[clipNum], _volumeSE);
        }
    }
    public void ChangeSEVolume(float volume)
    {
        _volumeSE = volume;
    }
    public void ChangeBGMVolume(float volume)
    {
        _audioSource.volume = volume;
        _volumeBGM = volume;
    }
    public void ChangeAudio(int num)
    {
        _audioSource.clip = _audioClip[num];
        _audioSource.Play();
    }
    IEnumerator IStart(float delaySec)
    {
        _audioSource.Stop();
        _audioSource.volume = _volumeBGM;
        yield return new WaitForSeconds(_delayAudioSec);
        _audioSource.Play();
    }
}
