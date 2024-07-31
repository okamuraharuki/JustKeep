using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] float _startChangeAddTime = 2.0f;
    [SerializeField] float _fadeTime = 1.0f;
    [SerializeField] Image _fadeImage;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(IFadeActiveFalse());
    }
    public void FadeScene(float value)
    {
        _fadeImage.enabled = true;
        _fadeImage.DOFade(value, _fadeTime);
    }
    public void ChangeScene(int sceneIndex)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            _fadeTime = _startChangeAddTime + 1;
        }
        else
        {
            _fadeTime = 1;
        }
        FadeScene(1);
        StartCoroutine(IChangeScene(sceneIndex));
    }
    public IEnumerator IFadeActiveFalse()
    {
        FadeScene(0);
        yield return new WaitForSeconds(_fadeTime);
        _fadeImage.enabled = false;
    }
    public IEnumerator IChangeScene(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return new WaitForSeconds(_fadeTime + _startChangeAddTime);
        }
        else
        {
            yield return new WaitForSeconds(_fadeTime);
        }
        SceneManager.LoadSceneAsync(sceneIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}