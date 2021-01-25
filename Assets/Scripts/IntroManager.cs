using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    private Button playButton;
    private Button exitButton;
    private void Awake()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
    }
    
    
}
