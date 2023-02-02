using System;
using UnityEngine;

public class StartGame : MonoBehaviour, IInteractable
{
    public static event Action<GameState> OnStartGame;

    public void Interact(GameObject host)
    {
        OnStartGame?.Invoke(GameState.Game);
        gameObject.SetActive(false);
    }
}
