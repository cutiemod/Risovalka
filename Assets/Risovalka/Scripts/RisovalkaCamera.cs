using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisovalkaCamera : MonoBehaviour
{
    public float moveDistance = 4.5f;
    public float moveSpeed = 5f;

    internal void MoveUp()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        WebGM GM = FindObjectOfType<WebGM>();

        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
        while (transform.position != desiredPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            GM.transform.position = (Vector2)transform.position;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
