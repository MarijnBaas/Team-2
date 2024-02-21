using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public static int currentLevel = 0;
    private AudioSource finishSound;
    private bool levelCompleted = false;

    private void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !levelCompleted)
        {
            finishSound.Play();
            levelCompleted = true;
            Invoke("CompleteLevel", 1f);
        }
    }

    private void CompleteLevel()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        /*
            Un-hide Finish screen UI
        */
        Invoke("NextLevel", 1f); // remove when UI works
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(currentLevel+1);
    }
}
