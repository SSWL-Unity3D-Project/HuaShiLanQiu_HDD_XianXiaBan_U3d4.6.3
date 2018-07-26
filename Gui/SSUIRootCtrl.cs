using System;
using System.Collections;
using UnityEngine;

public class SSUIRootCtrl : SSGameMono
{
    /// <summary>
    /// UI摄像机.
    /// </summary>
    public Transform m_UICameraTr;
    /// <summary>
    /// UI中心锚点.
    /// </summary>
    public Transform m_UIAnchorCenter;
    /// <summary>
    /// 玩家UI中心坐标.
    /// </summary>
    Transform[] m_UICenterTrArray = new Transform[2];
    /// <summary>
    /// 对话框UI中心锚点.
    /// </summary>
    Transform m_DlgUICenterRoot;
    /// <summary>
    /// 引导界面.
    /// </summary>
    internal SSYinDaoUI m_SSYinDaoUI;
    /// <summary>
    /// 游戏总二维码预制.
    /// </summary>
    public GameObject m_GameErWeiMaPrefab;
    [HideInInspector]
    public UnityEngine.Object m_GameErWeiMa;
    /// <summary>
    /// 玩家UIRoot.
    /// </summary>
    public Transform[] m_PlayerUIRoot;
    /// <summary>
    /// 玩家游戏二维码预制.
    /// </summary>
    public GameObject[] m_PlayerErWeiMaPrefab;
    /// <summary>
    /// 玩家游戏二维码.
    /// </summary>
    UnityEngine.Object[] m_PlayerErWeiMa = new UnityEngine.Object[2];
    /// <summary>
    /// 玩家游戏模式选择UI预制.
    /// </summary>
    public GameObject m_GameModePrefab;
    /// <summary>
    /// 玩家模式选择UI.
    /// </summary>
    SSGameModeCtrl[] m_GameModeCom = new SSGameModeCtrl[2];
    /// <summary>
    /// 玩家游戏联机等待界面UI预制.
    /// </summary>
    public GameObject m_GameLianJiWaitPrefab;
    /// <summary>
    /// 玩家联机等待界面UI.
    /// </summary>
    SSGameLianJiWait[] m_GameLianJiWaitCom = new SSGameLianJiWait[2];
    /// <summary>
    /// 游戏倒计时界面UI预制.
    /// </summary>
    public GameObject m_GameDaoJiShiPrefab;
    /// <summary>
    /// 游戏倒计时界面UI.
    /// </summary>
    SSGameDaoJiShi[] m_GameDaoJiShiCom = new SSGameDaoJiShi[3];
    /// <summary>
    /// 游戏分数界面UI预制.
    /// </summary>
    public GameObject m_GameScorePrefab;
    /// <summary>
    /// 游戏分数界面UI.
    /// </summary>
    [HideInInspector]
    public SSGameScoreUI[] m_GameScoreCom = new SSGameScoreUI[2];
    /// <summary>
    /// 游戏结束界面UI预制.
    /// </summary>
    public GameObject m_GameOverPrefab;
    /// <summary>
    /// 游戏结束界面UI.
    /// </summary>
    [HideInInspector]
    public SSGameOverUI[] m_GameOverCom = new SSGameOverUI[2];
    /// <summary>
    /// 游戏飘分界面UI预制.
    /// m_GameScoreAyPrefab[0] 1.
    /// m_GameScoreAyPrefab[1] 2.
    /// m_GameScoreAyPrefab[2] 4.
    /// m_GameScoreAyPrefab[3] 8.
    /// </summary>
    public GameObject[] m_GameScoreAyPrefab;
    /// <summary>
    /// 游戏结束比分排行界面UI预制.
    /// </summary>
    public GameObject m_GameLinkRankUIPrefab;
    /// <summary>
    /// 游戏结束比分排行界面UI.
    /// </summary>
    [HideInInspector]
    public SSGameRankUI m_GameLinkRankUICom;
    /// <summary>
    /// 游戏文本消息UI管理.
    /// </summary>
    SSGameTextManage m_GameTextManage;
    /// <summary>
    /// 游戏对话框UI管理.
    /// </summary>
    [HideInInspector]
    public SSGameDlgManage m_SSGameDlgManage;
    /// <summary>
    /// 初始化.
    /// </summary>
    public void Init()
    {
        CreatUICenterTrArray();
        CreatDlgUICenterRoot();
        if (m_GameTextManage == null)
        {
            m_GameTextManage = gameObject.AddComponent<SSGameTextManage>();
        }

        if (m_SSGameDlgManage == null)
        {
            m_SSGameDlgManage = gameObject.AddComponent<SSGameDlgManage>();
        }
        SpawnGamneErWeiMa();
        //SpawnGameLanKuangFangDaDingBuPanel(); //test.
    }
    
    /// <summary>
    /// 创建玩家UI中心坐标父级.
    /// </summary>
    void CreatUICenterTrArray()
    {
        GameObject obj = null;
        for (int i = 0; i < m_UICenterTrArray.Length; i++)
        {
            obj = new GameObject("UICenP" + (i + 1));
            obj.transform.SetParent(m_UIAnchorCenter);
            switch (i)
            {
                case 0:
                    {
                        obj.transform.localPosition = new Vector3(-320f, 0f, 0f);
                        break;
                    }
                case 1:
                    {
                        obj.transform.localPosition = new Vector3(320f, 0f, 0f);
                        break;
                    }
            }
            obj.transform.localScale = Vector3.one;
            m_UICenterTrArray[i] = obj.transform;
        }
    }

    /// <summary>
    /// 创建对话框UI中心锚点.
    /// </summary>
    void CreatDlgUICenterRoot()
    {
        if (m_DlgUICenterRoot == null)
        {
            m_DlgUICenterRoot = new GameObject("DlgUICenterRoot").transform;
            m_DlgUICenterRoot.SetParent(m_UIAnchorCenter, false);
        }
    }

    /// <summary>
    /// 产生游戏总二维码UI.
    /// </summary>
    public void SpawnGamneErWeiMa()
    {
        if (m_GameErWeiMa == null)
        {
            if (m_GameErWeiMaPrefab != null)
            {
                m_GameErWeiMa = Instantiate(m_GameErWeiMaPrefab, m_UICameraTr);
                m_SSYinDaoUI = ((GameObject)m_GameErWeiMa).GetComponent<SSYinDaoUI>();
            }
            else
            {
                UnityLogWarning("m_GameErWeiMaPrefab was null");
            }
        }

        if (SSGameDataCtrl.GetInstance().m_LanKuang != null)
        {
            //隐藏所有玩家的篮筐.
            int max = SSGameDataCtrl.GetInstance().m_LanKuang.Length;
            if (max > 0)
            {
                for (int i = 0; i < max; i++)
                {
                    if (SSGameDataCtrl.GetInstance().m_LanKuang[i] != null)
                    {
                        SSGameDataCtrl.GetInstance().m_LanKuang[i].SetActiveRealBallKuang(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 删除游戏总二维码UI.
    /// </summary>
    public void RemoveGamneErWeiMa()
    {
        if (m_GameErWeiMa != null)
        {
            Destroy(m_GameErWeiMa);
        }
    }

    /// <summary>
    /// 产生玩家二维码UI.
    /// </summary>
    public void SpawnPlayerErWeiMa(SSGameDataCtrl.PlayerIndex index)
    {
        if (m_PlayerErWeiMaPrefab[(int)index] != null)
        {
            if (m_PlayerErWeiMa[(int)index] == null)
            {
                UnityLog("SpawnPlayerErWeiMa -> index == " + index);
                m_PlayerErWeiMa[(int)index] = Instantiate(m_PlayerErWeiMaPrefab[(int)index], m_DlgUICenterRoot);
                GameObject obj = (GameObject)m_PlayerErWeiMa[(int)index];
                SSPlayerErWeiMa playerErWeiMa = obj.GetComponent<SSPlayerErWeiMa>();
                if (playerErWeiMa != null)
                {
                    playerErWeiMa.Init();
                }
                else
                {
                    UnityLogWarning("playerErWeiMa was null!!!");
                }
            }
        }
        else
        {
            UnityLogWarning("m_PlayerErWeiMaPrefab was null");
        }

        if (SSGameDataCtrl.GetInstance().m_LanKuang != null)
        {
            //隐藏玩家的篮筐.
            int max = SSGameDataCtrl.GetInstance().m_LanKuang.Length;
            if (max > (int)index)
            {
                if (SSGameDataCtrl.GetInstance().m_LanKuang[(int)index] != null)
                {
                    SSGameDataCtrl.GetInstance().m_LanKuang[(int)index].SetActiveRealBallKuang(false);
                }
            }
        }
    }

    /// <summary>
    /// 删除玩家二维码UI.
    /// </summary>
    public void RemovePlayerErWeiMa(SSGameDataCtrl.PlayerIndex index)
    {
        if (m_PlayerErWeiMa[(int)index] != null)
        {
            UnityLog("RemovePlayerErWeiMa -> index == " + index);
            Destroy(m_PlayerErWeiMa[(int)index]);
            m_PlayerErWeiMa[(int)index] = null;
        }
    }
    
    /// <summary>
    /// 产生玩家游戏模式选择UI.
    /// </summary>
    public void SpawnGameModeUI(SSGameDataCtrl.PlayerIndex index)
    {
        if (m_GameModePrefab != null)
        {
            if (m_GameModeCom[(int)index] == null)
            {
                UnityLog("SpawnGameModeUI -> index == " + index);
                GameObject obj = (GameObject)Instantiate(m_GameModePrefab, m_PlayerUIRoot[(int)index]);
                m_GameModeCom[(int)index] = obj.GetComponent<SSGameModeCtrl>();
                m_GameModeCom[(int)index].Init(index);
            }
        }
        else
        {
            UnityLogWarning("m_GameModePrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家游戏模式选择UI.
    /// </summary>
    public void RemoveGameModeUI(SSGameDataCtrl.PlayerIndex index)
    {
        if (m_GameModeCom[(int)index] != null)
        {
            UnityLog("RemoveGameModeUI -> index == " + index);
            m_GameModeCom[(int)index].RemoveSelf();
            m_GameModeCom[(int)index] = null;
        }
    }
    
    /// <summary>
    /// 产生玩家联机等待UI界面.
    /// </summary>
    public void SpawnGameLianJiWaitUI(SSGameDataCtrl.PlayerIndex index)
    {
        if (m_GameLianJiWaitPrefab != null)
        {
            if (m_GameLianJiWaitCom[(int)index] == null)
            {
                UnityLog("SpawnGameLianJiWaitUI -> index == " + index);
                GameObject obj = (GameObject)Instantiate(m_GameLianJiWaitPrefab, m_PlayerUIRoot[(int)index]);
                m_GameLianJiWaitCom[(int)index] = obj.GetComponent<SSGameLianJiWait>();
                m_GameLianJiWaitCom[(int)index].Init(index);
            }
        }
        else
        {
            UnityLogWarning("m_GameLianJiWaitPrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家联机等待UI界面.
    /// </summary>
    public void RemoveGameLianJiWaitUI(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            for (int i = 0; i < m_GameLianJiWaitCom.Length; i++)
            {
                if (m_GameLianJiWaitCom[i] != null)
                {
                    UnityLog("*** RemoveGameLianJiWaitUI -> index == " + (SSGameDataCtrl.PlayerIndex)i);
                    m_GameLianJiWaitCom[i].RemoveSelf();
                    m_GameLianJiWaitCom[i] = null;
                }
            }
        }
        else
        {
            if (m_GameLianJiWaitCom[(int)index] != null)
            {
                UnityLog("RemoveGameLianJiWaitUI -> index == " + index);
                m_GameLianJiWaitCom[(int)index].RemoveSelf();
                m_GameLianJiWaitCom[(int)index] = null;
            }
        }
    }

    /// <summary>
    /// 是否在播放游戏倒计时.
    /// </summary>
    [HideInInspector]
    public bool IsPlayGameDaoJiShi = false;
    /// <summary>
    /// 产生玩家游戏倒计时UI界面.
    /// </summary>
    public void SpawnGameDaoJiShiUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        int index = (int)indexVal;
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            //联机模式下公用倒计时.
            index = m_GameDaoJiShiCom.Length - 1;
        }

        if (m_GameDaoJiShiPrefab != null)
        {
            if (m_GameDaoJiShiCom[index] == null)
            {
                UnityLog("SpawnGameDaoJiShiUI -> index == " + indexVal);
                //停止引导界面音乐.
                SSGameDataCtrl.GetInstance().m_AudioData.StopYinDaoAudio();
                GameObject obj = (GameObject)Instantiate(m_GameDaoJiShiPrefab, m_DlgUICenterRoot);
                m_GameDaoJiShiCom[index] = obj.GetComponent<SSGameDaoJiShi>();
                m_GameDaoJiShiCom[index].Init(indexVal);
                IsPlayGameDaoJiShi = true;
            }
        }
        else
        {
            UnityLogWarning("m_GameDaoJiShiPrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家游戏倒计时UI界面.
    /// </summary>
    public void RemoveGameDaoJiShiUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        int index = (int)indexVal;
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            //联机模式下公用倒计时.
            index = m_GameDaoJiShiCom.Length - 1;
        }
        else
        {
            RemoveDuiFangBuYingZhan_DengDuiFangJieShu();
            SSGameDataCtrl.PlayerIndex playerReverseIndex = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(indexVal);
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)playerReverseIndex].IsActiveGame)
            {
                //显示请等待对方本场游戏结束UI.
                SpawnDengDuiFangJieShu(playerReverseIndex);
            }
        }

        if (m_GameDaoJiShiCom[index] != null)
        {
            UnityLog("RemoveGameDaoJiShiUI -> index == " + indexVal);
            m_GameDaoJiShiCom[index].RemoveSelf();
            m_GameDaoJiShiCom[index] = null;
        }
        IsPlayGameDaoJiShi = false;
    }

    internal SSGameDaoJuDaoJiShi m_SSGameDaoJuDaoJiShi;
    /// <summary>
    /// 产生玩家购买游戏道具后的倒计时UI界面.
    /// </summary>
    public void SpawnGameDaoJuDaoJiShiUI(SSGameDataCtrl.PlayerIndex indexVal, SSLanKuangTimeAni.DaoJiShiState daoJiShi, SSLanKuangTimeAni.DaoJuState type)
    {
        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/GameDaoJuDaoJiShi/GameDaoJuDaoJiShiUI");
        if (gmDataPrefab != null)
        {
            if (m_SSGameDaoJuDaoJiShi == null)
            {
                UnityLog("SpawnGameDaoJuDaoJiShiUI -> index == " + indexVal);
                GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_DlgUICenterRoot);
                m_SSGameDaoJuDaoJiShi = obj.GetComponent<SSGameDaoJuDaoJiShi>();
                m_SSGameDaoJuDaoJiShi.Init(indexVal, daoJiShi, type);
            }
        }
        else
        {
            UnityLogWarning("SpawnGameDaoJuDaoJiShiUI -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家购买游戏道具后的倒计时UI界面.
    /// </summary>
    public void RemoveGameDaoJuDaoJiShiUI()
    {
        if (m_SSGameDaoJuDaoJiShi != null)
        {
            UnityLog("RemoveGameDaoJuDaoJiShiUI");
            m_SSGameDaoJuDaoJiShi.RemoveSelf();
            m_SSGameDaoJuDaoJiShi = null;
        }
    }
    
    /// <summary>
    /// 产生玩家分数UI界面.
    /// </summary>
    public void SpawnGameScoreUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        int index = (int)indexVal;
        if (m_GameScorePrefab != null)
        {
            if (m_GameScoreCom[index] == null)
            {
                UnityLog("SpawnGameScoreUI -> index == " + indexVal);
                GameObject obj = (GameObject)Instantiate(m_GameScorePrefab, m_PlayerUIRoot[index]);
                m_GameScoreCom[index] = obj.GetComponent<SSGameScoreUI>();
                m_GameScoreCom[index].Init(indexVal);
            }
        }
        else
        {
            UnityLogWarning("m_GameScorePrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家分数UI界面.
    /// </summary>
    public void RemoveGameScoreUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        int index = (int)indexVal;
        if (m_GameScoreCom[index] != null)
        {
            UnityLog("RemoveGameScoreUI -> index == " + indexVal);
            m_GameScoreCom[index].RemoveSelf();
            m_GameScoreCom[index] = null;
        }
    }

    /// <summary>
    /// 产生游戏结束UI界面.
    /// </summary>
    public void SpawnGameOverUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        int index = (int)indexVal;
        if (m_GameOverPrefab != null)
        {
            if (m_GameOverCom[index] == null)
            {
                UnityLog("SpawnGameOverUI -> index == " + indexVal);
                GameObject obj = (GameObject)Instantiate(m_GameOverPrefab, m_PlayerUIRoot[index]);
                m_GameOverCom[index] = obj.GetComponent<SSGameOverUI>();
                m_GameOverCom[index].Init(indexVal, SSGameDataCtrl.GetInstance().m_PlayerData[index].Score);
            }
        }
        else
        {
            UnityLogWarning("m_GameOverPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏结束UI界面.
    /// </summary>
    public void RemoveGameOverUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        int index = (int)indexVal;
        if (m_GameOverCom[index] != null)
        {
            UnityLog("RemoveGameOverUI -> index == " + indexVal);
            m_GameOverCom[index].RemoveSelf();
            m_GameOverCom[index] = null;

            SSGameDataCtrl.PlayerIndex indexReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(indexVal);
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexReverse].IsActiveGame)
            {
                if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexReverse].IsCreateGameBall)
                {
                    //对方开始发球了(游戏中).
                    SSGameDataCtrl.GetInstance().CheckCreateGameErWeiMa(indexVal);
                }
                else
                {
                    //对方还在激活游戏中.
                    //对方没有开始发球.
                    //是否接受对方挑战.
                    SpawnGameShiFouJieShouTiaoZhanPanel(indexVal);
                }
            }
            else
            {
                //对方没有玩家.
                SpawnGameShiFouChongXinKaiShiPanel(indexVal);
            }
        }
    }
    
    /// <summary>
    /// 产生游戏飘分UI界面.
    /// </summary>
    public void SpawnGamePiaoFenUI(SSGameDataCtrl.PlayerIndex indexVal, int scoreVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        int index = (int)indexVal;
        int indexScoreVal = 0;
        switch (scoreVal)
        {
            case 1:
                {
                    indexScoreVal = 0;
                    break;
                }
            case 2:
                {
                    indexScoreVal = 1;
                    break;
                }
            case 4:
                {
                    indexScoreVal = 2;
                    break;
                }
            case 8:
                {
                    indexScoreVal = 3;
                    break;
                }
        }

        GameObject scorePrefab = m_GameScoreAyPrefab[indexScoreVal];
        if (scorePrefab != null)
        {
            //UnityLog("SpawnGamePiaoFenUI -> index == " + indexVal + ", indexScoreVal == " + indexScoreVal);
            Instantiate(scorePrefab, m_UICenterTrArray[index]);
        }
        else
        {
            UnityLogWarning("scorePrefab was null, indexScoreVal == " + indexScoreVal);
        }
    }
    
    /// <summary>
    /// 产生联机游戏结束排行UI界面.
    /// </summary>
    public void SpawnGameLinkRankUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        int index = (int)indexVal;
        if (m_GameLinkRankUIPrefab != null)
        {
            UnityLog("SpawnGameLinkRankUI -> index == " + indexVal);
            GameObject obj = (GameObject)Instantiate(m_GameLinkRankUIPrefab, m_PlayerUIRoot[index]);
            m_GameLinkRankUICom = obj.GetComponent<SSGameRankUI>();
            m_GameLinkRankUICom.Init(indexVal);
        }
        else
        {
            UnityLogWarning("m_GameLinkRankUIPrefab was null");
        }
    }

    /// <summary>
    /// 删除联机游戏结束排行UI界面.
    /// </summary>
    public void RemoveGameLinkRankUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        
        if (m_GameLinkRankUICom != null)
        {
            UnityLog("RemoveGameLinkRankUI...");
            m_GameLinkRankUICom.RemoveSelf();
            m_GameLinkRankUICom = null;
            
            SSGameDataCtrl.PlayerIndex indexReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(indexVal);
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexReverse].IsActiveGame)
            {
                if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexReverse].IsCreateGameBall)
                {
                    //对方开始发球了(游戏中).
                    SSGameDataCtrl.GetInstance().CheckCreateGameErWeiMa(indexVal);
                }
                else
                {
                    //对方还在激活游戏中.
                    //对方没有开始发球.
                    //是否接受对方挑战.
                    SpawnGameShiFouJieShouTiaoZhanPanel(indexVal);
                }
            }
            else
            {
                //对方没有玩家.
                SpawnGameShiFouChongXinKaiShiPanel(indexVal);
            }
        }
    }
    
    /// <summary>
    /// 游戏复活道具界面UI.
    /// </summary>
    [HideInInspector]
    public FuHuoDaoJu m_FuHuoDaoJu;
    /// <summary>
    /// 产生游戏复活道具界面UI.
    /// </summary>
    public void SpawnGameFuHuoDaoJuPanel(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        if (m_FuHuoDaoJu != null)
        {
            return;
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/FuHuoDaoJu/FuHuoDaoJu");
        int index = (int)indexVal;
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameFuHuoDaoJuPanel -> index == " + indexVal);
            Transform parent = m_PlayerUIRoot[index];
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, parent);
            m_FuHuoDaoJu = obj.GetComponent<FuHuoDaoJu>();
            m_FuHuoDaoJu.Init(indexVal);
        }
        else
        {
            UnityLogWarning("SpawnGameFuHuoDaoJuPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏复活道具界面UI
    /// </summary>
    public void RemoveGameFuHuoDaoJuPanel(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        if (m_FuHuoDaoJu != null)
        {
            UnityLog("RemoveGameFuHuoDaoJuPanel...");
            m_FuHuoDaoJu.RemoveSelf();
            m_FuHuoDaoJu = null;
        }
    }

    /// <summary>
    /// 处理玩家购买复活道具的信息.
    /// </summary>
    public void HandlePlayerBuyFuHuoDaoJuInfo(FuHuoDaoJu.FuHuoCiShuState type)
    {
        int health = 0;
        switch (type)
        {
            case FuHuoDaoJu.FuHuoCiShuState.Num03:
                {
                    health = 3;
                    break;
                }
            case FuHuoDaoJu.FuHuoCiShuState.Num05:
                {
                    health = 5;
                    break;
                }
            case FuHuoDaoJu.FuHuoCiShuState.Num07:
                {
                    health = 7;
                    break;
                }
            case FuHuoDaoJu.FuHuoCiShuState.Num09:
                {
                    health = 9;
                    break;
                }
        }

        //health += 1;
        SSGameDataCtrl.GetInstance().m_PlayerData[(int)SSGameDataCtrl.PlayerIndex.Player01].PlayerHealth = health;
        SSGameDataCtrl.GetInstance().m_PlayerData[(int)SSGameDataCtrl.PlayerIndex.Player02].PlayerHealth = health;
    }
    
    /// <summary>
    /// 产生复活道具支付界面.
    /// </summary>
    public void SpawnGameFuHuoDaoJu_ZhiFuPanel(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        Transform parent = m_PlayerUIRoot[(int)index];
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.FuHuoDaoJu_ZhiFu, index, parent);
    }

    /// <summary>
    /// 删除复活道具支付界面.
    /// </summary>
    public void RemoveGameFuHuoDaoJu_ZhiFuPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.FuHuoDaoJu_ZhiFu);
    }


    /// <summary>
    /// 游戏复活次数界面UI.
    /// </summary>
    [HideInInspector]
    public FuHuoCiShuManage m_FuHuoCiShu;
    /// <summary>
    /// 产生游戏复活次数界面UI.
    /// </summary>
    public void SpawnGameFuHuoCiShuPanel(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (m_FuHuoCiShu != null)
        {
            //用原先复活次数UI进行初始化.
            m_FuHuoCiShu.Init(indexVal);
            return;
            //清理复活次数UI.
            //RemoveGameFuHuoCiShuPanel();
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/FuHuoCiShu/FuHuoCiShu");
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameFuHuoCiShuPanel -> index == " + indexVal);
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_DlgUICenterRoot);
            m_FuHuoCiShu = obj.GetComponent<FuHuoCiShuManage>();
            m_FuHuoCiShu.Init(indexVal);
        }
        else
        {
            UnityLogWarning("SpawnGameFuHuoCiShuPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏复活次数界面UI
    /// </summary>
    public void RemoveGameFuHuoCiShuPanel()
    {
        if (m_FuHuoCiShu != null)
        {
            UnityLog("RemoveGameFuHuoCiShuPanel...");
            m_FuHuoCiShu.RemoveSelf();
            m_FuHuoCiShu = null;
        }
    }

    /// <summary>
    /// 游戏篮筐放大界面UI.
    /// </summary>
    [HideInInspector]
    public LanKuangFangDa m_LanKuangFangDa;
    /// <summary>
    /// 产生游戏篮筐放大界面UI.
    /// </summary>
    public void SpawnGameLanKuangFangDaPanel(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (m_LanKuangFangDa != null)
        {
            return;
        }
        SetActiveLanKuangFangDaDingBu(false);
        SetActiveLanQiuJianSuDingBu(false);

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/LanKuangFangDa/LanKuangFangDa");
        int index = (int)indexVal;
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameLanKuangFangDaPanel -> index == " + indexVal);
            GameObject obj = null;
            if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
            {
                obj = (GameObject)Instantiate(gmDataPrefab, m_DlgUICenterRoot);
            }
            else
            {
                Transform parent = null;
                if (SSGameDataCtrl.GetInstance().m_PlayerData[index].IsJieShouTiaoZhan)
                {
                    parent = m_DlgUICenterRoot;
                }
                else
                {
                    parent = m_PlayerUIRoot[index];
                }
                obj = (GameObject)Instantiate(gmDataPrefab, parent);
            }

            m_LanKuangFangDa = obj.GetComponent<LanKuangFangDa>();
            m_LanKuangFangDa.Init(indexVal);
            //世界静止,清理篮球.
            SSGameDataCtrl.GetInstance().IsPauseGame = true;
        }
        else
        {
            UnityLogWarning("SpawnGameLanKuangFangDaPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏篮筐放大界面UI
    /// </summary>
    public void RemoveGameLanKuangFangDaPanel()
    {
        if (m_LanKuangFangDa != null)
        {
            UnityLog("RemoveGameLanKuangFangDaPanel...");
            m_LanKuangFangDa.RemoveSelf();
            m_LanKuangFangDa = null;
        }
    }
    
    /// <summary>
    /// 游戏篮筐放大顶部界面UI.
    /// </summary>
    [HideInInspector]
    public LanKuangFangDaDingBu m_LanKuangFangDaDingBu;
    /// <summary>
    /// 产生游戏篮筐放大顶部界面UI.
    /// </summary>
    public void SpawnGameLanKuangFangDaDingBuPanel()
    {
        if (m_LanKuangFangDaDingBu != null)
        {
            return;
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/LanKuangFangDa/LanKuangFangDa_DingBu");
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameLanKuangFangDaDingBuPanel...");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_UIAnchorCenter);
            m_LanKuangFangDaDingBu = obj.GetComponent<LanKuangFangDaDingBu>();
            m_LanKuangFangDaDingBu.Init();
            CreatGameDaoJuTimeEvent();
        }
        else
        {
            UnityLogWarning("SpawnGameLanKuangFangDaDingBuPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏篮筐放大顶部界面UI
    /// </summary>
    public void RemoveGameLanKuangFangDaDingBuPanel()
    {
        if (m_LanKuangFangDaDingBu != null)
        {
            UnityLog("RemoveGameLanKuangFangDaDingBuPanel...");
            m_LanKuangFangDaDingBu.RemoveSelf();
            m_LanKuangFangDaDingBu = null;
        }
    }

    /// <summary>
    /// 设置篮筐放大顶部UI的显示或隐藏.
    /// </summary>
    internal void SetActiveLanKuangFangDaDingBu(bool isActive)
    {
        if (m_LanKuangFangDaDingBu != null)
        {
            UnityLog("SetActiveLanKuangFangDaDingBu -> isActive == " + isActive);
            m_LanKuangFangDaDingBu.gameObject.SetActive(isActive);
            if (isActive)
            {
                m_DaoJuState = DaoJuState.LanKuangFangDa;
            }
        }
    }

    /// <summary>
    /// 购买的道具状态.
    /// </summary>
    public enum DaoJuState
    {
        /// <summary>
        /// 篮球放大.
        /// </summary>
        LanKuangFangDa = 0,
        /// <summary>
        /// 篮球减速.
        /// </summary>
        LanQiuJianSu = 1,
    }
    [HideInInspector]
    public DaoJuState m_DaoJuState = DaoJuState.LanKuangFangDa;
    SSTimeUpCtrl m_DaoJuTimeUpCom;
    /// <summary>
    /// 是否关闭顶部道具购买提示UI.
    /// </summary>
    [HideInInspector]
    public bool IsCloseDaoJuBuyDingBu = false;
    /// <summary>
    /// 创建篮球时间节点时间组件.
    /// </summary>
    public void CreatGameDaoJuTimeEvent()
    {
        if (m_DaoJuTimeUpCom != null)
        {
            RemoveDaoJuTimeUpCom();
        }
        Debug.Log("CreatGameDaoJuTimeEvent -> time " + Time.time);
        m_DaoJuTimeUpCom = gameObject.AddComponent<SSTimeUpCtrl>();
        m_DaoJuTimeUpCom.Init(4f);
        m_DaoJuTimeUpCom.OnTimeUpOverEvent += OnCreatGameDaoJuTimeEvent;
    }

    /// <summary>
    /// 当顶部UI事件响应.
    /// </summary>
    private void OnCreatGameDaoJuTimeEvent()
    {
        if (SSGameDataCtrl.GetInstance().m_LianKuangTimeAni != null)
        {
            if (!SSGameDataCtrl.GetInstance().m_LianKuangTimeAni.IsPlayTimeAni
                && m_LanKuangFangDa == null
                && m_LanQiuJianSu == null)
            {
                if (IsCloseDaoJuBuyDingBu)
                {
                    IsCloseDaoJuBuyDingBu = false;
                    SetActiveLanKuangFangDaDingBu(false);
                    SetActiveLanQiuJianSuDingBu(false);
                    m_DaoJuState = DaoJuState.LanKuangFangDa;
                }
                else
                {
                    switch (m_DaoJuState)
                    {
                        case DaoJuState.LanKuangFangDa:
                            {
                                if (m_LanQiuJianSuDingBu != null)
                                {
                                    if (m_LanQiuJianSuDingBu.gameObject.activeInHierarchy
                                        || m_LanKuangFangDaDingBu.gameObject.activeInHierarchy)
                                    {
                                        SetActiveLanKuangFangDaDingBu(false);
                                        SetActiveLanQiuJianSuDingBu(true);
                                        IsCloseDaoJuBuyDingBu = true;
                                    }
                                    else
                                    {
                                        SetActiveLanKuangFangDaDingBu(true);
                                    }
                                }
                                else
                                {
                                    if (m_LanKuangFangDaDingBu == null)
                                    {
                                        //创建篮球放大道具购买顶部提示.
                                        SpawnGameLanKuangFangDaDingBuPanel();
                                        SetActiveLanKuangFangDaDingBu(true);
                                    }

                                    if (m_LanQiuJianSuDingBu == null)
                                    {
                                        if (SSGameDataCtrl.GetInstance().IsCreatLanQiuJianSuDaoJuBuy)
                                        {
                                            //创建篮球减速道具购买顶部提示.
                                            SpawnGameLanQiuJianSuDingBuPanel();
                                        }
                                    }
                                }
                                break;
                            }
                        case DaoJuState.LanQiuJianSu:
                            {
                                if (m_LanKuangFangDaDingBu != null)
                                {
                                    if (m_LanQiuJianSuDingBu.gameObject.activeInHierarchy
                                        || m_LanKuangFangDaDingBu.gameObject.activeInHierarchy)
                                    {
                                        SetActiveLanKuangFangDaDingBu(true);
                                        SetActiveLanQiuJianSuDingBu(false);
                                    }
                                    else
                                    {
                                        SetActiveLanQiuJianSuDingBu(true);
                                        IsCloseDaoJuBuyDingBu = true;
                                    }
                                }
                                else
                                {
                                    if (m_LanQiuJianSuDingBu == null)
                                    {
                                        SpawnGameLanQiuJianSuDingBuPanel();
                                        SetActiveLanQiuJianSuDingBu(true);
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            //else
            //{
            //    SetActiveLanKuangFangDaDingBu(false);
            //    SetActiveLanQiuJianSuDingBu(false);
            //}
        }
        CreatGameDaoJuTimeEvent();
    }

    /// <summary>
    /// 删除道具购买顶部UI事件组件.
    /// </summary>
    public void RemoveDaoJuTimeUpCom()
    {
        if (m_DaoJuTimeUpCom != null)
        {
            Destroy(m_DaoJuTimeUpCom);
        }
    }

    /// <summary>
    /// 产生篮筐放大道具支付界面.
    /// </summary>
    public void SpawnGameLanKuangFangDa_ZhiFuPanel(SSGameDataCtrl.PlayerIndex index)
    {
        Transform parent = null;
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            parent = m_DlgUICenterRoot;
        }
        else
        {
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsJieShouTiaoZhan)
            {
                parent = m_DlgUICenterRoot;
            }
            else
            {
                parent = m_PlayerUIRoot[(int)index];
            }
        }
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.LanKuangFangDa_ZhiFu, index, parent);
    }

    /// <summary>
    /// 删除篮筐放大道具支付界面.
    /// </summary>
    public void RemoveGameLanKuangFangDa_ZhiFuPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.LanKuangFangDa_ZhiFu);
    }

    /// <summary>
    /// 游戏篮球减速界面UI.
    /// </summary>
    [HideInInspector]
    public LanQiuJianSu m_LanQiuJianSu;
    /// <summary>
    /// 产生游戏篮球减速界面UI.
    /// </summary>
    public void SpawnGameLanQiuJianSuPanel(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (m_LanQiuJianSu != null)
        {
            return;
        }
        SetActiveLanKuangFangDaDingBu(false);
        SetActiveLanQiuJianSuDingBu(false);

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/LanQiuJianSu/LanQiuJianSu");
        int index = (int)indexVal;
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameLanQiuJianSuPanel -> index == " + indexVal);
            GameObject obj = null;
            if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
            {
                obj = (GameObject)Instantiate(gmDataPrefab, m_DlgUICenterRoot);
            }
            else
            {
                Transform parent = null;
                if (SSGameDataCtrl.GetInstance().m_PlayerData[index].IsJieShouTiaoZhan)
                {
                    parent = m_DlgUICenterRoot;
                }
                else
                {
                    parent = m_PlayerUIRoot[index];
                }
                obj = (GameObject)Instantiate(gmDataPrefab, parent);
            }

            m_LanQiuJianSu = obj.GetComponent<LanQiuJianSu>();
            m_LanQiuJianSu.Init(indexVal);
            //世界静止,清理篮球.
            SSGameDataCtrl.GetInstance().IsPauseGame = true;
        }
        else
        {
            UnityLogWarning("SpawnGameLanQiuJianSuPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏篮球减速界面UI
    /// </summary>
    public void RemoveGameLanQiuJianSuPanel()
    {
        if (m_LanQiuJianSu != null)
        {
            UnityLog("RemoveGameLanQiuJianSuPanel...");
            m_LanQiuJianSu.RemoveSelf();
            m_LanQiuJianSu = null;
        }
    }

    /// <summary>
    /// 游戏篮球减速顶部界面UI.
    /// </summary>
    [HideInInspector]
    public LanQiuJianSuDingBu m_LanQiuJianSuDingBu;
    /// <summary>
    /// 产生游戏篮球减速顶部界面UI.
    /// </summary>
    public void SpawnGameLanQiuJianSuDingBuPanel()
    {
        if (m_LanQiuJianSuDingBu != null)
        {
            return;
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/LanQiuJianSu/LanQiuJianSu_DingBu");
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameLanQiuJianSuDingBuPanel...");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_UIAnchorCenter);
            m_LanQiuJianSuDingBu = obj.GetComponent<LanQiuJianSuDingBu>();
            m_LanQiuJianSuDingBu.Init();
        }
        else
        {
            UnityLogWarning("SpawnGameLanQiuJianSuDingBuPanel -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除游戏篮球减速顶部界面UI
    /// </summary>
    public void RemoveGameLanQiuJianSuDingBuPanel()
    {
        if (m_LanQiuJianSuDingBu != null)
        {
            UnityLog("RemoveGameLanQiuJianSuDingBuPanel...");
            m_LanQiuJianSuDingBu.RemoveSelf();
            m_LanQiuJianSuDingBu = null;
        }
    }

    /// <summary>
    /// 设置篮球减速顶部UI的显示或隐藏.
    /// </summary>
    internal void SetActiveLanQiuJianSuDingBu(bool isActive)
    {
        if (m_LanQiuJianSuDingBu != null)
        {
            UnityLog("SetActiveLanQiuJianSuDingBu -> isActive == " + isActive);
            m_LanQiuJianSuDingBu.gameObject.SetActive(isActive);
            if (isActive)
            {
                m_DaoJuState = DaoJuState.LanQiuJianSu;
            }
        }
    }
    
    /// <summary>
    /// 产生篮球减速道具支付界面.
    /// </summary>
    public void SpawnGameLanQiuJianSu_ZhiFuPanel(SSGameDataCtrl.PlayerIndex index)
    {
        Transform parent = null;
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            parent = m_DlgUICenterRoot;
        }
        else
        {
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsJieShouTiaoZhan)
            {
                parent = m_DlgUICenterRoot;
            }
            else
            {
                parent = m_PlayerUIRoot[(int)index];
            }
        }
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.LanQiuJianSu_ZhiFu, index, parent);
    }

    /// <summary>
    /// 删除篮球减速道具支付界面.
    /// </summary>
    public void RemoveGameLanQiuJianSu_ZhiFuPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.LanQiuJianSu_ZhiFu);
    }

    /// <summary>
    /// 产生是否接受对方挑战界面UI.
    /// </summary>
    public void SpawnGameShiFouJieShouTiaoZhanPanel(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        bool isCreatDengDuiFangTongYiPK = false;
        if (m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.DengDuiFangJieShu) != null)
        {
            //删除等对方结束本场游戏.
            isCreatDengDuiFangTongYiPK = true;
            RemoveDengDuiFangJieShu();
        }

        if (m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.DuiFangBuYingZhan_JiXuDengDai) != null)
        {
            //删除对方不敢应战,请继续等待.
            isCreatDengDuiFangTongYiPK = true;
            RemoveDuiFangBuYingZhan_JiXuDengDai();
        }

        if (isCreatDengDuiFangTongYiPK)
        {
            //创建等待对方同意PK
            SSGameDataCtrl.PlayerIndex indexReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(index);
            SpawnDengDaiDuiFangTongYiPK(indexReverse);
        }
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouJieShouTiaoZhan, index, m_PlayerUIRoot[(int)index]);
    }

    /// <summary>
    /// 删除是否接受对方挑战界面UI.
    /// </summary>
    public void RemoveGameShiFouJieShouTiaoZhanPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouJieShouTiaoZhan);
    }
    
    /// <summary>
    /// 产生是否接受对方挑战支付界面UI.
    /// </summary>
    public void SpawnGameShiFouJieShouTiaoZhan_ZhiFuPanel(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouJieShouTiaoZhan_ZhiFu, index, m_PlayerUIRoot[(int)index]);
    }

    /// <summary>
    /// 删除是否接受对方挑战支付界面UI.
    /// </summary>
    public void RemoveGameShiFouJieShouTiaoZhan_ZhiFuPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouJieShouTiaoZhan_ZhiFu);
    }

    /// <summary>
    /// 产生是否重新开始游戏(是否继续游戏界面).
    /// </summary>
    public void SpawnGameShiFouChongXinKaiShiPanel(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi, index, m_PlayerUIRoot[(int)index]);
    }

    /// <summary>
    /// 删除是否重新开始游戏(是否继续游戏界面).
    /// </summary>
    public void RemoveGameShiFouChongXinKaiShiPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi);
    }

    /// <summary>
    /// 产生是否重新开始游戏的支付界面UI(是否继续游戏支付界面).
    /// </summary>
    public void SpawnGameShiFouChongXinKaiShi_ZhiFuPanel(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        m_SSGameDlgManage.SpawnGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi_ZhiFu, index, m_PlayerUIRoot[(int)index]);
    }

    /// <summary>
    /// 删除是否重新开始游戏的支付界面UI(是否继续游戏支付界面).
    /// </summary>
    public void RemoveGameShiFouChongXinKaiShi_ZhiFuPanel()
    {
        m_SSGameDlgManage.RemoveGameDlgUI(SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi_ZhiFu);
    }

    /// <summary>
    /// 产生"等待对方同意PK"
    /// </summary>
    internal void SpawnDengDaiDuiFangTongYiPK(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        m_GameTextManage.SpawnGameTextUI(SSGameTextManage.GameTextState.DengDuiFangTongYiPK, m_PlayerUIRoot[(int)indexVal]);
    }

    /// <summary>
    /// 删除"等待对方同意PK"
    /// </summary>
    internal void RemoveDengDaiDuiFangTongYiPK()
    {
        m_GameTextManage.RemoveGameTextUI(SSGameTextManage.GameTextState.DengDuiFangTongYiPK);
    }
    
    /// <summary>
    /// 产生"开始双人PK"
    /// </summary>
    internal void SpawnKaiShiShuangRenPK()
    {
        SSGameTextManage.TextDate textDt = m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.KaiShiShuangRenPK);
        if (textDt != null)
        {
            UnityLogWarning("SpawnKaiShiShuangRenPK -> The KaiShiShuangRenPKUI have been created!");
            return;
        }

        if (m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.DuiFangBuYingZhan_JiXuDengDai) != null)
        {
            //删除"对方不敢应战,请继续等待".
            RemoveDuiFangBuYingZhan_JiXuDengDai();
        }

        if (m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.DengDuiFangJieShu) != null)
        {
            //删除"请等待对方本场游戏结束".
            RemoveDengDuiFangJieShu();
        }
        m_GameTextManage.SpawnGameTextUI(SSGameTextManage.GameTextState.KaiShiShuangRenPK, m_UIAnchorCenter);
        StartCoroutine(RemoveKaiShiShuangRenPK());
    }

    /// <summary>
    /// 删除"开始双人PK"
    /// </summary>
    IEnumerator RemoveKaiShiShuangRenPK()
    {
        yield return new WaitForSeconds(3f);
        m_GameTextManage.RemoveGameTextUI(SSGameTextManage.GameTextState.KaiShiShuangRenPK);
        //显示游戏倒计时界面.
        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJiShiUI(SSGameDataCtrl.PlayerIndex.Null);
    }
    
    /// <summary>
    /// 产生"对方不应战,等对方本场游戏结束"
    /// </summary>
    internal void SpawnDuiFangBuYingZhan_DengDuiFangJieShu(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        m_GameTextManage.SpawnGameTextUI(SSGameTextManage.GameTextState.DuiFangBuYingZhan_DengDuiFangJieShu, m_PlayerUIRoot[(int)indexVal]);
    }

    /// <summary>
    /// 删除"对方不应战,等对方本场游戏结束"
    /// </summary>
    internal void RemoveDuiFangBuYingZhan_DengDuiFangJieShu()
    {
        m_GameTextManage.RemoveGameTextUI(SSGameTextManage.GameTextState.DuiFangBuYingZhan_DengDuiFangJieShu);
    }
    
    /// <summary>
    /// 产生"等对方本场游戏结束"
    /// </summary>
    internal void SpawnDengDuiFangJieShu(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        if (m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.DuiFangBuYingZhan_JiXuDengDai) == null
            && m_GameTextManage.FindGameTextUIByType(SSGameTextManage.GameTextState.DengDuiFangJieShu) == null)
        {
            //没有"对方不敢应战,请继续等待"提示时,才允许显示"请等待对方本场游戏结束"信息.
            m_GameTextManage.SpawnGameTextUI(SSGameTextManage.GameTextState.DengDuiFangJieShu, m_PlayerUIRoot[(int)indexVal]);
        }
    }

    /// <summary>
    /// 删除"等对方本场游戏结束"
    /// </summary>
    internal void RemoveDengDuiFangJieShu()
    {
        m_GameTextManage.RemoveGameTextUI(SSGameTextManage.GameTextState.DengDuiFangJieShu);
    }

    /// <summary>
    /// 产生"对方不敢应战,请继续等待"
    /// </summary>
    internal void SpawnDuiFangBuYingZhan_JiXuDengDai(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        RemoveDengDuiFangJieShu();
        m_GameTextManage.SpawnGameTextUI(SSGameTextManage.GameTextState.DuiFangBuYingZhan_JiXuDengDai, m_PlayerUIRoot[(int)indexVal]);
    }

    /// <summary>
    /// 删除"对方不敢应战,请继续等待"
    /// </summary>
    internal void RemoveDuiFangBuYingZhan_JiXuDengDai()
    {
        m_GameTextManage.RemoveGameTextUI(SSGameTextManage.GameTextState.DuiFangBuYingZhan_JiXuDengDai);
    }

    /// <summary>
    /// 产生"对方已离开,切换为单人模式"
    /// </summary>
    internal void SpawnDuiFangYiLiKai_QieWeiDanRen(SSGameDataCtrl.PlayerIndex indexVal)
    {
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }
        m_GameTextManage.SpawnGameTextUI(SSGameTextManage.GameTextState.DuiFangYiLiKai_QieWeiDanRen, m_PlayerUIRoot[(int)indexVal]);
        StartCoroutine(RemoveDuiFangYiLiKai_QieWeiDanRen(indexVal));
    }

    /// <summary>
    /// 删除"对方已离开,切换为单人模式"
    /// </summary>
    internal IEnumerator RemoveDuiFangYiLiKai_QieWeiDanRen(SSGameDataCtrl.PlayerIndex indexVal)
    {
        yield return new WaitForSeconds(3f);
        m_GameTextManage.RemoveGameTextUI(SSGameTextManage.GameTextState.DuiFangYiLiKai_QieWeiDanRen);

        //提示购买复活币.
        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameFuHuoDaoJuPanel(indexVal);
    }

    /// <summary>
    /// 设置所有UI界面的显示或隐藏状态.
    /// </summary>
    void SetActiveAllUIPanel(bool isActive)
    {
        m_DlgUICenterRoot.gameObject.SetActive(isActive);
        for (int i = 0; i < m_PlayerUIRoot.Length; i++)
        {
            m_PlayerUIRoot[i].gameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// 是否退出游戏窗口.
    /// </summary>
    [HideInInspector]
    public SSExitGameUI m_ExitGameUI;
    /// <summary>
    /// 产生"是否退出游戏"窗口.
    /// </summary>
    internal void SpawnExitGameDlg()
    {
        if (m_ExitGameUI != null)
        {
            return;
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load("Prefabs/GUI/ExitGame/ExitGameUI");
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnExitGameDlg...");
            //隐藏UI界面.
            SetActiveAllUIPanel(false);
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_UIAnchorCenter);
            m_ExitGameUI = obj.GetComponent<SSExitGameUI>();
            m_ExitGameUI.Init();
            if (m_SSYinDaoUI != null)
            {
                m_SSYinDaoUI.SetActiveYinDaoAniObj(false);
            }
        }
        else
        {
            UnityLogWarning("SpawnExitGameDlg -> gmDataPrefab was null");
        }
    }

    /// <summary>
    /// 删除"是否退出游戏"窗口.
    /// </summary>
    internal void RemoveExitGameDlg(SSExitGameUI.ExitEnum type)
    {
        if (m_ExitGameUI != null)
        {
            if (m_SSYinDaoUI != null)
            {
                m_SSYinDaoUI.SetActiveYinDaoAniObj(true);
            }

            m_ExitGameUI.RemoveSelf();
            m_ExitGameUI = null;
            if (type == SSExitGameUI.ExitEnum.QuXiao)
            {
                //显示UI界面.
                SetActiveAllUIPanel(true);
            }
        }
    }

    public delegate void EventHandel();
    /// <summary>
    /// 创建退出游戏对话框事件.
    /// </summary>
    public event EventHandel OnCreatExitGameUIEvent;
    public void CreatExitGameUIEvent()
    {
        if (OnCreatExitGameUIEvent != null)
        {
            OnCreatExitGameUIEvent();
        }
    }

    /// <summary>
    /// 删除退出游戏对话框事件.
    /// </summary>
    public event EventHandel OnRemoveExitGameUIEvent;
    public void RemoveExitGameUIEvent()
    {
        if (OnRemoveExitGameUIEvent != null)
        {
            OnRemoveExitGameUIEvent();
        }
    }
}