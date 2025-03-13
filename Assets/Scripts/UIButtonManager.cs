using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonManager : MonoBehaviour
{
    public string gameSceneName = "GameScene";  // Name of the main game scene
    public string mainMenuSceneName = "MainMenu";  // Name of the main menu scene

    // Play Button - Load the game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Return Button - Load the main menu scene
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Restart Button - Reload the current scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quit Button - Exit the game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}