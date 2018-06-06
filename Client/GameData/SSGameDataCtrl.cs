using System.Collections.Generic;
using UnityEngine;

public class SSGameDataCtrl : SSGameMono
{
    /// <summary>
    /// 游戏模式.
    /// </summary>
    public enum GameMode
    {
        Null = -1,
        DanJi = 0,         //单机模式.
        LianJi = 1,        //联机模式.
    }

    /// <summary>
    /// 游戏UI总控制预制.
    /// </summary>
    public GameObject m_GameScenePrefab;
    /// <summary>
    /// 游戏UI总控制预制.
    /// </summary>
    public GameObject m_GameUIRootPrefab;
    /// <summary>
    /// 游戏UI总控制.
    /// </summary>
    [HideInInspector]
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
        /// 每一阶段的发球时长.
        /// </summary>
        public float TimeVal = 15f;
        /// <summary>
        /// 篮球运动速度的倍率控制.
        /// </summary>
        [Range(0.1f, 10f)]
        public float BallSpeedBeiLv = 1f;
        /// <summary>
        /// 单发球的间隔时间.
        /// </summary>
        [Range(0.1f, 50f)]
        public float m_TimeDanFa = 1.5f;
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
        /// 连发的最后一球之后,间隔多长时间产生下一球.
        /// </summary>
        [Range(0f, 50f)]
        public float m_TimeLianFa = 3f;
        /// <summary>
        /// 普通球的概率.
        /// </summary>
        [Range(0f, 1f)]
        public float PuTongBall = 0.5f;
        /// <summary>
        /// 炸弹球的概率.
        /// </summary>
        [Range(0f, 1f)]
        public float ZhaDanBall = 0.5f;
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
        Null = -1,
        PuTong = 0,        //普通篮球.
        HuaShi = 1,        //花式篮球.
        ZhaDan = 2,        //炸弹篮球.
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

    /// <summary>
    /// 玩家索引编号.
    /// </summary>
    public enum PlayerIndex
    {
        Null = -1,
        Player01 = 0,
        Player02 = 1,
    }

    public class PlayerRankData
    {
        /// <summary>
        /// 玩家索引.
        /// </summary>
        public PlayerIndex Index;
        /// <summary>
        /// 玩家分数.
        /// </summary>
        public int Score = 0;
        public PlayerRankData(PlayerIndex indexVal)
        {
            Index = indexVal;
        }
    }
    /// <summary>
    /// 玩家排名数据.
    /// </summary>
    public PlayerRankData[] m_PlayerRankData = new PlayerRankData[2];

    /// <summary>
    /// 排名数据比较器.
    /// </summary>
    int CompareRankDt(PlayerRankData x, PlayerRankData y)//排序器  
    {
        if (x == null)
        {
            if (y == null)
            {
                return 0;
            }
            return 1;
        }

        if (y == null)
        {
            return -1;
        }

        int retval = y.Score.CompareTo(x.Score);
        return retval;
    }

    /// <summary>
    /// 对排名数据进行排序来获取最高分数玩家的数据.
    /// </summary>
    public PlayerRankData GetMaxScorePlayerIndex()
    {
        List<PlayerRankData> rankDtList = new List<PlayerRankData>(m_PlayerRankData);
        rankDtList.Sort(CompareRankDt);
        UnityLog("GetMaxScorePlayerIndex -> index == " + rankDtList[0].Index
            + ", score == " + rankDtList[0].Score);
        return rankDtList[0];
    }

    public class LinkGamePlayerData
    {
        /// <summary>
        /// 玩家分数信息.
        /// </summary>
        public int Score = 0;
    }
    /// <summary>
    /// 联机游戏时玩家的数据信息.
    /// </summary>
    public LinkGamePlayerData[] m_LinkGamePlayerData = new LinkGamePlayerData[2];


    /// <summary>
    /// 联机游戏排名数据比较器.
    /// </summary>
    int CompareLinkGameRankDt(LinkGamePlayerData x, LinkGamePlayerData y)//排序器  
    {
        if (x == null)
        {
            if (y == null)
            {
                return 0;
            }
            return 1;
        }

        if (y == null)
        {
            return -1;
        }

        int retval = y.Score.CompareTo(x.Score);
        return retval;
    }

    /// <summary>
    /// 获取联机排名数据信息.
    /// </summary>
    public LinkGamePlayerData[] GetLinkGamePlayerRankDt()
    {
        List<LinkGamePlayerData> listPlayerDt = new List<LinkGamePlayerData>(m_LinkGamePlayerData);
        listPlayerDt.Sort(CompareLinkGameRankDt);
        return listPlayerDt.ToArray();
    }


    public class PlayerData
    {
        /// <summary>
        /// 玩家索引.
        /// </summary>
        public PlayerIndex Index;
        /// <summary>
        /// 是否激活了游戏.
        /// </summary>
        public bool IsActiveGame = false;
        /// <summary>
        /// 是否开始创建篮球.
        /// </summary>
        public bool IsCreateGameBall = false;
        /// <summary>
        /// 游戏分数界面UI.
        /// </summary>
        public SSGameScoreUI m_GameScoreCom = null;
        int _Score = 0;
        /// <summary>
        /// 玩家分数信息.
        /// </summary>
        public int Score
        {
            set
            {
                _Score = value;
                GetInstance().m_PlayerRankData[(int)Index].Score = value;
                if (m_GameMode == GameMode.LianJi)
                {
                    GetInstance().m_LinkGamePlayerData[(int)Index].Score = value;
                }

                if (m_GameScoreCom != null)
                {
                    m_GameScoreCom.ShowPlayerScore(value);
                }
            }
            get
            {
                return _Score;
            }
        }
        /// <summary>
        /// 游戏模式.
        /// </summary>
        public GameMode m_GameMode = GameMode.Null;
        /// <summary>
        /// 是否选择了游戏模式.
        /// </summary>
        public bool IsChooseGameMode = false;
        /// <summary>
        /// 是否播放了游戏开始倒计时.
        /// </summary>
        public bool IsPlayGameDaoJiShi = false;
        /// <summary>
        /// 是否激活了联机等待UI界面.
        /// </summary>
        public bool IsActiveLianJiWaitUI = false;

        public PlayerData(PlayerIndex indexVal)
        {
            Index = indexVal;
        }

        /// <summary>
        /// 重置信息.
        /// </summary>
        public void Reset()
        {
            Score = 0;
            m_GameMode = GameMode.Null;
            IsCreateGameBall = false;
            IsChooseGameMode = false;
            IsPlayGameDaoJiShi = false;
            IsActiveLianJiWaitUI = false;
            m_GameScoreCom = null;
        }
    }
    /// <summary>
    /// 玩家数据信息.
    /// </summary>
    public PlayerData[] m_PlayerData = new PlayerData[2];

    bool _IsActiveDeFenWangUI = false;
    /// <summary>
    /// 是否显示得分王UI.
    /// </summary>
    [HideInInspector]
    public bool IsActiveDeFenWangUI
    {
        set
        {
            _IsActiveDeFenWangUI = value;
        }
        get
        {
            return _IsActiveDeFenWangUI;
        }
    }

    bool _IsActiveGameLinkRankUI = false;
    /// <summary>
    /// 是否显示联机游戏比分排行UI界面.
    /// </summary>
    [HideInInspector]
    public bool IsActiveGameLinkRankUI
    {
        set
        {
            _IsActiveGameLinkRankUI = value;
        }
        get
        {
            return _IsActiveGameLinkRankUI;
        }
    }

    /// <summary>
    /// 篮筐控制脚本.
    /// </summary>
    public SSLanKuangCtrl[] m_LanKuang = new SSLanKuangCtrl[2];
    [HideInInspector]
    public SSTriggerRemoveBall m_TriggerRemoveBall;

    static SSGameDataCtrl _Instance;
    public static SSGameDataCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
        SpawnGameScene();
        for (int i = 0; i < m_PlayerData.Length; i++)
        {
            m_PlayerData[i] = new PlayerData((PlayerIndex)i);
            m_PlayerRankData[i] = new PlayerRankData((PlayerIndex)i);
            m_LinkGamePlayerData[i] = new LinkGamePlayerData();
        }
        _Instance = this;
        InputEventCtrl.GetInstance();

        SpawnGameUIRoot();

        //SetActivePlayer(PlayerIndex.Player01, true); //test
        InputEventCtrl.GetInstance().OnClickStartBtEvent += OnClickStartBtEvent;
    }

    /// <summary>
    /// 产生GameUIRoot.
    /// </summary>
    void SpawnGameUIRoot()
    {
        if (m_GameUIRootPrefab != null)
        {
            GameObject obj = (GameObject)Instantiate(m_GameUIRootPrefab);
            m_SSUIRoot = obj.GetComponent<SSUIRootCtrl>();
            if (m_SSUIRoot != null)
            {
                m_SSUIRoot.Init();
            }
        }
        else
        {
            UnityLogWarning("m_GameUIRootPrefab was null");
        }
    }
    
    /// <summary>
    /// 产生游戏场景数据对象.
    /// </summary>
    void SpawnGameScene()
    {
        if (m_GameScenePrefab != null)
        {
            Instantiate(m_GameScenePrefab);
        }
        else
        {
            UnityLogWarning("m_GameScenePrefab was null");
        }
    }

    private void OnClickStartBtEvent(PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            SetActivePlayer(index, true);
        }
    }

    /// <summary>
    /// 初始化创建篮球.
    /// </summary>
    public void InitCreateGameBall(PlayerIndex index)
    {
        if (m_PlayerData[(int)index].IsCreateGameBall == true)
        {
            return;
        }

        UnityLog("InitCreateGameBall -> index == " + index);
        m_PlayerData[(int)index].IsCreateGameBall = true;
        m_BallSpawnArray[(int)index].Init();
        //m_BallSpawnArray[(int)index].CreateGameBall();
    }
    
    public void SetActivePlayer(PlayerIndex index, bool isActive)
    {
        if (m_SSUIRoot.m_GameOverCom[(int)index] != null && isActive)
        {
            m_SSUIRoot.RemoveGameOverUI(index);
        }

        if (m_PlayerData[(int)index].IsActiveGame == isActive)
        {
            return;
        }

        UnityLog("SetActivePlayer -> index " + index + ", isActive " + isActive);
        m_PlayerData[(int)index].IsActiveGame = isActive;
        if (isActive)
        {
            //重置信息.
            m_PlayerData[(int)index].Score = 0;
            m_LanKuang[(int)index].m_SSTriggerScore.Init();
            //m_BallSpawnArray[(int)index].Init();
            //m_BallSpawnArray[(int)index].CreateGameBall();
            if (m_SSUIRoot != null)
            {
                m_SSUIRoot.RemoveGamneErWeiMa();
            }

            bool isCreateGameModeUI = true;
            bool isCreateGameDaoJiShi = false;
            //是否进入联机游戏.
            bool isGotoLinkGame = false;
            m_SSUIRoot.RemovePlayerErWeiMa(index);
            switch (index)
            {
                case PlayerIndex.Player01:
                    {
                        if (!m_PlayerData[(int)PlayerIndex.Player02].IsActiveGame)
                        {
                            //玩家2未激活游戏.
                            m_SSUIRoot.SpawnPlayerErWeiMa(PlayerIndex.Player02);
                        }

                        if (m_PlayerData[(int)PlayerIndex.Player02].IsPlayGameDaoJiShi)
                        {
                            //玩家2已经播放了游戏开始倒计时.
                            //玩家1直接进入单机游戏.
                            isCreateGameModeUI = false;
                            isCreateGameDaoJiShi = true;
                        }

                        if (m_PlayerData[(int)PlayerIndex.Player02].IsActiveLianJiWaitUI)
                        {
                            //玩家2激活了联机等待界面,使玩家进入联机游戏.
                            isGotoLinkGame = true;
                            isCreateGameModeUI = false;
                            isCreateGameDaoJiShi = true;
                            m_SSUIRoot.RemoveGameLianJiWaitUI(PlayerIndex.Player02);
                        }
                        break;
                    }
                case PlayerIndex.Player02:
                    {
                        if (!m_PlayerData[(int)PlayerIndex.Player01].IsActiveGame)
                        {
                            //玩家1未激活游戏.
                            m_SSUIRoot.SpawnPlayerErWeiMa(PlayerIndex.Player01);
                        }

                        if (m_PlayerData[(int)PlayerIndex.Player01].IsPlayGameDaoJiShi)
                        {
                            //玩家1已经播放了游戏开始倒计时.
                            //玩家2直接进入单机游戏.
                            isCreateGameModeUI = false;
                            isCreateGameDaoJiShi = true;
                        }

                        if (m_PlayerData[(int)PlayerIndex.Player01].IsActiveLianJiWaitUI)
                        {
                            //玩家1激活了联机等待界面,使玩家进入联机游戏.
                            isGotoLinkGame = true;
                            isCreateGameModeUI = false;
                            isCreateGameDaoJiShi = true;
                            m_SSUIRoot.RemoveGameLianJiWaitUI(PlayerIndex.Player01);
                        }
                        break;
                    }
            }

            if (isCreateGameModeUI)
            {
                m_SSUIRoot.SpawnGameModeUI(index);
            }

            if (isCreateGameDaoJiShi)
            {
                if (isGotoLinkGame)
                {
                    //激活联机模式倒计时.
                    m_SSUIRoot.SpawnGameDaoJiShiUI(PlayerIndex.Null);
                }
                else
                {
                    //激活单人模式倒计时.
                    m_SSUIRoot.SpawnGameDaoJiShiUI(index);
                }
            }
        }
        else
        {
            bool isOpenLinkRankUI = false;
            if (IsActiveGameLinkRankUI && !IsActiveDeFenWangUI)
            {
                //显示联机游戏排行界面.
                isOpenLinkRankUI = true;
                IsActiveGameLinkRankUI = false;
                m_SSUIRoot.SpawnGameLinkRankUI(index);
            }

            IsActiveDeFenWangUI = false;
            if (!isOpenLinkRankUI)
            {
                m_SSUIRoot.SpawnGameOverUI(index);
            }

            m_PlayerData[(int)index].Reset();
            m_SSUIRoot.RemoveGameScoreUI(index);
            m_BallSpawnArray[(int)index].ResetInfo();
        }
        pcvr.GetInstance().SetIndexPlayerActiveGameState((int)index, (byte)(isActive == true ? 1 : 0));
        m_LanKuang[(int)index].SetActiveRealBallKuang(isActive);
    }

    /// <summary>
    /// 检测游戏结束后二维码UI的产生.
    /// </summary>
    public void CheckCreateGameErWeiMa(PlayerIndex indexVal)
    {
        //是否产生公用二维码.
        bool isCreateBigErWeiMa = true;
        for (int i = 0; i < m_PlayerData.Length; i++)
        {
            if (m_PlayerData[i].IsActiveGame == true)
            {
                isCreateBigErWeiMa = false;
                break;
            }
        }

        if (isCreateBigErWeiMa)
        {
            //所有玩家都需要重新开始游戏了.
            m_SSUIRoot.SpawnGamneErWeiMa();
            m_SSUIRoot.RemovePlayerErWeiMa(PlayerIndex.Player01);
            m_SSUIRoot.RemovePlayerErWeiMa(PlayerIndex.Player02);
        }
        else
        {
            if (!m_PlayerData[(int)indexVal].IsActiveGame)
            {
                m_SSUIRoot.SpawnPlayerErWeiMa(indexVal);
            }
        }
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

#if UNITY_EDITOR
    bool IsPrintGameInfo = false;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            IsPrintGameInfo = !IsPrintGameInfo;
        }
    }

    void OnGUI()
    {
        if (IsPrintGameInfo)
        {
            string infoP1 = "p1: jieDuan " + m_BallSpawnArray[0].IndexCreatBallJieDuan;
            GUI.Box(new Rect(10f, 10f, Screen.width - 20f, 25f), infoP1);

            string infoP2 = "p2: jieDuan " + m_BallSpawnArray[1].IndexCreatBallJieDuan;
            GUI.Box(new Rect(10f, 35f, Screen.width - 20f, 25f), infoP2);
        }
    }
#endif
}