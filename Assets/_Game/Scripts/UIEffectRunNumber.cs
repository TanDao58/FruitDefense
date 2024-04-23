using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DarkcupGames
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIEffectRunNumber : UIEffectBase
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private long targetValue;
        [SerializeField] private long currentValue;
        [SerializeField] private long addValue;
        [SerializeField] private bool run;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        public override void DoEffect()
        {
            run = true;
        }

        public override void Prepare()
        {
            text = GetComponent<TextMeshProUGUI>();
            long.TryParse(text.text, out targetValue);
            float div = Time.smoothDeltaTime == 0 ? 0.02f : Time.smoothDeltaTime;
            addValue = (long)(targetValue / (effectTime / div));
            if (addValue <= 0) addValue = 1;
            currentValue = 0;
            text.text = "";
            run = false;
        }

        private void Update()
        {
            if (!run) return;
            currentValue += addValue;
            if (currentValue > targetValue)
            {
                currentValue = targetValue;
                run = false;
            }
            text.text = currentValue.ToString();
        }

        private void OnDisable()
        {
            currentValue = 0;
        }
    }
}