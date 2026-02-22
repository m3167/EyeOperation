using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleUpAndDownPoint : MonoBehaviour
{
    public Vector3 startScale = new Vector3(1f, 1f, 1f);
    public Vector3 endScale = new Vector3(2f, 2f, 2f);
    public float duration = 1f;

    void Start()
    {
        // Set the initial scale
        transform.localScale = startScale;

        // Call the ScaleObject method to start the scaling animation
        ScaleObjectUpDown();
    }

    void ScaleObjectUpDown()
    {
        // Scale up
        transform.DOScale(endScale, duration)
            .OnComplete(() =>
            {
                // Scale down
                transform.DOScale(startScale, duration)
                    .OnComplete(() =>
                    {
                        // Repeat the scaling loop
                        ScaleObjectUpDown();
                    });
            });
    }
}