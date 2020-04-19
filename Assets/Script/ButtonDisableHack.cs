using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisableHack : MonoBehaviour
{
    public GameObject DesignatedButtonHolder;

    void OnEnable()
    {
        DesignatedButtonHolder.GetComponent<Image>().enabled = true;
        //DesignatedButtonHolder.SetActive(true);
    }

    void OnDisable()
    {
        DesignatedButtonHolder.GetComponent<Image>().enabled = false;
        //if(DesignatedButtonHolder.activeSelf)
        //    DesignatedButtonHolder.SetActive(false);
    }
}
