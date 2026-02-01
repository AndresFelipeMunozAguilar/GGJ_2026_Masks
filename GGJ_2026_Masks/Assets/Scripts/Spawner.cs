using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs (cada uno con SpriteRenderer, Animator, BoxCollider2D, ObjectByType)")]
    public GameObject prefabTipo1;
    public GameObject prefabTipo2;
    public GameObject prefabTipo3;

    [Header("Número de espíritus (sin contar impostor)")]
    public int numberItems = 20;
    public int impostorCount = 1;

    [Header("Área de spawn (rectángulo)")]
    public Vector2 spawnAreaMin = new Vector2(-5, -5);
    public Vector2 spawnAreaMax = new Vector2(5, 5);

    [Header("Distancia mínima entre objetos")]
    public float minDistance = 1.0f;

    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        SpawnAllRandomized();
    }

    void SpawnAllRandomized()
    {
        int total = numberItems + impostorCount;
        HashSet<int> impostorIndices = new HashSet<int>();
        while (impostorIndices.Count < impostorCount)
            impostorIndices.Add(Random.Range(0, total));

        for (int i = 0; i < total; i++)
        {
            GameObject chosenPrefab = ChooseRandomPrefab();
            Vector3 pos = GetValidPosition();
            GameObject obj = Instantiate(chosenPrefab, pos, Quaternion.identity, transform);

            var sr = obj.GetComponent<SpriteRenderer>();
            var ob = obj.GetComponent<ObjectByType>();

            if (sr == null || ob == null)
            {
                Debug.LogError("El prefab debe tener SpriteRenderer y ObjectByType.");
                continue;
            }

            if (impostorIndices.Contains(i))
            {
                ob.tipo = "Impostor";
                ob.isImpostor = true;
                int tipoImpostor = Random.Range(1,4);
                switch (tipoImpostor)
                {
                case 1:
                    ob.tipo = "";
                    sr.color = HexToColor("FFF700");
                    ob.impostorColor = HexToColor("FFF700");
                    break;
                case 2:
                    ob.tipo = "";
                    sr.color = HexToColor("00FFFC");
                    ob.impostorColor = HexToColor("00FFFC");
                    break;
                case 3:
                    ob.tipo = "";
                    sr.color = HexToColor("FF0020");
                    ob.impostorColor = HexToColor("FF0020");
                    break; 
                }
            }
            else
            {
                int tipo = Random.Range(1, 4);
                switch (tipo)
                {
                    case 1:
                        ob.tipo = "Tipo1";
                        sr.color = HexToColor("FFF700");
                        break;
                    case 2:
                        ob.tipo = "Tipo2";
                        sr.color = HexToColor("00FFFC");
                        break;
                    case 3:
                        ob.tipo = "Tipo3";
                        sr.color = HexToColor("FF0020");
                        break;
                }
            }
        }
    }

    GameObject ChooseRandomPrefab()
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            default:
            case 0: return prefabTipo1;
            case 1: return prefabTipo2;
            case 2: return prefabTipo3;
        }
    }

    private Vector3 GetValidPosition()
    {
        Vector3 pos;
        int attempts = 0;
        do
        {
            pos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                0f
            );
            attempts++;
        }
        while (!IsFarEnough(pos) && attempts < 50);

        usedPositions.Add(pos);
        return pos;
    }

    private bool IsFarEnough(Vector3 pos)
    {
        foreach (var used in usedPositions)
            if (Vector3.Distance(pos, used) < minDistance)
                return false;
        return true;
    }

    Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
            return color;
        return Color.white;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2f, (spawnAreaMin.y + spawnAreaMax.y) / 2f, 0f);
        Vector3 size = new Vector3(Mathf.Abs(spawnAreaMax.x - spawnAreaMin.x), Mathf.Abs(spawnAreaMax.y - spawnAreaMin.y), 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}
