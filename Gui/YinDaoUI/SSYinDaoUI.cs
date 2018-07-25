using UnityEngine;

public class SSYinDaoUI : MonoBehaviour
{
    /// <summary>
    /// 引导界面动画界面.
    /// 当退出游戏窗口显示时关闭该界面.
    /// </summary>
    public GameObject m_YinDaoPanelAniObj;
    public void SetActiveYinDaoAniObj(bool isActive)
    {
        if (m_YinDaoPanelAniObj != null)
        {
            m_YinDaoPanelAniObj.SetActive(isActive);
        }
    }
}