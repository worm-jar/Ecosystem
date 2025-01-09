using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese_Animations : MonoBehaviour
{
    public Animator animator;
    public void Cheese_Anim_1()
    {
        animator.SetBool("Cheese_Anim", true);
    }
    public void SeeCheese_2()
    {
        animator.SetBool("Cheese_Anim", false);
    }
}
