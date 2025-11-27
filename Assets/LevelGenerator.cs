using UnityEngine;
using System.Collections.Generic;

public class LevelGeneratorPath : MonoBehaviour
{
    [Header("Prefabs bloków")]
    public GameObject normalBlock;
    public GameObject fallingBlock;
    public GameObject spikeBlock;
    public GameObject boostBlock; // opcjonalny
    public GameObject finishBlock;

    [Header("Ustawienia")]
    public int totalBlocks = 20;
    public float blockDistance = 2f;

    [Header("Prawdopodobieñstwo bloków")]
    [Range(0f, 1f)] public float fallingChance = 0.1f;
    [Range(0f, 1f)] public float spikeChance = 0.1f;
    [Range(0f, 1f)] public float boostChance = 0.05f;

    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();
    public List<GameObject> spawnedBlocks = new List<GameObject>(); // lista stworzonych bloków
    private Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    [HideInInspector]
    public Vector3 startPos = Vector3.zero;

    void Start()
    {
        GeneratePath();
    }

    public void GeneratePath()
    {
        ClearLevel();

        Vector3 spawnPos = startPos;
        usedPositions.Add(spawnPos);

        // Startowy blok normalny
        GameObject startBlock = Instantiate(normalBlock, spawnPos, Quaternion.identity);
        spawnedBlocks.Add(startBlock);

        Vector3 lastPos = spawnPos;

        // generujemy bloki losowe (bez ostatniego)
        for (int i = 1; i < totalBlocks - 1; i++)
        {
            Vector3 nextPos = GetNextPosition(lastPos);
            lastPos = nextPos;

            GameObject block = GetRandomBlock();
            spawnedBlocks.Add(Instantiate(block, nextPos, Quaternion.identity));
            usedPositions.Add(nextPos);
        }

        // ostatni blok finish
        Vector3 finishPos = GetNextPosition(lastPos);
        GameObject finish = Instantiate(finishBlock, finishPos, Quaternion.identity);
        spawnedBlocks.Add(finish);
        usedPositions.Add(finishPos);
    }

    Vector3 GetNextPosition(Vector3 currentPos)
    {
        Vector3 nextPos;
        int attempts = 0;

        do
        {
            Vector3 dir = directions[Random.Range(0, directions.Length)];
            nextPos = currentPos + dir * blockDistance;
            attempts++;
        }
        while (usedPositions.Contains(nextPos) && attempts < 20);

        return nextPos;
    }

    GameObject GetRandomBlock()
    {
        float rnd = Random.value;
        if (rnd < fallingChance) return fallingBlock;
        else if (rnd < fallingChance + spikeChance) return spikeBlock;
        else if (rnd < fallingChance + spikeChance + boostChance) return boostBlock;
        else return normalBlock;
    }

    public void ClearLevel()
    {
        foreach (var block in spawnedBlocks)
        {
            if (block != null)
                Destroy(block);
        }
        spawnedBlocks.Clear();
        usedPositions.Clear();
    }

    public Vector3 GetStartPosition()
    {
        return startPos;
    }
}
