using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Digit : MonoBehaviour
{
    // Start is called before the first frame update
    
    [System.NonSerialized]
    public int decrement = 50;
    public float smoothing = 1;
    public bool isFloating = true;//implementar animacion

    private int intDigit;
    private string strDigit;
    private Vector3 motion;
    private float speed = 0.2f;
 
    private void Start()
    {
        motion = Random.insideUnitCircle;
        motion = motion.normalized;
    }

    public void SetDigit(int integerDigit)
    {
        this.intDigit = integerDigit;
        strDigit = intDigit.ToString();
        GetComponentInChildren<TextMeshProUGUI>().text = strDigit;
    }
    public int GetDigit()
    {
        return intDigit;
    }
    private void Update()
    {
        MoveDigit();
    }
    private void MoveDigit()
    {
        transform.position = transform.position + (motion * speed * Time.deltaTime);
    }
    
}
