using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GMPanels : MonoBehaviour
{
    public Text topPanelText;
    public GameObject panelTop;
    public GameObject panelBottom;

    private Vector3 defaultButtonScale;

    internal void EnablePanels(int count = 0, bool enableFlag = true)
    {
        switch (count)
        {
            case -1:
                panelBottom.SetActive(enableFlag);
                if (enableFlag)
                    panelBottom.GetComponent<Animator>().Play("normal");

                break;
            case 1:
                topPanelText.gameObject.SetActive(enableFlag);
                panelTop.SetActive(enableFlag);
                if (enableFlag)
                {
                    panelTop.GetComponent<Animator>().Play("normal");
                }
                break;
            default:
                topPanelText.gameObject.SetActive(enableFlag);
                panelBottom.SetActive(enableFlag);
                if (enableFlag)
                {
                    panelBottom.GetComponent<Animator>().Play("normal");
                }
                panelTop.SetActive(enableFlag);
                if (enableFlag)
                {
                    panelTop.GetComponent<Animator>().Play("normal");
                }
                break;
        }
    }

    internal void SetText(string text, int size = 0, float time = 0)
    {
        if (Mathf.Approximately(time, 0))
        {
            StopCoroutine(DisplayNewText(text, size, time));
            topPanelText.text = text;
            if (size != 0)
            {
                topPanelText.fontSize = size;
            }
        }
        else
        {
            //if (text == "" || (text != "" && text != topPanelText.text))
            //{
                StopCoroutine(DisplayNewText(text, size, time));
                StartCoroutine(DisplayNewText(text, size, time));
            //}
        }
    }

    private IEnumerator DisplayNewText(string text, int size, float time)
    {
        float alpha = topPanelText.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time * 4)
        {
            Color newColor = new Color(topPanelText.color.r, topPanelText.color.g, topPanelText.color.b, Mathf.Lerp(alpha, 0, t));
            topPanelText.color = newColor;
            yield return null;
        }

        topPanelText.text = text;
        if (size != 0)
        {
            topPanelText.fontSize = size;
        }

        alpha = topPanelText.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time * 1.5f)
        {
            Color newColor = new Color(topPanelText.color.r, topPanelText.color.g, topPanelText.color.b, Mathf.Lerp(alpha, 1, t));
            topPanelText.color = newColor;
            yield return null;
        }
        yield break;
    }

    internal void FadeOutPanels(int f)
    {
        switch (f)
        {
            case 1:
                panelTop.GetComponent<Animator>().SetTrigger("fadeOut");
                SetText("");
                break;
            case -1:
                panelBottom.GetComponent<Animator>().SetTrigger("fadeOut");
                break;
            default:
                panelTop.GetComponent<Animator>().SetTrigger("fadeOut");
                SetText("");
                panelBottom.GetComponent<Animator>().SetTrigger("fadeOut");
                break;
        }
    }

    internal void MoveTopText(float x, float y)
    {
        topPanelText.rectTransform.Translate(new Vector3 (x, y, 0));
    }
}
