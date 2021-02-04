using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Control
{
    public enum Transition
    {
        None = 0,
        PlayerWithinRange,
        PlayerOutsideRange,
        PlayerDetected,
        PlayerLost,

        Dead
    }
}