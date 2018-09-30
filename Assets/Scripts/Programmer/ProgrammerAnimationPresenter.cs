using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProgrammerAnimationPresenter : MonoBehaviour
{
    [SerializeField]
    private Programmer programmer;
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        programmer.OnMovingStarted += position =>
        {
            animator.Play("Moving");
        };

        programmer.OnMovingEnded += position =>
        {
            animator.SetTrigger("MovingEnd");
        };
    }
}
