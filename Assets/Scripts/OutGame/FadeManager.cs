using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] Image fadePanel;

    [HideInInspector] public bool isFading = false;
    private bool isFadeInComp = false;

    private float fadeTime = 1f;
    private int flameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeInComp || isFading)
        {
            return;
        }

        if (flameCount > 2)
        {
            StartCoroutine(FadeIn());
        }

        ++flameCount;
    }

    public IEnumerator FadeIn()
    {
        isFading = true;
        fadePanel.DOFade(0, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        isFadeInComp = true;
        isFading = false;
        yield break;
    }

    public IEnumerator FadeAndChangeScene(float songOffset, int sceneNum)
    {
        //曲終了時のオフセット用
        yield return new WaitForSeconds(songOffset);

        isFading = true;
        fadePanel.DOFade(1, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        Debug.Log("シーン移動");
        SceneManager.LoadScene(sceneNum);
    }
}
