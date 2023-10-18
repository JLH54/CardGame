using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Character thisPlayer;

    public Animator animator;

    public int currentHealth;
    public int maxHealth;
    public int currentArmor = 0;
    public Slider healthBar;
    public Image fillHealth;
    public TMP_Text healthText;
    public GameObject armorGO;
    public TMP_Text armorText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = thisPlayer.maxHealth;
        currentHealth = maxHealth;
        UpdateHealth();
        CheckForArmor();
    }

    public void TakeDamage(int damage) {

        if(currentArmor > 1)
        {
            currentArmor -= damage;
            if(currentArmor < 0)
            {
                currentArmor = 0;
            }
            CheckForArmor();
            return;
        }
        AudioManager.Instance.PlaySound2D(SoundType.PlayerHurt);
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            //Player dies
            currentHealth = 0;
            BattleController.instance.CheckGameCondition();
            Destroy(gameObject);
        }
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        healthBar.value = (float)currentHealth / (float)maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void GiveArmor(int amount)
    {
        currentArmor += amount;
        CheckForArmor();
    }

    public void GiveHealth(int amount)
    {
        currentHealth += amount;
        UpdateHealth();
    }

    private void CheckForArmor()
    {
        if(currentArmor > 0) {
            fillHealth.color = new Color(20f/255f, 110f/255f, 215f/255f, 1f);
            armorGO.SetActive(true);
            armorText.text = currentArmor.ToString();
        }
        else
        {
            fillHealth.color = new Color(255f/255f, 0f, 0f, 1f);
            armorGO.SetActive(false);
            armorText.text = currentArmor.ToString();
        }
    }
}
