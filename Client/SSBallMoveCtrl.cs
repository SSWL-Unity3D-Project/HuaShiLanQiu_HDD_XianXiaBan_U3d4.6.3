using UnityEngine;

public class SSBallMoveCtrl : MonoBehaviour
{
    public Transform EndTr;
    public SSBallPathCtrl m_Path;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Init(EndTr.position); //test.
        }
    }


    public float m_MinHight = 1f;
    public float m_MaxHight = 5f;
    public float m_TimeMove = 3f;
    public Rigidbody m_Rigidbody;
    public void Init(Vector3 endPos)
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
        //if (m_Rigidbody != null)
        //{
        //    m_Rigidbody.isKinematic = false;
        //}
    }

    bool IsRemoveITwwen = false;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter...");
        if (m_Rigidbody != null)
        {
            m_Rigidbody.useGravity = true;
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