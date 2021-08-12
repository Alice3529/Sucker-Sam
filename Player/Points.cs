using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] float points=0;

    public void AddPoints(int amount)
    {
        points += amount;
    }

    void Update()
    {
        pointsText.text = points.ToString();
    }


}
