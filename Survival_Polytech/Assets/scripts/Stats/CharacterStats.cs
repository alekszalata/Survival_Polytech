﻿using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    #region Singleton

    public static CharacterStats instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Slomalis' stati chini daun");
            return;
        }
        instance = this;
    }

    #endregion
   

    private bool alive = true;
    private bool sleeping = false;
    private bool starvation = false;

    public bool showStats;

    [Range(0, 100)]
    public int maxHealth = 100;
    public float currentHealth { get; private set; }
    public int lvl;
    private int lvlUpPoints;
    public float ExpToLvlUp;
    public float EXP;
    public float money;    

    public float hunger;
    public float constOfHunger;
    public float hungerDyingConst;
    public float sleepiness;
    public float constOfSleepiness;
    public int sanity;

    public int intelligence;
    public int agility;
    public int authority;

    public CharacterStats(float currentHealth, int lvl, float EXP, float money, float hunger, float sleepiness, int sanity, int intelligence, int agility, int authority)
    {
        this.currentHealth = currentHealth;
        this.lvl = lvl;
        this.EXP = EXP;
        this.money = money;
        this.hunger = hunger;
        this.sleepiness = sleepiness;
        this.sanity = sanity;
        this.intelligence = intelligence;
        this.agility = agility;
        this.authority = authority;
    }
  
    private void Start()
    {
        alive = true;
        showStats = false;
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            FallingAsleep(constOfSleepiness);

            Starvation(constOfHunger);

            if (starvation)
                TakeDamage(hungerDyingConst);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // открыть/закрыть окно с параметрами
            showStats = !showStats;

        if (alive)
        {

            if (currentHealth <= 0 && alive)
                Die();

            if (EXP > ExpToLvlUp)
                lvlUp();
        }
    }

    public void lvlUp()
    {
        lvl++;
        EXP = 0;
        this.ExpToLvlUp += Mathf.Floor(ExpToLvlUp / 2.5f); //
        lvlUpPoints += 5;
    }

    public void TakeDamage(float damage)
    {
        // придумать что-то получше
        // damage *= (100 - sanity.GetValue()) / 100; 
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
        Debug.Log(transform.name + " takes " + damage + " damage.");        
    }

    public virtual void Die()
    {
        alive = false;
        // some cruel way of death
        Debug.Log(transform.name + " died.");
    }

    public void FallingAsleep(float amount)
    {
        sleepiness += amount;
        if (sleepiness >= 100)
        {
            sleepiness = 100;
            if (!sleeping)
                FallAsleep();
        }
        else
            if (sleeping) sleeping = false;
    }

    public void FallAsleep()
    {
        // some cruel way of falling asleep
        Debug.Log(transform.name + " is sleeping.");
        sleeping = true;
    }

    public void Starvation(float amount)
    {
        hunger += amount;
        if (hunger >= 100)
        {
            hunger = 100;
            if (!starvation)
                HungerDyingState();
        }
        else
            if (starvation) starvation = false;
           
    }

    public void HungerDyingState()
    {
        // some cruel way of starvation
        Debug.Log(transform.name + " starvation");
        starvation = true;
    }

    public void changeMoney(float amount)
    {
        money += amount;
        if (money < 0)
            money = 0;
    }

    public void EarnEXP(int amount)
    {
        EXP += amount;
        if (EXP > ExpToLvlUp)
            lvlUp();
    }

    void OnGUI()
    {
        if (showStats) //если статы отображаются 
        {
            //Рисуем наши статы 
            GUI.Box(new Rect(10, 70, 300, 300), "stats");
            GUI.Label(new Rect(10, 95, 300, 300), "LvL: " + lvl);
            GUI.Label(new Rect(10, 115, 300, 300), "EXP: " + Mathf.FloorToInt(EXP));
            GUI.Label(new Rect(10, 135, 300, 300), "HP: " + Mathf.FloorToInt(currentHealth));
            GUI.Label(new Rect(10, 155, 300, 300), "Money: " + money);
            GUI.Label(new Rect(10, 175, 300, 300), "Hunger: " + Mathf.FloorToInt(hunger));
            GUI.Label(new Rect(10, 195, 300, 300), "Sleepiness: " + Mathf.FloorToInt(sleepiness));
            GUI.Label(new Rect(10, 215, 300, 300), "Sanity: " + Mathf.FloorToInt(sanity));
            GUI.Label(new Rect(10, 235, 300, 300), "Intelligence: " + intelligence);
            GUI.Label(new Rect(10, 255, 300, 300), "Agility: " + agility);
            GUI.Label(new Rect(10, 275, 300, 300), "Authority: " + authority);

            if (lvlUpPoints > 0) //если очков статов больше 0 делаем кнопки для повышения статов 
            {
                GUI.Label(new Rect(10, 295, 300, 20), "points " + lvlUpPoints.ToString());
                if (GUI.Button(new Rect(150, 235, 20, 20), "+")) //Для ума 
                {
                    if (lvlUpPoints > 0)
                    {
                        lvlUpPoints -= 1;
                        intelligence += 5;
                    }
                }
                if (GUI.Button(new Rect(150, 255, 20, 20), "+")) //Для проворства  
                {
                    if (lvlUpPoints > 0)
                    {
                        lvlUpPoints -= 1;
                        agility += 5;
                    }
                }
                if (GUI.Button(new Rect(150, 275, 20, 20), "+")) //Для аворитета 
                {
                    if (lvlUpPoints > 0)
                    {
                        lvlUpPoints -= 1;
                        authority += 5;
                    }
                }
            }
        }
        else if (showStats)
            useGUILayout = false; //Скрываем окно статов 
    }

    public int getLVL()
    {
        return lvl;
    }

    public bool isAlive()
    {
        return alive;
    }

    public void setAlive(bool newAlive)
    {
        alive = newAlive;
    }

    public bool isSleeping()
    {
        return sleeping;
    }

    public void setSleeping(bool newSleep)
    {
        sleeping = newSleep;
    }

    public bool isStarvation()
    {
        return starvation;
    }

    public void setStarvation(bool newHunger)
    {
        starvation = newHunger;
    }
}