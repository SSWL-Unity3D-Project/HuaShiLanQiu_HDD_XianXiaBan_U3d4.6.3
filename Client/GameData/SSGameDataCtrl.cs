#define TEST_SHOW_GAME_INFO
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSGameDataCtrl : SSGameMono
{
    /// <summary>
    /// 游戏对话框界面选项枚举.
    /// </summary>
    public enum XuanXiangState
    {
        /// <summary>
        /// 选项1.
        /// </summary>
        XuanXiang01 = 0,
        XuanXiang02 = 1,
        XuanXiang03 = 2,
        XuanXiang04 = 3,
        XuanXiang05 = 4,
    }
    /// <summary>
    /// 选项移动方向.
    /// </summary>
    public enum XuanXiangMoveDir
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
    }

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
    /// 空心球动画特效.
    /// </summary>
    public SSKongXinQiuTXAni[] m_KongXinQiuTXArray = new SSKongXinQiuTXAni[2];
    /// <summary>
    /// 游戏背景图资源索引.
    /// </summary>
    int m_IndexGameBeiJingImg = 0;
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
        /// 单发球之后连发的第一球的等待时间信息.
        /// </summary>
        [Range(0f, 50f)]
        public float m_DanFaTimeLianFa = 0f;
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
        /// <summary>
        /// 正常篮筐的大小参数.
        /// </summary>
        [Range(0.1f, 10f)]
        public float m_LanKaungScale = 0.655f;
        /// <summary>
        /// 放大篮筐的大小参数.
        /// </summary>
        [Range(0.1f, 10f)]
        public float m_BigLanKaungScale = 0.8f;
        /// <summary>
        /// 空心球篮环爆炸粒子.
        /// </summary>
        public GameObject m_KongXinQiuExp;
        /// <summary>
        /// 花球空心球篮环爆炸粒子.
        /// </summary>
        public GameObject m_HuaQiuKongXinQiuExp;
        /// <summary>
        /// 非空心球篮环爆炸粒子.
        /// </summary>
        public GameObject m_NoKongXinQiuExp;
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
        return m_LinkGamePlayerData;
        //List<LinkGamePlayerData> listPlayerDt = new List<LinkGamePlayerData>(m_LinkGamePlayerData);
        //listPlayerDt.Sort(CompareLinkGameRankDt);
        //return listPlayerDt.ToArray();
    }

    /// <summary>
    /// 是否停止发球.
    /// </summary>
    [HideInInspector]
    public bool IsStopCreatBall = false;
    /// <summary>
    /// 创建篮球时间阶段管理组件.
    /// </summary>
    [HideInInspector]
    public SSCreatLanQiuStage m_CreatLanQiuStage = null;

    bool _IsPauseGame = false;
    /// <summary>
    /// 是否暂停游戏.
    /// </summary>
    public bool IsPauseGame
    {
        set
        {
            _IsPauseGame = value;
            IsStopCreatBall = value;

            if (_IsPauseGame)
            {
                CleanGameBalls();
            }
        }
        get
        {
            return _IsPauseGame;
        }
    }
    
    /// <summary>
    /// 清理游戏场景中的篮球.
    /// </summary>
    void CleanGameBalls()
    {
        UnityLog("CleanGameBalls...");
        SSBallAniCtrl[] ballAy = SSGameRootCtrl.GetInstance().BallCleanup.GetComponentsInChildren<SSBallAniCtrl>();
        for (int i = 0; i < ballAy.Length; i++)
        {
            if (ballAy[i] != null)
            {
                ballAy[i].RemoveSelf();
            }
        }
    }

    /// <summary>
    /// 获取游戏中篮球的个数.
    /// </summary>
    public int GetGameBallsCount()
    {
        SSBallAniCtrl[] ballAy = SSGameRootCtrl.GetInstance().BallCleanup.GetComponentsInChildren<SSBallAniCtrl>();
        //UnityLog("GetGameBallsCount -> count ================= " + ballAy.Length);
        return ballAy.Length;
    }


    /// <summary>
    /// 检测游戏是否主动弹出道具购买界面.
    /// </summary>
    public void CheckGameDaoJuGouMaiUI()
    {
        StopCoroutine(DelayCheckGameDaoJuGouMaiUI());
        StartCoroutine(DelayCheckGameDaoJuGouMaiUI());
    }

    IEnumerator DelayCheckGameDaoJuGouMaiUI()
    {
        yield return new WaitForSeconds(0.1f);
        if (IsStopCreatBall && GetGameBallsCount() <= 0)
        {
            //产生某一阶段的道具购买UI界面.
            if (m_CreatLanQiuStage != null)
            {
                m_CreatLanQiuStage.CheckSpawnDaoJuGouMaiUI();
            }
        }
    }

    /// <summary>
    /// 放大篮筐的阶段.
    /// </summary>
    [HideInInspector]
    public int IndexJieDuanFangDaLanKuang = 2;
    int _IndexJieDuanLanQiuJianSu = 3;
    /// <summary>
    /// 篮球减速的阶段.
    /// </summary>
    [HideInInspector]
    public int IndexJieDuanLanQiuJianSu = 3;
    /// <summary>
    /// 是否创建了篮球减速道具购买界面.
    /// </summary>
    [HideInInspector]
    public bool IsCreatLanQiuJianSuDaoJuBuy = false;

    public enum PlayerGameState
    {
        /// <summary>
        /// 游戏前.
        /// </summary>
        YouXiQian = 0,
        /// <summary>
        /// 游戏中.
        /// </summary>
        YouXiZhong = 1,
    }

    public class PlayerData
    {
        /// <summary>
        /// 玩家索引.
        /// </summary>
        public PlayerIndex Index;
        /// <summary>
        /// 微信头像url.
        /// </summary>
        public string PlayerHeadUrl = "";
        /// <summary>
        /// 是否激活了游戏.
        /// </summary>
        public bool IsActiveGame = false;
        /// <summary>
        /// 是否是红点点微信手柄玩家.
        /// </summary>
        public bool IsHddWxPadPlayer
        {
            get
            {
                bool isHddPad = false;
                if (PlayerHeadUrl != "" && PlayerHeadUrl.Length > 5f)
                {
                    isHddPad = true;
                }
                return isHddPad;
            }
        }
        bool _IsCreateGameBall = false;
        /// <summary>
        /// 是否开始创建篮球.
        /// </summary>
        public bool IsCreateGameBall
        {
            set
            {
                _IsCreateGameBall = value;
                if (_IsCreateGameBall)
                {
                    GameState = PlayerGameState.YouXiZhong;
                }
            }
            get
            {
                return _IsCreateGameBall;
            }
        }
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
                if (value > 9999)
                {
                    //玩家分数最高锁定在9999上.
                    value = 9999;
                }

                _Score = value;
                GetInstance().m_PlayerRankData[(int)Index].Score = value;
                if (m_GameMode == GameMode.LianJi
                    || IsJieShouTiaoZhan)
                {
                    //双方对战时记录玩家分数信息.
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
        /// <summary>
        /// 是否接受挑战对方.
        /// </summary>
        public bool IsJieShouTiaoZhan = false;
        /// <summary>
        /// 是否同意继续游戏.
        /// </summary>
        public bool IsTongYiJiXuGame = false;
        int _PlayerHealth = 0;
        /// <summary>
        /// 玩家血值.
        /// </summary>
        public int PlayerHealth
        {
            set
            {
                _PlayerHealth = value;
                if (value > 0)
                {
                    if (GetInstance().m_SSUIRoot.m_FuHuoCiShu != null)
                    {
                        GetInstance().m_SSUIRoot.m_FuHuoCiShu.ShowPlayerHealth(value, Index);
                    }
                }
            }
            get
            {
                return _PlayerHealth;
            }
        }
        /// <summary>
        /// 游戏状态.
        /// </summary>
        public PlayerGameState GameState = PlayerGameState.YouXiQian;

        public PlayerData(PlayerIndex indexVal)
        {
            Index = indexVal;
        }

        /// <summary>
        /// 重置信息.
        /// </summary>
        public void Reset()
        {
            //记录最后一个结束游戏玩家的数据.
            int indexCreatBallJieDuan = GetInstance().m_BallSpawnArray[(int)Index].IndexCreatBallJieDuan;
            bool isCreatLanQiuJianSuDaoJuBuy = GetInstance().IsCreatLanQiuJianSuDaoJuBuy;
            GetInstance().m_LastOverPlayerData = new PlayerOverData(Index, Score, indexCreatBallJieDuan, isCreatLanQiuJianSuDaoJuBuy);

            //重置玩家信息.
            //Score = 0;
            m_GameMode = GameMode.Null;
            IsCreateGameBall = false;
            IsChooseGameMode = false;
            IsPlayGameDaoJiShi = false;
            IsActiveLianJiWaitUI = false;
            IsJieShouTiaoZhan = false;
            IsTongYiJiXuGame = false;
            m_GameScoreCom = null;
            PlayerHealth = 0;
        }
    }
    /// <summary>
    /// 玩家数据信息.
    /// </summary>
    public PlayerData[] m_PlayerData = new PlayerData[2];

    public class PlayerOverData
    {
        /// <summary>
        /// 玩家索引信息.
        /// </summary>
        public PlayerIndex Index;
        /// <summary>
        /// 分数信息.
        /// </summary>
        public int Score = 0;
        /// <summary>
        /// 创建篮球的阶段索引.
        /// </summary>
        public int IndexCreatBallJieDuan = 0;
        /// <summary>
        /// 是否创建过篮球减速道具购买界面.
        /// </summary>
        public bool IsCreatLanQiuJianSuDaoJuBuy = false;
        public PlayerOverData(PlayerIndex indexVal, int scoreVal, int indexBallJieDuan, bool isCreatLanQiuJianSuDaoJuBuy)
        {
            Index = indexVal;
            Score = scoreVal;
            IndexCreatBallJieDuan = indexBallJieDuan;
            IsCreatLanQiuJianSuDaoJuBuy = isCreatLanQiuJianSuDaoJuBuy;
        }
    }
    /// <summary>
    /// 最后一个结束游戏的玩家数据缓存.
    /// </summary>
    public PlayerOverData m_LastOverPlayerData;

    /// <summary>
    /// 清理最后一个结束游戏玩家的数据.
    /// </summary>
    public void CleanLastOverPlayerData()
    {
        if (m_LastOverPlayerData != null)
        {
            m_LastOverPlayerData = null;
        }
    }

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
    /// <summary>
    /// 篮筐倒计时控制组件.
    /// </summary>
    public SSLanKuangTimeAni m_LianKuangTimeAni;
    /// <summary>
    /// 声音数据.
    /// </summary>
    [HideInInspector]
    public SSAudioData m_AudioData;
    /// <summary>
    /// 动态加载url图片组件.
    /// </summary>
    [HideInInspector]
    public AsyncImageDownload m_AsyncImg;

    static SSGameDataCtrl _Instance;
    public static SSGameDataCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        //Application.targetFrameRate = 60;
        Screen.SetResolution(1280, 720, false);
        Screen.showCursor = false;
        SpawnGameScene();
        for (int i = 0; i < m_PlayerData.Length; i++)
        {
            m_PlayerData[i] = new PlayerData((PlayerIndex)i);
            m_PlayerRankData[i] = new PlayerRankData((PlayerIndex)i);
            m_LinkGamePlayerData[i] = new LinkGamePlayerData();
            m_LanHuanExpList[i] = new List<GameObject>();
            if (m_KongXinQiuTXArray[i] != null)
            {
                m_KongXinQiuTXArray[i].Init();
            }
        }

        _Instance = this;
        InputEventCtrl.GetInstance();
        SpawnGameUIRoot();
        if (m_LianKuangTimeAni != null)
        {
            m_LianKuangTimeAni.Init();
        }

        SpawnAudioData();
        AddCreatLanQiuStage();
        CreatAsyncImageDownload();
        //显示游戏FPS.
        ShowGameFpsInfo();

        //SetActivePlayer(PlayerIndex.Player01, true); //test
        InputEventCtrl.GetInstance().OnClickStartBtEvent += OnClickStartBtEvent;
    }
    
    /// <summary>
    /// 创建动态加载url图片组件.
    /// </summary>
    void CreatAsyncImageDownload()
    {
        if (m_AsyncImg == null)
        {
            m_AsyncImg = gameObject.AddComponent<AsyncImageDownload>();
        }
    }

    /// <summary>
    /// 添加SSCreatLanQiuStage组件.
    /// </summary>
    void AddCreatLanQiuStage()
    {
        if (m_CreatLanQiuStage == null)
        {
            m_CreatLanQiuStage = gameObject.AddComponent<SSCreatLanQiuStage>();
        }
    }

    /// <summary>
    /// 产生游戏音效数据.
    /// </summary>
    void SpawnAudioData()
    {
        if (m_AudioData != null)
        {
            return;
        }

        string uiPrefabPath = "Prefabs/SSGmData/AudioData";
        GameObject gmDataPrefab = (GameObject)Resources.Load(uiPrefabPath);
        if (gmDataPrefab != null)
        {
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
            m_AudioData = obj.GetComponent<SSAudioData>();
            m_AudioData.Init();
        }
    }

    public void ShowPlayerLanKuangDaoJiShi(PlayerIndex index, SSLanKuangTimeAni.DaoJiShiState type, SSLanKuangTimeAni.DaoJuState daoJu)
    {
        if (m_LianKuangTimeAni != null)
        {
            m_LianKuangTimeAni.PlayDaoJiShi(index, type, daoJu);
        }
    }

    /// <summary>
    /// 显示游戏fps信息.
    /// </summary>
    void ShowGameFpsInfo()
    {
        GameObject obj = new GameObject("_GameFps");
        obj.AddComponent<XKGameFPSCtrl>();
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
        if (m_SSUIRoot.m_ExitGameUI != null)
        {
            return;
        }

        if (m_SSUIRoot.m_GameOverCom[(int)index] != null && isActive)
        {
            m_SSUIRoot.RemoveGameOverUI(index);
        }

        if (m_PlayerData[(int)index].IsActiveGame == isActive)
        {
            return;
        }

        UnityLog("SetActivePlayer -> index " + index + ", isActive " + isActive);
        if (m_KongXinQiuTXArray[(int)index] != null)
        {
            //改变玩家当前游戏场景的背景贴图.
            m_KongXinQiuTXArray[(int)index].ChangePlayerSceneImg(isActive);
        }
        m_PlayerData[(int)index].IsActiveGame = isActive;

        bool isAllPlayerDeath = true;
        if (isActive)
        {
            //重置信息.
            m_PlayerData[(int)index].PlayerHealth = 1;
            m_PlayerData[(int)index].Score = 0;
            m_LanKuang[(int)index].m_SSTriggerScore.Init();
            //m_BallSpawnArray[(int)index].Init();
            //m_BallSpawnArray[(int)index].CreateGameBall();
            if (m_SSUIRoot != null)
            {
                m_SSUIRoot.RemoveGamneErWeiMa();
            }
            
            bool isCreateGameFuHuoDaoJu = true;
            //bool isCreateGameModeUI = true;
            //bool isCreateGameDaoJiShi = false;
            //是否进入联机游戏.
            //bool isGotoLinkGame = false;
            m_SSUIRoot.RemovePlayerErWeiMa(index);
            switch (index)
            {
                case PlayerIndex.Player01:
                    {
                        if (!m_PlayerData[(int)PlayerIndex.Player02].IsActiveGame)
                        {
                            //玩家2未激活游戏.
                            if (m_PlayerData[(int)PlayerIndex.Player02].GameState == PlayerGameState.YouXiQian)
                            {
                                //当前没有弹出是否继续游戏提示框.
                                //产生二维码界面.
                                m_SSUIRoot.SpawnPlayerErWeiMa(PlayerIndex.Player02);
                            }
                            else
                            {
                                //显示请等待对方本场游戏结束UI.
                                m_SSUIRoot.SpawnDengDuiFangJieShu(index);
                                isCreateGameFuHuoDaoJu = false;
                            }
                        }
                        else
                        {
                            isCreateGameFuHuoDaoJu = false;
                        }

                        if (m_PlayerData[(int)PlayerIndex.Player02].IsPlayGameDaoJiShi)
                        {
                            //玩家2已经播放了游戏开始倒计时.
                            //玩家1直接进入单机游戏.
                            //isCreateGameModeUI = false;
                            //isCreateGameDaoJiShi = true;
                            isCreateGameFuHuoDaoJu = false;
                            //显示请等待对方本场游戏结束UI.
                            m_SSUIRoot.SpawnDengDuiFangJieShu(index);
                        }
                        else
                        {
                            if (m_PlayerData[(int)PlayerIndex.Player02].IsActiveGame)
                            {
                                if (m_PlayerData[(int)PlayerIndex.Player02].IsCreateGameBall)
                                {
                                }
                                else
                                {
                                    if (m_PlayerData[(int)PlayerIndex.Player02].IsTongYiJiXuGame)
                                    {
                                        //玩家选择继续游戏.
                                        //显示请等待对方本场游戏结束UI.
                                        m_SSUIRoot.SpawnDengDuiFangJieShu(index);
                                    }
                                    else
                                    {
                                        //没有选择继续游戏的选择.
                                        //玩家2激活了游戏,但是还没有开始发球.
                                        m_SSUIRoot.SpawnDengDaiDuiFangTongYiPK(PlayerIndex.Player01);
                                    }
                                }
                            }
                        }

                        //if (m_PlayerData[(int)PlayerIndex.Player02].IsActiveLianJiWaitUI)
                        //{
                        //    //玩家2激活了联机等待界面,使玩家进入联机游戏.
                        //    isGotoLinkGame = true;
                        //    isCreateGameModeUI = false;
                        //    isCreateGameDaoJiShi = true;
                        //    m_SSUIRoot.RemoveGameLianJiWaitUI(PlayerIndex.Player02);
                        //}
                        break;
                    }
                case PlayerIndex.Player02:
                    {
                        if (!m_PlayerData[(int)PlayerIndex.Player01].IsActiveGame)
                        {
                            //玩家1未激活游戏.
                            if (m_PlayerData[(int)PlayerIndex.Player01].GameState == PlayerGameState.YouXiQian)
                            {
                                //当前没有弹出是否继续游戏提示框.
                                //产生二维码界面.
                                m_SSUIRoot.SpawnPlayerErWeiMa(PlayerIndex.Player01);
                            }
                            else
                            {
                                //显示请等待对方本场游戏结束UI.
                                m_SSUIRoot.SpawnDengDuiFangJieShu(index);
                                isCreateGameFuHuoDaoJu = false;
                            }
                        }
                        else
                        {
                            isCreateGameFuHuoDaoJu = false;
                        }

                        if (m_PlayerData[(int)PlayerIndex.Player01].IsPlayGameDaoJiShi)
                        {
                            //玩家1已经播放了游戏开始倒计时.
                            //玩家2直接进入单机游戏.
                            //isCreateGameModeUI = false;
                            //isCreateGameDaoJiShi = true;
                            isCreateGameFuHuoDaoJu = false;
                            //显示请等待对方本场游戏结束UI.
                            m_SSUIRoot.SpawnDengDuiFangJieShu(index);
                        }
                        else
                        {
                            if (m_PlayerData[(int)PlayerIndex.Player01].IsActiveGame)
                            {
                                if (m_PlayerData[(int)PlayerIndex.Player01].IsCreateGameBall)
                                {
                                }
                                else
                                {
                                    if (m_PlayerData[(int)PlayerIndex.Player01].IsTongYiJiXuGame)
                                    {
                                        //玩家选择继续游戏.
                                        //显示请等待对方本场游戏结束UI.
                                        m_SSUIRoot.SpawnDengDuiFangJieShu(index);
                                    }
                                    else
                                    {
                                        //没有选择继续游戏的选择.
                                        //玩家1激活了游戏,但是还没有开始发球.
                                        m_SSUIRoot.SpawnDengDaiDuiFangTongYiPK(PlayerIndex.Player02);
                                    }
                                }
                            }
                        }

                        //if (m_PlayerData[(int)PlayerIndex.Player01].IsActiveLianJiWaitUI)
                        //{
                        //    //玩家1激活了联机等待界面,使玩家进入联机游戏.
                        //    isGotoLinkGame = true;
                        //    isCreateGameModeUI = false;
                        //    isCreateGameDaoJiShi = true;
                        //    m_SSUIRoot.RemoveGameLianJiWaitUI(PlayerIndex.Player01);
                        //}
                        break;
                    }
            }

            //if (isCreateGameModeUI)
            //{
            //    m_SSUIRoot.SpawnGameModeUI(index);
            //}

            if (isCreateGameFuHuoDaoJu)
            {
                if (m_SSUIRoot.m_SSGameDlgManage.FindGameDlgByType(SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi) == null)
                {
                    //当前没有弹出是否继续游戏提示框.
                    //创建复活道具购买界面.
                    m_SSUIRoot.SpawnGameFuHuoDaoJuPanel(index);
                }
            }
            m_LanKuang[(int)index].SetActiveRealBallKuang(true);

            //if (isCreateGameDaoJiShi)
            //{
            //    if (isGotoLinkGame)
            //    {
            //        //激活联机模式倒计时.
            //        m_SSUIRoot.SpawnGameDaoJiShiUI(PlayerIndex.Null);
            //    }
            //    else
            //    {
            //        //激活单人模式倒计时.
            //        m_SSUIRoot.SpawnGameDaoJiShiUI(index);
            //    }
            //}
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

            CleanLanHuanExplosionToList(index);
            m_PlayerData[(int)index].Reset();
            m_SSUIRoot.RemoveGameScoreUI(index);
            m_BallSpawnArray[(int)index].ResetInfo();
            //m_LanKuang[(int)index].SetActiveRealBallKuang(false);
            
            //重置篮筐特殊道具时的状态.
            if (m_LianKuangTimeAni != null)
            {
                m_LianKuangTimeAni.SetActivePlayerTime(index, false);
            }

            switch (index)
            {
                case PlayerIndex.Player01:
                    {
                        if (m_PlayerData[(int)PlayerIndex.Player02].IsActiveGame)
                        {
                            //玩家2激活游戏.
                            if (m_PlayerData[(int)PlayerIndex.Player02].IsCreateGameBall)
                            {
                                //玩家2开始发球了(游戏中).
                                //玩家1回到游戏前.
                                m_PlayerData[(int)index].GameState = PlayerGameState.YouXiQian;
                            }
                            else
                            {
                                //玩家2没有开始发球.
                            }
                            m_SSUIRoot.m_FuHuoCiShu.SetActivePlayerHealth(index, false);
                        }
                        else
                        {
                            m_AudioData.PlayYinDaoJieMianAudio();
                            //删除复活次数UI.
                            m_SSUIRoot.RemoveGameFuHuoCiShuPanel();
                        }
                        break;
                    }
                case PlayerIndex.Player02:
                    {
                        if (m_PlayerData[(int)PlayerIndex.Player01].IsActiveGame)
                        {
                            //玩家1激活游戏.
                            if (m_PlayerData[(int)PlayerIndex.Player01].IsCreateGameBall)
                            {
                                //玩家1开始发球了(游戏中).
                                //玩家2回到游戏前.
                                m_PlayerData[(int)index].GameState = PlayerGameState.YouXiQian;
                            }
                            else
                            {
                                //玩家1没有开始发球.
                            }
                            m_SSUIRoot.m_FuHuoCiShu.SetActivePlayerHealth(index, false);
                        }
                        else
                        {
                            m_AudioData.PlayYinDaoJieMianAudio();
                            //删除复活次数UI.
                            m_SSUIRoot.RemoveGameFuHuoCiShuPanel();
                        }
                        break;
                    }
            }

            for (int i = 0; i < m_PlayerData.Length; i++)
            {
                if (m_PlayerData[i].IsCreateGameBall)
                {
                    isAllPlayerDeath = false;
                }
            }

            if (isAllPlayerDeath)
            {
                //所有玩家都结束游戏了,重置游戏数据信息.
                m_SSUIRoot.RemoveGameLanKuangFangDaDingBuPanel();
                m_SSUIRoot.RemoveGameLanQiuJianSuDingBuPanel();
                IndexJieDuanLanQiuJianSu = _IndexJieDuanLanQiuJianSu;
                m_SSUIRoot.RemoveDaoJuTimeUpCom();
                m_CreatLanQiuStage.Reset();
                if (m_LianKuangTimeAni != null)
                {
                    m_LianKuangTimeAni.HiddenAllPlayerTime();
                }
                IsCreatLanQiuJianSuDaoJuBuy = false;
                IsPauseGame = false;
            }
        }

        if (isActive)
        {
            pcvr.GetInstance().SetIndexPlayerActiveGameState((int)index, (byte)(isActive == true ? pcvr.PlayerActiveState.JiHuo : pcvr.PlayerActiveState.WeiJiHuo));
        }
        else
        {
            if (!isAllPlayerDeath)
            {
                //还有一个玩家正在继续游戏,清理当前玩家的数据.
                pcvr.GetInstance().SetIndexPlayerActiveGameState((int)index, (byte)(isActive == true ? pcvr.PlayerActiveState.JiHuo : pcvr.PlayerActiveState.WeiJiHuo));
            }
        }
    }

    /// <summary>
    /// 获取反向玩家的索引.
    /// </summary>
    public PlayerIndex GetReversePlayerIndex(PlayerIndex index)
    {
        PlayerIndex indexPlayer = PlayerIndex.Null;
        switch (index)
        {
            case PlayerIndex.Player01:
                {
                    indexPlayer = PlayerIndex.Player02;
                    break;
                }
            case PlayerIndex.Player02:
                {
                    indexPlayer = PlayerIndex.Player01;
                    break;
                }
        }
        return indexPlayer;
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
    
    public enum LanQiuMoveSpeed
    {
        /// <summary>
        /// 正常速度.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 篮球慢速.
        /// </summary>
        Slow = 1,
    }
    /// <summary>
    /// 篮球运动速度类型.
    /// </summary>
    [HideInInspector]
    public LanQiuMoveSpeed m_LanQiuMoveSpeedType = LanQiuMoveSpeed.Normal;
    /// <summary>
    /// 篮球减速时的发球规则信息.
    /// </summary>
    public BallCreatRuleData m_BallSlowSpeedRule;

    /// <summary>
    /// 获取发球规则信息.
    /// </summary>
    public BallCreatRuleData GetBallCreatRuleDt(int index)
    {
        if (m_LanQiuMoveSpeedType == LanQiuMoveSpeed.Slow)
        {
            return m_BallSlowSpeedRule;
        }

        if (index < 0)
        {
            index = 0;
        }

        int indexTmp = index;
        if (index >= m_BallCreatRule.Length)
        {
            indexTmp = m_BallCreatRule.Length - 1;
        }
        return m_BallCreatRule[indexTmp];
    }

    /// <summary>
    /// 设置篮球速度类型.
    /// </summary>
    public void SetLanQiuMoveSpeedType(LanQiuMoveSpeed type)
    {
        m_LanQiuMoveSpeedType = type;
    }
    
    public void TestResetPlayerLanKuang(PlayerIndex index)
    {
        StartCoroutine(TestDelayResetPlayerLanKuang(index));
    }

    public void TestResetPlayerBallSpeed(PlayerIndex index)
    {
        StartCoroutine(TestDelayResetPlayerBallSpeed(index));
    }

    /// <summary>
    /// 延迟恢复篮筐大小.
    /// </summary>
    IEnumerator TestDelayResetPlayerLanKuang(PlayerIndex index)
    {
        yield return new WaitForSeconds(5f);
        if (index != PlayerIndex.Null)
        {
            //重置篮筐大小.
            m_LanKuang[(int)index].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Normal);
        }
    }
    
    /// <summary>
    /// 延迟恢复篮球速度.
    /// </summary>
    IEnumerator TestDelayResetPlayerBallSpeed(PlayerIndex index)
    {
        yield return new WaitForSeconds(5f);
        if (index != PlayerIndex.Null)
        {
            //重置篮球速度.
            SetLanQiuMoveSpeedType(LanQiuMoveSpeed.Normal);
        }
    }
    
    /// <summary>
    /// 篮环爆炸特效列表.
    /// </summary>
    public List<GameObject>[] m_LanHuanExpList = new List<GameObject>[2];
    /// <summary>
    /// 添加篮环爆炸粒子.
    /// </summary>
    public void AddLanHuanExplosionToList(GameObject obj, PlayerIndex index)
    {
        if (obj != null && index != PlayerIndex.Null)
        {
            DestroyThisTimed desCom = obj.GetComponent<DestroyThisTimed>();
            desCom.Init(DestroyThisTimed.DestroyState.LanHuanExp);
            m_LanHuanExpList[(int)index].Add(obj);
        }
    }

    /// <summary>
    /// 删除篮环爆炸粒子.
    /// </summary>
    public void RemoveLanHuanExplosionToList(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            if (m_LanHuanExpList[i].Contains(obj))
            {
                m_LanHuanExpList[i].Remove(obj);
                break;
            }
        }
    }
    
    /// <summary>
    /// 清理篮环爆炸粒子.
    /// </summary>
    public void CleanLanHuanExplosionToList(PlayerIndex index)
    {
        if (index == PlayerIndex.Null)
        {
            return;
        }

        GameObject[] objList = m_LanHuanExpList[(int)index].ToArray();
        for (int i = 0; i < objList.Length; i++)
        {
            if (objList[i] != null)
            {
                Destroy(objList[i]);
            }
        }
        m_LanHuanExpList[(int)index].Clear();
    }

    /// <summary>
    /// 播放空心球特效动画.
    /// </summary>
    public void PlayKongXinQiuTXAni(SSKongXinQiuTXAni.TeXiaoState type, PlayerIndex index)
    {
        if (m_KongXinQiuTXArray[(int)index] != null)
        {
            m_KongXinQiuTXArray[(int)index].PlayGameTeXiaoAni(type);
        }
    }

    /// <summary>
    /// 设置游戏背景资源图信息.
    /// </summary>
    internal void SetGameBeiJingImgInfo()
    {
        for (int i = 0; i < m_KongXinQiuTXArray.Length; i++)
        {
            if (m_KongXinQiuTXArray[i] != null)
            {
                m_KongXinQiuTXArray[i].SetScreenImgInfo(m_IndexGameBeiJingImg);
            }
        }
        m_IndexGameBeiJingImg++;
    }

#if UNITY_EDITOR && TEST_SHOW_GAME_INFO
    bool IsPrintGameInfo = false;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            IsPrintGameInfo = !IsPrintGameInfo;
            //SetActivePlayer(PlayerIndex.Player01, false);
            //SetActivePlayer(PlayerIndex.Player02, false);
        }
    }

    void OnGUI()
    {
        if (IsPrintGameInfo)
        {
            if (m_CreatLanQiuStage != null)
            {
                string info = "faQiuJieDuan == " + m_CreatLanQiuStage.IndexCreatBallJieDuan;
                GUI.Box(new Rect(10f, 10f, Screen.width - 20f, 25f), info);
            }
        }
    }
#endif
}