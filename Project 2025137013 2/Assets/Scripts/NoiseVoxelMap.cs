using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public enum BlockType
    {
        Grass,
        Dirt,
        Stone,
        Water,
        Diamond
    }

    [Header("Prefabs")]
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject stonePrefab;
    public GameObject waterPrefab;
    public GameObject diamondPrefab;

    [Header("Settings")]
    public int dirtLayerDepth = 3;
    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
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
                    // Vector3Int 좌표 생성
                    Vector3Int currentPos = new Vector3Int(x, y, z);

                    // 1. "현재 Y가 h(최고 높이)와 같은가?"
                    if (y == h)
                    {
                        // 맨 위: 잔디
                        PlaceTile(currentPos, BlockType.Grass);
                    }
                    // 2. "아니라면, 현재 Y가 (h - 3)보다 높은가?" (흙 깊이가 3일 때)
                    else if (y >= h - dirtLayerDepth)
                    {
                        // 표면 근처: 흙
                        PlaceTile(currentPos, BlockType.Dirt);
                    }
                    // 3. "둘 다 아니다" (즉, h도 아니고, h-3보다 낮다)
                    else
                    {
                        // 그 아래: 돌
                        PlaceTile(currentPos, BlockType.Stone);
                    }
                }
            }
        }
    }

    // PlaceTile 메서드
    public void PlaceTile(Vector3Int pos, BlockType type)
    {
        switch (type)
        {
            case BlockType.Dirt:
                PlaceDirt(pos.x, pos.y, pos.z);
                break;
            case BlockType.Grass:
                PlaceGrass(pos.x, pos.y, pos.z);
                break;
            case BlockType.Water:
                PlaceWater(pos.x, pos.y, pos.z);
                break;
            case BlockType.Diamond:
                PlaceDiamond(pos.x, pos.y, pos.z);
                break;
            case BlockType.Stone: // Stone 케이스 추가
                PlaceStone(pos.x, pos.y, pos.z);
                break;
        }
    }

    // 각 타입별 전용 메서드 구현 (기존 Place 메서드 재활용)
    void PlaceGrass(int x, int y, int z) => Place(grassPrefab, x, y, z);
    void PlaceDirt(int x, int y, int z) => Place(dirtPrefab, x, y, z);
    void PlaceStone(int x, int y, int z) => Place(stonePrefab, x, y, z);
    void PlaceWater(int x, int y, int z) => Place(waterPrefab, x, y, z);
    void PlaceDiamond(int x, int y, int z) => Place(diamondPrefab, x, y, z);

    public void Place(GameObject prefabToPlace, int x, int y, int z)
    {
        // 프리팹이 연결되지 않았을 경우 에러 방지
        if (prefabToPlace == null) return;

        // blockPrefab -> prefabToPlace 로 수정
        var go = Instantiate(prefabToPlace, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
        go.tag = "Block";
    }
}
