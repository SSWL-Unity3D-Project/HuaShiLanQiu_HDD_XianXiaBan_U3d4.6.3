
using UnityEngine;

public class SSGameModeCtrl : SSGameMono
{
    /// <summary>
    /// 模式选择动画.
    /// </summary>
    public Animator m_AniXuanZe;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    bool IsRemoveSelf = false;
    public void Init(SSGameDataCtrl.PlayerIndex index)
    {
        m_PlayerIndex = index;
        SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode = SSGameDataCtrl.GameMode.DanJi;
        SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsChooseGameMode = false;
        SetAnimationModeSelect(true);

        InputEventCtrl.GetInstance().OnClickStartBtEvent += OnClickStartBtEvent;
        InputEventCtrl.GetInstance().OnClickLeftHorBtEvent += OnClickLeftHorBtEvent;
        InputEventCtrl.GetInstance().OnClickRightHorBtEvent += OnClickRightHorBtEvent;
    }

    private void OnClickStartBtEvent(SSGameDataCtrl.PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (index != m_PlayerIndex)
        {
            return;
        }

        //SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode = SSGameDataCtrl.GameMode.DanJi; //test.
        SSGameDataCtrl.GameMode gameMode = SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode;
        UnityLog("SSGameModeCtrl::OnClickStartBtEvent -> gameMode == " + gameMode);

        if (gameMode == SSGameDataCtrl.GameMode.DanJi
            || gameMode == SSGameDataCtrl.GameMode.LianJi)
        {
            SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsChooseGameMode = true;
            SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameModeUI(index);
        }

        switch (gameMode)
        {
            case SSGameDataCtrl.GameMode.DanJi:
                {
                    //玩家选择了单机游戏模式.
                    //SSGameDataCtrl.GetInstance().InitCreateGameBall(index);
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJiShiUI(index);
                    break;
                }
            case SSGameDataCtrl.GameMode.LianJi:
                {
                    //玩家选择了联机游戏模式.
                    bool isOpenLianJiGame = true;
                    for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
                    {
                        if (SSGameDataCtrl.GetInstance().m_PlayerData[i].IsChooseGameMode)
                        {
                            //玩家已经选择了游戏模式.
                            if (SSGameDataCtrl.GetInstance().m_PlayerData[i].m_GameMode != SSGameDataCtrl.GameMode.LianJi)
                            {
                                isOpenLianJiGame = false;
                                break;
                            }
                        }
                        else
                        {
                            isOpenLianJiGame = false;
                            break;
                        }
                    }

                    if (isOpenLianJiGame)
                    {
                        //开启多人联机模式游戏.
                        UnityLog("DuoRen lianJi mode!");
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJiShiUI(SSGameDataCtrl.PlayerIndex.Null);
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLianJiWaitUI(SSGameDataCtrl.PlayerIndex.Null);
                    }
                    else
                    {
                        //产生等待界面.
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameLianJiWaitUI(index);
                    }
                    break;
                }
            case SSGameDataCtrl.GameMode.Null:
                {
                    //玩家应该选择游戏模式.
                    //UnityLogWarning("Should select gameMode!");
                    break;
                }
        }
    }

    float m_TimeLastBtEvent;
    private void OnClickLeftHorBtEvent(SSGameDataCtrl.PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (IsRemoveSelf)
        {
            return;
        }

        if (val == InputEventCtrl.ButtonState.UP)
        {
            return;
        }
        
        if (index != m_PlayerIndex)
        {
            return;
        }
        
        if (Time.time - m_TimeLastBtEvent < 0.5f)
        {
            return;
        }
        m_TimeLastBtEvent = Time.time;

        switch (SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode)
        {
            case SSGameDataCtrl.GameMode.LianJi:
                {
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode = SSGameDataCtrl.GameMode.DanJi;
                    break;
                }
            case SSGameDataCtrl.GameMode.DanJi:
                {
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode = SSGameDataCtrl.GameMode.LianJi;
                    break;
                }
        }
        SetAnimationModeSelect(true);
        UnityLog("player select " + SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode
            + " mode. index == " + index + ", val == " + val);
    }

    private void OnClickRightHorBtEvent(SSGameDataCtrl.PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (IsRemoveSelf)
        {
            return;
        }

        if (val == InputEventCtrl.ButtonState.UP)
        {
            return;
        }

        if (index != m_PlayerIndex)
        {
            return;
        }

        if (Time.time - m_TimeLastBtEvent < 0.5f)
        {
            return;
        }
        m_TimeLastBtEvent = Time.time;

        switch (SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode)
        {
            case SSGameDataCtrl.GameMode.LianJi:
                {
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode = SSGameDataCtrl.GameMode.DanJi;
                    break;
                }
            case SSGameDataCtrl.GameMode.DanJi:
                {
                    SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode = SSGameDataCtrl.GameMode.LianJi;
                    break;
                }
        }
        SetAnimationModeSelect(false);
        UnityLog("player select " + SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].m_GameMode
            + " mode. index == " + index + ", val == " + val);
    }

    void SetAnimationModeSelect(bool isLeft)
    {
        bool isDuoRen = SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].m_GameMode == SSGameDataCtrl.GameMode.LianJi ? true : false;
        if (isLeft)
        {
            if (isDuoRen)
            {
                m_AniXuanZe.SetTrigger("IsDuoRenL");
            }
            else
            {
                m_AniXuanZe.SetTrigger("IsDanRenL");
            }
        }
        else
        {
            if (isDuoRen)
            {
                m_AniXuanZe.SetTrigger("IsDuoRenR");
            }
            else
            {
                m_AniXuanZe.SetTrigger("IsDanRenR");
            }
        }
    }

    public void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;
        InputEventCtrl.GetInstance().OnClickStartBtEvent -= OnClickStartBtEvent;
        InputEventCtrl.GetInstance().OnClickLeftHorBtEvent -= OnClickLeftHorBtEvent;
        InputEventCtrl.GetInstance().OnClickRightHorBtEvent -= OnClickRightHorBtEvent;
        Destroy(gameObject);
        UnityLog("SSGameModeCtrl -> RemoveSelf...");
    }
}