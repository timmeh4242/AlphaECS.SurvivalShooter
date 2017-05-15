using UnityEngine;
using System.Collections;
using Zenject;
using AlphaECS.SurvivalShooter;
using AlphaECS.Unity;
using UnityEngine.UI;
using UniRx;
using AlphaECS;

public class ScoreHUD : ComponentBehaviour
{
	[Inject] private ScoringSystem ScoringSystem { get; set; }

	Text ScoreText;

	public override void Setup (IEventSystem eventSystem)
	{
		base.Setup (eventSystem);

		ScoreText = GetComponent<Text> ();
		ScoringSystem.Score.DistinctUntilChanged ().Subscribe (value =>
		{
			ScoreText.text = "Score: " + value.ToString();			
		}).AddTo (this);
	}
}
