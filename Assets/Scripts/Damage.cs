using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("hit enemy, splat bloods!");

            EnemyBehaviour_AI enemyBehaviour_AI = collision.GetComponent<EnemyBehaviour_AI>();
            //get damage and spawn bloods
            enemyBehaviour_AI.SplatBloods(5);
        }
    }
}