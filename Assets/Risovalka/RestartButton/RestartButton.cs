using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public Collider2D button;

    private void OnEnable()
    {
        GetComponentInChildren<Canvas>().worldCamera = FindObjectOfType<Camera>();
        GetComponentInChildren<Canvas>().sortingLayerName = "Default";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit && hit.collider != null)
            {
                if (hit.collider == button)
                {
                    Click();
                }
            }
        }
    }

    public void Click()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
