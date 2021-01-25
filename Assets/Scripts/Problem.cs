using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem
{
    public string strProblem;
    public int intSolution;
    public string strSolution;
    public bool isSolved = false;
    public int reward = 200;
    public bool isInitialized;
    private int numberOfDigits;
    public char digitToFind;//digito que debe encontrar el jugador
    private Queue<char> solutionDigitQueue = new Queue<char>();

    public Problem(string strProblem, int intSolution, string strSolution)
    {
        this.strProblem = strProblem;
        this.intSolution = intSolution;
        this.strSolution = strSolution;
    }

    //Inicializa este problema
    //Establece cantidad de digitos de la solucion
    public void InitializeProblem()
    {
        InitializeSolutionDigitQueue();
        numberOfDigits = strSolution.Length;
        isSolved = false;
        isInitialized = true;
    }
    //inicializa la cola de digitos de la solucion
    private void InitializeSolutionDigitQueue()
    {
        foreach (var c in strSolution)
        {
            solutionDigitQueue.Enqueue(c);
        }
    }
    //Establece el digito que debe encontrarse actualmente
    //Si retorna FALSE es porque no hay mas digitos en la cola y se ha resuelto el problema
    public bool SetDigitToFind()
    {
        if (solutionDigitQueue.Count != 0)
        {
            digitToFind = solutionDigitQueue.Dequeue();
            //Debug.Log("PROBLEM/SET_DIGIT_TO_FIND digitToFind: " + digitToFind);
            return true;
        }
        else { return false; }
        
    }
    public char GetDigitToFind()
    {
        //Debug.Log("PROBLEM/GET_DIGIT_TO_FIND return: " + digitToFind);
        return digitToFind;
    }
    public int GetNumberOfDigits() { return numberOfDigits; }

}
