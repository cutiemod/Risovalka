using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    public GameObject[] buttons;
    public Color[] colors;

    private RisovalkaController risovalka;
    private int activeColor;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        activeColor = 0;
        buttons[activeColor].GetComponent<Animator>().SetBool("active", true);

        risovalka = transform.parent.GetComponent<RisovalkaController>();
        risovalka.ChangeCurrentColor(colors[activeColor]);
    }

    void Update()
    {
        // Сделано через апдейт а не через систему ивентов и обычные кнопки в виду особенностей приложения, откуда эта мини-игра была вытащена

        if (Input.anyKeyDown)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit && hit.collider != null && System.Array.IndexOf(buttons, hit.collider.gameObject) >= 0 && buttons[activeColor] != hit.collider.gameObject)
            {
                buttons[activeColor].GetComponent<Animator>().SetBool("active", false);
                activeColor = System.Array.IndexOf(buttons, hit.collider.gameObject);
                buttons[activeColor].GetComponent<Animator>().SetBool("active", true);
                risovalka.ChangeCurrentColor(colors[activeColor]);
            }
        }
    }

    List<GameObject> GetChildrenWithTag(Transform gobject, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        foreach (Transform child in gobject.transform)
        {
            if (child.CompareTag(tag))
            {
                taggedGameObjects.Add(child.gameObject);
            }
            //recursive
            List<GameObject> taggedObjectsRecursive = new List<GameObject>();
            taggedObjectsRecursive = GetChildrenWithTag(child, tag);
            foreach (GameObject go in taggedObjectsRecursive)
            {
                taggedGameObjects.Add(go);
            }
        }

        return taggedGameObjects;
    }
}
