using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    int amount = 1000;

    private void Start() {
        for (int i = 0; i < amount; i++) {
            ObjectPool.Instance.GetGameObjectFromPool("MonsterAI", new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
        }

        Application.targetFrameRate = 60;
    }
}
