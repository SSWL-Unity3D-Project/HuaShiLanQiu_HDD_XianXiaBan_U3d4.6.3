using UnityEngine;

public class SSLanHuanUV : SSGameMono
{
    /// <summary>
    /// 篮环UV速度.
    /// m_LanHuanUVSpeedArray[0] -> 10秒.
    /// m_LanHuanUVSpeedArray[1] -> 15秒.
    /// m_LanHuanUVSpeedArray[2] -> 20秒.
    /// m_LanHuanUVSpeedArray[3] -> 25秒.
    /// </summary>
    public float[] m_LanHuanUVSpeedArray = new float[4] { 0.5f, 0.5f, 0.5f, 0.5f };
    /// <summary>
    /// UV速度.
    /// </summary>
    float m_SpeedUV = 0.5f;
    /// <summary>
    /// UV记录的信息.
    /// </summary>
    float m_UVRecordVal = 0f;
    /// <summary>
    /// UV的最大值.
    /// </summary>
    public float m_MaxUVVal = 0.3f;
    /// <summary>
    /// 篮环材质.
    /// </summary>
    public Material m_LanHuanMat;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_LanHuanMat != null)
        {
            m_UVRecordVal += Time.fixedDeltaTime * m_SpeedUV;
            if (m_UVRecordVal > m_MaxUVVal)
            {
                m_UVRecordVal = m_MaxUVVal;
            }
            m_LanHuanMat.SetTextureOffset("_MainTex", new Vector2(m_UVRecordVal, 0));

            if (m_UVRecordVal >= m_MaxUVVal)
            {
                //UV动画已经结束.
                enabled = false;
            }
        }
    }

    /// <summary>
    /// 初始化.
    /// </summary>
    public void InitPlayUVAni(SSLanKuangTimeAni.DaoJiShiState type)
    {
        int index = (int)type;
        m_SpeedUV = m_LanHuanUVSpeedArray[index];
        m_UVRecordVal = 0f;
        enabled = true;
    }

    /// <summary>
    /// 重置UV.
    /// </summary>
    public void ResetUV()
    {
        if (m_LanHuanMat != null)
        {
            m_LanHuanMat.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
        }
        enabled = false;
    }
}