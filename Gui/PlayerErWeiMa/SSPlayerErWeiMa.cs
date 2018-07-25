using UnityEngine;
public class SSPlayerErWeiMa : SSGameMono
{
    /// <summary>
    /// 电视遥控器图集.
    /// </summary>
    public GameObject m_TVYaoKongQiObj;
    public void Init()
    {
        SetActiveTVYaoKongImg();
    }

    void SetActiveTVYaoKongImg()
    {
        if (m_TVYaoKongQiObj != null)
        {
            if (pcvr.GetInstance().m_GmTVLoginDt != null)
            {
                m_TVYaoKongQiObj.SetActive(false);
            }
            else
            {
                m_TVYaoKongQiObj.SetActive(true);
            }
        }
    }
}