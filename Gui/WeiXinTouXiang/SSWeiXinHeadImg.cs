using UnityEngine;

public class SSWeiXinHeadImg : MonoBehaviour
{
    /// <summary>
    /// 微信头像.
    /// </summary>
    public UITexture m_HeadImg;
    /// <summary>
    /// 微信头像材质.
    /// </summary>
    public MeshRenderer m_HeadMesh;
    /// <summary>
    /// 默认头像.
    /// </summary>
    public Texture m_HeadDefaut;
    /// <summary>
    /// 微信头像.
    /// </summary>
    public Material[] m_HeadMat = new Material[2];
    public void Init(SSGameDataCtrl.PlayerIndex indexPlayer)
    {
        if (indexPlayer == SSGameDataCtrl.PlayerIndex.Null)
        {
            return;
        }

        string url = SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexPlayer].PlayerHeadUrl;
        if (m_HeadImg != null)
        {
            SSGameDataCtrl.GetInstance().m_AsyncImg.LoadPlayerHeadImg(url, m_HeadImg);
        }

        if (m_HeadMat.Length >= 2 && m_HeadMat[(int)indexPlayer] != null)
        {
            m_HeadMat[(int)indexPlayer].mainTexture = m_HeadDefaut;
            SSGameDataCtrl.GetInstance().m_AsyncImg.LoadPlayerHeadImg(url, m_HeadMat[(int)indexPlayer]);
            if (m_HeadMesh != null)
            {
                m_HeadMesh.material = m_HeadMat[(int)indexPlayer];
            }
        }
    }
}