using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
using Spine;
using System;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using UnityEditor;
using DG.Tweening;
using DamageNumbersPro;

public class MonsterAI : MonoBehaviour, IDamable
{
    protected const string LAYER_ENEMY = "Enemy";
    protected const string LAYER_ALLY = "Ally";

    public BattleStat battleStat;
    [System.NonSerialized] public float lastAttack;

    public Sprite portrait;
    public MonsterData monsterData;
    public UnityEvent idleAlly;
    public UnityEvent idleEnemy;
    public UnityEvent attackEvent;
    public UnityEvent dieEvent;
    public UnityEvent skillEvent;
    public UnityEvent takeDamgeEvent;
    public string idleAnimationName;
    public string attackAnimationName;
    public string dieAnimationName;
    public string runAnimationName;
    public float moneyGain;
    public int moneyCost;
    public int atkToSkill = 3;
    public float cooldownTime;
    public int amountToLevelUp;
    public int baseAmountToLevelUp;
    public float coinToUpgrade;
    public float baseMoneyToUpgrade;
    public bool isBoss;
    public bool targetable;
    // enemy
    public List<string> enemyTags;
    protected Hp healthBar;
    protected TextMeshPro txtName;
    protected HitParam hitParam;
    [SerializeField] protected Transform attackTarget;
    protected AnimationSetter animationSetter;
    protected DamgeCounter damgeCounter;
    protected MonsterEffect monsterEffect;
    protected Transform attacker;
    protected Collider2D col;
    protected MeshOrderFixer meshOrder;
    [SerializeField] protected string targetLayer;
    protected float baseScale;
    [SerializeField] protected int numberOfAttack;
    protected int attackId = 0;

    private Camera mainCam;
    private int atkBeforeSkill = 3;
    private SpriteRenderer hpSprite;
    private bool isDied = false;
    private float count = 0;
    public Color allyColor;

    public bool debug;
    public bool isEnemy = false;

    public Transform AttackTarget { get { return attackTarget; } set { attackTarget = value; } }
    public BattleStat BattleStat => battleStat;
    public Hp HealthBar => healthBar;
    public HitParam HitParam => hitParam;
    public HitParam testHitParam;
    public AnimationSetter AimSetter => animationSetter;
    public string TargetLayer { get { return targetLayer; } set { targetLayer = value; } }
    public Transform Attacker => attacker;
    public Collider2D Col => col;
    public MeshOrderFixer MeshOrder => meshOrder;
    public MonsterEffect Effect => monsterEffect;
    [HideInInspector]
    public bool active = true;
    public string vfxPrefab;
    public AudioClip attackSound;
    public AudioClip getHitSound;
    public float waitForSkillFinish;

    public float ManaSKill
    {
        get
        {
            if (atkToSkill == 0) return 0;
            return ((float)numberOfAttack) / atkToSkill;
        }
    }

    public bool IsEnemy
    {
        set
        {
            isEnemy = value;
            Init(battleStat, isEnemy);
        }
        get { return isEnemy; }
    }

    public virtual void Awake()
    {
        moneyGain = Constants.DEFAULT_MONEY_GAIN;
        healthBar = gameObject.GetChildComponent<Hp>("hpBox");
        txtName = gameObject.GetChildComponent<TextMeshPro>("txtName");
        animationSetter = GetComponentInChildren<AnimationSetter>();
        damgeCounter = GetComponent<DamgeCounter>();
        monsterEffect = GetComponent<MonsterEffect>();
        col = GetComponent<Collider2D>();
        mainCam = Camera.main;
        meshOrder = gameObject.GetChildComponent<MeshOrderFixer>("skeletonAnim");

        if (animationSetter == null)
        {
            Debug.LogError($"No animation setter in {gameObject}'s children");
        }

        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError($"No collider2d in {gameObject}");
        }


        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f);
    }



    public virtual void OnEnable()
    {
        monsterData.monsterName = gameObject.name.Replace("(Clone)", "").Trim();
        isDied = false;
        col.enabled = true;
        Init(battleStat, isEnemy);
        healthBar.gameObject.SetActive(true);
        healthBar.intansceHp.transform.localScale = new Vector3(1, 1, 1); ;
        atkBeforeSkill = atkToSkill;
        txtName.color = new Color(1, 1, 1, 0);
    }

    public void Init(BattleStat stat, bool isEnemy = false)
    {
        if (isEnemy)
        {
            battleStat = stat;
            numberOfAttack = 0;
        }
        else
        {
            if (GameSystem.userdata.unlockedHeroesLevel.ContainsKey(monsterData.monsterName))
            {
                battleStat = new BattleStat(monsterData, monsterData.rarity.ToString(), (float)GameSystem.userdata.unlockedHeroesLevel[monsterData.monsterName]);
            }
            else
            {
                battleStat = new BattleStat(monsterData);
            }
        }
        //battleStat.attackRange = Random.Range(battleStat.attackRange * 0.75f, battleStat.attackRange * 1.25f);
        //battleStat.visionRange = Random.Range(battleStat.visionRange * 0.75f, battleStat.visionRange * 1.25f);
        healthBar.transform.localScale = new Vector3(1, 1, 1);
        //txtName.text = data.name;
        attackTarget = null;

        hitParam = new HitParam();
        hitParam.damage = battleStat.damage;
        hitParam.owner = transform;
        SetIdentity(isEnemy);
    }


    protected virtual void SetIdentity(bool isEnemy)
    {
        if (isEnemy)
        {
            healthBar.intansceHp.color = Color.red;
            SetEnemyLayer();
            numberOfAttack = atkBeforeSkill - 1;

        }
        else
        {
            healthBar.intansceHp.color = Color.green;
            SetAllyLayer();
        }
    }

    protected virtual void SetAllyLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(LAYER_ALLY);
        targetLayer = LAYER_ENEMY;
    }

    protected virtual void SetEnemyLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(LAYER_ENEMY);
        targetLayer = LAYER_ALLY;
    }

    public virtual void Update()
    {
        if (debug)
        {
            //Debug.Log("A");
        }
        if (Time.time < waitForSkillFinish) return;
        if (!isEnemy && transform.position.y > Gameplay.Intansce.vurnerableZone.y)
        {
            animationSetter.SetAnimation(runAnimationName);
            transform.position += new Vector3(0, -1) * battleStat.speed * Time.deltaTime;
            return;
        }
        testHitParam = hitParam;

        targetable = transform.position.y < Gameplay.Intansce.vurnerableZone.y;
        if (isDied || !active || monsterEffect.froze || Gameplay.Intansce.GameState == GameplayState.Lose || Gameplay.Intansce.GameState == GameplayState.Win) return;

        if (attackTarget == null /*&& (Time.time - lastAttack > battleStat.attackInterval)*/)
        {

            StateIdle();
            return;
        }

        //if (attackTarget == null) return;

        float distance = Vector2.Distance(transform.position, attackTarget.position);

        bool refuseEnemyAttack = isEnemy && transform.position.y > Gameplay.Intansce.vurnerableZone.y + Constants.ENEMY_BONUS_ATTACK_RANGE;

        if (distance < battleStat.attackRange + 0.1f && !refuseEnemyAttack)
        {
            StateAttack();
        }
        else
        {
            StateChase();
        }
    }

    void StateIdle()
    {
        if (isDied && monsterEffect.stun) return;
        if (!isEnemy && idleAlly != null)
        {
            idleAlly.Invoke();
        }
        else if (isEnemy && idleEnemy != null)
        {
            idleEnemy.Invoke();
        }
        else
        {
            animationSetter.SetAnimation(idleAnimationName);
        }
    }

    //public void OnInspectorGUI() {
    //    GUILayout.Label("This is a Label in a Custom Editor");
    //}

    //TODO: cần tối ưu performance chỗ này
    public virtual void UpdateTarget()
    {
        if (isEnemy && !targetable) return;
        if (monsterEffect.invisible)
        {
            attackTarget = null;
            return;
        }
        if (attackTarget != null && attackTarget.gameObject.activeSelf == true) return;

        float shortestDistance = Mathf.Infinity;
        Collider2D[] allEnemies = Physics2D.OverlapCircleAll(transform.position, 100f, LayerMask.GetMask(targetLayer));
        Collider2D nearestEnemy = null;

        foreach (Collider2D enemy in allEnemies)
        {
            if (!enemyTags.Contains(enemy.tag)) continue;
            if (isEnemy && transform.position.y < enemy.transform.position.y) continue;
            if (!isEnemy && enemy.transform.position.y > Gameplay.Intansce.vurnerableZone.y) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        try
        {
            nearestEnemy.TryGetComponent<MonsterEffect>(out var targetEffect);
            nearestEnemy.TryGetComponent<MonsterAI>(out var monsterAI);
            if (nearestEnemy != null && shortestDistance <= battleStat.visionRange + 0.1f && !targetEffect.froze && monsterAI.targetable && !monsterAI.isDied)
            {
                attackTarget = nearestEnemy.transform;
            }
            else
            {
                attackTarget = null;
            }
        }
        catch
        {
            attackTarget = null;
            //Debug.LogError("Error at game object " + gameObject.name);
        }
    }

    private void StateChase()
    {
        if (isDied) return;
        animationSetter.SetAnimation(runAnimationName);
        animationSetter.SkeletonAnimation.loop = true;
        Vector3 direction = attackTarget.position - transform.position;
        var translateVector = direction.normalized * battleStat.speed * Time.deltaTime;
        transform.Translate(translateVector);

        count += Time.deltaTime;
        if (count > 1f)
        {
            count = 0;
            txtName.text = translateVector.magnitude.ToString();
        }
        animationSetter.SetFacing(attackTarget.transform.position);
    }

    public virtual void StateAttack()
    {
        if (debug)
        {

        }
        if (isDied) return;
        if (attackTarget.GetComponent<MonsterAI>().isDied) UpdateTarget();
        if (Time.time - lastAttack > battleStat.attackInterval)
        {
            lastAttack = Time.time;
            if (numberOfAttack == atkBeforeSkill)
            {
                if (skillEvent != null)
                {
                    skillEvent.Invoke();
                }
                else
                {
                    Attack();
                }
                numberOfAttack = 0;
            }
            else
            {
                Attack();
            }
        }
    }

    public virtual void Attack()
    {
        if (attackTarget != null)
        {
            attackEvent.Invoke();
        }
    }

    public virtual void DefaultAttack()
    {
        if (attackTarget == null) return;
        attackId = Random.Range(0, 99999);
        int rand = attackId;

        animationSetter.SetAnimation(attackAnimationName, () =>
        {
            if (rand == attackId && !isDied && Time.time > waitForSkillFinish)
            {
                if (animationSetter.SkeletonAnimation.AnimationName == attackAnimationName)
                {
                    animationSetter.SetAnimation(idleAnimationName);
                }
            }
        });
        animationSetter.SetFacing(attackTarget.transform.position);
        animationSetter.SkeletonAnimation.AnimationState.Event -= AttackEventHandle;
        animationSetter.SkeletonAnimation.AnimationState.Event += AttackEventHandle;
    }

    protected virtual void AttackEventHandle(TrackEntry entry, Spine.Event e)
    {
        if (e.Data.Name == "OnAttack")
        {
            AttackEvent();
            if (numberOfAttack < atkToSkill)
                numberOfAttack++;
        }
    }
    protected virtual void AttackEvent()
    {
        if (attackTarget == null) UpdateTarget();
        if (attackTarget == null) return;
        if (attackTarget.TryGetComponent<MonsterAI>(out var target))
        {
            if (attackTarget == null) return; //không hiểu sao check null phía trên rồi xuống đây attacktarget vẫn null, nên phải check lại
            target.TakeDame(new HitParam
            {
                damage = battleStat.damage,
                crit = false,
                owner = transform
            });
            if (vfxPrefab != "")
            {
                var fxPath = GeneralUltility.BuildString("", "Vfx/", vfxPrefab);
                //string debugLog = GeneralUltility.BuildString("fxPath=", fxPath, ",attackTarget=", attackTarget.ToString());
                try
                {
                    var particalController = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>(fxPath, attackTarget.transform.position);
                    particalController.SetSortingOrder(target.meshOrder.order);
                }
                catch
                {
                    Debug.LogWarning($"error at game object {gameObject}, fxPath = {fxPath}, attackTarget = {attackTarget}");
                }
            }
            if (attackSound != null)
            {
                SFXSystem.Instance.Play(attackSound);
            }
        }
    }

    public virtual void TakeDame(HitParam attackerHitParam)
    {
        if (isDied || monsterEffect.invisible) return;
        if (attackerHitParam.owner != null)
        {
            attacker = attackerHitParam.owner.transform;
            // Khi bị đánh, nếu chưa có attack target thì đánh lại
            if (enemyTags.Contains(attackerHitParam.owner.tag))
            {
                if ((attackTarget == null || attackTarget.gameObject.activeSelf == false) && attackerHitParam.owner != null)
                {
                    attackTarget = attackerHitParam.owner;
                }
                // Khi bị đánh, nếu vị trí của kẻ tấn công gần hơn --> chuyển mục tiêu tấn công
                else if (attackerHitParam.owner != null && attackerHitParam.owner != transform && attackerHitParam.owner.gameObject.layer != gameObject.layer)
                {
                    if (transform.DistanceTo(attackerHitParam.owner) < transform.DistanceTo(attackTarget))
                    {
                        attackTarget = attackerHitParam.owner;
                    }
                }
            }
        }


        if (isEnemy && attackTarget != null && transform.position.y < attackTarget.position.y)
        {
            attackTarget = null;
        }
        var hpBeforeAttacked = battleStat.hp;
        float damgeTaken = attackerHitParam.damage;

        battleStat.hp -= damgeTaken;
        takeDamgeEvent?.Invoke();
        healthBar.intansceHp.transform.localScale = new Vector3(battleStat.hp / battleStat.maxhp, 1);

        monsterEffect.TurnWhite(0.2f);
        if (battleStat.hp <= 0)
        {
            Die();
            damgeCounter.TakeDamage(damgeTaken - hpBeforeAttacked, attackerHitParam.crit, true); ;
        }
        else
        {
            damgeCounter.TakeDamage(damgeTaken, attackerHitParam.crit);
        }
        ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/HitFx", transform.position).SetSortingOrder(meshOrder.order);
        if (!isEnemy)
        {
            var parController = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/Blood", transform.position);
            parController.SetSortingOrder(meshOrder.order);
            parController.ChangeColor(allyColor);
        }
        if (getHitSound != null && !attackerHitParam.silentSound) SFXSystem.Instance.Play(Gameplay.Intansce.takeDameSound);
    }

    public void Heal(float inputAmount)
    {
        float healAmount = 0;
        if (battleStat.hp + inputAmount <= battleStat.maxhp)
        {
            healAmount = inputAmount;
        }
        if (battleStat.hp + inputAmount > battleStat.maxhp)
        {
            var newAmount = battleStat.maxhp - battleStat.hp;
            healAmount = newAmount;
        }
        battleStat.hp += healAmount;
        var number = ObjectPool.Instance.GetGameObjectFromPool("Number/HealNumber", transform.position);
        var numberMesh = number.GetComponent<DamgeNumber>();
        numberMesh.ShowNumber(healAmount);
        healthBar.intansceHp.transform.DOScaleX(battleStat.hp / battleStat.maxhp, 0.5f);
        ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/HealFx", transform.position).SetSortingOrderWithChildren(meshOrder.order);
    }

    public virtual void Die()
    {
        if (debug)
        {
            Debug.Log("s");
        }
        if (attacker != null) attacker.GetComponent<MonsterAI>().attackTarget = null;
        isDied = true;
        if (isBoss)
        {
            StartCoroutine(BossDie());
            return;
        }
        if (IsEnemy)
        {
            SpawnDP();
            Gameplay.Intansce.EnemyAmount--;
        }
        healthBar.gameObject.SetActive(false);
        if (SelectManagerGameplay.Instance.spawnedHero.Contains(this)) SelectManagerGameplay.Instance.spawnedHero.Remove(this);
        Gameplay.Intansce.UpdateHeroAmount();

        animationSetter.SkeletonAnimation.loop = false;
        healthBar.intansceHp.transform.localScale = new Vector3(0, 1, 1);

        try
        {
            var dieFxPath = GeneralUltility.BuildString("", "Vfx/", IsEnemy ? "Enemy" : "Ally", "DieVfx");
            var controller = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>(dieFxPath, transform.position);
            controller.SetSortingOrder(meshOrder.order);
            if (!isEnemy)
            {
                controller.ChangeColor(allyColor);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(GeneralUltility.BuildString("Failed to load die vfx at ", gameObject.name, ", detail = ", e.Message));
        }
        animationSetter.SetAnimation(dieAnimationName, () =>
        {
            gameObject.SetActive(false);
        });
        dieEvent?.Invoke();
    }

    private IEnumerator BossDie()
    {
        animationSetter.SetAnimation("<None>");
        var wait = new WaitForSeconds(0.2f);
        var r = Random.Range(10, 20);
        for (int i = 0; i < r; i++)
        {
            monsterEffect.TurnWhite(0.2f);
            var position = (Vector2)transform.position + Random.insideUnitCircle.normalized * 1.5f;
            var expolse = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/EnemyDieVfx", position);
            //var slaceFactor = Random.Range(0.3f, 1f);
            //expolse.transform.localScale = Vector3.one * slaceFactor;
            expolse.SetSortingOrder(meshOrder.order);
            yield return wait;
        }
        for (int i = 0; i < r; i++)
        {
            var position = (Vector2)transform.position + Random.insideUnitCircle.normalized * 1.5f;
            var controller = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/EnemyDieVfx", position);
            controller.SetSortingOrder(meshOrder.order);
        }
        animationSetter.SetAnimation(dieAnimationName, () =>
        {
            gameObject.SetActive(false);
            Gameplay.Intansce.EnemyAmount--;
        });
    }

    private void SpawnDP()
    {
        var raidus = 4f;
        var number = Random.Range(3, 7);
        for (int i = 0; i < number; i++)
        {
            var position = (Vector2)transform.position + (raidus * Random.insideUnitCircle);
            var coin = ObjectPool.Instance.GetGameObjectFromPool("DP", transform.position);
            coin.transform.DOMove(position, 0.5f).OnComplete(() =>
            {
                var destination = mainCam.ScreenToWorldPoint(Gameplay.Intansce.MoneyUpdate.transform.position);
                float amount = ((float)moneyGain) / number;
                if (amount == 0) amount = 1;
                coin.transform.DOMove(destination, Vector2.Distance(coin.transform.position, destination) * 0.05f).OnComplete(() =>
                   {
                       coin.gameObject.SetActive(false);
                       Gameplay.Intansce.GainDP(amount);
                   });
            });

        }
    }
    public bool IsDead()
    {
        return battleStat.hp <= 0;
    }

    public virtual void ChangeSide()
    {
        if (gameObject.layer == LayerMask.NameToLayer(LAYER_ALLY))
        {
            gameObject.layer = LayerMask.NameToLayer(LAYER_ENEMY);
            targetLayer = LAYER_ALLY;
        }
        else if (gameObject.layer == LayerMask.NameToLayer(LAYER_ENEMY))
        {
            gameObject.layer = LayerMask.NameToLayer(LAYER_ALLY);
            targetLayer = LAYER_ENEMY;
        }
    }

    [ContextMenu("Get Info")]
    private void GetInfo()
    {
        var content = Resources.Load<TextAsset>("Dynasty JD - Description.tsv").text;

        var lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var curentLine = lines[i].Split('\t');
            if (gameObject.name == curentLine[0])
            {
                monsterData.nickName = curentLine[1];
                monsterData.decripsion = curentLine[2];
                switch (curentLine[3].Trim().Replace("\r", ""))
                {
                    case "C":
                        monsterData.rarity = Rarity.C;
                        break;
                    case "R":
                        monsterData.rarity = Rarity.R;
                        break;
                    //case "S":
                    //    monsterData.rarity = Rarity.S;
                    //    break;
                    case "SR":
                        monsterData.rarity = Rarity.SR;
                        break;
                    case "SSR":
                        monsterData.rarity = Rarity.SSR;
                        break;
                    default:
                        Debug.LogError("There is no such rartiry");
                        break;
                }
            }
        }
    }
}