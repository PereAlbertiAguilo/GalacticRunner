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
            bossController.health.currentHealth -= collision.gameObject.GetComponent<BulletController>().bulletDamage;

            if (!bossController.health.isAlive)
            {
                bossController.hudManager.pointsScore += bossController.scorePoints;
                bossController.hudManager.pointsText.GetComponent<Animator>().Play("ScorePop");

                TextMeshProUGUI t = bossController.InstantiateObject(bossController.textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                t.text = "" + bossController.scorePoints + " S";
                t.fontSize = 2;

                bossController.hudManager.pointsText.text = "Scraps " + bossController.hudManager.pointsScore;

                StartCoroutine(bossController.ExitScene(true));
            }

            bossController.healthBar.fillAmount = (float)bossController.health.currentHealth / bossController.health.maxHealth;
        }
    }
}
