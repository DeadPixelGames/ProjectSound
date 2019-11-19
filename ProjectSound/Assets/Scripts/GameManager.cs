using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Joystick joystick;
    public ButtonController jumpButton;
    public ButtonController actionButton;

    public static bool paused = false;

    public static bool operatingInMobile = false;

    

    private void Awake()
    {
        Debug.Log(SystemInfo.operatingSystem);
        if (SystemInfo.operatingSystem.Split(' ')[0].Equals("Android"))
        {
            operatingInMobile = true;
            jumpButton.gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);
            joystick.gameObject.SetActive(true);
        }
        else
        {
            operatingInMobile = false;
            jumpButton.gameObject.SetActive(false);
            actionButton.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
