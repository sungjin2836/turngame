using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

public class Enemy : Character
{
    [Header("적 캐릭터 정보")]
    public int maxShield;
    public Skill normalAttack;
    public ElementType[] weakElements;

    [SerializeField]
    GameObject BarPosition;
    [SerializeField]
    Slider enemyShieldBar;
    [SerializeField]
    Slider enemyHpBar;

    int _shield;
    int speedDebuff = 10;
    Camera mainCamera;

    public int hp
    {
        get => _hp;

        private set => _hp = Mathf.Clamp(value, 0, maxHP);
    }

    public int shield
    {
        get => _shield;
        private set => _shield = Mathf.Clamp(value, 0, maxShield);
    }

    public override void Initialize(int id)
    {
        var enemyData = DataManager.Instance.GetEnemyData(id);
        charName = enemyData.charName;
        level = enemyData.level;
        maxHP = enemyData.hp;
        speed = enemyData.speed;
        attackStat = enemyData.attackStat;
        weakElements = enemyData.elem;
        maxShield = enemyData.shield;
        actionGauge = Mathf.FloorToInt(10000 / enemyData.speed);

        hp = maxHP;
        finalSpeed = speed;
        finalAttackStat = attackStat;
        shield = maxShield;
        currentActionGauge = 1;

        mainCamera = Camera.main;

        if (enemyHpBar != null && enemyShieldBar != null)
        {
            SetMaxHealth();
            SetMaxShield();
            SetBarPosition();
        }

        Debug.Log($"{currentActionGauge} / {actionGauge}");
    }

    public bool ContainsElement(ElementType element)
    {
        return weakElements.Contains(element);
    }

    public bool HasShield()
    {
        return shield > 0; // 
    }

    public void DamageToShield(int damage)
    {
        shield -= damage;
    }

    public void RegenShield()
    {
        shield = maxShield;
    }

    public void SetMaxHealth()
    {
        enemyHpBar.maxValue = maxHP;
        enemyHpBar.value = hp;
    }
    public void SetHealth()
    {
        Debug.Log($"sethealth 매개변수 : {hp}, 실제 체력 {_hp}");
        enemyHpBar.value = hp;
        if (hp == 0)
        {
            enemyHpBar.gameObject.SetActive(false);
            enemyShieldBar.gameObject.SetActive(false);
        }
    }
    public void SetMaxShield()
    {
        enemyShieldBar.maxValue = maxShield;
        enemyShieldBar.value = shield;
    }
    public void SetShield()
    {
        enemyShieldBar.value = shield;
        if (shield == 0)
        {
            speed -= speedDebuff;
            Debug.Log($"속성 실드가 파괴되어 속도가 {speedDebuff}만큼 느려져서 {speed}가 됨");
        }
    }

    private void CreateBar()
    {
        enemyHpBar = BarPosition.transform.Find("EnemyHp").GetComponent<Slider>();
        enemyShieldBar = BarPosition.transform.Find("EnemyShield").GetComponent<Slider>();
    }

    private void SetBarPosition()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + Vector3.up * 2.0f);
        //Debug.Log($" {charName} bar 포지션1 : {screenPosition}");
        //Debug.Log($" {charName} bar 포지션2 : {transform.position}");
        BarPosition.transform.position = screenPosition;
    }

    public int NormalAttack(Player target, float value = 0.5f)
    {
        Debug.Log($" {charName}의 NormalAttack의 공격력 {attackStat}");
        var player = target as Player;
        int dam = player.GetDamage(Mathf.FloorToInt(attackStat));
        Debug.Log($"{player.charName}의 체력은 {player.hp}/{player.maxHP}");
        return dam;
    }

    public void SetPrevHpAndShield(int prevShieldAttack, int prevAttack)
    {
        enemyHpBar.value -= prevAttack;
        enemyShieldBar.value -= prevShieldAttack;
    }

    public void SetPrevFinalSpeed()
    {
        if (enemyShieldBar.value == 0) finalSpeed -= speedDebuff;
    }
    public void ReturnPrevFinalSpeed()
    {
        finalSpeed = speed;
    }

}