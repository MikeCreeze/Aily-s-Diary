using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicCubeGround
{
    public float CubeType;
    public float AppearTime;
    public int AppearPlace;
    
    public MusicCubeGround()
    {
        AppearTime = 0;
        AppearPlace = 0;
    }
    public MusicCubeGround(float CT, float AT, int AP)
    {
        CubeType = CT;
        AppearTime = AT;
        AppearPlace = AP;
    }

    //=================旧版音乐块组件，可用于自定义谱面故不删除

}
public class GrobalVariable : MonoBehaviour
{

}
public class GrobalClass : GrobalVariable
{
    public static bool FirstComing = true;
    public static string LastScene="default";         //场景切换(由谁到达的该场景，目前只用于背景音乐切换)
    public static int AreaSkin = 2;         //0城市，1沙滩，2工业区

    public static int KHS = 8;
    public static int CY = 3;
    public static int YH =3;
    public static int AllRob = 15;


    public static float Iscore = 0;
    public static float Power = 0.8f;

    public static bool RealGameStart = false;

    public static float[] CubeSpeed = { 19, 23, 3 };          //音乐块移动速度，暂时只有两种音乐块(3号无用)

    public static string SongName = "要牢记";          //曲名
    public static int SongList = 999;                 //歌曲编号(读取自动修改)
    public static float PlayerAppearTime = 999;         //玩家模型入场时间(读取自动修改)
    public static float SongEndTime = 999;               //歌曲结束时间(读取自动修改)
    public static int Combo = 0;

  
    public static MusicCubeGround MuCubeGround;
    public static List<MusicCubeGround> MuCubeGroundList = new List<MusicCubeGround>();

    public static RectTransform FirstItem;            //被选中的Item(中间那一个)
    

    public static bool If_Game_End=false;         //游戏是否结局(菜单远景改变，解锁后台曲目)

    public static int nextNum = 1;//(1/2日记)

    public static int BossHealth = 0;

    public static float BossWinTime=0;        //Boss何时胜利(倒计时结束冲刺攻击玩家，退场)

    public static int UnLockLevel = 0;  //解锁的关卡
    public static void LoadToScene(int i)
    {
        SceneManager.LoadScene(i);
    }

}



