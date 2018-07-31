using UnityEngine;

public class SSGameDaoJuDaoJiShi : SSGameMono
{
    /// <summary>
    /// 倒计时UI.
    /// </summary>
    public GameObject[] m_DaoJiShiUI;
    bool IsRemoveSelf = false;
    SSLanKuangTimeAni.DaoJiShiState m_DaoJiShiState;
    SSLanKuangTimeAni.DaoJuState m_DaoJuType;
    internal void Init(SSLanKuangTimeAni.DaoJiShiState daoJiShi, SSLanKuangTimeAni.DaoJuState type)
    {
        m_DaoJiShiState = daoJiShi;
        m_DaoJuType = type;
        bool isActive = false;
        for (int i = 0; i < m_DaoJiShiUI.Length; i++)
        {
            if (m_DaoJiShiUI[i] != null)
            {
                if (SSGameDataCtrl.GetInstance().m_PlayerData[i].IsActiveGame
                    && SSGameDataCtrl.GetInstance().m_PlayerData[i].IsCreateGameBall)
                {
                    isActive = true;
                }
                else
                {
                    isActive = false;
                }
                m_DaoJiShiUI[i].SetActive(isActive);
            }
        }
    }

    internal void RemoveSelf()
    {
        //UnityLog("SSGameDaoJuDaoJiShi -> RemoveSelf, m_PlayerIndex == " + m_PlayerIndex);
        Destroy(gameObject);
        //显示篮筐倒计时.
        SSGameDataCtrl.GetInstance().ShowPlayerLanKuangDaoJiShi(SSGameDataCtrl.PlayerIndex.Null, m_DaoJiShiState, m_DaoJuType);
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
        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameDaoJuDaoJiShiUI();
    }
}