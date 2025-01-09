using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Animations : MonoBehaviour
{
    public Animator animator;
    public void SeeCheese_1()
    {
        animator.SetBool("See_Cheese", true);
    }
    public void SeeCheese_2()
    {
        animator.SetBool("See_Cheese", false);
    }
    public void CheeseCaught_1()
    {
        animator.SetBool("Cheese_Caught", true);
    }
    public void CheeseCaught_2()
    {
        animator.SetBool("Cheese_Caught", false);
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

