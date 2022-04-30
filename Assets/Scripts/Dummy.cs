using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dummy : MonoBehaviour
{
    public static Dummy instance;
    public GameObject[] dummyIMG;

    private void Awake()
    {
        instance = this;
    }

    public void OnDummy(/*InputAction.CallbackContext value*/)
    {
        // Debug.Log("Dummy1");
        dummyIMG[0].SetActive(true);
    }

    public void OnDummy2(/*InputAction.CallbackContext value*/)
    {
        // Debug.Log("Dummy2");
        dummyIMG[1].SetActive(true);
    }

    private void Update()
    {
        if (dummyIMG[0].active == true)
        {
            StartCoroutine(HideIMG1());
        }

        if (dummyIMG[1].active == true)
        {
            StartCoroutine(HideIMG2());
        }
    }

    IEnumerator HideIMG1()
    {
        yield return new WaitForSeconds(5);
        dummyIMG[0].SetActive(false);
    }

    IEnumerator HideIMG2()
    {
        yield return new WaitForSeconds(5);
        dummyIMG[1].SetActive(false);
    }
}
