using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemUI : MonoBehaviour
{
    private TextMeshProUGUI problemTMP;
    public GameObject arrowGranpa;
    public GameObject[] solutionDigits;
    public Transform[] waypoints;
    private Transform currentWaypoint;
    


    private void Awake()
    {
        problemTMP = GameObject.Find("Problem text").GetComponent<TextMeshProUGUI>();
        currentWaypoint = waypoints[0];
        
    }
    private void Start()
    {
        arrowGranpa.transform.position = currentWaypoint.position;
        ClearDigits();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AdvanceArrow();
        } 
    }
    public void ShowProblem(string strProblem, int numberOfDigits)
    {
        Debug.Log("PROBLEM_UI/SHOW_PROBLEM numberOfDigits: " + numberOfDigits);
        problemTMP.SetText(strProblem);
        for (int i = 0; i < numberOfDigits; i++)
        {
            solutionDigits[i].GetComponentInChildren<TextMeshProUGUI>().SetText("_");
        }

    }
    private void MoveArrow( Transform toWaypoint)
    {

        arrowGranpa.transform.position = toWaypoint.position;
        
    }
    public void AdvanceArrow()
    {
        

        if (currentWaypoint == waypoints[0]) 
        {
            MoveArrow(waypoints[1]);
            currentWaypoint = waypoints[1];
        }
        else if (currentWaypoint == waypoints[1])
        {
            MoveArrow(waypoints[2]);
            currentWaypoint = waypoints[2];
        }
        else if (currentWaypoint == waypoints[2])
        {
            MoveArrow(waypoints[0]);
            currentWaypoint = waypoints[0];
        }
    }
    public void ClearDigits()
    {
        foreach (var o in solutionDigits)
        {
            o.GetComponentInChildren<TextMeshProUGUI>().SetText("");
        }
    }
    public void ResetArrow()
    {
        MoveArrow(waypoints[0]);
        currentWaypoint = waypoints[0];
    }
    public void SetGoodDigit(int digit)
    {
        if (currentWaypoint == waypoints[0])
        {
            solutionDigits[0].GetComponentInChildren<TextMeshProUGUI>().SetText(digit.ToString());
        }
        else if (currentWaypoint == waypoints[1])
        {
            solutionDigits[1].GetComponentInChildren<TextMeshProUGUI>().SetText(digit.ToString());
        }
        else if (currentWaypoint == waypoints[2])
        {
            solutionDigits[2].GetComponentInChildren<TextMeshProUGUI>().SetText(digit.ToString());
        }
        else { Debug.Log("PROBLEM_UI/SET_GOOD_DIGIT sOMETHING WANT HORRIBLY WRONG!!!"); }
    }
    
}
