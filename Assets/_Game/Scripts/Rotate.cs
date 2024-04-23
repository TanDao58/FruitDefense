using UnityEngine;
using System;

namespace DarkcupGames
{
    public class Rotate : MonoBehaviour
    {
        public float angle = 1f;

        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, angle) * Time.deltaTime);
        }
    }
}