using UnityEngine;

public class SSBallSpawnPoint : MonoBehaviour
{
    /// <summary>
    /// 篮球预制.
    /// </summary>
    public GameObject m_BallPrefab;
    public float m_TimeMinSpawn = 2f;
    float m_LastBallSpawn = 0f;
	// Update is called once per frame
	void Update()
    {
        if (Time.time - m_LastBallSpawn >= m_TimeMinSpawn)
        {
            m_LastBallSpawn = Time.time;
            SpawnGameBall();
        }
	}
    
    public Transform[] m_SpawnPointTrArray;
    void SpawnGameBall()
    {
        if (m_BallPrefab == null)
        {
            Debug.LogWarning("SpawnGameBall -> m_BallPrefab is null!");
            return;
        }

        if (m_SpawnPointTrArray.Length <= 0)
        {
            Debug.LogWarning("SpawnGameBall -> m_SpawnPointTrArray was wrong!");
            return;
        }

        int randVal = Random.Range(0, 100) % m_SpawnPointTrArray.Length;
        if (m_SpawnPointTrArray[randVal] == null)
        {
            return;
        }

        Transform trSpawn = m_SpawnPointTrArray[randVal].transform;
        GameObject obj = (GameObject)Instantiate(m_BallPrefab, trSpawn.position, trSpawn.rotation);
        obj.transform.parent = SSGameRootCtrl.GetInstance().MissionCleanup;
        SSBallAniCtrl ballAni = obj.GetComponent<SSBallAniCtrl>();
        ballAni.Init();
    }
}