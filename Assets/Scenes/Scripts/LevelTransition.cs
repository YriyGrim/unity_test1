using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                int totalScenes = SceneManager.sceneCountInBuildSettings;

                if (currentSceneIndex + 1 < totalScenes)
                {
                    SceneManager.LoadScene(currentSceneIndex + 1);
                }
                else
                {
                    SceneManager.LoadScene("MainMenu"); // Замените "MainMenu" на имя сцены главного меню
                }
            }
        }
    }
}

