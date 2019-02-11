using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
//using UnityEditor;
using System;

public struct AIData
{
	public int index;
	public int rotation;
	public int rank;
	public bool bestFit;
	public bool perfectFit;
}
public class GameConstants
{
	
//	public static Dictionary<string, float> _payoutsDict = new Dictionary<string, float>();
//		/*{
//		{ "4No", 1.25 },
//		{ "JP_3No", 1.75 },
//		{ "JP_L_3No", 2 },
//		{ "JP_4No", 2.5 },
//		{ "5No", 3 },
//		{ "JP_L_4No", 4.5 },
//		{ "JP_5No", 5 },
//		{ "6No", 20 },
//		{ "JP_L_5No", 50 },
//		{ "JP_6No", 200 },
//		{ "HotSpot", 0.5 }
//	};*/


//	public static float[] payOuts = new float[]{1.25f,1.75f,2.5f,3.0f,4.5f,5.0f,100.0f,2.5f,4.0f,1000.0f};
//	public static string[] payOutsNames = {"Jackpot+3",
//		"4 Numbers",
//		"Jackpot+4",
//		"5 Numbers",
//		"Jackpot+5",
//		"6 Numbers",
//		"Jackpot+6",
//		"JP+Logo+3",
//		"JP+Logo+4",
//		"SLS JACKPOT: JP + Logo + 5 Numbers"};

	public static int TOWER_X = 3;
	public static int TOWER_Y = 3;
	public static int TOWER_Z = 10;

	public static int gamePlayCount = 0;
	public static string DIAMONDS = "DIA";
	public static string BOMBS = "BOMBS";

	public static string HIGHSCORE = "HIGH";
	public static string CURRENTSCORE = "CURRENT";
	public static string NOADS = "NOAD";

    public static string CURRENT_LEVEL = "CURRENT_LEVEL";
	public static string ALL_LEVELS_UNLOCKED = "ALL_LEVEL_BUY";

	public static int START_BOMB_COUNT = 1;
	public static int BASE_UNLOCKED_LEVELS = 4;

	public static string BUY_LEVELS_IAP = "com.tetrid.buyalllevels";
	public static string BUY_COINS_IAP = "com.tetrid.buycoins";

    public static string GOOGLE_API_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAzEGWzePrwhztSI/Mhsfsj5GBNbKeniGrgCJo/jLOasQh48scO0H9whADXa43/JkA7N3bRdAkPpNGl8hLDh1JbJc3mstPV3xxH1OtYaHlPIiKhU9TNjYS120FES9hplIAVKhcAAHxqcaFnsgKMRag43btsVULRW8Tui4GJNwAfkpHDMkhN5PQpJKpLtJ3winH3OLlF0FTYC2X+yq2d4AVIz0xkhdr1Nb1tE8h78AcfW+g4ZAXejUzT9LoJtwd5sPg8q5UbHzZrYMn+Cgbymlg6qUx+JE5mZYIAC7lLnU3qevs2VskH2fgYrur9JD/HHqUp32pIRKXaD/PRTncEGl4HwIDAQAB";

	public static int BUY_COINS_NUMBER = 300;
	public static string LEADERBOARD_ID_IOS = "com.tetrid.leaderboard";
    public static string LEADERBOARD_ID_ANDROID = "CgkIpMHhzpYTEAIQAA";
	public static string APPLE_ID = "1224182665";

	public static string SHARE_TEXT_BODY = "Amazing 3D Tetris Game";
	public static string SHARE_TEXT_HEAD = "TETRID";
//	public const float JP_ThreeNo = 1.25f;
//	public const float FourNo = 1.75f;
//	public const float JP_FourNo = 2.5f;
//	public const float FiveNo = 3.0f;
//	public const float JP_FiveNo = 4.5f;
//	public const float SixNo = 5.0f;
//	public const float JP_SixNo = 100.0f;
//	public const float JP_Logo_ThreeNo = 2.5f;
//	public const float JP_Logo_FourNo = 4.0f;
//	public const float SeqJP_FiveNo_Logo = 1000.0f;

//    public static void ClearLog()
//    {
//        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
//        Type type = assembly.GetType("UnityEditor.LogEntries");
//        MethodInfo method = type.GetMethod("Clear");
//        method.Invoke(new object(), null);
//    }

	public static int[,] rotateShape(int[,] arr)//,int angle)
	{
		int n = 3;
		int[,] ret = new int[n, n];
		Array.Clear(ret, 0, ret.Length);
		//int incre = angle/90;
		//if (angle == 90)
		{
			//for(int k = 0; k<incre ;k++)
			{
				//Array.Clear(ret, 0, ret.Length);

				for (int i = 0; i < n; ++i)
				{
					for (int j = 0; j < n; ++j)
					{
						ret[i, j] = arr[n - j - 1, i];

					}
				}

				//arr = ret;
			}
		}

		return ret;
	}
}
