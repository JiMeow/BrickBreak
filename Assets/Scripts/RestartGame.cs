using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    [SerializeField]
    Text completeText;
    void Start()
    {
        completeText.gameObject.SetActive(false);
    }

    // If all the blocks are destroyed, the game is complete and the complete text is displayed
    void Update()
    {
        if (BrickManager.instance.AllBlockWasDestroy())
        {
            completeText.gameObject.SetActive(true);
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}