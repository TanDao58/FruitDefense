using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class ParticalSystemController : MonoBehaviour
{
    private ParticleSystem particle;
    private ParticleSystemRenderer particleRender;

    private Vector3 particalPivot;

    public ParticleSystem Particle => particle;

    public void Init()
    {
        if (particle == null) particle = GetComponent<ParticleSystem>();
        if (particleRender == null)
        {
            particleRender = GetComponent<ParticleSystemRenderer>();
            particalPivot = particleRender.pivot;
        }
    }

    public void Flip(float facingValue)
    {
        Init();
        var facing = particleRender.flip;
        if (facingValue < 0)
            facing.x = 0;
        else
            facing.x = 1;
        particleRender.flip = facing;
    }
    public void FlipWithPivot(float facingValue)
    {
        Init();
        var facing = particleRender.flip;
        if (facingValue < 0)
        {
            facing.x = 0;
            particleRender.pivot = particalPivot;
        }
        else
        {
            facing.x = 1;
            particleRender.pivot = new Vector3(-particalPivot.x, particalPivot.y, particalPivot.z);
        }
        particleRender.flip = facing;
    }

    public void SetSortingOrder(LayerMask targetLayer)
    {
        Init();
        particleRender.sortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(targetLayer));
    }
    public void SetSortingOrder(int order)
    {
        Init();
        particleRender.sortingOrder = order + 1;
    }

    public void ChangeColor(Color color)
    {
        Init();
        var mainModlue = particle.main;
        mainModlue.startColor = color;
    }
    public void SetSortingOrderWithChildren(int order)
    {
        Init();
        particleRender.sortingOrder = order + 100;
        if (transform.childCount == 0) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            var controller = transform.GetChild(i).GetComponent<ParticalSystemController>();
            if (controller == null) continue;
            controller.SetSortingOrder(order);
        }
    }
}
