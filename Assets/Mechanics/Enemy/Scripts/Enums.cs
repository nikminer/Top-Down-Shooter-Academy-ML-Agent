using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ActionsEnum
{
    Move = 0,
    Rotate = 1,
    Attack = 2
}

enum MovementEnum
{
    nothing = 0,
    forward = 1,
    backward = 2,
    left = 3,
    right = 4
}

enum RotationEnum
{
    nothing = 0,
    left = 1,
    right = 2
}


enum AttackEnum
{
    nothing = 1,
    attack = 0
}

public enum Team
{
    Blue = 0,
    Red = 1
}

public enum Role
{
    Knight = 0,
    Mage = 1
}
