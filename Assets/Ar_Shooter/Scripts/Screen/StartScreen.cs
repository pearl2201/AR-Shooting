using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Dinosaur.Scripts;
public class StartScreen : MonoBehaviour {

	public GameObject deactiveSoundState;
	public GameObject activeSoundState;

	void Start()
	{
		SetSoundIcon ();
	}

	public void StartGame()
	{
		Application.LoadLevel (1);

	}

	public void ClickRate()
	{
		GameExtensions.OpenRateGame ();
	}

	public void ClickSound()
	{
		if (Prefs.Instance.GetVolumeMusic () == 0)
		{
			MusicManager.Instance.volume = 1;
			Prefs.Instance.SetVolumeMusic (1);
			Prefs.Instance.SetVolumeSoundFx (1);
			SoundManager.Instance.SetVolume (1);

		}else{
			MusicManager.Instance.volume = 0;
			Prefs.Instance.SetVolumeMusic (0);
			Prefs.Instance.SetVolumeSoundFx (0);
			SoundManager.Instance.SetVolume (0);
		}
		EventDispatcher.Instance.PostEvent (EventID.OnMusicChange);
		EventDispatcher.Instance.PostEvent (EventID.OnSoundChange);
		SetSoundIcon ();
	}

	public void SetSoundIcon()
	{
		if (Prefs.Instance.GetVolumeMusic () == 0)
		{
			deactiveSoundState.gameObject.SetActive (true);
			activeSoundState.gameObject.SetActive (false);
		}else
		{
			deactiveSoundState.gameObject.SetActive (false);
			activeSoundState.gameObject.SetActive (true);
		}
	}

	public void OpenLeaderBoard()
	{
		GameCenterController.Instance.ShowLeaderboard ();
	}
}
