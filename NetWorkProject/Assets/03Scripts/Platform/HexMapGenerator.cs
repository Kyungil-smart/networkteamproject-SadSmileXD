
using Unity.Netcode;
using UnityEngine;

public class HexMapGenerator : NetworkBehaviour
{
    public GameObject hexPrefab;

    public int width = 10;
    public int height = 10;

    public float hexSize = 1f;
    private NetworkVariable< Color > m_color = new NetworkVariable<Color>();
    MaterialPropertyBlock mpb;
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            m_color.Value = new Color(Random.value, Random.value, Random.value);
        }
        mpb = new MaterialPropertyBlock();
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
                mpb.Clear();
                mpb.SetColor("_BaseColor", m_color.Value);
                var prefab = Instantiate(hexPrefab, pos, Quaternion.Euler(-90, 0, 0));

                var renderer =
                    prefab.GetComponentInChildren<MeshRenderer>();
                renderer.SetPropertyBlock(mpb);
                 
            }
        }
       //this.NetworkObject.Despawn();
    }
}