using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames{
[RequireComponent(typeof(Rigidbody2D))]
public class DCamera : MonoBehaviour
{
    public GameObject target;
    public float maxDistance = 4f;
    public float speed = 25f;

    public GameObject background;

    private Vector3 vectorToTarget;
    private Rigidbody2D rigidbody2D;
   
    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.isKinematic = true;
        rigidbody2D.gravityScale = 0;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPos = target.transform.position;
        newPos.z = transform.position.z;
        transform.position = newPos;
        //GoToTargetPosition(target, maxDistance);
    }

    void GoToTargetPosition(GameObject target, float maxDistanceToTarget)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget <= maxDistanceToTarget)
        {
            rigidbody2D.velocity = new Vector3(0f, 0f);
            return;
        }
        else
        {
            vectorToTarget = target.transform.position - transform.position;
            vectorToTarget = vectorToTarget * speed / distanceToTarget;
            rigidbody2D.velocity = vectorToTarget;
        }
    }

    public void MoveTo(Transform target)
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
    }
}
}
