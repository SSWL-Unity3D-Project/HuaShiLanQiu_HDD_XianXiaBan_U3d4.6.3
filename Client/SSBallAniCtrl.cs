using System.Collections;
using UnityEngine;

public class SSBallAniCtrl : MonoBehaviour
{
    public SSBallMoveCtrl m_BallMove;
    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex IndexPlayer;
    /// <summary>
    /// 是否为开始加分篮球.
    /// </summary>
    [HideInInspector]
    public bool IsStartJiaFenLanQiu = false;
    public void Init(SSGameDataCtrl.PlayerIndex index)
    {
        //Debug.Log("index == " + index);
        IndexPlayer = index;
        if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].Score > 0)
        {
            IsStartJiaFenLanQiu = true;
        }
        m_BallMove.Init(this);
    }

    public IEnumerator DelayDestroyThis(float time)
    {
        SSBallAniCtrl ballAni = GetComponent<SSBallAniCtrl>();
        if (ballAni != null)
        {
            if (ballAni.IsStartJiaFenLanQiu && ballAni.m_BallMove != null && !ballAni.m_BallMove.IsDeFenQiu)
            {
                int scoreVal = SSGameDataCtrl.GetInstance().m_PlayerData[(int)ballAni.IndexPlayer].Score;
                if (scoreVal > 0)
                {
                    //分数大于零后,接篮球有失误时则关闭该玩家的控制权.
                    SSGameDataCtrl.GetInstance().SetActivePlayer(ballAni.IndexPlayer, false);
                }
            }
        }
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}