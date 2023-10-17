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
    public TMP_Text healthText;

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
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            //Player dies
            currentHealth = 0;
        }
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        healthBar.value = (float)currentHealth / (float)maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
}
