﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHealth = 0;
    public int maxHealth = 100;
    public HealthBar healthBar;
    void Start()
    {
        curHealth = maxHealth;
    }
    
    public void DamagePlayer(int damage)
    {
        curHealth -= damage;
        healthBar.SetHealth(curHealth);
        
    }
}