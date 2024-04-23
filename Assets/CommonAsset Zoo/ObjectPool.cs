using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        private Dictionary<string, List<GameObject>> allPools = new Dictionary<string, List<GameObject>>();

        [SerializeField] private Transform spawnedObjectsParent;
        GameObject source;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetGameObjectFromPool(string objectName, Vector3 position)
        {
            GameObject created = FindOrCreate(objectName);
            created.transform.position = position;
            //allPools[objectName].Add(created);
            //created.transform.localScale = source.transform.localScale;
            //created.transform.SetParent(spawnedObjectsParent);
            return created;
        }

        public GameObject GetGameObjectFromPool(string objectName, Transform parentTransform)
        {
            GameObject created = FindOrCreate(objectName);
            created.transform.SetParent(parentTransform);
            //allPools[objectName].Add(created);
            //created.transform.localScale = source.transform.localScale;
            //created.transform.SetParent(spawnedObjectsParent);
            return created;
        }

        public T GetGameObjectFromPool<T>(T inputObj,Vector3 position , Transform parentTransform = null) where T : Component
        {
            if(!allPools.ContainsKey(inputObj.name))
                {
                    allPools.Add(inputObj.name, new List<GameObject>());
                }
            List<GameObject> list = allPools[inputObj.name];
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

            var created = Instantiate(inputObj, position,Quaternion.identity , parentTransform);
            allPools[inputObj.name].Add(created.gameObject);
            return created;
        }

        public GameObject FindOrCreate(string objectName)
        {
            if (!allPools.ContainsKey(objectName))
            {
                allPools.Add(objectName, new List<GameObject>());
            }
            List<GameObject> list = allPools[objectName];
            foreach (var obj in list)
            {
                if (obj.activeSelf == false)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            source = Resources.Load<GameObject>(objectName);
            var created = Instantiate(source, Vector3.zero, source.transform.rotation);
            allPools[objectName].Add(created);
            return created;
        }

        public T GetGameObjectFromPool<T>(string objectName, Vector3 position)
        {
            var allPoolList = Instance.allPools;

            if (!allPoolList.ContainsKey(objectName))
            {
                allPoolList.Add(objectName, new List<GameObject>());
            }
            List<GameObject> queue = allPoolList[objectName];
            foreach (var obj in queue)
            {
                if (obj.activeSelf == false)
                {
                    obj.transform.position = position;
                    obj.SetActive(true);
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

            var objBase = Resources.Load<GameObject>(objectName);
            var created = Instantiate(objBase, position, Quaternion.identity);
            created.transform.localScale = objBase.transform.localScale;
            allPoolList[objectName].Add(created);
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
    }
}
