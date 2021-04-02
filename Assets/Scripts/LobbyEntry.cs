using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    public void ChangeName(string playerName)
        => nameText.text = playerName;
}
