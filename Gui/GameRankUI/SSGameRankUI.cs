using System;

public class SSGameRankUI : SSGameMono
{
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