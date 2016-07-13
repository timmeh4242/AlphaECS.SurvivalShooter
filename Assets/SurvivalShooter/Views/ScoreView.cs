using UnityEngine;
using System.Collections;
using Zenject;
using EcsRx.SurvivalShooter;
using UnityEngine.UI;
using UniRx;

public class ScoreView : MonoBehaviour {

	private ScoringSystem ScoringSystem { get; set; }

	Text ScoreText;

	[Inject]
	public void Initialize(ScoringSystem scoringSystem)
	{
		ScoreText = GetComponent<Text> ();
		ScoringSystem = scoringSystem;
		ScoringSystem.Score.DistinctUntilChanged ().Subscribe (value =>
		{
			ScoreText.text = "Score: " + value.ToString();			
		}).AddTo (this);
	}
}
