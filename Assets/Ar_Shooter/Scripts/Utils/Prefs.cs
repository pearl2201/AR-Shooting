using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Prefs
{
	#region Default

	private static Prefs _instance;

	public static Prefs Instance {
		get {
			if (_instance == null) {
				_instance = new Prefs ();
			}
			return _instance;
		}
	}
	// must = -1 when release
	private static string KEY_VERSION_CODE = "key_version_code";
	private static int VERSION_KEYCODE = 0;

	public Prefs ()
	{
        
		if (PlayerPrefs.HasKey (KEY_VERSION_CODE)) {
			if (PlayerPrefs.GetInt (KEY_VERSION_CODE) != VERSION_KEYCODE) {
				Init ();
			} else {
				ReadData ();
			}
		} else {
			Init ();
		}
	}

	private int GetInt (string name)
	{
		if (PlayerPrefs.HasKey (name)) {
			return PlayerPrefs.GetInt (name);
		} else
			return 0;
	}

	private void SetInt (string name, int value)
	{
		PlayerPrefs.SetInt (name, value);
		PlayerPrefs.Save ();
	}

	private bool GetBool (string name)
	{
		if (PlayerPrefs.HasKey (name)) {
			if (PlayerPrefs.GetInt (name) == 1)
				return true;
			else
				return false;
		} else
			return false;
	}

	private void SetBool (string name, bool value)
	{
		if (value)
			PlayerPrefs.SetInt (name, 1);
		else
			PlayerPrefs.SetInt (name, 0);
		PlayerPrefs.Save ();
	}

	private void SetString (string name, string value)
	{
		PlayerPrefs.SetString (name, value);
		PlayerPrefs.Save ();
	}

	private string GetString (string name)
	{
		return PlayerPrefs.GetString (name);
	}

	private void SetFloat (string name, float value)
	{
		PlayerPrefs.SetFloat (name, value);
		PlayerPrefs.Save ();
	}

	private float GetFloat (string name)
	{
		if (PlayerPrefs.HasKey (name)) {
			return PlayerPrefs.GetFloat (name);
		} else
			return 0f;
	}

	#endregion default

	///// KEYCODE DERCLARE
	private static string KEY_VOLUME_MUSIC = "volume_music";
	private static string KEY_VOLUME_SOUNDFX = "volume_soundfx";
	private static string KEY_HIGHSCORE = "high_scores";
	private static string KEY_ADS = "ads";

	///// INIT

	void Init ()
	{

		SetInt (KEY_VERSION_CODE, VERSION_KEYCODE);
		SetVolumeMusic (1f);
		SetVolumeSoundFx (1);
	
	



    }

	///// READ DATA
	void ReadData ()
	{

	}



	public float GetVolumeSoundFx ()
	{
		return GetFloat (KEY_VOLUME_SOUNDFX);
	}

	public float GetVolumeMusic ()
	{
		return GetFloat (KEY_VOLUME_MUSIC);
	}

	public void SetVolumeSoundFx (float volume)
	{
		SetFloat (KEY_VOLUME_SOUNDFX, volume);
	}

	public void SetVolumeMusic (float volume)
	{
		SetFloat (KEY_VOLUME_MUSIC, volume);
	}

	public int HightScore{
		get{
			return GetInt (KEY_HIGHSCORE);
		}
		set{
			SetInt (KEY_HIGHSCORE, value);
		}
	}
}

