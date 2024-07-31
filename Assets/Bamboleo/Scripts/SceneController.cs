using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    [SerializeField] Image _fadeImage;
    [SerializeField] float _fadeTime = 0.5f;
    /// <summary>
    /// シーンを切り替える時間（秒　float）
    /// </summary>
    [SerializeField] float _changeSpeed = 0.5f;
    private void Awake()
    {
        StartCoroutine(IFadeActiveFalse());
    }
    public void FadeScene(float value)
    {
        _fadeImage.enabled = true;
        _fadeImage.DOFade(value, _fadeTime);
    }
    public void ChangeScene(int sceneIndex)
    {
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
        yield return new WaitForSeconds(_fadeTime);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
