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
    internal void Init(SSGameDataCtrl.PlayerIndex indexVal, SSLanKuangTimeAni.DaoJiShiState daoJiShi, SSLanKuangTimeAni.DaoJuState type)
    {
        m_DaoJiShiState = daoJiShi;
        m_DaoJuType = type;
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