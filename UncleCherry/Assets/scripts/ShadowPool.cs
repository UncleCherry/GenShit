using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject ShadowPrefab;

    public int ShadowCount;

    private Queue<GameObject> AvailableObjects = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < ShadowCount; i++)
        {
            var newshadow = Instantiate(ShadowPrefab);
            newshadow.transform.SetParent(transform);

            ReturnPool(newshadow);
        }
    }
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        AvailableObjects.Enqueue(gameObject);
    }

    public GameObject GetFromPool()
    {
        if (AvailableObjects.Count == 0)
        {
            FillPool();
        }
        var outShadow = AvailableObjects.Dequeue();
        outShadow.SetActive(true);
        return outShadow;
    }
}
