using UnityEngine;

public class SSBallSpawnPoint : MonoBehaviour
{
    /// <summary>
    /// 篮球预制.
    /// </summary>
    public GameObject[] m_BallPrefabArray;
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
        if (m_BallPrefabArray.Length <= 0)
        {
            Debug.LogWarning("m_BallPrefabArray -> m_BallPrefab is wrong!");
            return;
        }

        int randBallVal = Random.Range(0, 100) % m_BallPrefabArray.Length;
        if (m_BallPrefabArray[randBallVal] == null)
        {
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
        GameObject obj = (GameObject)Instantiate(m_BallPrefabArray[randBallVal], trSpawn.position, trSpawn.rotation);
        obj.transform.parent = SSGameRootCtrl.GetInstance().MissionCleanup;
        SSBallAniCtrl ballAni = obj.GetComponent<SSBallAniCtrl>();
        ballAni.Init();
    }
}