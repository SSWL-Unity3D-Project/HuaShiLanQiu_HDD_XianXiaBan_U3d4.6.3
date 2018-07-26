using UnityEngine;

public class SSGameDlgUI : SSGameMono
{
    public enum BtState
    {
        /// <summary>
        /// 同意.
        /// </summary>
        TongYi = 0,
        /// <summary>
        /// 拒绝.
        /// </summary>
        JuJue = 1,
    }
    BtState m_BtState = BtState.TongYi;

    public enum GameDlgState
    {
        Null = -1,
        /// <summary>
        /// 是否接受对方挑战.
        /// </summary>
        ShiFouJieShouTiaoZhan = 0,
        /// <summary>
        /// 是否重新始游戏(是否继续游戏).
        /// </summary>
        ShiFouChongXinKaiShi = 1,
        /// <summary>
        /// 复活道具支付界面.
        /// </summary>
        FuHuoDaoJu_ZhiFu = 2,
        /// <summary>
        /// 篮筐放大支付界面.
        /// </summary>
        LanKuangFangDa_ZhiFu = 3,
        /// <summary>
        /// 篮球减速支付界面.
        /// </summary>
        LanQiuJianSu_ZhiFu = 4,
        /// <summary>
        /// 是否接受对方挑战支付界面.
        /// </summary>
        ShiFouJieShouTiaoZhan_ZhiFu = 5,
        /// <summary>
        /// 是否重新开始游戏支付界面(是否继续游戏支付界面).
        /// </summary>
        ShiFouChongXinKaiShi_ZhiFu = 6,
    }
    GameDlgState m_GameDlgState = GameDlgState.Null;

    [System.Serializable]
    public class ZhiFuJieMianData
    {
        /// <summary>
        /// 支付云币图片.
        /// </summary>
        public UITexture ZhiFuCoinImg;
        /// <summary>
        /// 购买道具支付的视博云币图片.
        /// </summary>
        public Texture[] m_ZhiFuYunBiImgArray = new Texture[4];
    }
    /// <summary>
    /// 支付界面数据.
    /// </summary>
    public ZhiFuJieMianData m_ZhiFuJieMianData;
    
    /// <summary>
    /// 选项框.
    /// </summary>
    public Transform m_XuanXiangKuang;
    /// <summary>
    /// 选项框坐标.
    /// 该坐标列表长度等于界面的最大选项框数量.
    /// </summary>
    public Vector2[] m_XuanXiangPosArray = new Vector2[2];
    /// <summary>
    /// 默认选项框编号.
    /// </summary>
    public SSGameDataCtrl.XuanXiangState m_XuanXiangState = SSGameDataCtrl.XuanXiangState.XuanXiang02;
    /// <summary>
    /// 选项索引.
    /// </summary>
    int IndexXuanXiang = 0;
    /// <summary>
    /// 是否锁定界面.
    /// </summary>
    bool IsLockPanel = false;

    bool IsRemoveSelf = false;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex = SSGameDataCtrl.PlayerIndex.Null;
    // Use this for initialization
    public void Init(SSGameDataCtrl.PlayerIndex indexVal, GameDlgState dlgState)
    {
        m_GameDlgState = dlgState;
        m_PlayerIndex = indexVal;
        IndexXuanXiang = (int)m_XuanXiangState;
        switch (dlgState)
        {
            case GameDlgState.FuHuoDaoJu_ZhiFu:
                {
                    //初始化复活道具支付界面.
                    if (m_ZhiFuJieMianData.ZhiFuCoinImg != null)
                    {
                        int indexImg = (int)SSGameDataCtrl.GetInstance().m_SSUIRoot.m_FuHuoDaoJu.m_XuanXiangState;
                        m_ZhiFuJieMianData.ZhiFuCoinImg.mainTexture = m_ZhiFuJieMianData.m_ZhiFuYunBiImgArray[indexImg];
                    }
                    break;
                }
            case GameDlgState.LanKuangFangDa_ZhiFu:
                {
                    //初始化篮筐放大支付界面.
                    if (m_ZhiFuJieMianData.ZhiFuCoinImg != null)
                    {
                        int indexImg = (int)SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanKuangFangDa.m_XuanXiangState;
                        m_ZhiFuJieMianData.ZhiFuCoinImg.mainTexture = m_ZhiFuJieMianData.m_ZhiFuYunBiImgArray[indexImg];
                    }
                    break;
                }
            case GameDlgState.LanQiuJianSu_ZhiFu:
                {
                    //初始化篮球减速支付界面.
                    if (m_ZhiFuJieMianData.ZhiFuCoinImg != null)
                    {
                        int indexImg = (int)SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanQiuJianSu.m_XuanXiangState;
                        m_ZhiFuJieMianData.ZhiFuCoinImg.mainTexture = m_ZhiFuJieMianData.m_ZhiFuYunBiImgArray[indexImg];
                    }
                    break;
                }
        }

        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent += ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent += ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent += ClickTVYaoKongRightBtEvent;
    }

    private void ClickTVYaoKongEnterBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }

        if (IsLockPanel)
        {
            return;
        }

        UnityLog("ClickTVYaoKongEnterBtEvent -> m_GameDlgState ===== " + m_GameDlgState);
        switch (m_GameDlgState)
        {
            case GameDlgState.ShiFouJieShouTiaoZhan:
                {
                    HandleShiFouJieShouTiaoZhanEnterBtEvent();
                    break;
                }
            case GameDlgState.ShiFouJieShouTiaoZhan_ZhiFu:
                {
                    HandleShiFouJieShouTiaoZhan_ZhiFuEnterBtEvent();
                    break;
                }
            case GameDlgState.ShiFouChongXinKaiShi:
                {
                    HandleShiFouChongXinKaiShiEnterBtEvent();
                    break;
                }
            case GameDlgState.ShiFouChongXinKaiShi_ZhiFu:
                {
                    HandleShiFouChongXinKaiShi_ZhiFuEnterBtEvent();
                    break;
                }
            case GameDlgState.FuHuoDaoJu_ZhiFu:
                {
                    HandleFuHuoDaoJu_ZhiFuEnterBtEvent();
                    break;
                }
            case GameDlgState.LanKuangFangDa_ZhiFu:
                {
                    HandleLanKuangFangDa_ZhiFuEnterBtEvent();
                    break;
                }
            case GameDlgState.LanQiuJianSu_ZhiFu:
                {
                    HandleLanQiuJianSu_ZhiFuEnterBtEvent();
                    break;
                }
        }
    }

    /// <summary>
    /// 处理是否重新开始游戏支付界面按键事件(继续游戏支付界面).
    /// </summary>
    void HandleShiFouChongXinKaiShi_ZhiFuEnterBtEvent()
    {
        UnityLog("HandleShiFouChongXinKaiShi_ZhiFuEnterBtEvent -> m_BtState ==== " + m_BtState);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    //同意购买重新开始游戏道具(继续游戏界面).
                    //删除购买重新开始游戏道具界面(继续游戏界面).
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouChongXinKaiShiPanel();
                    //删除重新开始游戏道具支付界面(继续游戏支付界面).
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouChongXinKaiShi_ZhiFuPanel();
                    //激活该玩家.
                    SSGameDataCtrl.GetInstance().SetActivePlayer(m_PlayerIndex, true);
                    //恢复玩家的游戏数据.
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score = SSGameDataCtrl.GetInstance().m_LastOverPlayerData.Score;
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsTongYiJiXuGame = true;
                    //SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)m_PlayerIndex].IndexCreatBallJieDuan = SSGameDataCtrl.GetInstance().m_LastOverPlayerData.IndexCreatBallJieDuan;
                    //产生复活道具界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameFuHuoDaoJuPanel(m_PlayerIndex);
                    //删除等待对方同意PK.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDengDaiDuiFangTongYiPK();
                    break;
                }
            case BtState.JuJue:
                {
                    //拒绝购买重新开始游戏道具(继续游戏).
                    //删除重新开始游戏道具支付界面(继续游戏支付界面).
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouChongXinKaiShi_ZhiFuPanel();
                    //解锁重新开始游戏(继续游戏)道具购买界面.
                    SSGameDlgUI dlg = SSGameDataCtrl.GetInstance().m_SSUIRoot.m_SSGameDlgManage.FindGameDlgByType(GameDlgState.ShiFouChongXinKaiShi);
                    if (dlg != null)
                    {
                        dlg.SetIsLockPanel(false);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 处理是否接受挑战支付界面按键事件.
    /// </summary>
    void HandleShiFouJieShouTiaoZhan_ZhiFuEnterBtEvent()
    {
        UnityLog("HandleShiFouJieShouTiaoZhan_ZhiFuEnterBtEvent -> m_BtState ==== " + m_BtState);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    //清理最后一个结束游戏玩家的数据.
                    SSGameDataCtrl.GetInstance().CleanLastOverPlayerData();
                    //激活该玩家.
                    SSGameDataCtrl.GetInstance().SetActivePlayer(m_PlayerIndex, true);
                    //删除等待对方同意PK.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDengDaiDuiFangTongYiPK();
                    //删除对方不敢应战,请继续等待.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDuiFangBuYingZhan_JiXuDengDai();
                    //同意购买挑战对方道具.
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsJieShouTiaoZhan = true;
                    //删除购买挑战对方道具界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouJieShouTiaoZhanPanel();
                    //删除挑战对方道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouJieShouTiaoZhan_ZhiFuPanel();

                    if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].GameState == SSGameDataCtrl.PlayerGameState.YouXiQian)
                    {
                        //游戏前.
                        //产生开始双人PK提示.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnKaiShiShuangRenPK();
                    }
                    else
                    {
                        //游戏中.
                        //产生购买复活道具界面.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameFuHuoDaoJuPanel(m_PlayerIndex);
                    }
                    break;
                }
            case BtState.JuJue:
                {
                    //拒绝购买挑战对方道具.
                    //删除挑战对方道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouJieShouTiaoZhan_ZhiFuPanel();
                    //解锁挑战对方道具购买界面.
                    SSGameDlgUI dlg = SSGameDataCtrl.GetInstance().m_SSUIRoot.m_SSGameDlgManage.FindGameDlgByType(GameDlgState.ShiFouJieShouTiaoZhan);
                    if (dlg != null)
                    {
                        dlg.SetIsLockPanel(false);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 处理购买篮球减速道具支付界面按键事件.
    /// </summary>
    void HandleLanQiuJianSu_ZhiFuEnterBtEvent()
    {
        UnityLog("HandleLanQiuJianSu_ZhiFuEnterBtEvent -> m_BtState ==== " + m_BtState);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    //恢复篮筐大小.
                    SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player01].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Normal);
                    SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player02].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Normal);

                    SSLanKuangTimeAni.DaoJiShiState daoJiShi = (SSLanKuangTimeAni.DaoJiShiState)SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanQiuJianSu.m_XuanXiangState;
                    //同意购买篮球减速道具.
                    //删除购买篮球减速道具界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanQiuJianSuPanel();
                    //删除篮球减速道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanQiuJianSu_ZhiFuPanel();
                    //篮球减速.
                    SSGameDataCtrl.GetInstance().SetLanQiuMoveSpeedType(SSGameDataCtrl.LanQiuMoveSpeed.Slow);
                    //显示篮筐倒计时.
                    //SSGameDataCtrl.GetInstance().ShowPlayerLanKuangDaoJiShi(SSGameDataCtrl.PlayerIndex.Null, daoJiShi, SSLanKuangTimeAni.DaoJuState.BallSlowSpeed);
                    //显示道具启动倒计时.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJuDaoJiShiUI(m_PlayerIndex, daoJiShi, SSLanKuangTimeAni.DaoJuState.BallSlowSpeed);
                    break;
                }
            case BtState.JuJue:
                {
                    //拒绝购买篮球减速道具.
                    //删除篮球减速道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanQiuJianSu_ZhiFuPanel();
                    //解锁篮球减速道具购买界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanQiuJianSu.SetIsLockPanel(false);
                    break;
                }
        }
    }

    /// <summary>
    /// 处理购买篮筐放大道具支付界面按键事件.
    /// </summary>
    void HandleLanKuangFangDa_ZhiFuEnterBtEvent()
    {
        UnityLog("HandleLanKuangFangDa_ZhiFuEnterBtEvent -> m_BtState ==== " + m_BtState);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    //恢复篮球速度.
                    SSGameDataCtrl.GetInstance().SetLanQiuMoveSpeedType(SSGameDataCtrl.LanQiuMoveSpeed.Normal);

                    SSLanKuangTimeAni.DaoJiShiState daoJiShi = (SSLanKuangTimeAni.DaoJiShiState)SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanKuangFangDa.m_XuanXiangState;
                    //同意购买篮筐放大道具.
                    //删除购买篮筐放大道具界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanKuangFangDaPanel();
                    //删除篮筐放大道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanKuangFangDa_ZhiFuPanel();
                    //放大篮筐.
                    if (m_PlayerIndex == SSGameDataCtrl.PlayerIndex.Null)
                    {
                        SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player01].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Big);
                        SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player02].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Big);
                        //显示篮筐倒计时.
                        //SSGameDataCtrl.GetInstance().ShowPlayerLanKuangDaoJiShi(m_PlayerIndex, daoJiShi, SSLanKuangTimeAni.DaoJuState.LanKuangFangDa);
                        //显示道具启动倒计时.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJuDaoJiShiUI(m_PlayerIndex, daoJiShi, SSLanKuangTimeAni.DaoJuState.LanKuangFangDa);
                    }
                    else
                    {
                        if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsJieShouTiaoZhan)
                        {
                            SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player01].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Big);
                            SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player02].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Big);
                            //显示篮筐倒计时.
                            //SSGameDataCtrl.GetInstance().ShowPlayerLanKuangDaoJiShi(SSGameDataCtrl.PlayerIndex.Null, daoJiShi, SSLanKuangTimeAni.DaoJuState.LanKuangFangDa);
                            //显示道具启动倒计时.
                            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJuDaoJiShiUI(SSGameDataCtrl.PlayerIndex.Null, daoJiShi, SSLanKuangTimeAni.DaoJuState.LanKuangFangDa);
                        }
                        else
                        {
                            SSGameDataCtrl.GetInstance().m_LanKuang[(int)m_PlayerIndex].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Big);
                            //显示篮筐倒计时.
                            //SSGameDataCtrl.GetInstance().ShowPlayerLanKuangDaoJiShi(m_PlayerIndex, daoJiShi, SSLanKuangTimeAni.DaoJuState.LanKuangFangDa);
                            //显示道具启动倒计时.
                            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJuDaoJiShiUI(m_PlayerIndex, daoJiShi, SSLanKuangTimeAni.DaoJuState.LanKuangFangDa);
                        }
                    }
                    break;
                }
            case BtState.JuJue:
                {
                    //拒绝购买篮筐放大道具.
                    //删除篮筐放大道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanKuangFangDa_ZhiFuPanel();
                    //解锁篮筐放大道具购买界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanKuangFangDa.SetIsLockPanel(false);
                    break;
                }
        }
    }

    /// <summary>
    /// 处理购买复活道具支付界面按键事件.
    /// </summary>
    void HandleFuHuoDaoJu_ZhiFuEnterBtEvent()
    {
        UnityLog("HandleFuHuoDaoJu_ZhiFuBtEvent -> m_BtState ==== " + m_BtState);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    FuHuoDaoJu.FuHuoCiShuState xuanXiangType = (FuHuoDaoJu.FuHuoCiShuState)SSGameDataCtrl.GetInstance().m_SSUIRoot.m_FuHuoDaoJu.m_XuanXiangState;
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.HandlePlayerBuyFuHuoDaoJuInfo(xuanXiangType);

                    //同意购买复活道具.
                    //删除复活道具垢面界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameFuHuoDaoJuPanel(m_PlayerIndex);
                    //删除复活道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameFuHuoDaoJu_ZhiFuPanel();
                    break;
                }
            case BtState.JuJue:
                {
                    //拒绝购买复活道具.
                    //删除复活道具支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameFuHuoDaoJu_ZhiFuPanel();
                    //解锁复活道具购买界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.m_FuHuoDaoJu.SetIsLockPanel(false);
                    break;
                }
        }
    }

    /// <summary>
    /// 处理是否接受挑战界面按键事件.
    /// </summary>
    void HandleShiFouJieShouTiaoZhanEnterBtEvent()
    {
        UnityLog("HandleShiFouJieShouTiaoZhanBtEvent -> m_BtState ==== " + m_BtState);
        //删除等待对方同意PK.
        //SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDengDaiDuiFangTongYiPK();
        SSGameDataCtrl.PlayerIndex indexPlayerReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(m_PlayerIndex);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    //接受挑战.
                    SetIsLockPanel(true);
                    //产生接受挑战支付界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameShiFouJieShouTiaoZhan_ZhiFuPanel(m_PlayerIndex);
                    break;
                }
            case BtState.JuJue:
                {
                    //删除等待对方同意PK.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDengDaiDuiFangTongYiPK();
                    //拒绝挑战.
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsJieShouTiaoZhan = false;
                    if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].GameState == SSGameDataCtrl.PlayerGameState.YouXiQian)
                    {
                        //游戏前.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnDuiFangBuYingZhan_DengDuiFangJieShu(indexPlayerReverse);
                        //显示游戏倒计时界面.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJiShiUI(m_PlayerIndex);
                        //删除购买挑战对方道具界面.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouJieShouTiaoZhanPanel();
                    }
                    else if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].GameState == SSGameDataCtrl.PlayerGameState.YouXiZhong)
                    {
                        //游戏中.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnDuiFangBuYingZhan_JiXuDengDai(indexPlayerReverse);
                        //是否重新开始游戏(是否继续游戏).
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameShiFouChongXinKaiShiPanel(m_PlayerIndex);
                        //删除购买挑战对方道具界面.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouJieShouTiaoZhanPanel();
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 处理是否重新开始游戏界面(是否继续游戏界面)按键事件.
    /// </summary>
    void HandleShiFouChongXinKaiShiEnterBtEvent()
    {
        UnityLog("HandleShiFouChongXinKaiShiBtEvent -> m_BtState ==== " + m_BtState);
        SSGameDataCtrl.PlayerIndex indexPlayerReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(m_PlayerIndex);
        switch (m_BtState)
        {
            case BtState.TongYi:
                {
                    //同意重新开始游戏(继续游戏).
                    SetIsLockPanel(true);
                    //产生重新开始游戏的支付界面(继续游戏支付界面).
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameShiFouChongXinKaiShi_ZhiFuPanel(m_PlayerIndex);
                    break;
                }
            case BtState.JuJue:
                {
                    //重置该玩家的微信头像url信息.
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].PlayerHeadUrl = "";
                    //刷新pcvr玩家信息.
                    pcvr.GetInstance().SetIndexPlayerActiveGameState((int)m_PlayerIndex, (byte)pcvr.PlayerActiveState.WeiJiHuo);
                    //清理最后一个结束游戏玩家的数据.
                    SSGameDataCtrl.GetInstance().CleanLastOverPlayerData();
                    //拒绝重新开始游戏(继续游戏).
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameShiFouChongXinKaiShiPanel();
                    if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexPlayerReverse].IsActiveGame)
                    {
                        //对方激活游戏.
                        //删除对方不应战,继续等待UI.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDuiFangBuYingZhan_JiXuDengDai();
                        //删除等待对方本场游戏结束UI.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveDengDuiFangJieShu();
                        //对方已离开,切换为单人模式.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnDuiFangYiLiKai_QieWeiDanRen(indexPlayerReverse);
                        //提示购买复活币.
                        //SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameFuHuoDaoJuPanel(indexPlayerReverse);
                        //产生二维码界面.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnPlayerErWeiMa(m_PlayerIndex);
                    }
                    else
                    {
                        //对方未激活游戏.
                        SSGameDataCtrl.GetInstance().CheckCreateGameErWeiMa(m_PlayerIndex);
                    }
                    //玩家回到游戏前.
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].GameState = SSGameDataCtrl.PlayerGameState.YouXiQian;
                    break;
                }
        }
    }

    private void ClickTVYaoKongLeftBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }
        SetXuanXiangKuangPos(SSGameDataCtrl.XuanXiangMoveDir.Left);
    }

    private void ClickTVYaoKongRightBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }
        SetXuanXiangKuangPos(SSGameDataCtrl.XuanXiangMoveDir.Right);
    }
    
    void SetXuanXiangKuangPos(SSGameDataCtrl.XuanXiangMoveDir dir)
    {
        if (IsLockPanel || m_XuanXiangKuang == null)
        {
            return;
        }

        int offsetVal = dir == SSGameDataCtrl.XuanXiangMoveDir.Right ? 1 : -1;
        IndexXuanXiang += offsetVal;
        if (IndexXuanXiang < (int)SSGameDataCtrl.XuanXiangState.XuanXiang01)
        {
            //到达右极限.
            IndexXuanXiang = m_XuanXiangPosArray.Length - 1;
        }

        if (IndexXuanXiang >= m_XuanXiangPosArray.Length)
        {
            //到达左极限.
            IndexXuanXiang = (int)SSGameDataCtrl.XuanXiangState.XuanXiang01;
        }
        m_XuanXiangState = (SSGameDataCtrl.XuanXiangState)IndexXuanXiang;
        m_XuanXiangKuang.localPosition = m_XuanXiangPosArray[IndexXuanXiang];

        switch (m_GameDlgState)
        {
            case GameDlgState.Null:
                {
                    break;
                }
            default:
                {
                    //对话框界面只有2个按键的.
                    if (m_XuanXiangState == SSGameDataCtrl.XuanXiangState.XuanXiang01)
                    {
                        //拒绝按键.
                        m_BtState = BtState.JuJue;
                    }
                    else if (m_XuanXiangState == SSGameDataCtrl.XuanXiangState.XuanXiang02)
                    {
                        //同意按键.
                        m_BtState = BtState.TongYi;
                    }
                    break;
                }
        }
        UnityLog("SetXuanXiangKuangPos -> dir == " + dir + ", m_XuanXiangState == " + m_XuanXiangState
            + ", GameDlgState == " + m_GameDlgState + ", BtState == " + m_BtState);
    }

    /// <summary>
    /// 设置界面锁定状态.
    /// </summary>
    public void SetIsLockPanel(bool isLock)
    {
        IsLockPanel = isLock;
        gameObject.SetActive(!isLock);
    }

    public void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;

        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent -= ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent -= ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent -= ClickTVYaoKongRightBtEvent;
        Destroy(gameObject);
    }
}