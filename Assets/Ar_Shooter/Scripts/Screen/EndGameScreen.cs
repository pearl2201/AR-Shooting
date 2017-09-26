using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{

	public Text txtCurrScore;
	public Text txtHighScore;
	public Text txtTotalHit;

	public void Show (int score, int hit, int totalHit)
	{
		if (score > Prefs.Instance.HightScore) {
			Prefs.Instance.HightScore = score;
		}
		txtCurrScore.text = "GAME SCORE: " + score;
		txtHighScore.text = Prefs.Instance.HightScore.ToString ();
		int shootRate = 0;
		if (totalHit > 0) {
			shootRate = (int)(hit * 100 / totalHit);
		}
		txtTotalHit.text = shootRate + "%"; 
		if (GameExtensions.COUNT_GAME % 2 == 0)
			GoogleMobileAdsControl.Instance.ShowInterstitial ();
	}

	public void Restart ()
	{
		Time.timeScale = 1f;
		Application.LoadLevel (Application.loadedLevel);
	}

	public void BackToMenu ()
	{
		Application.LoadLevel (0);
	}

	public void OpenRate ()
	{
		GameExtensions.OpenRateGame ();
	}
}
