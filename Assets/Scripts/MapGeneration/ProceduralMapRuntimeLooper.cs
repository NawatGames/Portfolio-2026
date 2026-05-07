using UnityEngine;

/// <summary>
/// Geração procedural
/// quando o player sai dos limites do mapa atual, gera outro mapa e recentraliza o player.
/// </summary>
[DisallowMultipleComponent]
public class ProceduralMapRuntimeLooper : MonoBehaviour
{
    [SerializeField] private ProceduralMapGenerator generator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector3 worldOffset = Vector3.zero;
    [SerializeField] private bool useNewSeedOnEachLoop = true;
    [SerializeField] [Min(0f)] private float respawnHeight = 0.9f;
    [SerializeField] private bool enableLooping = true;

    private void Reset()
    {
        generator = GetComponent<ProceduralMapGenerator>();
        playerTransform = null;
    }

    private void Update()
    {
        if (!enableLooping || generator == null)
            return;

        var player = GetPlayerTransform();
        var terrain = generator.LastTerrain;
        if (player == null || terrain == null)
            return;

        int w = terrain.GetLength(0);
        int h = terrain.GetLength(1);
        float size = Mathf.Max(0.001f, cellSize);

        Vector3 origin = generator.transform.position + worldOffset;
        Vector3 local = player.position - origin;
        int cx = Mathf.FloorToInt(local.x / size);
        int cy = Mathf.FloorToInt(local.z / size);

        bool outside = cx < 0 || cy < 0 || cx >= w || cy >= h;
        if (!outside)
            return;

        int nextSeed = useNewSeedOnEachLoop ? generator.LastSeedUsed + 1 : generator.LastSeedUsed;
        bool ok = generator.GenerateNewRun(nextSeed);
        if (!ok)
            return;

        int centerX = w / 2;
        int centerY = h / 2;
        Vector3 center = origin + new Vector3((centerX + 0.5f) * size, respawnHeight, (centerY + 0.5f) * size);
        player.position = center;
    }

    private Transform GetPlayerTransform()
    {
        if (playerTransform != null)
            return playerTransform;

        var found = GameObject.FindGameObjectWithTag("Player");
        return found != null ? found.transform : null;
    }
}