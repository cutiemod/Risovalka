using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisovalkaHeli : RisovalkaObject
{
    protected override void CalculateMovement(RisovalkaWaypoint waypoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint.transform.position, currentSpeed * Time.deltaTime);

        if (transform.rotation != waypoint.targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, waypoint.targetRotation, currentSpeed / 5 * Time.deltaTime);
        }

        if (transform.localScale != waypoint.targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, waypoint.targetScale, currentSpeed / 30 * Time.deltaTime);
        }
    }

    protected override bool MovingToWaypoint(RisovalkaWaypoint waypoint)
    {
        return transform.position != waypoint.transform.position;
    }
}
