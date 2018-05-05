using UnityEngine;

public class SSBallMoveCtrl : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    [HideInInspector]
    public SSBallAniCtrl m_BallAni;
    public void Init(SSBallAniCtrl ballAni, Transform realBallTr, float ballMoveSpeedBeiLv = 1f)
    {
        IsInitMoveBall = true;
        m_BallAni = ballAni;
        m_BallMoveSpeedBeiLv = ballMoveSpeedBeiLv;
        if (m_Rigidbody != null)
        {
            m_Rigidbody.useGravity = false;
        }

        if (realBallTr != null)
        {
            realBallTr.localEulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
        }
    }

    int m_MoveCount = 0;
    /// <summary>
    /// 篮球运动速度倍率控制.
    /// </summary>
    float m_BallMoveSpeedBeiLv = 1f;
    void FixedUpdate()
    {
        if (!IsInitMoveBall)
        {
            return;
        }

        m_MoveCount++;
        Vector3 ballPos = Vector3.zero;
        float timeZ = Time.fixedDeltaTime;
        float timeVal = timeZ * m_MoveCount * m_BallMoveSpeedBeiLv;
        ballPos.z = timeVal * m_SpeedZ;
        ballPos.y = m_SpeedY * timeVal + 0.5f * m_JiaSuDu * Mathf.Pow(timeVal, 2f);
        transform.localPosition = ballPos;
    }

    bool IsInitMoveBall = false;
    /// <summary>
    /// 抛物线Y轴速度.
    /// </summary>
    [Range(0.1f, 100f)]
    public float m_SpeedY = 10f;
    /// <summary>
    /// 抛物线Z轴速度.
    /// </summary>
    [Range(0.1f, 100f)]
    public float m_SpeedZ = 10f;
    [Range(-0.1f, -100f)]
    public float m_JiaSuDu = -9.8f;
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            return;
        }

        Vector3 startPos = transform.position;
        Vector3 nextPos = Vector3.zero;
        float timeZ = 0.1f;
        float timeVal = 0f;
        Gizmos.color = Color.green;
        for (int i = 1; i < 40; i++)
        {
            timeVal = timeZ * i;
            nextPos = Vector3.zero;
            nextPos.z = timeVal * m_SpeedZ;
            nextPos.y = m_SpeedY * timeVal + 0.5f * m_JiaSuDu * Mathf.Pow(timeVal, 2f);
            nextPos += transform.position;
            Gizmos.DrawLine(startPos, nextPos);
            startPos = nextPos;
        }
    }

    public void DrawPath()
    {
        OnDrawGizmosSelected();
    }
#endif

    /// <summary>
    /// 篮球撞击的推力.
    /// </summary>
    [Range(0f, 10000f)]
    public float m_ForceBall = 50f;
    /// <summary>
    /// 篮球衰减力.
    /// </summary>
    [Range(0f, 10000f)]
    public float m_SubForceBall = 10f;
    /// <summary>
    /// 篮球最小推力.
    /// </summary>
    [Range(0f, 10000f)]
    public float m_MinForceBall = 10f;
    /// <summary>
    /// 篮球撞击的转动力.
    /// </summary>
    [Range(0f, 10000f)]
    public float m_TorqueBall = 50f;
    /// <summary>
    /// 碰撞次数累计,用来判断是否为空心球.
    /// </summary>
    [HideInInspector]
    public int CountOnHit = 0;
    /// <summary>
    /// 是否是得分球.
    /// </summary>
    [HideInInspector]
    public bool IsDeFenQiu = false;
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter...");
        IsInitMoveBall = false;
        SSTriggerScore triScore = collision.gameObject.GetComponent<SSTriggerScore>();
        if (triScore == null)
        {
            CountOnHit++;
        }

        if (m_Rigidbody != null)
        {
            if (!m_Rigidbody.useGravity)
            {
                m_Rigidbody.useGravity = true;
            }

            Vector3 hitPos = collision.transform.position;
            Vector3 ballPos = transform.position;
            hitPos.z = ballPos.z = 0f;
            Vector3 vecHB = Vector3.Normalize(ballPos - hitPos);
            m_Rigidbody.AddForce(vecHB * m_ForceBall, ForceMode.Force);
            rigidbody.AddTorque(transform.right * m_TorqueBall);
            if (m_ForceBall > m_MinForceBall)
            {
                m_ForceBall -= m_SubForceBall;
                if (m_ForceBall < m_MinForceBall)
                {
                    m_ForceBall = m_MinForceBall;
                }
            }
        }
    }
}