using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class RandomAnimationController : BaseComponet
{


    public Animator animator;
    public string[] stateNames;

    void Start()
    {
        EventBus.GetInstance().Sub(Cheer, EventType.win_or_lose, this);
        transform.LookAt(GameUtils.Find("Football").transform, Vector3.up);
        animator = GetComponent<Animator>();
        Reset();
        int index = Random.Range(0, stateNames.Length);
        animator.Play(stateNames[index], 0, Random.Range(0f, 1f));

        //DelayAction(2, () => { Cheer(); });

    }

    void Cheer(MessageObj msg, string str)
    {
        if (GameUtils.GetRandom(0, 100) > 30)
        {
            animator.Play("cheer", 0, Random.Range(0f, 1f));
        }
        else
        {
            animator.Play("claphands", 0, Random.Range(0f, 1f));
        }

    }


    void Reset()
    {
#if UNITY_EDITOR
        AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
        if (ac != null)
        {
            var states = ac.layers[0].stateMachine.states;
            stateNames = new string[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                stateNames[i] = states[i].state.name;
            }
        }
#endif
    }
}