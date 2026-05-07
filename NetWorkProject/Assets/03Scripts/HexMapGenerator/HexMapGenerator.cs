using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    public GameObject hexPrefab;

    public int width = 10;
    public int height = 10;

    public float hexSize = 1f;
    private Color m_color;
    MaterialPropertyBlock mpb;
    void Start()
    {
          mpb = new MaterialPropertyBlock();
        
        Generate();
    }
  
    void Generate()
    {
        Color randomColor = new Color(
                  Random.value,
                  Random.value,
                  Random.value
              );
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
                mpb.Clear();
                mpb.SetColor("_BaseColor", randomColor);
                var prefab=Instantiate(hexPrefab, pos, Quaternion.Euler(-90, 0, 0));
                var renderer =
                    prefab.GetComponentInChildren<MeshRenderer>();
                renderer.SetPropertyBlock(mpb);
                //prefab.transform.SetParent(transform);
            }
        }
    }
}