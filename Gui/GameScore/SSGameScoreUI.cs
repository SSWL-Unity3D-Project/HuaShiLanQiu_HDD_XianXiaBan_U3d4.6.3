using UnityEngine;

public class SSGameScoreUI : SSGameMono
{
    /// <summary>
    /// 需要移动位置的UI.
    /// </summary>
    public Transform m_UIPosTr;
    /// <summary>
    /// 位置的x轴信息.
    /// </summary>
    public int[] m_UIPosX = new int[2];
    /// <summary>
    /// 得分王UI对象.
    /// </summary>
    public GameObject m_DeFenWangObj;
    /// <summary>
    /// 玩家分数坐标信息.
    /// DeFenWangPosXArray[0] -> 分数为1位数时的坐标.
    /// </summary>
    public int[] DeFenWangPosXArray = new int[4];
    /// <summary>
    /// 分数UI列表(由低位到高位进行填充).
    /// </summary>
    public UISprite[] m_ScoreNumArray = new UISprite[4];
    /// <summary>
    /// 玩家分数父级.
    /// </summary>
    public Transform PlayerScoreParent;
    /// <summary>
    /// 玩家分数坐标信息.
    /// PlayerScorePosXArray[0] -> 分数为1位数时的坐标.
    /// </summary>
    public int[] PlayerScorePosXArray = new int[4];
    bool IsRemoveSelf = false;
    internal void Init(SSGameDataCtrl.PlayerIndex indexVal)
    {
        SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexVal].m_GameScoreCom = this;
        SetActiveDeFenWang(false);
        ShowPlayerScore(SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexVal].Score);
        if (m_UIPosTr != null && m_UIPosX.Length > (int)indexVal)
        {
            //动态修改UI坐标.
            Vector3 posTmp = m_UIPosTr.localPosition;
            posTmp.x = m_UIPosX[(int)indexVal];
            m_UIPosTr.localPosition = posTmp;
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
        //UnityLog("ShowPlayerScore -> val == " + val);
        int valTmp = 0;
        string valStr = val.ToString();
        if (PlayerScoreParent != null)
        {
            //调整玩家的分数坐标.
            Vector3 posTmp = PlayerScoreParent.localPosition;
            posTmp.x = PlayerScorePosXArray[valStr.Length - 1];
            PlayerScoreParent.localPosition = posTmp;
        }

        if (m_DeFenWangObj != null)
        {
            //调整玩家的得分王UI坐标.
            Vector3 posTmpDeFenWang = m_DeFenWangObj.transform.localPosition;
            posTmpDeFenWang.x = DeFenWangPosXArray[valStr.Length - 1];
            m_DeFenWangObj.transform.localPosition = posTmpDeFenWang;
        }

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

        if (SSGameDataCtrl.GetInstance().IsActiveDeFenWangUI
            && SSGameDataCtrl.GetInstance().m_SSUIRoot.m_GameScoreCom.Length > 1)
        {
            //设置得分王UI信息.
            SSGameDataCtrl.PlayerRankData playerDt = SSGameDataCtrl.GetInstance().GetMaxScorePlayerIndex();
            if (playerDt.Score > 0)
            {
                for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
                {
                    if ((int)playerDt.Index == i)
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