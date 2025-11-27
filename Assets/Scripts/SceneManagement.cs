using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("The index of the scene to load, according to the project build settings")]
    public int sceneIndex;

    public void SceneChange() {
        SceneManager.LoadScene(sceneIndex);
    }
}
