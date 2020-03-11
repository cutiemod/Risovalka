using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisovalkaController : MonoBehaviour
{
    public string gmName = "GameMaster";

    public string[] topPanelText;
    public int[] topPanelTextSize;

    public Material lineMaterial;
    public Material maskMaterial;

    public Vector3 drawPoint;
    public float objectMoveSpeed;

    public float buttonPressCoolDown;

    public GameObject acceptButton;
    public GameObject denyButton;
    public GameObject palette;
    public GameObject presentBox;

    public GameObject restartButton;

    public GameObject background;
    public GameObject drawBackground;

    public GameObject[] objects;

    [SerializeField] private GameObject line;
    private Vector2 mousePosition;

    private WebGM GM;

    private RisovalkaObject currentObject;
    private Color currentColor;
    private int currentZ;

    private bool objectSelected;

    private void Awake()
    {
        GM = GameObject.Find(gmName).GetComponent<WebGM>();
        GM.EnablePanels();
    }

    private void Start()
    {
        objectSelected = false;

        currentObject = objects[0].GetComponent<RisovalkaObject>();
        currentColor = Color.red;
        currentZ = 0;

        GM.SetText(topPanelText[0], topPanelTextSize[0], 0);
    }

    private void Update()
    {
        if (objectSelected) //
        {
            Draw();
        }
        else
        {
            SelectObject();
        }
    }

    private void SelectObject()
    {
        if (Input.GetMouseButtonDown(0)) //
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit && hit.collider != null)
            {
                if (System.Array.IndexOf(objects, hit.collider.gameObject) > -1)
                {
                    foreach(GameObject o in objects)
                    {
                        o.GetComponent<Collider2D>().enabled = false;
                        if (o == hit.collider.gameObject)
                        {
                            currentObject = o.GetComponent<RisovalkaObject>();
                        }
                        else
                        {
                            o.GetComponent<RisovalkaObject>().FadeOut(true);
                        }
                    }
                    SetUpForDrawing();
                }
            }
        }
    }

    private void SetUpForDrawing()
    {
        currentObject.SetUp(drawPoint, objectMoveSpeed);

        objectSelected = true;

        background.GetComponent<Animator>().SetTrigger("change");

        drawBackground = Instantiate(drawBackground);
        drawBackground.transform.parent = transform;

        acceptButton = Instantiate(acceptButton);
        acceptButton.transform.parent = transform;

        denyButton = Instantiate(denyButton);
        denyButton.transform.parent = transform;

        palette = Instantiate(palette);
        palette.transform.parent = transform;

        presentBox = Instantiate(presentBox);
        presentBox.transform.parent = transform;

        GM.SetText(topPanelText[1], topPanelTextSize[1]);
    }

    internal void EnableBrush(bool flag = true)
    {
        GetComponent<Collider2D>().enabled = flag;
    }

    private void Draw()
    {
        if (Input.GetMouseButtonDown(0)) //
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit && hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    LineRenderer LR = Instantiate(line, mousePosition, Quaternion.Euler(0.0f, 0.0f, 0.0f)).GetComponent<LineRenderer>();

                    currentObject.AddLine(LR.gameObject);

                    lineMaterial.color = currentColor;
                    LR.GetComponent<LineDrawer>().mat = lineMaterial;
                    LR.GetComponent<LineDrawer>().partNum = currentObject.current + 1;
                    LR.transform.name = "line";
                    currentZ -= 1;
                    LR.transform.position = new Vector3(LR.transform.position.x, LR.transform.position.y, currentZ / 1000);
                    LR.sortingOrder = -currentZ;
                    LR.transform.parent = currentObject.parts[currentObject.current].transform;
                }
                else if (hit.collider.gameObject == acceptButton)
                {
                    NextPart();
                    acceptButton.GetComponent<Animator>().SetTrigger("pressed");
                }
                else if (hit.collider.gameObject == denyButton)
                {
                    DeleteLastLine();
                    denyButton.GetComponent<Animator>().SetTrigger("pressed");
                }

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

    public void ChangeCurrentColor(Color c)
    {
        currentColor = c;
    }

    private void NextPart()
    {
        if (currentObject.NextPart())
        {       
            Win();
        }
        else
        {
            lineMaterial.SetInt("_StencilNum", -currentZ);
        }
    }

    private void DeleteLastLine()
    {
        currentObject.DeleteLastLine();
    }

    private void Win()
    {
        currentObject.Win();
        acceptButton.GetComponent<Animator>().SetTrigger("disable");
        denyButton.GetComponent<Animator>().SetTrigger("disable");
        FindObjectOfType<RisovalkaCamera>().MoveUp();
        GetComponent<Collider2D>().enabled = false;
        GM.FadeOutPanels();
        //GM.GivePrize();
    }

    internal void OpenBox()
    {
        presentBox.GetComponentInChildren<Animator>().SetTrigger("open");
    }

    internal void MoveBackgroundSprites(int move)
    {
        int i = 0;
        foreach(Transform c in drawBackground.transform)
        {
            if (i > 0)
            {
                c.position = new Vector3(c.position.x, c.position.y, c.position.z + move);
            }
            i++;
        }
    }

    internal void FinishGame()
    {
        restartButton = Instantiate(restartButton);
        restartButton.transform.parent = transform;
    }
}
