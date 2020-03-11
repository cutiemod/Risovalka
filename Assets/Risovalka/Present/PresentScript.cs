using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentScript : MonoBehaviour
{
    public float speed;
    public Animator chute;
    public float droppedY;

    private bool dropped = false;

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);

        if(transform.position.y < droppedY && !dropped)
        {
            Dropped();
        }

        if (transform.position.y < droppedY - 3)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        chute.SetTrigger("collapseChute");
    }

    private void Dropped()
    {
        dropped = true;
        FindObjectOfType<RisovalkaController>().FinishGame();
    }
}
