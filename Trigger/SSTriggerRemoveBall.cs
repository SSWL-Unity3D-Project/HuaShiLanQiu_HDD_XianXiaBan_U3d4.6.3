using UnityEngine;

public class SSTriggerRemoveBall : SSGameMono
{
    [Range(0f, 100f)]
    public float TimeRemove = 5f;

    void Start()
    {
        SSGameDataCtrl.GetInstance().m_TriggerRemoveBall = this;
    }
}