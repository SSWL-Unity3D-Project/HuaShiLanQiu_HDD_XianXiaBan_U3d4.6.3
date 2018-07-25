using UnityEngine;

public class SSGameRankUI : SSGameMono
{
    /// <summary>
    /// 微信头像控制组件.
    /// </summary>
    public SSWeiXinHeadImg[] m_WeiXinHead = new SSWeiXinHeadImg[2];
    /// <summary>
    /// 玩家1头像父级.
    /// </summary>
    public Transform P1HeadParent;
    /// <summary>
    /// 玩家1头像坐标信息.
    /// P1ScorePosArray[0] -> 分数为1位数时的坐标.
    /// </summary>
    public int[] P1HeadPosXArray = new int[4];
    /// <summary>
    /// 玩家2分数父级.
    /// </summary>
    public Transform P2ScoreParent;
    /// <summary>
    /// 玩家2分数坐标信息.
    /// P2ScorePosArray[0] -> 分数为1位数时的坐标.
    /// </summary>
    public int[] P2ScorePosXArray = new int[4];
    /// <summary>
    /// 自销毁延迟时间.
    /// </summary>
    [UnityEngine.Range(1f, 10f)]
    public float m_TimeDestroy = 3f;
    [System.Serializable]
    public class RankUIData
    {
        /// <summary>
        /// 分数UI列表(由低位到高位进行填充).
        /// </summary>
        public UISprite[] m_ScoreNumArray = new UISprite[4];
    }
    /// <summary>
    /// 排行界面UI信息.
    /// </summary>
    public RankUIData[] m_RankUIDt = new RankUIData[2];
    bool IsRemoveSelf = false;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    // Use this for initialization
    public void Init(SSGameDataCtrl.PlayerIndex indexVal)
    {
        m_PlayerIndex = indexVal;
        SSGameDataCtrl.LinkGamePlayerData[] rankDt = SSGameDataCtrl.GetInstance().GetLinkGamePlayerRankDt();
        for (int i = 0; i < rankDt.Length; i++)
        {
            ShowPlayerScore(rankDt[i].Score, i);
        }

        //添加延迟删除时间事件.
        SSTimeUpCtrl timeUpCom = gameObject.AddComponent<SSTimeUpCtrl>();
        timeUpCom.OnTimeUpOverEvent += OnTimeUpOverEvent;
        timeUpCom.Init(m_TimeDestroy);
        for (int i = 0; i < m_WeiXinHead.Length; i++)
        {
            if (m_WeiXinHead[i] != null)
            {
                //string url = SSGameDataCtrl.GetInstance().m_PlayerData[i].PlayerHeadUrl;
                //UnityLog("url ===== " + url);
                m_WeiXinHead[i].Init((SSGameDataCtrl.PlayerIndex)i);
            }
        }
    }
    
    private void OnTimeUpOverEvent()
    {
        UnityLog("SSGameRankUI -> OnTimeUpOverEvent...");
        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLinkRankUI(m_PlayerIndex);
    }

    /// <summary>
    /// 显示玩家的分数.
    /// </summary>
    public void ShowPlayerScore(int val, int index)
    {
        UnityLog("ShowPlayerScore -> val == " + val + ", index == " + index);
        int valTmp = 0;
        string valStr = val.ToString();
        
        if (index == (int)SSGameDataCtrl.PlayerIndex.Player01 && P1HeadParent != null)
        {
            //调整1P玩家的头像坐标.
            Vector3 posTmp = P1HeadParent.localPosition;
            posTmp.x = P1HeadPosXArray[valStr.Length - 1];
            P1HeadParent.localPosition = posTmp;
        }

        if (index == (int)SSGameDataCtrl.PlayerIndex.Player02 && P2ScoreParent != null)
        {
            //调整2P玩家的分数坐标.
            Vector3 posTmp = P2ScoreParent.localPosition;
            posTmp.x = P2ScorePosXArray[valStr.Length - 1];
            P2ScoreParent.localPosition = posTmp;
        }
        
        for (int i = 0; i < 4; i++)
        {
            if (m_RankUIDt[index].m_ScoreNumArray[i] == null)
            {
                continue;
            }

            if (valStr.Length > i)
            {
                m_RankUIDt[index].m_ScoreNumArray[i].enabled = true;
                valTmp = val % 10;
                m_RankUIDt[index].m_ScoreNumArray[i].spriteName = valTmp.ToString();
                val = (int)(val / 10f);
            }
            else
            {
                m_RankUIDt[index].m_ScoreNumArray[i].enabled = false;
            }
        }
    }

    internal void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;
        Destroy(gameObject);
    }
}