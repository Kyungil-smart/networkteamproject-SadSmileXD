using UnityEngine;

public class TestGem : MonoBehaviour
{
    public GameObject hexPrefab;

    public int width = 10;
    public int height = 10;

    public float hexSize = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate();
    }

    void Generate()
    {

        float xOffset = hexSize * 1.5f;
        float zOffset = Mathf.Sqrt(3f) * hexSize;
      
        for (int x = 0; x < width; x++)
        {

            for (int z = 0; z < height; z++)
            {
                float posX = x * xOffset;
                float posZ = z * zOffset;

                if (x % 2 == 1)
                    posZ += zOffset / 2f;

                Vector3 pos = new Vector3(posX, this.transform.position.y, posZ);
               
                var prefab = Instantiate(hexPrefab, pos, Quaternion.Euler(0, -90, 0));

                var renderer = prefab.GetComponentInChildren<MeshRenderer>();
              

            }
        }
        //this.NetworkObject.Despawn();
    }
}
