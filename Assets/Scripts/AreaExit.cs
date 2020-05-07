﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AreaExit : MonoBehaviour
{

    public string areaToLoad;

    public string areaTransistionName;

    public AreaEntrance theEntrance;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransistionName;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade) 
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0 ) 
            {
                shouldLoadAfterFade = false;
                GameManager.instance.fadingBetweenAreas = true;
                SceneManager.LoadScene(areaToLoad);
            
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            //SceneManager.LoadScene(areaToLoad)
            shouldLoadAfterFade = true;
            UIFade.instance.fadeToBlack();
            PlayerController.instance.areaTransistionName = areaTransistionName;
        }
    }
}
