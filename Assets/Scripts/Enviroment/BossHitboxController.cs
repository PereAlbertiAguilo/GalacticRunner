using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHitboxController : MonoBehaviour
{
    [SerializeField] BossController bossController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        // If this gameobject collides with a bullet takes away 2 points of its life
        if ((tag == "Bullet" || tag == "Player") && bossController.canTakeDamage)
        {
            if (bossController.health.currentHealth == bossController.health.maxHealth)
            {
                //InstantiateObject(explosionParticle);
            }

            if (PlayerPrefs.HasKey("BulletsSelect") && PlayerPrefs.GetInt("BulletsSelect") > 0)
            {
                bossController.health.currentHealth -= PlayerPrefs.GetInt("BulletsSelect") * 2;
            }
            else
            {
                bossController.health.currentHealth -= 1;
            }

            if (!bossController.health.isAlive)
            {
                bossController.hudManager.pointsScore += bossController.scorePoints;
                bossController.hudManager.pointsText.GetComponent<Animator>().Play("ScorePop");

                TextMeshProUGUI t = bossController.InstantiateObject(bossController.textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                t.text = "" + bossController.scorePoints + " S";
                t.fontSize = 2;

                StartCoroutine(bossController.ExitScene(true));
            }

            bossController.healthBar.fillAmount = (float)bossController.health.currentHealth / bossController.health.maxHealth;

            if (Mathf.RoundToInt(bossController.health.currentHealth) == Mathf.RoundToInt((bossController.health.maxHealth * 20) / 100) ||
                Mathf.RoundToInt(bossController.health.currentHealth) == Mathf.RoundToInt((bossController.health.maxHealth * 40) / 100) ||
                Mathf.RoundToInt(bossController.health.currentHealth) == Mathf.RoundToInt((bossController.health.maxHealth * 60) / 100) ||
                Mathf.RoundToInt(bossController.health.currentHealth) == Mathf.RoundToInt((bossController.health.maxHealth * 80) / 100))
            {
                //InstantiateObject(explosionParticle);
            }

        }
    }
}
