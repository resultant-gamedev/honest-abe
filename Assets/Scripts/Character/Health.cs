﻿using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int additionalHealthFloor;
    public int additionalHealthCeiling;
	public bool alive;
	private GameManager _gameManager;
	private Boss _boss;

	private System.Random _rnd;
    private Attack playerAttack;

    void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_boss = GetComponent<Boss>();
		alive = true;
		_rnd = new System.Random();
        health += _rnd.Next(additionalHealthFloor, additionalHealthCeiling + 1);

        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<Attack>();
    }

    public void RandomizeHealth()
    {
    }

    public virtual void Increase(int amount)
    {
        health += amount;
    }

    public virtual void Decrease(int damage)
    {
        health -= damage;
        //If the hit would kill the gameObject
        if (health <= 0)
        {
			alive = false;
            health = 0;
			// Execution Check
			if (gameObject.tag == "Boss")
			{
				_gameManager.win = true;
			}
            else if (gameObject.tag == "Enemy")
            {
			if (gameObject.tag != "Player" && GlobalSettings.performingHeavyAttack)
            {
                GlobalSettings.executionsPerformed++;
                ShowExecution();
            }
                if (playerAttack.attackState == Attack.State.Heavy) EventHandler.SendEvent(EventHandler.Events.HEAVY_KILL);            else if (gameObject.tag == "Enemy")
            {
                if (playerAttack.attackState == Attack.State.Heavy) EventHandler.SendEvent(EventHandler.Events.HEAVY_KILL);
                else if (playerAttack.attackState == Attack.State.Light) EventHandler.SendEvent(EventHandler.Events.LIGHT_KILL);
            }
            Destroy(gameObject);
        }
    }

    public void ShowExecution()
    {
        GameObject number = new GameObject();
        number.name = "Execution";
        TextMesh tm = number.AddComponent<TextMesh>();
        tm.text = "RIP";
        tm.fontSize = 24;
        tm.color = Color.red;
        tm.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        tm.transform.position = transform.position;
        FloatUpAndDestroy f = number.AddComponent<FloatUpAndDestroy>();
        f.floatGravityMultiplier = 0.5f;
        f.floatVelocity = 2;
    }
}
