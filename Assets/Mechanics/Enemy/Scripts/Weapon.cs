using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public int Damage = 50;

    [HideInInspector]
    public bool IsAttack = false;
}
