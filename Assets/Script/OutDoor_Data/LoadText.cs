using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//============================================


//配合谱面录入工具对接外部资源加载，用于玩家自定义谱面，暂时不需要了
//若需要，请解除注释后重新将脚本挂在GameEvent组件上！！！！


//============================================
public class LoadText : MonoBehaviour
{

    public string[] TextFirst;


    public string[] MusicList;
    public string[] Cube2ndTemp;        //第一次分割，存储 种类与出现时间+类型
    public string[] Cube3rdTemp;        //第二次分割，存储 种类+出现时间
    // Start is called before the first frame update
    void Start()
    {


        TextAsset txt = Resources.Load("M01") as TextAsset;

        MusicList = txt.text.Split('\n');  // 以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容

        // 将每行字符串的内容以逗号作为分割点，并将每个逗号分隔的字符串内容遍历输出
        //foreach (string strs in MusicList[0])
        //{
        //    TextTemp = strs.Split(',');

        //}
        switch (GrobalClass.SongName)
        {
            case "萨哈星球":
                GrobalClass.SongList = 0;
             
                break;
            case "要牢记":
                GrobalClass.SongList = 1;
                
                break;
            case "谁不会":
                GrobalClass.SongList = 2;
                break;
            case "环保歌":
                GrobalClass.SongList = 3;
                break;
            case "骄傲的少年":
                GrobalClass.SongList = 4;
                break;
            case "我的战争":
                GrobalClass.SongList = 5;
                break;
            case "逐蜥":
                GrobalClass.SongList = 6;
                break;
            default:
                GrobalClass.SongList = 999;
                break;
        }
        Debug.Log("读入列表歌曲为:" + GrobalClass.SongList.ToString()+" 曲名 "+GrobalClass.SongName);
     
        MusicCube_Insert();
    

    }

    void MusicCube_Insert()
    {


        TextFirst = MusicList[GrobalClass.SongList].Split(',');      //切割选中曲目，第一次分割(分开各个音乐块)

        for (int i = 3; i < TextFirst.Length; i++)
        {

            Cube2ndTemp = TextFirst[i].Split('-');              //进行二次分割，如将其中一个string 5-1分割为5和1并进行录入
            //Debug.Log(Cube2ndTemp[0]);
            Cube3rdTemp = Cube2ndTemp[0].Split('&');            //进行三次分割，将音乐块标识符与产生时间分割(如 & 10.5分割为 & 与10.5)
            //Debug.Log(Cube3rdTemp[0]);

            //============分割的数据各回各家============
            float CubeTypeTemp = float.Parse(Cube3rdTemp[0]);   //种类
            float Atemp = float.Parse(Cube3rdTemp[1]);      //出现时间
            int Ptemp = int.Parse(Cube2ndTemp[1]);          //出现位置
            //==========================================

            int ChangeInt = (int)CubeTypeTemp;
            switch (ChangeInt)       //区分音乐块类型，分别存储
            {
                //种类1：普通块-可回收
                case 1:

                //种类1：普通块-厨余
                case 2:

                //种类1：普通块-有害
                case 3:
                    GrobalClass.MuCubeGround = new MusicCubeGround(CubeTypeTemp, Atemp - 1.8f, Ptemp);        //导入该音乐块,1.1为延迟(提前1.1秒刷出，为到达判定点的时间，越大越早刷新)
                    GrobalClass.MuCubeGroundList.Add(GrobalClass.MuCubeGround);
                    break;
                case 4://种类2：陷阱块
                    GrobalClass.MuCubeGround = new MusicCubeGround(CubeTypeTemp, Atemp - 1.65f, Ptemp);    
                    GrobalClass.MuCubeGroundList.Add(GrobalClass.MuCubeGround);
                    break;

                case 5://种类3：奖励块
                    GrobalClass.MuCubeGround = new MusicCubeGround(CubeTypeTemp, Atemp - 1.65f, Ptemp);    
                    GrobalClass.MuCubeGroundList.Add(GrobalClass.MuCubeGround);
                    break;
                //种类3：长条块
                case 10:
                    GrobalClass.MuCubeGround = new MusicCubeGround(CubeTypeTemp, Atemp - 1.8f, Ptemp);        //导入该奖励音乐块,飞得更快因此延迟降低(提前0.5秒刷出，为到达判定点的时间，越大越早刷新)
                    GrobalClass.MuCubeGroundList.Add(GrobalClass.MuCubeGround);
                    break;
                default:
                    Debug.Log("无该类型块("+ChangeInt+")");break;
            }





        }

        GrobalClass.PlayerAppearTime = float.Parse(TextFirst[1]);
        GrobalClass.SongEndTime = float.Parse(TextFirst[2]);

        Debug.Log("歌曲准备完毕！");
    }




}
