using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    // This method quits the application when called.
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Use this to stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Quits the application
        Application.Quit();
        #endif
    }
}