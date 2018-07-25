using UnityEngine;

/// <summary>
/// 复活次数UI管理.
/// </summary>
public class FuHuoCiShuManage : SSGameMono
{
    /// <summary>
    /// 玩家血值UI.
    /// </summary>
    public GameObject[] m_PlayerHealthObj = new GameObject[2];
    /// <summary>
    /// 玩家1血值.
    /// </summary>
    public UISprite[] m_HealthNumP1 = new UISprite[2];
    /// <summary>
    /// 玩家2血值.
    /// </summary>
    public UISprite[] m_HealthNumP2 = new UISprite[2];
    public Animator[] m_FuHuoCiShuAni = new Animator[2];
    bool IsRemoveSelf = false;
    // Use this for initialization
    public void Init(SSGameDataCtrl.PlayerIndex index)
    {
        switch (index)
        {
            case SSGameDataCtrl.PlayerIndex.Player01:
            case SSGameDataCtrl.PlayerIndex.Player02:
                {
                    int health = SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].PlayerHealth;
                    SSGameDataCtrl.PlayerIndex playerIndexReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(index);
                    SetActivePlayerHealth(playerIndexReverse, false);
                    SetActivePlayerHealth(index, true);
                    ShowPlayerHealth(health, index);
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Null:
                {
                    int healthP1 = SSGameDataCtrl.GetInstance().m_PlayerData[(int)SSGameDataCtrl.PlayerIndex.Player01].PlayerHealth;
                    int healthP2 = SSGameDataCtrl.GetInstance().m_PlayerData[(int)SSGameDataCtrl.PlayerIndex.Player02].PlayerHealth;
                    SetActivePlayerHealth(SSGameDataCtrl.PlayerIndex.Player01, true);
                    SetActivePlayerHealth(SSGameDataCtrl.PlayerIndex.Player02, true);
                    ShowPlayerHealth(healthP1, SSGameDataCtrl.PlayerIndex.Player01);
                    ShowPlayerHealth(healthP2, SSGameDataCtrl.PlayerIndex.Player02);
                    break;
                }
        }
    }

    /// <summary>
    /// 显示玩家复活次数信息.
    /// </summary>
    public void ShowPlayerHealth(int health, SSGameDataCtrl.PlayerIndex index)
    {
        UISprite[] healthUI = null;
        switch (index)
        {
            case SSGameDataCtrl.PlayerIndex.Player01:
                {
                    healthUI = m_HealthNumP1;
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Player02:
                {
                    healthUI = m_HealthNumP2;
                    break;
                }
        }

        UnityLog("ShowPlayerHealth -> health == " + health + ", index == " + index);
        int valTmp = 0;
        string valStr = health.ToString();
        for (int i = 0; i < 2; i++)
        {
            if (healthUI[i] == null)
            {
                continue;
            }

            if (valStr.Length > i)
            {
                healthUI[i].enabled = true;
                valTmp = health % 10;
                healthUI[i].spriteName = valTmp.ToString();
                health = (int)(health / 10f);
            }
            else
            {
                healthUI[i].spriteName = "0";
            }
        }
    }

    /// <summary>
    /// 播放玩家接球失误时的动画.
    /// </summary>
    public void PlayFuHuoCiShuAni(SSGameDataCtrl.PlayerIndex index)
    {
        if (index == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        if (m_FuHuoCiShuAni[(int)index] != null)
        {
            m_FuHuoCiShuAni[(int)index].SetTrigger("IsPlay");
        }
    }

    /// <summary>
    /// 设置玩家血值UI是否可见.
    /// </summary>
    public void SetActivePlayerHealth(SSGameDataCtrl.PlayerIndex index, bool isActive)
    {
        if (m_PlayerHealthObj[(int)index] != null)
        {
            m_PlayerHealthObj[(int)index].SetActive(isActive);
        }
    }

    public void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;
        Destroy(gameObject);
    }
}