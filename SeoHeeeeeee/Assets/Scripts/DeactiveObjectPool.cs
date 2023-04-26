using System.Collections.Generic;
using UnityEngine;

public class DeactiveObjectPool : MonoBehaviour
{
    public static DeactiveObjectPool current;

    [Tooltip("Assign the box prefab.")]
    public Indicator pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = false;

    [SerializeField]
    List<Indicator> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<Indicator>();

        for (int i = 0; i < pooledAmount; i++)
        {
            Indicator obj = Instantiate(pooledObject);
            obj.transform.SetParent(transform, false);
            obj.Activate(false);
            pooledObjects.Add(obj);
        }
    }

    public Indicator GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            Indicator obj = Instantiate(pooledObject);
            obj.transform.SetParent(transform, false);
            obj.Activate(false);
            pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator obj in pooledObjects)
        {
            obj.Activate(false);
        }
    }
}
