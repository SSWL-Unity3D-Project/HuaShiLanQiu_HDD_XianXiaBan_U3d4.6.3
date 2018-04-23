using System.Collections.Generic;
using UnityEngine;

public class SSBallPathCtrl : MonoBehaviour
{
    void Start()
    {
        enabled = false;
    }

    public Transform[] GetPath()
    {
        Transform parTran = transform;
        if (parTran.childCount > 1)
        {
            List<Transform> nodesTran = new List<Transform>(parTran.GetComponentsInChildren<Transform>()) { };
            nodesTran.Remove(parTran);
            return nodesTran.ToArray();
        }
        return null;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Transform parTran = transform;
        if (parTran.childCount > 1)
        {
            List<Transform> nodesTran = new List<Transform>(parTran.GetComponentsInChildren<Transform>()) { };
            nodesTran.Remove(parTran);
            iTween.DrawPath(nodesTran.ToArray(), Color.green);

            Gizmos.color = Color.blue;
            for (int i = 0; i < nodesTran.Count; i++)
            {
                Gizmos.DrawWireSphere(nodesTran[i].position, 0.5f);
            }
        }
    }
#endif

    public void DrawPath()
    {
        OnDrawGizmosSelected();
    }
}