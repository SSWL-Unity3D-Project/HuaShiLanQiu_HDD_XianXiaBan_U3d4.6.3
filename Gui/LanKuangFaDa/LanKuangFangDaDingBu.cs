public class LanKuangFangDaDingBu : SSGameMono
{
    bool IsRemoveSelf = false;
    internal void Init()
    {
        gameObject.SetActive(false);
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent += ClickTVYaoKongEnterBtEvent;
    }

    private void ClickTVYaoKongEnterBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }

        if (!gameObject.activeSelf)
        {
            return;
        }

        UnityLog("LanKuangFangDaDingBu -> ClickTVYaoKongEnterBtEvent...");
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanKuangFangDa == null)
        {
            SSGameDataCtrl.PlayerIndex index = SSGameDataCtrl.GetInstance().m_CreatLanQiuStage.GetPlayerIndexDaoJu();
            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameLanKuangFangDaPanel(index);
        }
    }

    internal void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent -= ClickTVYaoKongEnterBtEvent;
        Destroy(gameObject);
    }
}