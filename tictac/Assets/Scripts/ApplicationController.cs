using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ApplicationController : GameElement {

	
    public void Reload()
    {
        SceneManager.LoadScene("Tic-tac");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public Player ChangePlayer(Player[] players, Player currentPlayer)
    {
        if (!currentPlayer.Equals(default(Player)))
        {
            for (int i = 0; i < players.Length; i++)
            {

                if (players[i].name != currentPlayer.name)
                {
                    currentPlayer = players[i];

                    return currentPlayer;

                }
            }

        }
        return default(Player);
    }
}
