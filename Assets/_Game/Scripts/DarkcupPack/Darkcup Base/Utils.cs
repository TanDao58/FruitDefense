using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Linq.Expressions;

public static class Utils
{
    public static WaitForSeconds WAIT_1_SECOND = new WaitForSeconds(1f);
    public static WaitForSeconds WAIT_2_SECOND = new WaitForSeconds(2f);
    public static WaitForSeconds WAIT_3_SECOND = new WaitForSeconds(3f);
    public static WaitForSeconds WAIT_0_DOT_02_SECOND = new WaitForSeconds(0.02f);
    public static WaitForSeconds WAIT_0_DOT_2_SECOND = new WaitForSeconds(0.2f);
    public static WaitForSeconds WAIT_0_DOT_1_SECOND = new WaitForSeconds(0.1f);

    #region GET_CHILD_COMPONENT
    public static T GetChildComponent<T>(this GameObject obj, string name)
    {
        if (name.Contains("/"))
        {
            var find = GetGameObjectWithPath(obj, name);
            if (find == null)
            {
                return default(T);
            }
            else
            {
                return GetComponentFromObject<T>(find.gameObject);
            }
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i);
            if (child.name == name)
            {
                return GetComponentFromObject<T>(child.gameObject);
            }
        }
        Debug.LogError($"Not found object with name {name} in {obj.name}'s childrens!");
        return default(T);
    }

    public static T GetComponentFromObject<T>(GameObject baseObj)
    {
        if (typeof(T) == typeof(GameObject))
        {
            return (T)((object)baseObj.gameObject);
        }
        var result = baseObj.GetComponent<T>();

        if (result == null)
        {
            Debug.LogError("Component not found!");
        }
        return result;
    }
    public static T GetOrCreateComponent<T>(this GameObject obj) where T : Component
    {
        var result = obj.GetComponent<T>();
        if (result == null)
        {
            result = obj.AddComponent<T>();
        }
        return result;
    }

    private static string[] _paths;

    public static GameObject GetGameObjectWithPath(this GameObject root, string path)
    {
        _paths = path.Split('/');
        GameObject result = root;
        for (int i = 0; i < _paths.Length; i++)
        {
            result = FindObjectInChildren(result, _paths[i]);
            if (!result)
            {
                Debug.LogError($"Cant find {_paths[i]} in {result.name}");
                return null;
            }
        }

        return result;
    }

    public static GameObject FindObjectInChildren(GameObject root, string name)
    {
        for (int i = 0; i < root.transform.childCount; i++)
        {
            if (root.transform.GetChild(i).name == name)
            {
                return root.transform.GetChild(i).gameObject;
            }
        }
        Debug.LogError($"Cant find {name} in {root.name}");
        return null;
    }

    public static GameObject FindObjectIncludingInactive(GameObject root, string name)
    {
        if (root.transform.childCount <= 0)
        {
            Debug.LogError($"{root} have no child!");
            return null;
        }
        GameObject result = null;

        if (root.name == name)
        {
            return root;
        }

        for (int i = 0; i < root.transform.childCount; i++)
        {
            result = FindObjectIncludingInactive(root.transform.GetChild(i).gameObject, name);
            if (result)
            {
                return result;
            }
        }

        Debug.LogError($"Cant find {name} in {root.name}");
        return null;
    }

    public static GameObject FindObjectInRoot(string name)
    {
        return GameObject.Find(name);
    }

    public static GameObject FindObjectInRootIncludingInactive(string name)
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            Debug.LogError("No scene loaded");
            return null;
        }

        var roots = new List<GameObject>();
        scene.GetRootGameObjects(roots);

        foreach (GameObject obj in roots)
        {
            if (obj.transform.name == name) return obj;
        }

        Debug.LogError($"Cant find {name} in root");
        return null;
    }
    #endregion

    public static Vector3 Where(this Vector3 origin, float x = Mathf.Infinity, float y = Mathf.Infinity, float z = Mathf.Infinity)
    {
        Vector3 result = origin;
        if (x != Mathf.Infinity)
        {
            result.x = x;
        }
        if (y != Mathf.Infinity)
        {
            result.y = y;
        }
        if (z != Mathf.Infinity)
        {
            result.z = z;
        }
        return result;
    }

    public static Transform Nearest(GameObject[] enemies, Transform pivot, float maxRange)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<IDamable>().IsDead()) continue;
            if (enemy.gameObject.activeSelf == false) continue;

            float distanceToEnemy = Vector3.Distance(pivot.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.gameObject;
            }
        }

        if (nearestEnemy != null && shortestDistance <= maxRange)
        {
            return nearestEnemy.transform;
        }
        else
        {
            return null;
        }
    }

    public static Transform Nearest(MonoBehaviour[] enemies, Transform pivot, float maxRange)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (MonoBehaviour enemy in enemies)
        {
            if (enemy.GetComponent<IDamable>().IsDead()) continue;
            if (enemy.gameObject.activeSelf == false) continue;

            float distanceToEnemy = Vector3.Distance(pivot.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.gameObject;
            }
        }

        if (nearestEnemy != null && shortestDistance <= maxRange)
        {
            return nearestEnemy.transform;
        }
        else
        {
            return null;
        }
    }

    public static float DistanceTo(this Transform me, Transform other)
    {
        return Vector2.Distance(me.position, other.position);
    }
}