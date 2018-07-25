using UnityEngine;

public class SSGameDaoJiShi : SSGameMono
{
    /// <summary>
    /// 倒计时UI.
    /// </summary>
    public GameObject[] m_DaoJiShiUI;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    bool IsRemoveSelf = false;

    internal void Init(SSGameDataCtrl.PlayerIndex indexVal)
    {
        m_PlayerIndex = indexVal;
        if (indexVal == SSGameDataCtrl.PlayerIndex.Null)
        {
            SSGameDataCtrl.GetInstance().IsActiveDeFenWangUI = true;
            SSGameDataCtrl.GetInstance().IsActiveGameLinkRankUI = true;
            for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
            {
                SSGameDataCtrl.GetInstance().m_PlayerData[i].IsJieShouTiaoZhan = true;
                SSGameDataCtrl.GetInstance().m_PlayerData[i].IsPlayGameDaoJiShi = true;
                SSGameDataCtrl.GetInstance().m_LanKuang[i].SetActiveRealBallKuang(true);
            }
        }
        else
        {
            SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexVal].IsPlayGameDaoJiShi = true;
            SSGameDataCtrl.GetInstance().m_LanKuang[(int)indexVal].SetActiveRealBallKuang(true);
        }

        switch (indexVal)
        {
            case SSGameDataCtrl.PlayerIndex.Player01:
                {
                    m_DaoJiShiUI[(int)SSGameDataCtrl.PlayerIndex.Player02].SetActive(false);
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Player02:
                {
                    m_DaoJiShiUI[(int)SSGameDataCtrl.PlayerIndex.Player01].SetActive(false);
                    break;
                }
        }
        //设置篮球速度为正常.
        SSGameDataCtrl.GetInstance().SetLanQiuMoveSpeedType(SSGameDataCtrl.LanQiuMoveSpeed.Normal);
        //产生复活次数UI.
        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameFuHuoCiShuPanel(indexVal);


        if (SSGameDataCtrl.GetInstance().m_LastOverPlayerData != null
            && SSGameDataCtrl.GetInstance().m_LastOverPlayerData.Index == m_PlayerIndex)
        {
            //显示继续游戏玩家的分数信息.
            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameScoreUI(m_PlayerIndex);
        }
    }

    internal void RemoveSelf()
    {
        //UnityLog("SSGameDaoJiShi -> RemoveSelf, m_PlayerIndex == " + m_PlayerIndex);
        switch (m_PlayerIndex)
        {
            case SSGameDataCtrl.PlayerIndex.Null:
                {
                    for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
                    {
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameScoreUI((SSGameDataCtrl.PlayerIndex)i);
                        SSGameDataCtrl.GetInstance().InitCreateGameBall((SSGameDataCtrl.PlayerIndex)i);
                    }
                    break;
                }
            default:
                {
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameScoreUI(m_PlayerIndex);
                    SSGameDataCtrl.GetInstance().InitCreateGameBall(m_PlayerIndex);
                    break;
                }
        }
        SSGameDataCtrl.GetInstance().m_CreatLanQiuStage.Init();
        Destroy(gameObject);
    }

    /// <summary>
    /// 动画结束事件.
    /// </summary>
    public override void OnEndAnimationTrigger()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;
        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameDaoJiShiUI(m_PlayerIndex);
        SSGameDataCtrl.GetInstance().m_AudioData.PlayGameBeiJingAudio();
        SSGameDataCtrl.GetInstance().m_CreatLanQiuStage.CreatBallJieDuanTimeUp();
    }
}