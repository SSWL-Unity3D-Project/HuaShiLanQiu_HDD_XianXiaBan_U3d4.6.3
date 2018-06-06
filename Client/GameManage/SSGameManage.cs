
using UnityEngine;

public class SSGameManage : SSGameMono
{
    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        SpawnGameData();
    }

    /// <summary>
    /// 产生游戏数据管理对象.
    /// </summary>
    void SpawnGameData()
    {
        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/SSGmData/SSGmData");
        if (gmDataPrefab != null)
        {
            Instantiate(gmDataPrefab);
        }
        else
        {
            UnityLogWarning("gmDataPrefab was null");   
        }
    }
}