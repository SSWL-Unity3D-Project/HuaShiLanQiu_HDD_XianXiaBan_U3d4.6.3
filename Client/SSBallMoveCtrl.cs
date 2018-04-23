using UnityEngine;

public class SSBallMoveCtrl : MonoBehaviour
{
    //public Transform EndTr;
    public SSBallPathCtrl m_Path;
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        Init(m_Path); //test.
    //    }
    //}


    public float m_MinHight = 1f;
    public float m_MaxHight = 5f;
    public float m_TimeMove = 3f;
    public Rigidbody m_Rigidbody;
    public void Init(SSBallPathCtrl pathBall)
    {
        //float lobTime = m_TimeMove;
        //float lobTimePosY = 0.5f * lobTime;
        //float lobHeight = Random.Range(m_MinHight, m_MaxHight);
        //GameObject coreObj = transform.GetChild(0).gameObject;
        //iTween.MoveBy(coreObj, iTween.Hash("y", lobHeight,
        //                                    "time", lobTimePosY,
        //                                    "easeType", iTween.EaseType.easeOutQuad));
        //iTween.MoveBy(coreObj, iTween.Hash("y", -lobHeight,
        //                                    "time", lobTimePosY,
        //                                    "delay", lobTimePosY,
        //                                    "easeType", iTween.EaseType.easeInCubic));

        //Vector3[] posArray = new Vector3[2];
        //posArray[0] = transform.position;
        //posArray[1] = endPos;

        m_Path = pathBall;
        Transform[] path = m_Path.GetPath();
        if (path == null)
        {
            Debug.LogWarning("path was wrong!");
            return;
        }

        IsRemoveITwwen = false;
        if (m_Rigidbody != null)
        {
            m_Rigidbody.useGravity = false;
        }
        transform.position = path[0].position;
        iTween.MoveTo(gameObject, iTween.Hash("path", path,
                                           "time", m_TimeMove,
                                           "orienttopath", true,
                                           "easeType", iTween.EaseType.linear,
                                           "oncomplete", "MoveBallOnCompelteITween"));
    }

    void MoveBallOnCompelteITween()
    {
        Debug.Log("MoveBallOnCompelteITween...");
        if (m_Rigidbody != null)
        {
            m_Rigidbody.useGravity = true;
        }
    }

    bool IsRemoveITwwen = false;
    /// <summary>
    /// 篮球撞击的推力.
    /// </summary>
    [Range(0f, 10000f)]
    public float m_ForceBall = 50f;
    /// <summary>
    /// 篮球撞击的转动力.
    /// </summary>
    [Range(0f, 10000f)]
    public float m_TorqueBall = 50f;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter...");
        if (m_Rigidbody != null)
        {
            m_Rigidbody.useGravity = true;
            Vector3 hitPos = collision.transform.position;
            Vector3 ballPos = transform.position;
            Vector3 vecHB = Vector3.Normalize(ballPos - hitPos);
            m_Rigidbody.AddForce(vecHB * m_ForceBall, ForceMode.Force);
            rigidbody.AddTorque(transform.right * m_TorqueBall);
        }

        if (!IsRemoveITwwen)
        {
            IsRemoveITwwen = true;
            iTween itweenCom = GetComponent<iTween>();
            if (itweenCom != null)
            {
                itweenCom.isRunning = false;
                itweenCom.isPaused = true;
                Destroy(itweenCom);
            }
        }
    }
}