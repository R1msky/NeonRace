using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVibrator : MonoBehaviour
{
    [SerializeField] private float magnitude = 0.4f;
    [SerializeField] private float sizeCoefficient = 1.01f;

   public   IEnumerator Vibration(float time)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * this.magnitude;
            float y = Random.Range(-1f, 1f) * this.magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            transform.localScale *= sizeCoefficient;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        //yield return new WaitForSeconds(time);

       // StartCoroutine(destroyManager.Explosion(gameObject, explosionParticles, cameraShake));
    }
}
