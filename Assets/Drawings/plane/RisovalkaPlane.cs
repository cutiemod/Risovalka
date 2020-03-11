using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisovalkaPlane : RisovalkaObject
{
    public int forwardRotation = 180;
    public float distanceToWaypoint = 0.5f;

    protected override void CalculateMovement(RisovalkaWaypoint waypoint)
    {
        Vector3 relativeTarget = transform.InverseTransformPoint(waypoint.transform.position); //get relative target position

        float targetAngle = Mathf.Atan2(relativeTarget.y, relativeTarget.x); //get desired angle
        targetAngle *= Mathf.Rad2Deg; // convert to degrees
        targetAngle -= forwardRotation;

        if (targetAngle != transform.rotation.z) //only turn when needed
        {
            float turningThisFrame;
            if (Mathf.Abs(targetAngle - transform.rotation.z) > currentTurnSpeed * Time.deltaTime)
            {
                turningThisFrame = Mathf.Sign(targetAngle - transform.rotation.z) * currentTurnSpeed * Time.deltaTime;
            }
            else
            {
                turningThisFrame = targetAngle - transform.rotation.z;
            }

            transform.Rotate(0, 0, turningThisFrame);
        }

        Vector3 movementThisFrame = new Vector3(currentSpeed * Time.deltaTime, 0, 0);

        Quaternion rotateMovementVector = transform.rotation;
        rotateMovementVector *= Quaternion.Euler(0, 0, forwardRotation);

        movementThisFrame = rotateMovementVector * movementThisFrame;
        transform.position += movementThisFrame;
    }

    protected override bool MovingToWaypoint(RisovalkaWaypoint waypoint)
    {
        if(Mathf.Abs(transform.position.x - waypoint.transform.position.x) < distanceToWaypoint && Mathf.Abs(transform.position.y - waypoint.transform.position.y) < distanceToWaypoint)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
