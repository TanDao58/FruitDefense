using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames {
    public class UIEffect : MonoBehaviour {
        public bool doEffectOnEnable = false;

        protected UIEffectBase[] effects;
        protected float nextChangeEffect;
        int current = -1;

        private void Awake() {
            effects = GetComponentsInChildren<UIEffectBase>(true);
            if (GetComponentsInChildren<UIEffect>(true).Length > 1)
            {
                GeneralUltility.Log("Error: nested effect found in game object " ,gameObject.name);
            }
        }

        private void OnEnable()
        {
            if (doEffectOnEnable)
            {
                DoEffect();
            }
        }

        private void Update() {
            if (current == -1) return;

            if (Time.unscaledTime > nextChangeEffect) {
                if (current == effects.Length - 1) {
                    current = -1;
                    return;
                }

                current++;
                nextChangeEffect = Time.unscaledTime + effects[current].effectTime;
                effects[current].DoEffect();
            }
        }

        public void DoEffect(bool inclueAll = true) 
        {
            gameObject.SetActive(true);
            effects = GetComponentsInChildren<UIEffectBase>(inclueAll);
            if (effects.Length == 0) return;

            for (int i = 0; i < effects.Length; i++) {
                effects[i].Prepare();
            }
            current = 0;
            effects[0].DoEffect();
            nextChangeEffect = Time.unscaledTime + effects[0].effectTime;
        }

        [ContextMenu("Get All Effects")]
        public void GetAllEffect()
        {
            effects = GetComponentsInChildren<UIEffectBase>(true);
        }
    }
}