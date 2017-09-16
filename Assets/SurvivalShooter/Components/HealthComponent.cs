using AlphaECS;
using AlphaECS.Unity;
using UniRx;
using System;
using UnityEngine;

namespace AlphaECS.SurvivalShooter
{
    public class HealthComponent : ComponentBase
	{
        public int StartingHealth;
        public IntReactiveProperty CurrentHealth = new IntReactiveProperty();
        public bool IsDamaged;
        //		public BoolReactiveProperty IsDead { get; set; }

        public GameObject aaa;

//		public HealthComponent()
//		{
//			//CurrentHealth = new IntReactiveProperty ();
//			//IsDead = new BoolReactiveProperty ();
		//}
	}
}
