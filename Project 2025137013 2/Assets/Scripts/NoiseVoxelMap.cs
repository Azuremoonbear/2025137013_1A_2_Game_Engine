using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject stonePrefab;

    public int dirtLayerDepth = 3;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16; //Y
    [SerializeField] float noiseScale = 20f;

    // Start is called before the first frame update
    void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);

                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) continue;

                for (int y = 0; y <= h; y++)
                {
                    // 1. "현재 Y가 h(최고 높이)와 같은가?"
                    if (y == h)
                    {
                        // "그렇다" -> 잔디 블록 좌표임
                        Place(grassPrefab, x, y, z);
                    }
                    // 2. "아니라면, 현재 Y가 (h - 3)보다 높은가?" (흙 깊이가 3일 때)
                    else if (y >= h - dirtLayerDepth)
                    {
                        // "그렇다" -> 흙 블록 좌표임
                        Place(dirtPrefab, x, y, z);
                    }
                    // 3. "둘 다 아니다" (즉, h도 아니고, h-3보다 낮다)
                    else
                    {
                        // "그렇다" -> 돌 블록 좌표임
                        Place(stonePrefab, x, y, z);
                    }
                }
            }
        }
    }

    public void Place(GameObject prefabToPlace, int x, int y, int z)
    {
        // blockPrefab -> prefabToPlace 로 수정
        var go = Instantiate(prefabToPlace, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
        go.tag = "Block";
    }
}