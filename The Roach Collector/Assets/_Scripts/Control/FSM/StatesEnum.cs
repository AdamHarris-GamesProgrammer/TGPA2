using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Control
{
    public enum StateID
    {
        None = 0,
        Patrol,
        Engage,
        Attack,
        Guard,
        Suspicious,
        Dead
    }
}
