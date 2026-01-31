using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs de cada tipo")]
    public GameObject prefabTipo1;
    public GameObject prefabTipo2;
    public GameObject prefabTipo3;
    public GameObject prefabImpostor;

    [Header("Número de espíritus")]
    public int numberItems = 20;

    [Header("Área de spawn (rectángulo)")]
    public Vector2 spawnAreaMin = new Vector2(-5, -5);
    public Vector2 spawnAreaMax = new Vector2(5, 5);

    [Header("Distancia mínima entre objetos")]
    public float minDistance = 1.0f;

    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        Populate(numberItems);
        SpawnImpostor();
    }

    public GameObject Spawn(GameObject prefab)
    {
        Vector3 pos = GetValidPosition();
        return Instantiate(prefab, pos, Quaternion.identity);
    }

    public void Populate(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            int tipo = Random.Range(1, 4); // 1, 2 o 3
            GameObject prefab = null;

            switch (tipo)
            {
                case 1: prefab = prefabTipo1; break;
                case 2: prefab = prefabTipo2; break;
                case 3: prefab = prefabTipo3; break;
            }

            Spawn(prefab);
        }
    }

    public void SpawnImpostor()
    {
        Spawn(prefabImpostor);
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
        {
            if (Vector3.Distance(pos, used) < minDistance)
                return false;
        }
        return true;
    }

    void OnDrawGizmos() { 
        Gizmos.color = Color.green; // Calculamos centro y tamaño del rectángulo 
        Vector3 center = new Vector3( (spawnAreaMin.x + spawnAreaMax.x) / 2f, (spawnAreaMin.y + spawnAreaMax.y) / 2f, 0f ); 
        Vector3 size = new Vector3( Mathf.Abs(spawnAreaMax.x - spawnAreaMin.x), Mathf.Abs(spawnAreaMax.y - spawnAreaMin.y), 0.1f ); 
        Gizmos.DrawWireCube(center, size); 
    }
}
