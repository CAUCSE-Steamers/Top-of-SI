using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class AbstractBoss : MonoBehaviour {
    private int max_HP = 100, HP = 100;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMaxHP(int i)
    {
        max_HP = i;
    }

    public int GetCurrentHP()
    {
        return HP;
    }

    public bool GetDamage(int damage)
    {
        HP -= damage;
        return HP < 1;
    }
}
