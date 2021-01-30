using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProblemManager
{

    private Problem currentProblem;
    private char currentDigitToFind;
    private int numberOfDigits;
    public int MakeProblem(ProblemTypes type)
    {
        if (type == ProblemTypes.MULTIPLICATION)
        {
            int a = Mathf.RoundToInt(Random.Range(2, 9));
            int b = Mathf.RoundToInt(Random.Range(2, 9));
            int intSolution = a * b;
            string strSolution = intSolution.ToString();
            string strProblem = string.Concat(a.ToString(), "X", b.ToString() + " =");
            currentProblem = new Problem(strProblem, intSolution, strSolution);
            currentProblem.InitializeProblem();
            currentProblem.isSolved = false;
            currentProblem.isInitialized = true;

            if (currentProblem.SetDigitToFind())
            {
                currentDigitToFind = currentProblem.digitToFind;
                //Debug.Log("MAKE_MULTIPLICATION_PROBLEM. SetDigitToFind in MakeMultiplicationProblem Success: " + digitToFind);

            }
            else { Debug.Log("MAKE_MULTIPLICATION_PROBLEM. SetDigitToFind empty queue"); }
            // Debug.Log("MAKE_MULTIPLICATION_PROBLE. currentProblem is: " + currentProblem.strProblem + " and isInitialized is: " + currentProblem.isInitialized);
        }
        else if (type == ProblemTypes.ADDITION)
        {
            int a, b, intSolution;
            a = Random.Range(10, 20);
            b = Random.Range(10, 20);
            intSolution = a + b;
            string strSolution = intSolution.ToString();
            string strProblem = string.Concat(a.ToString(), "+", b.ToString() + " =");

            currentProblem = new Problem(strProblem, intSolution, strSolution);
            currentProblem.InitializeProblem();
            currentProblem.isSolved = false;
            currentProblem.isInitialized = true;
            if (currentProblem.SetDigitToFind())
            {
                currentDigitToFind = currentProblem.digitToFind;
                //Debug.Log("MAKE_MULTIPLICATION_PROBLEM. SetDigitToFind in MakeMultiplicationProblem Success: " + digitToFind);

            }
            else { Debug.Log("MAKE_MULTIPLICATION_PROBLEM. SetDigitToFind empty queue"); }
            // Debug.Log("MAKE_MULTIPLICATION_PROBLE. currentProblem is: " + currentProblem.strProblem + " and isInitialized is: " + currentProblem.isInitialized);
        }
        numberOfDigits = GetCurrentProblem().GetNumberOfDigits();
        Debug.Log("PROBLEM_MANAGER/MAKE_PROBLEM numberOfDigits: " + numberOfDigits);
        return numberOfDigits;


    }
    
    public Problem GetCurrentProblem() { return currentProblem; }
    public int GetCurrentDigitToFind() { return int.Parse(GetCurrentProblem().GetDigitToFind().ToString());  }
    public int GetNumberOfDigits() { return numberOfDigits; }

}
