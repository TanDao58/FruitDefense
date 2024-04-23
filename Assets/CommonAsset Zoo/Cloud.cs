using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames{
public class Cloud : MonoBehaviour
{
    public List<Sprite> sprites;

    public Vector2 BOTTOM_LEFT;
    public Vector2 TOP_RIGHT;
    public Vector2 moveVector = new Vector2(1f, 0f);

    public float MIN_SPEED = 1f;
    public float MAX_SPEED = 2f;

    float BASE_SPEED = 0.2f;
    float speed = 0.05f;

    private void Start() {
        speed = BASE_SPEED * Random.Range(MIN_SPEED, MAX_SPEED);
        transform.position = new Vector3(Random.Range(BOTTOM_LEFT.x, TOP_RIGHT.x), Random.Range(BOTTOM_LEFT.y, TOP_RIGHT.y), transform.position.z);
        if (sprites != null && sprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Count)];
        }
    }

    void Update()
    {
        transform.position += (Vector3)moveVector * speed * Time.deltaTime;

        if (transform.position.x > TOP_RIGHT.x || transform.position.y > TOP_RIGHT.y) {
            transform.position = new Vector3(Random.Range(BOTTOM_LEFT.x, TOP_RIGHT.x), Random.Range(BOTTOM_LEFT.y, TOP_RIGHT.y), transform.position.z);
            speed = BASE_SPEED * Random.Range(MIN_SPEED, MAX_SPEED);
        }
    }
}
}