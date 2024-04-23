using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectEffect : MonoBehaviour
{
    public Transform startPosition;
    public Transform destination;
    public List<GameObject> particles;

    [Header("Parameter, can adjust or leave default")]
    public float RANGE = 0.2f;
    public float STEP = 0.1f;
    public float DELAY = 0.5f;
    public float FLY_TIME_1 = 0.3f;
    public float FLY_TIME_2 = 1f;

    public void DoEffect() {
        for (int i = 0; i < particles.Count; i++) {
            float distance = Vector2.Distance(transform.position, destination.position);
            float range = 0.2f * distance;
            Vector2 pos = UnityEngine.Random.insideUnitCircle.normalized * range + (Vector2)transform.position;
            float distance2 = Vector2.Distance(pos, destination.position);
            DoEffect(particles[i], pos, i*STEP, distance2 / distance * FLY_TIME_2);
        }
    }

    void DoEffect(GameObject particle, Vector2 pivot, float delay, float flyTime) {
        particle.transform.position = startPosition.position;
        Sequence sequence = DOTween.Sequence()
            .AppendInterval(Random.Range(0f, delay))
            .AppendCallback(() => {
                particle.gameObject.SetActive(true);
            })
            .Append(particle.transform.DOMove(pivot, FLY_TIME_1 * Random.Range(0.75f,1.25f)))
            .AppendInterval(DELAY * Random.Range(0.75f, 1.25f))
            .Append(particle.transform.DOMove(destination.position, flyTime))
            .AppendCallback(() => {
                particle.gameObject.SetActive(false);
            });
    }
}