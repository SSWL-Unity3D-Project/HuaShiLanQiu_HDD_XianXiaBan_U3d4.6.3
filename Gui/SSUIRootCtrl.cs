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
    /// 游戏总二维码预制.
    /// </summary>
    public GameObject m_GameErWeiMaPrefab;
    Object m_GameErWeiMa;
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
    Object[] m_PlayerErWeiMa = new Object[2];
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

    public void Init()
    {
        SpawnGamneErWeiMa();
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
            }
            else
            {
                UnityLogWarning("m_GameErWeiMaPrefab was null");
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
                m_PlayerErWeiMa[(int)index] = Instantiate(m_PlayerErWeiMaPrefab[(int)index], m_UICameraTr);
            }
        }
        else
        {
            UnityLogWarning("m_PlayerErWeiMaPrefab was null");
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
    /// 产生玩家联机等待UI界面.
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
                GameObject obj = (GameObject)Instantiate(m_GameDaoJiShiPrefab, m_UIAnchorCenter);
                m_GameDaoJiShiCom[index] = obj.GetComponent<SSGameDaoJiShi>();
                m_GameDaoJiShiCom[index].Init(indexVal);
            }
        }
        else
        {
            UnityLogWarning("m_GameDaoJiShiPrefab was null");
        }
    }

    /// <summary>
    /// 删除玩家联机等待UI界面.
    /// </summary>
    public void RemoveGameDaoJiShiUI(SSGameDataCtrl.PlayerIndex indexVal)
    {
        int index = (int)indexVal;
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            //联机模式下公用倒计时.
            index = m_GameDaoJiShiCom.Length - 1;
        }

        if (m_GameDaoJiShiCom[index] != null)
        {
            UnityLog("RemoveGameDaoJiShiUI -> index == " + indexVal);
            m_GameDaoJiShiCom[index].RemoveSelf();
            m_GameDaoJiShiCom[index] = null;
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
            SSGameDataCtrl.GetInstance().CheckCreateGameErWeiMa(indexVal);
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
            UnityLog("SpawnGamePiaoFenUI -> index == " + indexVal + ", indexScoreVal == " + indexScoreVal);
            Instantiate(scorePrefab, m_PlayerUIRoot[index]);
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
            SSGameDataCtrl.GetInstance().CheckCreateGameErWeiMa(indexVal);
        }
    }
}