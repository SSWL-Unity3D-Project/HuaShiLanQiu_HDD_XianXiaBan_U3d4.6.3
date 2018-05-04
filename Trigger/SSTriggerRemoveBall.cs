using System.Collections;
using UnityEngine;

public class SSTriggerRemoveBall : MonoBehaviour
{
    [Range(0f, 100f)]
    public float TimeRemove = 5f;
    public void OnTriggerExit(Collider other)
    {
        SSBallMoveCtrl ballMove = other.GetComponent<SSBallMoveCtrl>();
        if (ballMove != null)
        {
            SSBallAniCtrl ballAni = ballMove.m_BallAni;
            if (ballAni)
            {
                ballAni.StartCoroutine(ballAni.DelayDestroyThis(TimeRemove));
            }
        }
    }
}