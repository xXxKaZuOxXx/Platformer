﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloodState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<FloodController>();
        spawner.StartPooling();
    }
}
