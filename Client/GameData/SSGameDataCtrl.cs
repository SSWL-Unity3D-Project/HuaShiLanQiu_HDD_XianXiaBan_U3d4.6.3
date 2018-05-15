using UnityEngine;

public class SSGameDataCtrl : MonoBehaviour
{
    /// <summary>
    /// 游戏UI总控制.
    /// </summary>
    public SSUIRootCtrl m_SSUIRoot;
    /// <summary>
    /// 篮球创建规则数据.
    /// </summary>
    [System.Serializable]
    public class BallCreatRuleData
    {
        /// <summary>
        /// 在产生点中的最大索引.
        /// MaxIndex == 0 或 大于产生点最大数量时在整个产生点数组里随机取点.
        /// </summary>
        [Range(0, 100)]
        public int MaxIndex = 0;
        /// <summary>
        /// 发球的间隔时间.
        /// </summary>
        public float TimeVal = 15f;
        /// <summary>
        /// 篮球运动速度的倍率控制.
        /// </summary>
        [Range(0.1f, 10f)]
        public float BallSpeedBeiLv = 1f;
        /// <summary>
        /// 连发球的概率.
        /// </summary>
        [Range(0f, 1f)]
        public float LianFaBall = 0.5f;
        /// <summary>
        /// 连发2球的概率.
        /// </summary>
        [Range(0f, 1f)]
        public float LianFaBallNum02 = 0.5f;
        /// <summary>
        /// 连发球间隔最短时间控制.
        /// </summary>
        [Range(0.03f, 10f)]
        public float m_TimeMinLianFa = 0.3f;
        /// <summary>
        /// 普通球的概率.
        /// </summary>
        [Range(0f, 1f)]
        public float PuTongBall = 0.5f;
    }
    /// <summary>
    /// 创建篮球的规则数据列表.
    /// </summary>
    public BallCreatRuleData[] m_BallCreatRule = new BallCreatRuleData[3];

    /// <summary>
    /// 控制产生篮球的组件.
    /// </summary>
    public SSBallSpawnPoint[] m_BallSpawnArray = new SSBallSpawnPoint[2];

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
            m_BallSpawnArray[(int)index].Init();
            m_BallSpawnArray[(int)index].CreatGameBall();
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