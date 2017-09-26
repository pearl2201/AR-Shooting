using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameExtensions
{

	public static string IOS_APPLICATION_ID = "id1289056846";
	public static string IOS_FREE_APPLICATION_ID = "id1190835708";
	public static string ADS_BANNER_ANDROID = "ca-app-pub-8843155078230599/4436344462";
	public static string ADS_BANNER_IOS = "ca-app-pub-8843155078230599/6890283266";
	public static string ADS_INS_ANDROID = "ca-app-pub-8843155078230599/1343277266";
	public static string ADS_INS_IOS = "ca-app-pub-8843155078230599/2041281262";
	public static int COUNT_GAME = 0;
	#if UNITY_ANDROID

	public static string FREE_VERSION_URL = "https://play.google.com/store/apps/details?id=com.vkids.android.puzzle";
	#elif UNITY_IOS
	public static string FREE_VERSION_URL = "https://itunes.apple.com/us/app/kid-puzzle-game-for-kids/" + IOS_FREE_APPLICATION_ID + "?ls=1&mt=8";

	#else
	public static string FREE_VERSION_URL = "https://play.google.com/store/apps/details?id=com.vkids.android.puzzle";
	#endif
	public static void OpenRateGame ()
	{
		PlayerPrefs.SetInt("FirstPlay", 1);

		#if UNITY_IOS

		Application.OpenURL("itms-apps://itunes.apple.com/app/" + IOS_APPLICATION_ID);
		#elif UNITY_ANDROID
		Application.OpenURL ("https://play.google.com/store/apps/details?id=" + Application.identifier);
		#else
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);

		#endif
		}

		public static void OpenFreeVersionURL ()
		{
		Application.OpenURL (FREE_VERSION_URL);
		}
		}

