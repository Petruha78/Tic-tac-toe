using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public  struct Player
{
    public string name;

    public Sprite figure;

    public int id;

}


public class Gamefield : GameElement {

    

    private static TurnMethod method;
    public static TurnMethod Method
    {
        get
        {
            return method;
        }

        set
        {
            method = value;
        }
    }

    [SerializeField]
    GameObject Crossline;

    

    [SerializeField]
    private Button[] images;
    public Button [] Images
    {
        get
        {
            return images;
        }

        set
        {
            images = value;
        }
    }

    [SerializeField]
    private TextMeshPro gameStateText;
    public TextMeshPro GameStateText
    {
        get
        {
            return gameStateText;
        }

        set
        {
            gameStateText = value;
        }
    }


    [SerializeField]
    Sprite cross;
    [SerializeField]
    Sprite naught;

    public Player first;
    private Player second;
    public Player AIPlayer;
    private Player current;   
    public Player[] players;

    private Image image;
    private bool isStandOff;
    public bool isWin;
    

    private int turnCounter = 0;
    public int TurnCounter
    {
        get
        {
            return turnCounter;
        }

        
    }
    public int[] turns;
    int[] winCombinationIndexes;

    void Start ()
    {
        Crossline.SetActive(false);
        gameStateText.gameObject.SetActive(false);

        images = GetComponentsInChildren<Button>();
        
        
        players = new Player[2];

        first = new Player();
        second = new Player();
        AIPlayer = new Player();

        first.figure = cross;
        second.figure = naught;
        first.id = 1;
        second.id = 2;
        AIPlayer.id = 2;
        AIPlayer.figure = naught;
        first.name = "First ";
        second.name = "Second ";
        AIPlayer.name = "AI ";

        players[0] = first;
        players[1] = AIPlayer;
       
        current = first;

        turns = new int[Images.Length];
    }
	
	
	void Update ()
    {
        
            
    }

    public void Click()
    {
        

        

        if (current.Equals(first))
        {
            
            if (method != null)
                method(current);
            
            current = GameState.GameController.ChangePlayer(players, current);
            TurnsMapUpdate();
            turnCounter++;
        }

        isWin = GameState.CheckWin(turns);


        if (current.Equals(AIPlayer) && isWin == false)
        {
           
            GameState.AITurn(Images, current);
            current = GameState.GameController.ChangePlayer(players, current);
            TurnsMapUpdate();
            turnCounter++;

        }

        isWin = GameState.CheckWin(turns);
        isStandOff = GameState.CheckStandOff(Images,current);

        if (isWin == true)
        {
            winCombinationIndexes = DefineCrosslinePos(turns);
            
            Crossline.transform.position = Images[winCombinationIndexes[1]].transform.position;
            Vector3 direction = Crossline.transform.position - Images[winCombinationIndexes[2]].transform.position;
            Crossline.transform.Rotate(Vector3.forward, Vector3.Angle(Crossline.transform.right, direction));
            Crossline.SetActive(true);
            current = GameState.Win(current);
        }
            
        if (turnCounter > 6)
        {
            if(isStandOff == true)
                current = GameState.StandOff(current);
        }
            

    }

    private void TurnsMapUpdate()
    {
        for (int i = 0; i < turns.Length; i++)
        {
            if (Images[i].gameObject.GetComponentInChildren<Image>().sprite == cross)
            {
                turns[i] = 1;
            }

            else if (Images[i].gameObject.GetComponentInChildren<Image>().sprite == naught)
            {
                turns[i] = 2;
            }
            else turns[i] = 0;
            
        }
    }


    public int[] DefineCrosslinePos(int[] map)
    {
        int[] winCombinationIndexes = null;

         if (GameState.InWin(map[0], map[1], map[2])) winCombinationIndexes = new int[] {0, 1, 2 };
         if (GameState.InWin(map[3], map[4], map[5])) winCombinationIndexes = new int[] { 3, 4, 5 };
         if (GameState.InWin(map[6], map[7], map[8])) winCombinationIndexes = new int[] { 6, 7, 8 };
         if (GameState.InWin(map[0], map[3], map[6])) winCombinationIndexes = new int[] { 0, 3, 6 };
         if (GameState.InWin(map[1], map[4], map[7])) winCombinationIndexes = new int[] {1, 4, 7 };
         if (GameState.InWin(map[2], map[5], map[8])) winCombinationIndexes = new int[] { 2, 5, 8 };
         if (GameState.InWin(map[0], map[4], map[8])) winCombinationIndexes = new int[] { 0, 4, 8 };
         if (GameState.InWin(map[2], map[4], map[6])) winCombinationIndexes = new int[] { 2, 4, 6 };

        Debug.Log(winCombinationIndexes[1]);

        return winCombinationIndexes;
    }


    

}
