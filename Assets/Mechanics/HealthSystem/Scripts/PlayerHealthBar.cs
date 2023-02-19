using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    Image healthBarForeground;
    [SerializeField]
    float updateSpeedSeconds = 0.5f;

    Health health;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        
        if (health == null)
        {
            Debug.LogError("Not found class 'Health' in a parrent");
            this.enabled = false;
            return;
        }

        health.OnHealthChange += Health_OnHealthChange;
    }

    private void Health_OnHealthChange()
    {
        StartCoroutine(ChangeToPct(health.pct));
    }

    IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = healthBarForeground.fillAmount;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthBarForeground.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        healthBarForeground.fillAmount = pct;
    }
}
