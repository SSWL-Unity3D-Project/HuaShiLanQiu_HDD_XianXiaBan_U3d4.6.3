using UnityEngine;

public class UVA : MonoBehaviour
{
	public float speed=0.5f;
	public int Array=0;
	// Update is called once per frame
	void Update()
    {
        /*if (Camera.main == null)
        {
            return;
        }*/

        //Vector3 posA = Camera.main.transform.position;
        //Vector3 posB = transform.position;
        //posA.y = posB.y = 0f;
        //if (Vector3.Distance(posA, posB) > 100f)
        //{
        //    return;
        //}

		float offset = Time.time *speed;
		renderer.materials[Array].SetTextureOffset("_MainTex",new Vector2 (offset,0));
	}
}