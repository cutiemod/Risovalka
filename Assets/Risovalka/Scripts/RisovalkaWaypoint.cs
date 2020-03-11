using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisovalkaWaypoint : MonoBehaviour
{
    public bool dropPrize = false;
    public bool teleport = false;
    public int move = 0;
    public float speedIncrement = 0;
    public float turnSpeedIncrement = 0;
    public Quaternion targetRotation;
    public Vector3 targetScale;
    public bool disable = false;
}
