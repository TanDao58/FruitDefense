using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
public class BlinkingEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int blinkingTimes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Blink()
    {
        EasyEffect.Appear(this.gameObject, 15f, 1f);
    }
}
