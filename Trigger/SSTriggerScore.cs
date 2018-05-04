using UnityEngine;

public class SSTriggerScore : MonoBehaviour
{
    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    void OnTriggerEnter(Collider other)
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        SSBallMoveCtrl ballMove = other.GetComponent<SSBallMoveCtrl>();
        if (ballMove != null && !ballMove.IsDeFenQiu)
        {
            ballMove.IsDeFenQiu = true;
            if (ballMove.CountOnHit == 0)
            {
                Debug.Log("SSTriggerScore -> KongXinQiu, m_PlayerIndex == " + m_PlayerIndex);
            }
            SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score += 1;
            Debug.Log("SSTriggerScore -> Score == " + SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score + ", m_PlayerIndex == " + m_PlayerIndex);
        }
    }
}