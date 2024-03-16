using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWarningPanelOverTime : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;

    private void OnEnable()
    {
        StartCoroutine(DeactivateOverTime());
    }

    IEnumerator DeactivateOverTime()
    {
        yield return new WaitForSeconds(lifeTime);

        Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }
}
