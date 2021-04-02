using TMPro;
using UnityEngine;

namespace Network
{
    public class LobbyEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;

        public void ChangeName(string playerName)
            => nameText.text = playerName;
    }
}
