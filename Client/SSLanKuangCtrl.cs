using UnityEngine;

public class SSLanKuangCtrl : MonoBehaviour
{
    /// <summary>
    /// 篮筐Tr.
    /// </summary>
    public Transform m_RealKuangTr;
    /// <summary>
    /// X轴移动的最大距离.
    /// </summary>
    [Range(0.1f, 100f)]
    public float m_DisXMax = 2f;
    /// <summary>
    /// 篮筐移动的速度.
    /// </summary>
    [Range(0.01f, 100f)]
    public float m_SpeedX = 5f;
	
	// Update is called once per frame
	void Update()
    {
        float inputHorVal = Input.GetAxis("Horizontal");
        Vector3 pos = m_RealKuangTr.localPosition;
        //pos.x = m_DisXMax * inputHorVal;
        if (inputHorVal != 0f)
        {
            //硬件版篮筐的移动方案可能和软件版不一样.
            //软件版移动篮筐方案.
            if (Mathf.Abs(pos.x) <= m_DisXMax)
            {
                bool isMoveLK = false;
                if (Mathf.Abs(pos.x) != m_DisXMax)
                {
                    isMoveLK = true;
                }
                else
                {
                    if (Mathf.Sign(inputHorVal) == -1f && pos.x == m_DisXMax)
                    {
                        isMoveLK = true;
                    }

                    if (Mathf.Sign(inputHorVal) == 1f && pos.x == -m_DisXMax)
                    {
                        isMoveLK = true;
                    }
                }

                if (isMoveLK)
                {
                    pos.x += Mathf.Sign(inputHorVal) * (m_SpeedX * Time.deltaTime);
                }

                if (pos.x > m_DisXMax)
                {
                    pos.x = m_DisXMax;
                }

                if (pos.x < -m_DisXMax)
                {
                    pos.x = -m_DisXMax;
                }
            }
        }
        m_RealKuangTr.localPosition = pos;
    }
}