using UnityEngine;
using System.Collections;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter
{
	public class FollowerComponent : ComponentBehaviour
	{
	    public Transform Target;
	    public float Smoothing = 5f;
	    public Vector3 Offset;
	}
}
