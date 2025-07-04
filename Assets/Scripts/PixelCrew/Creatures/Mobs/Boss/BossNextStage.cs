using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNextStage : StateMachineBehaviour
{
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.Stage++;

        var changeLight = animator.GetComponent<ChangeLightComponent>();
        changeLight.SetColor();
    }


}
