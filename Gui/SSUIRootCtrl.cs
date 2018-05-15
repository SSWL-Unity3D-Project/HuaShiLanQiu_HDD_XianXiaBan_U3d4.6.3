using UnityEngine;

public class SSUIRootCtrl : SSGameMono
{
    /// <summary>
    /// UI中心锚点.
    /// </summary>
    public Transform m_UIAnchorCenter;
    /// <summary>
    /// 玩家UIRoot.
    /// </summary>
    public Transform[] m_PlayerUIRoot;
    /// <summary>
    /// 游戏总二维码预制.
    /// </summary>
    public GameObject m_GameErWeiMaPrefab;
    public Object m_GameErWeiMa;
    //public 
    public void Init()
    {
        SpawnGamneErWeiMa();
    }

    /// <summary>
    /// 产生游戏总二维码UI.
    /// </summary>
    public void SpawnGamneErWeiMa()
    {
        if (m_GameErWeiMa == null)
        {
            m_GameErWeiMa = Instantiate(m_GameErWeiMaPrefab, m_UIAnchorCenter);
        }
    }
    
    /// <summary>
    /// 删除游戏总二维码UI.
    /// </summary>
    public void RemoveGamneErWeiMa()
    {
        if (m_GameErWeiMa != null)
        {
            Destroy(m_GameErWeiMa);
        }
    }
}