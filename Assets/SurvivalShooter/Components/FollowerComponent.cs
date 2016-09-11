using UnityEngine;
using System.Collections;
using EcsRx.Unity;

public class FollowerComponent : ComponentBehaviour
{
    public Transform Target;
    public float Smoothing = 5f;
    public Vector3 Offset;
}
