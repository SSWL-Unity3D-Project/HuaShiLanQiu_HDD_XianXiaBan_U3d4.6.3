using UnityEngine;

public class SSGameRootCtrl : MonoBehaviour
{
    /// <summary>
    /// 场景动态产生对象收集器.
    /// </summary>
    [HideInInspector]
    public Transform MissionCleanup;
    /// <summary>
    /// 场景动态产生的篮球收集器.
    /// </summary>
    [HideInInspector]
    public Transform BallCleanup;
    static SSGameRootCtrl _Instance;
    public static SSGameRootCtrl GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_GameRoot");
            _Instance = obj.AddComponent<SSGameRootCtrl>();
            _Instance.Init();
        }
        return _Instance;
    }

    void Init()
    {
        GameObject obj = new GameObject("_MissionCleanup");
        MissionCleanup = obj.transform;

        obj = new GameObject("_BallCleanup");
        BallCleanup = obj.transform;
        BallCleanup.SetParent(MissionCleanup);
    }
}