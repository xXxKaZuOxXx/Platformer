using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNextStageWithoutLight : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.Stage++;

        
    }
}
