using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames{
[RequireComponent(typeof(Rigidbody2D))]
public class CollectCoinEffect : MonoBehaviour {
    public float speed = 0.1f;
    public float pushForce = 50f;
    public float flySpeed = 50f;
    public float decreaseRatioSpeed = 2f;

    [System.NonSerialized] public Vector3 direction;
    private Rigidbody2D rb;
    private Vector2 randomDirection;
    private float ratio = 1;

    private float flyTime = 0.2f;

    public Vector3 destination;
    public System.Action doneAction = null;

    private void OnEnable() {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        flyTime = 0.2f;
        ratio = 1;

        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    Vector3 dir;

    public void SetDestination(Vector3 destination) {
        this.destination = destination;
    }

    protected void Update() {
        ratio -= Time.deltaTime * decreaseRatioSpeed * Time.timeScale;

        if (Vector2.Distance(transform.position, destination) < 0.4f) {
            doneAction?.Invoke();
            gameObject.SetActive(false);
        }
        if (ratio > 0) {
            dir = (Vector3)randomDirection.normalized * pushForce * ratio;
            dir += (destination - transform.position).normalized * (flySpeed * (1 - ratio));
        } else {
            dir = (destination - transform.position).normalized * (flySpeed * (1 - ratio));
        }

        Vector3 targetPos = transform.position + dir * Time.deltaTime * speed;
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.9f);
    }
    private void FlyToTarget(Vector3 targetPosition, float speed) {
        direction = targetPosition - transform.position;
        var route = randomDirection.normalized * ratio + (Vector2)direction.normalized * (1 - ratio);
        rb.velocity += route * speed * Time.timeScale;
    }
}
}