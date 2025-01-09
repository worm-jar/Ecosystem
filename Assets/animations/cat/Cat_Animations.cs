using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Animations : MonoBehaviour
{
    public Animator animator;
    public void SeeMouse_1()
    {
        animator.SetBool("See_Mouse", true);
    }
    public void SeeMouse_2()
    {
        animator.SetBool("See_Mouse", false);
    }
    public void MouseCaught_1()
    {
        animator.SetBool("Mouse_Caught", true);
    }
    public void MouseCaught_2()
    {
        animator.SetBool("Mouse_Caught", false);
    }
    public void EatDrink_1()
    {
        animator.SetBool("Eat_Drink", true);
    }
    public void Eat_Drink_2()
    {
        animator.SetBool("Eat_Drink", false);
    }
    public void Find_Mate_1()
    {
        animator.SetBool("Find_Mate", true);
    }
    public void Find_Mate_2()
    {
        animator.SetBool("Find_Mate", false);
    }
}

