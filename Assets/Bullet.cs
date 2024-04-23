using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CartoonFX;

public class Bullet : MonoBehaviour
{
    [System.NonSerialized] public MonsterAI owner;
    [System.NonSerialized] public Vector2 direction;
    [System.NonSerialized] public Transform target;
    [System.NonSerialized] public SpriteRenderer spriteRenderer;
    [System.NonSerialized] public Sprite defaultImg;
    [System.NonSerialized] public AudioClip relectSound;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    public bool cirt;
    public bool applyFlyFx;
    bool canDealDamage = true;
    bool reflected = false;
    public HitParam hitParam;

    //private void Awake()
    //{
    //    hitParam = new HitParam()
    //    {damage = 10f};
    //}

    protected virtual void OnEnable()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        //if (collider == null) collider = GetComponent<Collider2D>();
        //collider.enabled = true;
        canDealDamage = true;
        reflected = false;
        EasyEffect.Appear(gameObject, 0f, 1f);
    }
    protected virtual void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime * Time.timeScale, Space.World);
        transform.right = direction;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = reflected ? owner.gameObject.layer : LayerMask.NameToLayer(owner.TargetLayer);
        if (collision.gameObject.layer == layer && canDealDamage)
        {
            canDealDamage = false;
            collision.TryGetComponent<MonsterAI>(out var target);
            if (target.GetComponent<GainSkill>() != null && Random.Range(0f,1f) < Constants.BLOCK_SHIELD_PERCENT)
            {
                direction = Vector2.Reflect(direction,-Vector2.up);
                canDealDamage = true;
                reflected = true;
                hitParam = new HitParam()
                {
                    damage = 10f,
                    owner = target.transform,
                };
                ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/ReflectFx", transform.position).SetSortingOrder(target.MeshOrder.meshRenderer.sortingOrder);
                ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/ShieldFx", target.transform.position);
                if (relectSound != null)
                {
                    SFXSystem.Instance.Play(relectSound);
                }
            }
            else
            {
                owner.HitParam.crit = false;
                if (reflected)
                {
                    target.TakeDame(hitParam);
                } else
                {
                    owner.HitParam.damage = owner.battleStat.damage;
                    target.TakeDame(hitParam);
                }
                Disspear();
            }
        }
    }
    protected virtual void Disspear()
    {
        gameObject.SetActive(false);
        var explosePath = GeneralUltility.BuildString("", "Vfx/", LayerMask.LayerToName(owner.gameObject.layer), "BulletExplose");
        var explosion = ObjectPool.Instance.GetGameObjectFromPool(explosePath, transform.position);        
        if (!owner.IsEnemy)
        {
            explosion.GetComponent<BulletExplose>().Explode(owner.HitParam);
            var controller = explosion.GetComponent<ParticalSystemController>();
            controller.ChangeColor(owner.allyColor);
        }
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
