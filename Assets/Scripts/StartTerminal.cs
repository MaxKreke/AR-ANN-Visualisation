using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTerminal : MonoBehaviour
{

    public void LoadScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
