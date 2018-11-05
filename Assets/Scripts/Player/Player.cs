using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
// : MonoBehaviour
{
    public int Money { get; set; }
    List<Programmer> Programmers;

    public Player()
    {
        this.Money = 1000;
    }
}
