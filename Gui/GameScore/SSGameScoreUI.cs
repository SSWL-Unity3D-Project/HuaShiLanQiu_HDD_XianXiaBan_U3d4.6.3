using UnityEngine;

public class SSGameScoreUI : SSGameMono
{
    /// <summary>
    /// 得分王UI对象.
    /// </summary>
    public GameObject m_DeFenWangObj;
    /// <summary>
    /// 分数UI列表(由低位到高位进行填充).
    /// </summary>
    public UISprite[] m_ScoreNumArray = new UISprite[4];
    bool IsRemoveSelf = false;
    internal void Init(SSGameDataCtrl.PlayerIndex indexVal)
    {
        SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexVal].m_GameScoreCom = this;
        SetActiveDeFenWang(false);
        ShowPlayerScore(0);
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

    /// <summary>
    /// 设置得分王的激活状态.
    /// </summary>
    public void SetActiveDeFenWang(bool isActive)
    {
        m_DeFenWangObj.SetActive(isActive);
    }

    /// <summary>
    /// 显示玩家的分数.
    /// </summary>
    public void ShowPlayerScore(int val)
    {
        UnityLog("ShowPlayerScore -> val == " + val);
        int valTmp = 0;
        string valStr = val.ToString();
        for (int i = 0; i < 4; i++)
        {
            if (m_ScoreNumArray[i] == null)
            {
                continue;
            }

            if (valStr.Length > i)
            {
                m_ScoreNumArray[i].enabled = true;
                valTmp = val % 10;
                m_ScoreNumArray[i].spriteName = valTmp.ToString();
                val = (int)(val / 10f);
            }
            else
            {
                m_ScoreNumArray[i].enabled = false;
            }
        }

        if (SSGameDataCtrl.GetInstance().IsActiveDeFenWangUI)
        {
            //设置得分王UI信息.
            SSGameDataCtrl.PlayerRankData playerDt = SSGameDataCtrl.GetInstance().GetMaxScorePlayerIndex();
            if (playerDt.Score > 0)
            {
                for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
                {
                    if (playerDt.Index != (SSGameDataCtrl.PlayerIndex)i)
                    {
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.m_GameScoreCom[i].SetActiveDeFenWang(true);
                    }
                    else
                    {
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.m_GameScoreCom[i].SetActiveDeFenWang(false);
                    }
                }
            }
        }
    }
}