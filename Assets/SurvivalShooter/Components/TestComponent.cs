using EcsRx.Components;
using UniRx;
using System;
using UnityEngine;

namespace EcsRx.SurvivalShooter
{
	public class TestComponent : IComponent
	{
		public Vector2 Position { get; set; }
		public Color Color { get; set; }
//		public Bounds Bounds { get; set; }
	}
}
