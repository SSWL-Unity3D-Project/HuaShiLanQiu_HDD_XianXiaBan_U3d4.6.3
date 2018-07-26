using UnityEngine;

public class UVA : MonoBehaviour
{
	public float UVSpeed = 0.5f;
	public int IndexMat = 0;
    /// <summary>
    /// UV记录的信息.
    /// </summary>
    public float m_UVRecordVal = 0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        m_UVRecordVal += Time.fixedDeltaTime * UVSpeed;
		renderer.materials[IndexMat].SetTextureOffset("_MainTex", new Vector2(m_UVRecordVal, 0f));
	}
}