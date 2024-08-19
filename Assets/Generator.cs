using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gen
{

    [MenuItem("MyTools/Generate")]
    public static void Generate()
    {
        Generator generator = GameObject.FindObjectOfType<Generator>();

        GameObject go;
        for (int i = 0; i < generator.maxCount; i++)
        {
            int prefabIndex = 0;// Random.Range(1, generator.prefabs.Length);
            Object prefab = AssetDatabase.LoadAssetAtPath(generator.prefabs[prefabIndex], typeof(GameObject));
            go = PrefabUtility.InstantiatePrefab(prefab, SceneManager.GetActiveScene()) as GameObject;
            go.transform.position = RandomPointInBounds(generator.boxCollider.bounds);
            Vector3 newRot = Vector3.zero;
            if (prefabIndex == 1 || prefabIndex == 4) newRot.x = 90;
            if (prefabIndex == 3 || prefabIndex == 5) go.transform.localScale = Vector3.one * 2;
            newRot.y = Random.Range(0, 360);
            go.transform.eulerAngles = newRot;
            go.name = i.ToString();
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}

public class Generator : MonoBehaviour
{
    public BoxCollider boxCollider;
    public string[] prefabs;
    public float maxCount;
}
