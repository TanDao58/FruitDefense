using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeshOrderFixer : MonoBehaviour
{
    [System.NonSerialized] public MeshRenderer meshRenderer;
    [System.NonSerialized] public SpriteRenderer spriteRenderer;
    [System.NonSerialized] public int order;
    private Material white;

    private Color defaultColor;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            defaultColor = spriteRenderer.color;
        }
        if(spriteRenderer == null)
        {
            white = new Material(meshRenderer.material);
            
        }
    }
       

    void Update()
    {
        if(meshRenderer != null)
        {
            meshRenderer.sortingOrder = -(int)(transform.position.y * 100);
            order = meshRenderer.sortingOrder;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
            order = spriteRenderer.sortingOrder;
        }               
    }

    public void SetWhite()
    {
        if (meshRenderer != null)
        {
            //meshRenderer.material = white;
            meshRenderer.material.SetFloat("_FillPhase", 0.7f);
        }
        if (spriteRenderer != null)
        {
            float alpha = spriteRenderer.color.a;
            spriteRenderer.color = new Color(1,0,0,alpha);
        }
              
    }
    public void SetNormal()
    {
        if (meshRenderer != null)
        {
            LeanTween.value(1, 0, 0.1f).setOnUpdate((float value) => { 
                if (meshRenderer) {
                    meshRenderer.material.SetFloat("_FillPhase", value);
                }
            });
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.DOColor(defaultColor, 0.1f) ;
        }
    }
}
