using UnityEngine;
using System.Collections;

public class GameElement : MonoBehaviour
{

    public GameStateManager GameState
    {
        get
        {
            return FindObjectOfType<GameStateManager>();
        }
    }

    public ApplicationController GameController
    {
        get
        {
            return FindObjectOfType<ApplicationController>();
        }
    }

    public Gamefield Field
    {
        get
        {
            return FindObjectOfType<Gamefield>();

        }
    }
}
