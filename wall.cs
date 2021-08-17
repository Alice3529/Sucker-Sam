using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    [SerializeField] float timeToWait = 1f;
    [SerializeField] GameObject wallBotom;
  
    public void SetTime()
    {
        StopAllCoroutines();
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        wallBotom.SetActive(false);
        yield return new WaitForSeconds(timeToWait);
        wallBotom.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = true;

    }
}
