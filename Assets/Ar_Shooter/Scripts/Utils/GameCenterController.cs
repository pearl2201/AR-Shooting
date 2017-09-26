using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
//using GooglePlayGames;

public class GameCenterController : MonoBehaviour
{
	private static GameCenterController _instance;
	public static GameCenterController Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
		DontDestroyOnLoad(_instance.gameObject);

	}

	//#if UNITY_IPHONE
	private ILeaderboard m_Leaderboard = null;
	private long scorePlayer;
	private string leaderboardid = "com.catviet.ios.dinohunters.highscore";



	private bool canReportScore = true;

	void Start()
	{

		if (Application.platform == RuntimePlatform.IPhonePlayer)
			signInGameCenter();
	}

	public void ShowLeaderboard()
	{
		if (Social.localUser.authenticated)
		{
			m_Leaderboard = Social.CreateLeaderboard();
			Social.ShowLeaderboardUI();
		}
		else
		{
			Social.localUser.Authenticate(ProAuthToShowBoardGeneral);
		}
	}

	void ProAuthToShowBoardGeneral(bool success)
	{
		if (success)
		{
			m_Leaderboard = Social.CreateLeaderboard();
			Social.ShowLeaderboardUI();
		}
	}

	void ProAuthToSignIn(bool success)
	{
		if (success)
		{

		}
	}

	public bool CanReportScore
	{
		get
		{
			return canReportScore;
		}
		set
		{
			canReportScore = value;
		}
	}

	public void ReportScore(long score)
	{
		scorePlayer = score;
		CanReportScore = false;

		if (Social.localUser.authenticated)
		{
			m_Leaderboard = Social.CreateLeaderboard();
			m_Leaderboard.id = leaderboardid;
			ReportScore(score, leaderboardid);
		}
		else
		{
			Social.localUser.Authenticate(ProAuthToReportScore);
		}
	}

	void ProAuthToReportScore(bool success)
	{
		if (success)
		{
			m_Leaderboard = Social.CreateLeaderboard();
			m_Leaderboard.id = leaderboardid;
			ReportScore(scorePlayer, leaderboardid);
		}
	}

	void ReportScore(long score, string leaderboard)
	{
		Social.ReportScore(score, leaderboard, success =>
			{
				if (success)
				{
					CanReportScore = true;
				}
			});
	}

	public void signInGameCenter()
	{
		if (!Social.localUser.authenticated)
		{
			Social.localUser.Authenticate(ProAuthToSignIn);
		}
	}

	//#elif UNITY_ANDROID


	//#endif




}


