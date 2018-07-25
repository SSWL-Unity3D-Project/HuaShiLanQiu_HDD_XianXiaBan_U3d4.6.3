
using UnityEngine;

public class SSGameOverUI : SSGameMono
{
    /// <summary>
    /// 微信头像控制组件.
    /// </summary>
    public SSWeiXinHeadImg m_WeiXinHead;
    /// <summary>
    /// 玩家分数父级.
    /// </summary>
    public Transform PlayerScoreParent;
    /// <summary>
    /// 玩家分数坐标信息.
    /// PlayerScorePosXArray[0] -> 分数为1位数时的坐标.
    /// </summary>
    public int[] PlayerScorePosXArray = new int[4];
    /// <summary>
    /// 自销毁延迟时间.
    /// </summary>
    [UnityEngine.Range(1f, 10f)]
    public float m_TimeDestroy = 3f;
    /// <summary>
    /// 分数UI列表(由低位到高位进行填充).
    /// </summary>
    public UISprite[] m_ScoreNumArray = new UISprite[4];
    bool IsRemoveSelf = false;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    internal void Init(SSGameDataCtrl.PlayerIndex indexVal, int score)
    {
        m_PlayerIndex = indexVal;
        ShowPlayerScore(score);
        //添加延迟删除时间事件.
        SSTimeUpCtrl timeUpCom = gameObject.AddComponent<SSTimeUpCtrl>();
        timeUpCom.OnTimeUpOverEvent += OnTimeUpOverEvent;
        timeUpCom.Init(m_TimeDestroy);
        if (m_WeiXinHead != null)
        {
            m_WeiXinHead.Init(indexVal);
        }
    }

    private void OnTimeUpOverEvent()
    {
        UnityLog("SSGameOverUI -> OnTimeUpOverEvent...");
        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameOverUI(m_PlayerIndex);
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
    /// 显示玩家的分数.
    /// </summary>
    public void ShowPlayerScore(int val)
    {
        UnityLog("SSGameOverUI::ShowPlayerScore -> val == " + val);
        int valTmp = 0;
        string valStr = val.ToString();
        if (PlayerScoreParent != null)
        {
            //调整玩家的分数坐标.
            Vector3 posTmp = PlayerScoreParent.localPosition;
            posTmp.x = PlayerScorePosXArray[valStr.Length - 1];
            PlayerScoreParent.localPosition = posTmp;
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
    }
}