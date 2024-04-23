using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance;
    private Dictionary<string, Queue<GameObject>> allPools = new Dictionary<string, Queue<GameObject>>();

    [SerializeField] private Transform spawnedObjectsParent;

    private void Awake() {
        Instance = this;
    }

    public GameObject GetGameObjectFromPool(string objectName, Vector3 position) {
        var allPoolList = Instance.allPools;

        if (!allPoolList.ContainsKey(objectName)) {
            allPoolList.Add(objectName, new Queue<GameObject>());
        }
        Queue<GameObject> queue = allPoolList[objectName];
        foreach (var obj in queue) {
            if (obj.activeSelf == false) {
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }

        var objBase = Resources.Load<GameObject>(objectName);
        var created = Instantiate(objBase, position, Quaternion.identity);
        created.transform.localScale = objBase.transform.localScale;
        allPoolList[objectName].Enqueue(created);
        //created.transform.SetParent(Instance.transform);
        return created;
    }

    public T GetGameObjectFromPool<T>(T inputObj, Vector3 position, Transform parentTransform = null) where T : Component
    {
        if (!allPools.ContainsKey(inputObj.name))
        {
            allPools.Add(inputObj.name, new Queue<GameObject>());
        }
        Queue<GameObject> list = allPools[inputObj.name];
        foreach (var obj in list)
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.SetParent(parentTransform);
                if (obj.TryGetComponent<T>(out var returnType))
                {
                    return returnType;
                }
                else
                {
                    Debug.LogError("Can't find component with your gameobject, please check the gameobject you want to get or use orther overload instead");
                    return default;
                }
            }
        }

        var created = Instantiate(inputObj, position, Quaternion.identity, parentTransform);
        allPools[inputObj.name].Enqueue(created.gameObject);
        return created;
    }

    public T GetGameObjectFromPool<T>(string objectName, Vector3 position)
    {
        var allPoolList = Instance.allPools;

        if (!allPoolList.ContainsKey(objectName))
        {
            allPoolList.Add(objectName, new Queue<GameObject>());
        }
        Queue<GameObject> queue = allPoolList[objectName];
        foreach (var obj in queue)
        {
            if (obj == null) continue;
            if (obj.activeSelf == false)
            {
                obj.transform.position = position;
                obj.SetActive(true);
                if(obj.TryGetComponent<T>(out var returnType))
                {
                    return returnType;
                }            
                else
                {
                    Debug.LogError("Can't find component with your gameobject, please check the gameobject you want to get or use orther overload instead");
                    return default;
                }
            }
        }

        var objBase = Resources.Load<GameObject>(objectName);
        var created = Instantiate(objBase, position, Quaternion.identity);
        created.transform.localScale = objBase.transform.localScale;
        allPoolList[objectName].Enqueue(created);
        if (created.TryGetComponent<T>(out var newReturnType))
        {
            return newReturnType;
        }
        else
        {
            Debug.LogError("Can't find component with your gameobject, please check the gameobject you want to get or use orther overload instead");
            return default;
        }
    }
    public GameObject GetGameObjectFromPool(GameObject gameObject, Vector3 position)
    {
        var allPoolList = Instance.allPools;

        if (!allPoolList.ContainsKey(gameObject.name))
        {
            allPoolList.Add(gameObject.name, new Queue<GameObject>());
        }
        Queue<GameObject> queue = allPoolList[gameObject.name];
        foreach (var obj in queue)
        {
            if (obj.activeSelf == false)
            {
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }

        //var objBase = Resources.Load<GameObject>(gameObject.name);
        var created = Instantiate(gameObject, position, Quaternion.identity);
        created.transform.localScale = gameObject.transform.localScale;
        allPoolList[gameObject.name].Enqueue(created);
        //created.transform.SetParent(Instance.transform);
        return created;
    }
}
