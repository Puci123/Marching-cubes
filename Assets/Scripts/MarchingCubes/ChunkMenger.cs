using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMenger : MonoBehaviour
{
    public Vector2Int chunksNumber;
    public Vector3 chunkSize;

    public float cellSize;
    public float isoValue = 0.5f;

    public bool aplyColider = true;

    public NoiseKenel noiseKenel;

    public Transform chunkPrefab;
    public Transform map;

    private Vector3 chunkScale;
    private Chunk[,] chunks;

    private void OnValidate()
    {
        if(chunkPrefab != null)
            chunkScale = chunkPrefab.localScale;
    }

    private void Start()
    {
        Vector3 halfMapSize = new Vector3(chunkScale.x * chunkSize.x * chunksNumber.x, 0,chunkScale.z * chunkSize.z * chunksNumber.y);
        halfMapSize.x = halfMapSize.x / 2 - (chunkSize.x * chunkScale.x / 2);
        halfMapSize.z = halfMapSize.z / 2 - (chunkSize.z * chunkScale.z /2);

        Vector3 spawnPos = transform.position - halfMapSize;

        chunks = new Chunk[chunksNumber.x, chunksNumber.y];

        for (int z = 0; z < chunksNumber.y; z++)
        {
            for (int x = 0; x < chunksNumber.x; x++)
            {
                Vector3 offset = new Vector3(x * chunkScale.x * chunkSize.x, 0, z * chunkSize.z * chunkScale.z);
                Vector3 pos = spawnPos + offset;
                Chunk chunk =  Instantiate(chunkPrefab, pos, Quaternion.identity, map).GetComponent<Chunk>();
                chunk.SetUpChunk(chunkSize, cellSize, isoValue, aplyColider, noiseKenel);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 size = new Vector3(chunksNumber.x * chunkScale.x * chunkSize.x, chunkSize.y * chunkScale.y, chunksNumber.y * chunkScale.z * chunkSize.z);
        Gizmos.DrawWireCube(transform.position,size);
    }

}
