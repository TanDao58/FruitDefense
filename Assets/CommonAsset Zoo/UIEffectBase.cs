 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DarkcupGames {

    public abstract class UIEffectBase : MonoBehaviour {
        public float effectTime = 0.2f;

        public abstract void Prepare();

        public abstract void DoEffect();
    }
}