using UnityEngine;

public class SSBallSpawnPoint : MonoBehaviour
{
    /// <summary>
    /// 篮球预制.
    /// </summary>
    public GameObject m_BallPrefab;
    public float m_TimeMinSpawn = 2f;
    float m_LastBallSpawn = 0f;
    //public float m_TimeMaxSpawn = 2f;
    /// <summary>
    /// 篮球路径.
    /// </summary>
    public SSBallPathCtrl[] m_BallPathArray;
	// Update is called once per frame
	void Update()
    {
        if (Time.time - m_LastBallSpawn >= m_TimeMinSpawn)
        {
            m_LastBallSpawn = Time.time;
            SpawnGameBall();
        }
	}

    void SpawnGameBall()
    {
        if (m_BallPrefab == null)
        {
            Debug.LogWarning("SpawnGameBall -> m_BallPrefab is null!");
            return;
        }

        if (m_BallPathArray.Length <= 0)
        {
            Debug.LogWarning("SpawnGameBall -> m_BallPathArray was wrong!");
            return;
        }

        int randVal = Random.Range(0, 100) % m_BallPathArray.Length;
        if (m_BallPathArray[randVal] == null)
        {
            return;
        }

        Transform trSpawn = m_BallPathArray[randVal].transform.GetChild(0);
        GameObject obj = (GameObject)Instantiate(m_BallPrefab, trSpawn.position, trSpawn.rotation);
        SSBallMoveCtrl ballMove = obj.GetComponent<SSBallMoveCtrl>();
        ballMove.Init(m_BallPathArray[randVal]);
    }
}