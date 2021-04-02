using MLAPI;
using MLAPI.SceneManagement;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject restartButton;
    private void Start()
    {
        if (NetworkManager.Singleton.IsHost)
            restartButton.SetActive(true);
    }

    public void OnRestart()
    {
        NetworkSceneManager.SwitchScene("Game");
    }
}
