using UnityEngine;

public class SSTriggerLanKuang : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        SSBallMoveCtrl ballMove = other.GetComponent<SSBallMoveCtrl>();
        if (ballMove != null)
        {
            ballMove.IsEnterLanKuang = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        SSBallMoveCtrl ballMove = other.GetComponent<SSBallMoveCtrl>();
        if (ballMove != null)
        {
            ballMove.IsExitLanKuang = true;
        }
    }
}