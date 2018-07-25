using System;
using UnityEngine;

public class SSBallMoveCtrl : SSGameMono
{
    /// <summary>
    /// 是否为连发球.
    /// </summary>
    [HideInInspector]
    public bool IsLianFaQiu = false;
    /// <summary>
    /// 是否为最后一个连发球.
    /// </summary>
    [HideInInspector]
    public bool IsLastLianFaQiu = false;
    /// <summary>
    /// 篮球碰到篮筐的音效.
    /// </summary>
    public AudioSource m_HitLanKuangAudio;
    /// <summary>
    /// 篮球的碰撞刚体.
    /// </summary>
    public Rigidbody m_Rigidbody;
    [HideInInspector]
    public SSBallAniCtrl m_BallAni;
    /// <summary>
    /// 篮球转动控制脚本.
    /// </summary>
    TweenRotation m_BallTweenRot;
    public class BallMoveData
    {
        public SSGameDataCtrl.LanQiuType m_LanQiuType = SSGameDataCtrl.LanQiuType.PuTong;
        public Transform m_RealBallTr;
        public SSBallAniCtrl m_SSBallAni;
        /// <summary>
        /// 是否为烟雾特效篮球.
        /// </summary>
        public bool IsYanWuTXBall = false;
        public BallMoveData(SSGameDataCtrl.LanQiuType type, Transform tr, SSBallAniCtrl ballAni, bool isYanWuBall)
        {
            m_LanQiuType = type;
            m_RealBallTr = tr;
            m_SSBallAni = ballAni;
            IsYanWuTXBall = isYanWuBall;
        }
    }
    public BallMoveData m_BallMoveData;

    void Start()
    {
        if (!IsInitMoveBall)
        {
            UnityLogWarning("Should be hidden the ball!");
            gameObject.SetActive(false);
        }
    }

    public void Init(BallMoveData ballDt, float ballMoveSpeedBeiLv = 1f)
    {
        IsInitMoveBall = true;
        m_BallMoveData = ballDt;
        m_BallAni = ballDt.m_SSBallAni;
        m_BallMoveSpeedBeiLv = ballMoveSpeedBeiLv;
        if (m_Rigidbody != null)
        {
            m_Rigidbody.useGravity = false;
        }

        if (ballDt.m_RealBallTr != null)
        {
            ballDt.m_RealBallTr.localEulerAngles = new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        }
        m_BallTweenRot = m_BallAni.m_BallData.m_BallSpawnTr.gameObject.GetComponent<TweenRotation>();

        SSGameDataCtrl.GetInstance().m_SSUIRoot.OnCreatExitGameUIEvent += OnCreatExitGameUIEvent;
        SSGameDataCtrl.GetInstance().m_SSUIRoot.OnRemoveExitGameUIEvent += OnRemoveExitGameUIEvent;
    }

    private void OnCreatExitGameUIEvent()
    {
        if (m_BallTweenRot != null && m_BallTweenRot.enabled)
        {
            m_BallTweenRot.enabled = false;
        }
    }

    private void OnRemoveExitGameUIEvent()
    {
        if (m_BallTweenRot != null && m_BallTweenRot.enabled)
        {
            m_BallTweenRot.enabled = true;
        }
    }

    /// <summary>
    /// 删除退出游戏窗口事件.
    /// </summary>
    public void RemoveUIRootEvent()
    {
        SSGameDataCtrl.GetInstance().m_SSUIRoot.OnCreatExitGameUIEvent -= OnCreatExitGameUIEvent;
        SSGameDataCtrl.GetInstance().m_SSUIRoot.OnRemoveExitGameUIEvent -= OnRemoveExitGameUIEvent;
    }

    int m_MoveCount = 0;
    /// <summary>
    /// 篮球运动速度倍率控制.
    /// </summary>
    float m_BallMoveSpeedBeiLv = 1f;
    [HideInInspector]
    public bool IsRemoveSelf = false;
    void FixedUpdate()
    {
        if (m_BallAni == null)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,停止篮球运动.
            return;
        }

        if (!IsRemoveSelf)
        {
            if (SSGameDataCtrl.GetInstance().m_TriggerRemoveBall != null)
            {
                Vector3 triggerPos = SSGameDataCtrl.GetInstance().m_TriggerRemoveBall.transform.position;
                float disRemove = 15f;
                if (SSGameDataCtrl.GetInstance().IsStopCreatBall)
                {
                    disRemove = 8f;
                }

                if (transform.position.y < triggerPos.y - disRemove)
                {
                    IsRemoveSelf = true;
                    IsInitMoveBall = false;
                    //UnityLog("Remove the ball, time == " + Time.time.ToString("f3"));
                    m_BallAni.StartCoroutine(m_BallAni.DelayDestroyThis(0.1f));
                }
            }
        }

        if (!IsInitMoveBall)
        {
            if (m_Rigidbody != null && m_Rigidbody.useGravity)
            {
                Vector3 ballDownPos = transform.localPosition;
                ballDownPos.y -= 0.5f;
                transform.localPosition = ballDownPos;
            }
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
    /// <summary>
    /// 是否进入篮筐.
    /// </summary>
    [HideInInspector]
    public bool IsEnterLanKuang = false;
    /// <summary>
    /// 是否在篮筐外面.
    /// </summary>
    [HideInInspector]
    public bool IsExitLanKuang = false;

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter -> colName ================== " + collision.gameObject.name + ", CountOnHit == " + CountOnHit);
        InteractiveCloth cloth = collision.gameObject.GetComponent<InteractiveCloth>();
        SSTriggerScore triScore = collision.gameObject.GetComponent<SSTriggerScore>();
        if (triScore == null
            && cloth == null
            && !IsDeFenQiu)
        {
            //没有碰上分数触发器和篮网的布料碰撞器.
            CountOnHit++;
        }

        if (CountOnHit == 0)
        {
            //空心球不去打开篮球的物理碰撞.
            //Debug.Log("OnCollisionEnter -> player get a kongXinQiu*********************");
        }
        else
        {
            IsInitMoveBall = false;
            if (m_Rigidbody != null)
            {
                if (m_HitLanKuangAudio != null)
                {
                    //播放篮球碰撞的音效.
                    m_HitLanKuangAudio.Play();
                }

                if (!m_Rigidbody.useGravity)
                {
                    m_Rigidbody.useGravity = true;
                }

                Vector3 hitPos = collision.transform.position;
                Vector3 ballPos = transform.position;
                hitPos.z = ballPos.z = 0f;
                Vector3 vecHB = Vector3.Normalize(ballPos - hitPos);
                if (vecHB.x == 0f)
                {
                    vecHB.x = UnityEngine.Random.Range(0, 100) % 2 == 0 ? 1f : -1f;
                }
                float randForceOffset = UnityEngine.Random.Range(0.06f, 0.075f);
                m_Rigidbody.AddForce(vecHB.normalized * m_ForceBall * randForceOffset, ForceMode.Impulse);
                //m_Rigidbody.AddTorque(transform.right * m_TorqueBall);
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
}