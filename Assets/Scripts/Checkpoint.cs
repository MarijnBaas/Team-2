using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public TimerManager timer;
    private Collider2D playerCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            timer.isDraining = false;
            timer.isFilling = true;
            playerCollider = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            timer.isDraining = true;
            timer.isFilling = false;
        }
    }

    
}
