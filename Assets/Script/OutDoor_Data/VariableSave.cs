using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class VariableSave : MonoBehaviour
{
   public static VariableSave Instance;
    public GameObject Canvas;       //等这边脚本完毕再启用画布那边的脚本
    public void Awake()
    {
        if (Instance == null)//如果
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);//如果是多余的就销毁
        }
        Canvas.GetComponent<Main>().enabled = true;
       
    }
    [System.Serializable]
    public class Save
    {
        public bool Firstcoming =false;
        public bool StoryEnd = false;
        public int Unlock_Level=0;
    }
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.Firstcoming = GrobalClass.FirstComing;
        save.StoryEnd = GrobalClass.If_Game_End;
        save.Unlock_Level = GrobalClass.UnLockLevel;
        return save;
    }

    public void Start_SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.Mxlf");
        bf.Serialize(file, save);
        file.Close();

 
        Debug.Log("游戏已保存！");
    }
    public void LoadGame()
    {

        if (File.Exists(Application.persistentDataPath + "/gamesave.Mxlf"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.Mxlf", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            GrobalClass.FirstComing = save.Firstcoming;
            GrobalClass.If_Game_End = save.StoryEnd;
            GrobalClass.UnLockLevel = save.Unlock_Level;
            Debug.Log("游戏已加载！");
        }
        else
        {
            Debug.Log("不存在存档！");
            Start_SaveGame();
        }
    }
    /*
    创建了一个 Save 对象，同时当前游戏的所有数据都会保存到这个对象中。
    创建了一个 BinaryFormatter，然后创建一个 FileStream，在创建时指定文件路径和要保存的 Save 对象。它会序列化数据（转换成字节），然后写磁盘，关闭 FileStream。现在在电脑上会多一个名为 gamesave.save 的文件。.save 后缀只是一个示例，你可以使用任意扩展名。
    重置游戏，以便玩家保存后所有东西都处于默认状态。
    */
}
