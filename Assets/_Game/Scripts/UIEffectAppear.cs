using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames
{   
    public class UIEffectAppear : UIEffectBase
    {
        [SerializeField] private float endSize = 1f;
        [SerializeField] private ParticleSystem uiParticle;
        [SerializeField] private Canvas canvas;
        public override void DoEffect()
        {
            //Debug.Log($"do effect appear on {gameObject}, easy effect");
            EasyEffect.Appear(gameObject, 0f, endSize, effectTime,1.2f,() => 
            {
                if (uiParticle != null)
                    ObjectPool.Instance.GetGameObjectFromPool(uiParticle, transform.position, canvas.transform); 
            });
        }
        public override void Prepare()
        {
            //Debug.Log($"prepare effect appear on {gameObject}, set game object active to false");
            gameObject.SetActive(false);
        }
    }
}