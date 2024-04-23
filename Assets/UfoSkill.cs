using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
public class UfoSkill : MonoBehaviour
{
    //public Transform target;

    [System.NonSerialized] public SpriteRenderer spriteRenderer;
    [System.NonSerialized] public MonsterAI ufoAI;
    [SerializeField] private float waitTime;
    SkeletonAnimation skeletonAnimation;
    private MonsterEffect ufoEffect;
    private bool catching;
    [SerializeField]private float currentTime;
    private Vector2 baseScale;
    private void OnEnable()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        catching = true;
        currentTime = waitTime;
        transform.localScale = baseScale;
        if(spriteRenderer == null) spriteRenderer = gameObject.GetChildComponent<SpriteRenderer>("skeletonAnim");
        if (ufoAI == null) ufoAI = GetComponent<MonsterAI>();
        if (ufoEffect == null) ufoEffect = GetComponent<MonsterEffect>();
    }
    private void Start()
    {
        baseScale = transform.localScale;
    }
    private void Update()
    {
        if (ufoAI.AttackTarget == null)
        {
            EasyEffect.Disappear(gameObject, 1f, 0f);          
            ufoAI.enabled = false;
            enabled = false;
            return;
            //var newTarget = Physics2D.OverlapCircle(transform.position, ufoAI.battleStat.visionRange, LayerMask.GetMask("Ally"));
            //if (newTarget != null)
            //{

            //target = newTarget.transform;
            //catching = true;
            //currentTime = waitTime;
            // }
            //else
            //{
            //    transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * Time.timeScale;
            //    gameObject.SetActive(transform.localScale.x > 0);
            //}
        }
        
        transform.position = Vector2.MoveTowards(transform.position, ufoAI.AttackTarget.transform.position + new Vector3(0, 1.5f, 0), ufoAI.battleStat.speed * Time.deltaTime * Time.timeScale);
        var lockOnAlly = Physics2D.Raycast(transform.position, -transform.up, 1.5f, LayerMask.GetMask("Ally"));
        if (!ufoAI.IsDead() && lockOnAlly && catching)
        {
            currentTime -= Time.deltaTime;
            if(lockOnAlly.collider.TryGetComponent<MonsterAI>(out var target))
            {
                skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = target.MeshOrder.order + 1;
            }
            if(currentTime <= 0 && ufoAI.AttackTarget.gameObject.activeInHierarchy)
            {
                CatchTarget(ufoAI.AttackTarget);
            }          
        }
    }
    public void CatchTarget(Transform target)
    {
        var targetAI = target.GetComponent<MonsterAI>();
        EasyEffect.UfoCatch(target.gameObject, transform.position.y, 0.5f, () => 
        {
            if (SelectManagerGameplay.Instance.spawnedHero.Contains(targetAI)) SelectManagerGameplay.Instance.spawnedHero.Remove(targetAI);
            Gameplay.Intansce.UpdateHeroAmount();
            EasyEffect.Disappear(gameObject, 1f, 0f,0.2f,1.2f,()=> 
            {
                ufoEffect.SetNullAtacker();
            });
        });
        targetAI.enabled = false;
        catching = false;
        ufoAI.enabled = false;
        enabled = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * 1.5f);
    }
}
