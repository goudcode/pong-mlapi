using MLAPI;
using UnityEngine;

namespace Util
{
    public class ExitOnDisconnect : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += _ => Application.Quit();
        }
    }
}
