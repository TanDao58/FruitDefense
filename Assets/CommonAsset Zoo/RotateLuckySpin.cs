using UnityEngine;
using System;

namespace DarkcupGames
{
    public class RotateLuckySpin : MonoBehaviour
    {
        public GameObject[] rewards;
        public GameObject locationCheck;
        public float spinSpeed = 500f;
        public Action<Transform> onReceiveReward;

        private void Start()
        {
            enabled = false;
        }
        private void Update()
        {
            spinSpeed -= Time.deltaTime * 50f;
            transform.Rotate(new Vector3(0, 0, 1f).normalized * spinSpeed * Time.deltaTime);
            if (spinSpeed <= 0f)
            {
                GiveReward();
                enabled = false;
                return;
            }
        }
        protected void GiveReward()
        {
            GameObject nearest = rewards[0];
            for (int i = 0; i < rewards.Length; i++)
            {
                if (Vector2.Distance(rewards[i].transform.position, locationCheck.transform.position) < Vector2.Distance(nearest.transform.position, locationCheck.transform.position))
                {
                    nearest = rewards[i];
                }
            }
            if (onReceiveReward != null)
            {
                onReceiveReward(nearest.transform);
            }
        }
    }
}