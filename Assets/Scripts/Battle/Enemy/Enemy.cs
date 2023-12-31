using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Base pour le enemis
public class Enemy : MonoBehaviour
{
    public EnemyScriptable thisEnemy;

    public GameObject parent;

    public List<EnemyAction> enemyActions = new List<EnemyAction>();
    public EnemyAction action;

    public Sprite[] ActionIcon;
    public SpriteRenderer Icon;
    public TMP_Text dmgAmount;
    public TMP_Text healthText;

    public int goldDrop;
    public Animator animator;

    public int currentHealth;
    public int damage = 0;
    public int maxHealth;
    public int currentArmor = 0;
    public Slider healthBar;

    public bool isDead = false;
    public int position;

    public Vector3 positionInWorld;
    public float timeItTakesToDie= 3f;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Random.Range(thisEnemy.minHealth, thisEnemy.maxHealth + 1);
        currentHealth = maxHealth;
        goldDrop = Random.Range(thisEnemy.minGold, thisEnemy.maxGold + 1);
        updateHealth();
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            isDead = true;
            currentHealth = 0;
        }
        animator.SetInteger("Health", currentHealth);
        if (currentHealth == 0)
        {
            AudioManager.Instance.PlaySound2D(SoundType.WildZombieDead);
            BattleController.instance.goldEarned += goldDrop;
            BattleController.instance.enemiesDead++;
            BattleController.instance.CheckGameCondition();
            Destroy(parent, timeItTakesToDie);
        }
        updateHealth();
    }

    public void SetAttack() {
        int attack = GetRandomAttack();
        action = enemyActions[attack];
        Debug.Log(gameObject.name + " : " + thisEnemy.attacks[attack].attack);
    }

    //Prend une de ces attaques dans le tableau de probabilite
    private int GetRandomAttack()
    {
        List<int> attackProbabilities = new List<int>();
        foreach(EnemyScriptable.Attack attack in thisEnemy.attacks)
        {
            attackProbabilities.Add(attack.weight);
        }
        int totalProbabilities = 0;
        foreach(int probability in attackProbabilities)
        {
            totalProbabilities += probability;
        }

        int randomValue = Random.Range(0, totalProbabilities);

        int cumulativeProbability = 0;
        for(int i=0;i<attackProbabilities.Count; i++)
        {
            cumulativeProbability += attackProbabilities[i];
            if(randomValue < cumulativeProbability)
            {
                if (thisEnemy.attacks[i].isAttack)
                {
                    damage = Random.Range(thisEnemy.attacks[i].minAttack, thisEnemy.attacks[i].maxAttack);
                    Icon.sprite = ActionIcon[i];
                    dmgAmount.text = damage.ToString();
                }
                else if (!thisEnemy.attacks[i].isAttack)
                {
                    damage = 0;
                    Icon.sprite = ActionIcon[i];
                    dmgAmount.text = "";
                }
                return i;
            }
        }

        return attackProbabilities.Count - 1;
    }

    public void updateHealth()
    {
        healthBar.value = (float)currentHealth / (float)maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void Attack()
    {
        if (isDead) return;
        if (action == null) SetAttack();
        List<GameObject> targets = new List<GameObject>();
        targets.Add(gameObject);
        foreach (GameObject obj in BattleController.instance.enemiesGO)
        {
            if (obj == this) continue;
            targets.Add(obj);
        }
        action.ApplyAction(targets);
    }

    public void playAnimation(string animation)
    {
        animator.SetTrigger(animation);
    }

    public void MoveToPlace()
    {
        parent.transform.position = positionInWorld;
    }
}
