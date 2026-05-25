using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    int index = 0;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    PlayerInputManager manager;
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = Random.Range(0, players.Count);
        manager.playerPrefab = players[index];
    }
    public void SwitchNextSpawnCharcter(PlayerInput input)
    {
        index = Random.Range(0, players.Count);
        manager.playerPrefab = players[index];
    }
}
