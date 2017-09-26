using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Constants
{
    public static string PATH_DATA_SAVE = Application.persistentDataPath + "/dinosaur.xml";

    #region MORE-RATE
    public static string URL_MORE_GAME_ANDROID = "";
    public static string URL_MORE_GAME_IOS = "";
    public static string URL_RATE_GAME_ANDROID = "";
    public static string URL_RATE_GAME_IOS = "";
    #endregion
    public static float HALF_WIDTH_SCREEN = 12.8f / 2;
    public static float HALF_HEIGHT_SCREEN = 7.2f / 2;
    public static float WIDTH_SCREEN = 12.8f;
    public static float HEIGHT_SCREEN = 7.2f;

    public static int MAX_RADIUS_ANTEN = 7;
    public static int SCROLL_SPEED = 600;

    public static float GRAVITY = -9.81f;
    public static float H_GRAVITY = GRAVITY * 0.75f;

    public static int TOTAL_DINOSAUR = 4;
}


public class TAG
{
    public static string TAG_ENEMY = "Enemy";
}
