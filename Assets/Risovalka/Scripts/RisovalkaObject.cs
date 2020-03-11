using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisovalkaObject : MonoBehaviour
{
    public GameObject prize;
    public GameObject waypointContainer;

    public float defaultSpeed;
    public float defaultTurnSpeed;

    internal int current;

    internal GameObject[] parts;
    protected List<GameObject> lines;
    protected List<GameObject> waypoints;

    protected bool remove;

    protected int waypointsTraversed;

    protected float currentSpeed;
    protected float currentTurnSpeed;
    protected float currentRotation;

    void Start()
    {
        remove = false;
        lines = new List<GameObject>();
        parts = GetChildrenWithTag(transform, "QuizAnswer").ToArray();
        int c = 0;

        foreach (GameObject p in parts)
        {
            c++;

            p.GetComponent<SpriteRenderer>().enabled = false;
            SpriteRenderer maskSR = GetChildrenWithTag(p.transform, "QuizExplanation").ToArray()[0].GetComponent<SpriteRenderer>();
            Material newMat = new Material(maskSR.material.shader); 
            newMat.SetInt("_StencilNum", c);
            newMat.renderQueue += c - 50;
            maskSR.material = newMat;

            maskSR.enabled = false;

            p.SetActive(false);
        }

        current = -1;
    }

    internal void SetUp(Vector3 point, float speed)
    {
        StartCoroutine(MoveInPosition(new Vector3(point.x, point.y, transform.position.z), speed));
    }

    protected IEnumerator MoveInPosition(Vector3 point, float speed)
    {
        while (transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        FadeOut(false);

        yield break;
    }

    internal void FadeOut(bool flag)
    {
        remove = flag;
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetFloat("speed", flag ? 2f : 1f);
        GetComponent<Animator>().SetTrigger("fadeOut");

        if (!remove)
        {
            foreach (GameObject p in parts)
            {
                p.SetActive(true);
            }
        }
    }

    protected void OnFadeComplete()
    {
        if (remove)
        {
            gameObject.SetActive(false);
        }
        else
        {
            FindObjectOfType<RisovalkaController>().EnableBrush(true);
            NextPart();
        }
    }

    internal bool NextPart()
    {
        lines.Clear();

        if(current >= 0)
        {
            parts[current].GetComponent<SpriteRenderer>().enabled = false;
        }

        current += 1;

        if (current < parts.Length)
        {
            StartCoroutine(FlashNewPart(parts[current]));
        }
        else
        {
            StopAllCoroutines();
            FinishDrawing();
            return true;
        }
        return false;
    }

    protected IEnumerator FlashNewPart(GameObject p)
    {
        FindObjectOfType<RisovalkaController>().EnableBrush(false);
        p.GetComponent<SpriteRenderer>().enabled = true;
        p.GetComponent<Animator>().SetTrigger("flash");
        GetChildrenWithTag(p.transform, "QuizExplanation").ToArray()[0].GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(1);
        FindObjectOfType<RisovalkaController>().EnableBrush(true);
        yield break;
    }

    protected void FinishDrawing()
    {
        FindObjectOfType<WebGM>().FadeOutPanels();

        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            if (a.GetParameter(0).name == "win")
            {
                a.SetTrigger("win");
            }
        }
    }

    internal void AddLine(GameObject l)
    {
        lines.Add(l);
    }

    internal void DeleteLastLine()
    {
        if (lines.Count > 0)
        {
            GameObject.Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
        }
        else
        {
            Debug.Log("Nothing to delete");
        }
    }

    protected List<GameObject> GetChildrenWithTag(Transform gobject, string tag)
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

    internal void Win()
    {
        waypoints = new List<GameObject>();

        waypointContainer = Instantiate(waypointContainer, transform.parent);

        foreach(Transform c in waypointContainer.transform)
        {
            waypoints.Add(c.gameObject);
        }

        GameObject nextWaypoint = waypoints[0];

        waypointsTraversed = 0;
        currentSpeed = defaultSpeed;
        currentTurnSpeed = defaultTurnSpeed;

        StartCoroutine(GoToWaypoint(nextWaypoint));
    }

    protected IEnumerator GoToWaypoint(GameObject waypointGO)
    {
        RisovalkaWaypoint waypoint = waypointGO.GetComponent<RisovalkaWaypoint>();

        currentSpeed += waypoint.speedIncrement;
        currentTurnSpeed += waypoint.turnSpeedIncrement;
        currentRotation = 0;

        if (waypoint.teleport)
        {
            transform.position = waypointGO.transform.position;
            transform.rotation = waypoint.targetRotation;
            transform.localScale = waypoint.targetScale;

            ApplyChanges(waypoint);

            yield break;
        }
        else
        {
            while (MovingToWaypoint(waypoint))
            {
                CalculateMovement(waypoint);

                yield return new WaitForEndOfFrame();
            }

            ApplyChanges(waypoint);
        }
        yield break;
    }

    protected virtual bool MovingToWaypoint(RisovalkaWaypoint waypoint)
    {
        return transform.position != waypoint.transform.position;
    }

    protected virtual void CalculateMovement(RisovalkaWaypoint waypoint)
    {

    }

    protected void ApplyChanges(RisovalkaWaypoint waypoint)
    {
        if (waypoint.move != 0)
        {
            FindObjectOfType<RisovalkaController>().MoveBackgroundSprites(waypoint.move);
        }

        if (waypoint.dropPrize)
        {
            prize = Instantiate(prize);
            prize.transform.position = waypoint.transform.position;

            FindObjectOfType<RisovalkaController>().OpenBox();
        }

        if (GetNextWaypoint(waypoint.gameObject) != null)
        {
            GoToNextWaypoint(GetNextWaypoint(waypoint.gameObject));
        }
        else
        {
            DisableThis();
        }
    }

    protected GameObject GetNextWaypoint(GameObject currentWaypoint)
    {
        if (waypoints.IndexOf(currentWaypoint) + 2 > waypoints.Count)
        {
            return null;
        }
        currentWaypoint.SetActive(false);
        return waypoints[waypoints.IndexOf(currentWaypoint) + 1];
    }

    protected void GoToNextWaypoint(GameObject nextWaypoint)
    {
        waypointsTraversed++;
        if (waypointsTraversed > 99)
        {
            Debug.Log("Too many waypoints, must be a mistake");
        }
        else
        {
            StartCoroutine(GoToWaypoint(nextWaypoint));
        }
    }

    protected void DisableThis()
    {
        StopAllCoroutines();
        //gameObject.SetActive(false);
        this.enabled = false;
    }
}
