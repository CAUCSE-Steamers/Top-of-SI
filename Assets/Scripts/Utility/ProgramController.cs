using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProgramController : MonoBehaviour
{
    public void QuitProgram()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
        Process.GetCurrentProcess().Kill();
#endif
    }

    public void ForceMoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadDefaultPlayer()
    {
        LobbyManager.Instance.LoadDefaultPlayer();
    }

    public void LoadPlayer()
    {
        LobbyManager.Instance.LoadPlayer();
    }

    public void SavePlayer()
    {
        LobbyManager.Instance.SavePlayer();
    }

    public void LoadStages()
    {
        LobbyManager.Instance.LoadStages();
    }

    public void SaveStages()
    {
        LobbyManager.Instance.SaveStages();
    }
}
