using UnityEngine;

public class SSBallPathNode : MonoBehaviour
{
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Transform trPar = transform.parent;
        if (trPar != null)
        {
            SSBallPathCtrl path = trPar.GetComponent<SSBallPathCtrl>();
            path.DrawPath();
        }
    }
#endif
}