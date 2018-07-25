using UnityEngine;

public class SSExitGameUI : SSGameMono
{
    public Vector3 m_BigScale = new Vector3(1.2f, 1.2f, 1f);
    public Vector3 m_SmallScale = Vector3.one;
    /// <summary>
    /// 确定按键的闪烁UI.
    /// </summary>
    public GameObject m_QueDingFlashObj;
    public UITexture QueDingUI;
    /// <summary>
    /// QueDingImg[0] 确定弹起.
    /// QueDingImg[1] 确定按下.
    /// </summary>
    public Texture[] QueDingImg;
    /// <summary>
    /// 返回按键的闪烁UI.
    /// </summary>
    public GameObject m_QuXiaoFlashObj;
    public UITexture QuXiaoUI;
    /// <summary>
    /// QuXiaoImg[0] 取消弹起.
    /// QuXiaoImg[1] 取消按下.
    /// </summary>
    public Texture[] QuXiaoImg;
    public enum ExitEnum
    {
        QueDing,
        QuXiao,
    }
    ExitEnum m_ExitType = ExitEnum.QueDing;

    public void Init ()
    {
        //创建退出游戏对话框事件.
        SSGameDataCtrl.GetInstance().m_SSUIRoot.CreatExitGameUIEvent();
        m_ExitType = ExitEnum.QuXiao;
        QueDingUI.mainTexture = QueDingImg[0];
        QuXiaoUI.mainTexture = QuXiaoImg[1];
        SetAcitveBtFlash();
        QueDingUI.transform.localScale = m_SmallScale;
        QuXiaoUI.transform.localScale = m_BigScale;
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent += ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongExitBtEvent += ClickTVYaoKongExitBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent += ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent += ClickTVYaoKongRightBtEvent;
    }

    public void RemoveSelf()
    {
        //删除退出游戏对话框事件.
        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveExitGameUIEvent();
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent -= ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongExitBtEvent -= ClickTVYaoKongExitBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent -= ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent -= ClickTVYaoKongRightBtEvent;
        Destroy(gameObject);
    }
    
    private void ClickTVYaoKongLeftBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.UP)
        {
            return;
        }
        m_ExitType = ExitEnum.QuXiao;
        QueDingUI.mainTexture = QueDingImg[0];
        QuXiaoUI.mainTexture = QuXiaoImg[1];
        QueDingUI.transform.localScale = m_SmallScale;
        QuXiaoUI.transform.localScale = m_BigScale;
        SetAcitveBtFlash();
    }

    private void ClickTVYaoKongRightBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.UP)
        {
            return;
        }
        m_ExitType = ExitEnum.QueDing;
        QueDingUI.mainTexture = QueDingImg[1];
        QuXiaoUI.mainTexture = QuXiaoImg[0];
        QueDingUI.transform.localScale = m_BigScale;
        QuXiaoUI.transform.localScale = m_SmallScale;
        SetAcitveBtFlash();
    }

    void SetAcitveBtFlash()
    {
        if (m_QueDingFlashObj == null || m_QuXiaoFlashObj == null)
        {
            return;
        }

        UnityLog("SSExitGameUI::SetAcitveBtFlash -> m_ExitType == " + m_ExitType);
        switch (m_ExitType)
        {
            case ExitEnum.QueDing:
                {
                    m_QueDingFlashObj.SetActive(true);
                    m_QuXiaoFlashObj.SetActive(false);
                    break;
                }
            case ExitEnum.QuXiao:
                {
                    m_QueDingFlashObj.SetActive(false);
                    m_QuXiaoFlashObj.SetActive(true);
                    break;
                }
        }
    }

    private void ClickTVYaoKongEnterBtEvent(InputEventCtrl.ButtonState val)
    {

        if (m_ExitType == ExitEnum.QuXiao)
        {
            switch (val)
            {
                case InputEventCtrl.ButtonState.DOWN:
                    {
                        QuXiaoUI.mainTexture = QuXiaoImg[1];
                        break;
                    }
                case InputEventCtrl.ButtonState.UP:
                    {
                        QuXiaoUI.mainTexture = QuXiaoImg[0];
                        Debug.Log("Unity:" + "Player close exit game ui...");
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveExitGameDlg(m_ExitType);
                        break;
                    }
            }
        }

        if (m_ExitType == ExitEnum.QueDing)
        {
            switch (val)
            {
                case InputEventCtrl.ButtonState.DOWN:
                    {
                        QueDingUI.mainTexture = QueDingImg[1];
                        break;
                    }
                case InputEventCtrl.ButtonState.UP:
                    {
                        QueDingUI.mainTexture = QueDingImg[0];
                        Debug.Log("Unity:" + "Player exit application...");
                        SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveExitGameDlg(m_ExitType);
                        Application.Quit();
                        break;
                    }
            }
        }
    }

    private void ClickTVYaoKongExitBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        switch (val)
        {
            case InputEventCtrl.ButtonState.DOWN:
                {
                    ClickTVYaoKongLeftBtEvent(val);
                    QuXiaoUI.mainTexture = QuXiaoImg[1];
                    break;
                }
            case InputEventCtrl.ButtonState.UP:
                {
                    QuXiaoUI.mainTexture = QuXiaoImg[0];
                    Debug.Log("Unity:" + "Player close exit game ui...");
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveExitGameDlg(ExitEnum.QuXiao);
                    break;
                }
        }
    }
}