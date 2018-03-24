using UnityEngine;
using System.Collections;

public static class Alias {

	public const int LEFT = -1;
	public const int RIGHT = 1;
	public const int STILL = 0;

	//USER LAYERS (!must match the layer numbers in Unity!)
	public const int LAYER_TILEMAP = 8;
	public const int LAYER_PC_SOLID = 9;
	public const int LAYER_ENEMIES = 10;
	public const int LAYER_DEADLY = 11;
	public const int LAYER_PC_PROJECTILES = 12;
	public const int LAYER_TRIGGERS = 13;
	public const int LAYER_ENEMY_PROJECTILES = 14;
	public const int LAYER_PC_TRIGGER = 15;
    public const int LAYER_BREAKABLE_SURFACE = 16;
    public const int LAYER_MOVING_PF = 25;

    //USEFUL LAYER MASKS (for collision)
    public const int LAYERMASK_TILEMAP = 1 << LAYER_TILEMAP;
    public const int LAYERMASK_BREAKABLE_SURFACE = 1 << LAYER_BREAKABLE_SURFACE;

    //public const float SOFT_SLOPE = 26.5651f; //tan(a) = opposite side length / adjacent side length --> tan(a) = 1 tile / 2 tiles --> a = arctan(1/2)
    //public const float HARD_SLOPE = 45f;

    //public const float SOFT_SLOPE_RAD = SOFT_SLOPE * Mathf.Deg2Rad;
    //public const float HARD_SLOPE_RAD = HARD_SLOPE * Mathf.Deg2Rad;

    //public static float SoftSlopeSin;
    //public static float HardSlopeSin;
    //public static float SoftSlopeCos;
    //public static float HardSlopeCos;


    public static void InitSomeAliases()
	{
		//SoftSlopeSin = Mathf.Sin(SOFT_SLOPE_RAD);
		//HardSlopeSin = Mathf.Sin(HARD_SLOPE_RAD);
		//SoftSlopeCos = Mathf.Cos(SOFT_SLOPE_RAD);
		//HardSlopeCos = Mathf.Cos(HARD_SLOPE_RAD);
	}
}
