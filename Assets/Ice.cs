using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [System.NonSerialized] public float freezeTime;
    [System.NonSerialized] public MonsterAI target;
    public SpriteRenderer spriteRenderer;

    private void Update()
    {       
        freezeTime -= Time.deltaTime;
        gameObject.SetActive(freezeTime > 0);
    }
    private void OnDisable()
    {
        ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/IceBreak", transform.position).SetSortingOrderWithChildren(spriteRenderer.sortingOrder);
    }
}
