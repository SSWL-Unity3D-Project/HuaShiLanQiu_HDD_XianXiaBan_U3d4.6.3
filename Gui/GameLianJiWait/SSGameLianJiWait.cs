public class SSGameLianJiWait : SSGameMono
{
    bool IsRemoveSelf = false;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    internal void Init(SSGameDataCtrl.PlayerIndex index)
    {
        m_PlayerIndex = index;
        if (index != SSGameDataCtrl.PlayerIndex.Null)
        {
            SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsActiveLianJiWaitUI = true;
        }
        InputEventCtrl.GetInstance().OnClickStartBtEvent += OnClickStartBtEvent;
    }

    private void OnClickStartBtEvent(SSGameDataCtrl.PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (IsRemoveSelf)
        {
            return;
        }

        if (index != m_PlayerIndex)
        {
            return;
        }

        UnityLog("Player click backBt! index == " + index);
        if (index != SSGameDataCtrl.PlayerIndex.Null)
        {
            SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsActiveLianJiWaitUI = false;
        }
        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLianJiWaitUI(index);
        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameModeUI(index);
    }

    internal void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;
        InputEventCtrl.GetInstance().OnClickStartBtEvent -= OnClickStartBtEvent;
        Destroy(gameObject);
    }
}