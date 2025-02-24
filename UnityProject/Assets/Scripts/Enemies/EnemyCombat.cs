﻿using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour {

    private Enemy enemy;
	[SerializeField] private float attack = 10.0f; //fuerza de ataque
	[SerializeField] private float attackRange = 2.0f; //Rango de ataque(lo lejos que llega)
    [SerializeField] private float attackRate = 2.5f; //Seconds / attack
    [SerializeField] private float recoverDelay = 1.0f; //Tiempo que tarda en recuperarse despues de recibir un ataque!
    [SerializeField] protected float maxLife = 100.0f; //vida maxima

    private float currentLife; //vida actual

    private float attackRateTime = 0.0f;
    private float recoverTime = 0.0f;

	void Start () 
	{
		enemy = GetComponentInParent<Enemy>();
        currentLife = maxLife; //Empieza con 100% vida
	}

	void Update ()
    {
        if (!GameState.IsPlaying() || GetComponent<EnemyCombat>().Dead()) return;
        if (Dead()) return;

		attackRateTime += Time.deltaTime;
        recoverTime += Time.deltaTime;
        if (recoverTime < recoverDelay) attackRateTime = 0.0f; //mientras se este recuperando de un golpe, no ataca

        Player target = enemy.GetTarget();
        if ( InRange() ) //Ya esta cerca del player, attaaack!
        {
            if (attackRateTime >= attackRate)
            {
                GetComponent<EnemyAnimation>().PlayAttack();
            }
        }
	}

    public bool InRange()
    {
        Player target = enemy.GetTarget();
        if (target == null) return false;
        float distanceToTarget = Vector3.Distance(target.gameObject.transform.position, transform.position);
        return distanceToTarget < attackRange;
    }

    public void ReceiveAttack(float damage)
    {
        if (Dead()) return;

        currentLife -= damage;
        attackRateTime = recoverTime = 0.0f;
        GetComponent<EnemyAnimation>().OnReceiveAttack();

        if(Dead())
        {
            GetComponent<EnemyAnimation>().OnDie();
            GetComponent<CharacterController>().enabled = false;
        }
    }

    public void OnAttackFinished()
    {
        if (Dead()) return;

        Player target = enemy.GetTarget();
        if (target != null)
        {
            if (InRange()) //Ya esta cerca del player, attaaack!
            {
                if (attackRateTime >= attackRate)
                {
                    target.GetComponent<PlayerCombat>().ReceiveAttack(GetComponent<Enemy>());
                }
            }

            attackRateTime = 0.0f;
        }
    }

    public bool Dead()
    {
        return currentLife <= 0;
    }

    public float GetCurrentLife() { return currentLife; }
    public float GetMaxLife() { return maxLife; }
    public float GetAttack() { return attack; }
    public float GetAttackRange() { return attackRange; }
    public float GetAttackRate() { return attackRate; }
}
