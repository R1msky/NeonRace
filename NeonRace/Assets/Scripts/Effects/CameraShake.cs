using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration;

    [SerializeField] private float magnitude;

    public IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
      
        float elapsed = 0.0f;

        while (elapsed < this.duration)
        {
            float x = Random.Range(-1f, 1f) * this.magnitude;
            float y = Random.Range(-1f, 1f) * this.magnitude;

           transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
        
            yield return null;
        }

      transform.localPosition = originalPos;
    }

    public void StartShake()
    {
        StartCoroutine(Shake());
    }
}
