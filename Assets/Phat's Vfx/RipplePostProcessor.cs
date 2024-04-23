using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePostProcessor : MonoBehaviour
{
    public static RipplePostProcessor Intansce;
    [SerializeField]private float exitTime;
    public Material RippleMaterial;
    public float MaxAmount;

    private float amount;
    private bool startCounting;
    private void Awake()
    {
        Intansce = this;
    }
    private void Start()
    {
        enabled = false;
        amount = MaxAmount;
    }
    void Update()
    {       
        RippleMaterial.SetFloat("_Amount", amount);
        if(startCounting)
        {
            amount -= 1;
            if (amount <= 0) startCounting = false;
        }
    }

    public void CallRippleEffect(Vector2 positionOnScreen)
    {
        enabled = true;
        //if (amount > 0) return;
        amount = MaxAmount;
        RippleMaterial.SetFloat("_CenterX", positionOnScreen.x);
        RippleMaterial.SetFloat("_CenterY", positionOnScreen.y);
        startCounting = true;
    }
    
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, RippleMaterial);
    }
}
