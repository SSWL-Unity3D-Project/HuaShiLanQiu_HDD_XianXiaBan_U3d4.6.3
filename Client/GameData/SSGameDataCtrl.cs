using System;
using UnityEngine;

public class SSGameDataCtrl : MonoBehaviour
{
    [System.Serializable]
    public class BallSpawnData
    {
        /// <summary>
        /// 产生篮球的间隔时间控制.
        /// </summary>
        [Range(0.03f, 10f)]
        public float m_TimeMinSpawn = 2f;
        /// <summary>
        /// 连发球间隔最短时间控制.
        /// </summary>
        [Range(0.03f, 10f)]
        public float m_TimeMinLianFa = 0.3f;
        public int m_MaxLianFaBallNum = 5;
        public int m_MinLianFaBallNum = 2;
    }
    /// <summary>
    /// 篮球产生的数据信息.
    /// </summary>
    public BallSpawnData m_BallSpawnData;

    [System.Serializable]
    public class LanQiuDeFenData
    {
        public int PuTongQiu = 1;
        public int KongXinQiu = 2;
        public LanQiuDeFenData(int pt, int kx)
        {
            PuTongQiu = pt;
            KongXinQiu = kx;
        }
    }
    /// <summary>
    /// 普通篮球得分数据.
    /// </summary>
    public LanQiuDeFenData m_PuTongBallScoreDt = new LanQiuDeFenData(1, 2);
    /// <summary>
    /// 花式篮球得分数据.
    /// </summary>
    public LanQiuDeFenData m_HuaShiBallScoreDt = new LanQiuDeFenData(2, 4);
    /// <summary>
    /// 烟雾特效球得分倍率.
    /// </summary>
    [Range(1, 10)]
    public int m_YanWuTXBallScoreBL = 2;

    /// <summary>
    /// 篮球类型.
    /// </summary>
    public enum LanQiuType
    {
        PuTong = 0,        //普通篮球.
        HuaShi = 1,        //花式篮球.
    }

    [System.Serializable]
    public class LanKuangData
    {
        /// <summary>
        /// X轴移动的最大距离.
        /// </summary>
        [Range(0.1f, 100f)]
        public float m_DisXMax = 2f;
        /// <summary>
        /// 篮筐移动的速度.
        /// </summary>
        [Range(0.01f, 100f)]
        public float m_SpeedX = 5f;
    }
    public LanKuangData m_LanKuangData;

    public enum PlayerIndex
    {
        Player01 = 0,
        Player02 = 1,
    }

    public class PlayerData
    {
        /// <summary>
        /// 是否激活了游戏.
        /// </summary>
        public bool IsActiveGame = false;
        /// <summary>
        /// 玩家分数信息.
        /// </summary>
        public int Score = 0;
    }
    public PlayerData[] m_PlayerData = new PlayerData[2];

    public SSLanKuangCtrl[] m_LanKuang = new SSLanKuangCtrl[2];

    static SSGameDataCtrl _Instance;
    public static SSGameDataCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
        m_PlayerData[0] = new PlayerData();
        m_PlayerData[1] = new PlayerData();
        _Instance = this;
        
        //SetActivePlayer(PlayerIndex.Player01, true); //test
        InputEventCtrl.GetInstance().OnClickStartBtEvent += OnClickStartBtEvent;
    }

    private void OnClickStartBtEvent(PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            SetActivePlayer(index, true);
        }
    }

    public void SetActivePlayer(PlayerIndex index, bool isActive)
    {
        if (m_PlayerData[(int)index].IsActiveGame == isActive)
        {
            return;
        }

        Debug.Log("SetActivePlayer -> index " + index + ", isActive " + isActive);
        m_PlayerData[(int)index].IsActiveGame = isActive;
        if (isActive)
        {
            //重置信息.
            m_PlayerData[(int)index].Score = 0;
            m_LanKuang[(int)index].m_SSTriggerScore.Init();
        }
        pcvr.GetInstance().SetIndexPlayerActiveGameState((int)index, (byte)(isActive == true ? 1 : 0));
    }

    /// <summary>
    /// Websocket预制.
    /// </summary>
    public GameObject m_WebSocketBoxPrefab;
    /// <summary>
    /// 产生Websocket预制.
    /// </summary>
    public GameObject SpawnWebSocketBox(Transform tr)
    {
        Debug.Log("SpawnWebSocketBox...");
        GameObject obj = (GameObject)Instantiate(m_WebSocketBoxPrefab);
        obj.transform.parent = tr;
        return obj;
    }
}