using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class GameStateManager :GameElement
{
    bool Pl_IsWin = false;
    int turnPos = -1;
    int turnsCount = 0;
    List<int> twoClearCellsCombination = new List<int>();
    int[] winCombinationIndexes = new int[3];

    public bool CheckWin(int[] map)
    {
        
        if (InWin(map[0], map[1], map[2]) ||
            InWin(map[3], map[4], map[5]) ||
            InWin(map[6], map[7], map[8]) ||
            InWin(map[0], map[3], map[6]) ||
            InWin(map[1], map[4], map[7]) ||
            InWin(map[2], map[5], map[8]) ||
            InWin(map[0], map[4], map[8]) ||
            InWin(map[2], map[4], map[6]))
        {
            return true;            
        }
        
        return false;
    }

    public bool InWin(int image1, int image2, int image3)
    {
        if (image1 != 0 && image2 != 0 && image3 != 0)
        {

            if (image1 == image2 && image1 == image3)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckStandOff(int[] map, Player current)
    {
        if (IsStandOff(map[0], map[1], map[2], current) &&
            IsStandOff(map[3], map[4], map[5], current) &&
            IsStandOff(map[6], map[7], map[8], current) &&
            IsStandOff(map[0], map[3], map[6], current) &&
            IsStandOff(map[1], map[4], map[7], current) &&
            IsStandOff(map[2], map[5], map[8], current) &&
            IsStandOff(map[0], map[4], map[8], current) &&
            IsStandOff(map[2], map[4], map[6], current))
        {
            return true;
            
        }
        return false;
    }

    bool IsStandOff(int image1, int image2, int image3, Player current)
    {

        if (image1 != image2 && image2 != image3 && image2 != 0 ||
            image1 != image2 && image1 == image3 && image1 != 0 && image2 != 0 ||
            image1 == image2 && image1 != image3 && image3 != 0 ||
            image1 != image2 && image1 != image3 && image1 != 0 ||
            (image1 == image2 && image3 == 0 && current.id != image1 ||
            image1 == 0 && image2 == image3 && current.id != image2 ||
            image2 == 0 && image1 == image3 && current.id != image1 ||
            image1 == 0 && image3 == 0 && current.id != image2 ||
            image1 == 0 && image2 == 0 && current.id != image3 ||
            image2 == 0 && image3 == 0 && current.id != image1) &&
            Field.TurnCounter > 7)
        {

            return true;

        }
        return false;



    }

    public Player Win(Player current)
    {
        current = GameController.ChangePlayer(Field.players, current);
        Field.GameStateText.text = string.Concat(current.name, Field.GameStateText.text);
        Field.GameStateText.gameObject.SetActive(true);
        current = default(Player);

        return current;
    }

    public Player StandOff(Player current)
    {
        Field.GameStateText.text = "Stand Off!";
        Field.GameStateText.gameObject.SetActive(true);
        current = default(Player);
        return current;
    }

    public void AITurn(Button[] images, Player current)
    { 

        CheckCombo(Field.turns, current);
        if(turnPos != -1)
        Field.Images[turnPos].gameObject.GetComponentInChildren<Image>().Turn(current);
        turnPos = -1;
        turnsCount = 0;
    }

    void CheckCombo(int[] map, Player current)
    {
        int[] turns = map;
        List<int> availableTurns;
        turnsCount++;        
        Pl_IsWin = false;
        
        for (int i = 0; i < turns.Length; i++)
        {
            if (turns[i] == 0)
            {
                turns[i] = current.id;

                if (current.Equals(Field.AIPlayer))
                {
                    if (CheckWin(turns) == false)
                    {
                        current = GameController.ChangePlayer(Field.players, current);
                        CheckCombo(turns, current);
                        current = GameController.ChangePlayer(Field.players, current);
                        
                        if (CheckOneClearCellInLine(turns, current) == true && Pl_IsWin == false)
                        {
                            twoClearCellsCombination.Add(i);
                        }

                        else if(Pl_IsWin == true)
                        {
                            twoClearCellsCombination.Clear();
                        }

                        turns[i] = 0;
                    }


                    else if (CheckWin(turns) == true)
                    {
                        turns[i] = 0;
                        turnPos = i;
                        return;
                    }
                }
                else if(current.Equals(Field.first))
                {
                    if (CheckWin(turns) == false)
                    {
                        turns[i] = 0;
                    }

                    else if (CheckWin(Field.turns) == true)
                    {
                        turnPos = i;
                        Pl_IsWin = true;
                        turns[i] = 0;
                        return;
                    }
                }
            }
        }
        if(twoClearCellsCombination.Count > 0)
        {
            int needfulIndex = Random.Range(0, twoClearCellsCombination.Count);
            turnPos = twoClearCellsCombination[needfulIndex];
        }
        
        if (current.Equals(Field.AIPlayer) && turnPos == -1)
        {
           if(Field.Images[4].gameObject.GetComponentInChildren<Image>().sprite == null)
            {
                Field.Images[4].gameObject.GetComponentInChildren<Image>().Turn(current);
            }
           else
            {
                availableTurns = GetAvailableTurns(Field.Images);
                RandomMove(availableTurns, current);
            }
        }
    }

    void RandomMove(List<int> indexList, Player current)
    {
        
            turnPos = indexList[Random.Range(0, indexList.Count)];
            
            

    }

    List<int> GetAvailableTurns(Button[] images)
    {
        List<int> indexList = new List<int>(); 
        for(int i = 0; i<images.Length; i++)
        {
            if(images[i].gameObject.GetComponentInChildren<Image>().sprite == null)
            {
                indexList.Add(i);
            }
        }
        
        return indexList;
    }

    bool CheckOneClearCellInLine(int[] map, Player current)
    {
        if (IsOneClearCellInLine(map[0], map[1], map[2], current) ||
            IsOneClearCellInLine(map[3], map[4], map[5], current) ||
            IsOneClearCellInLine(map[6], map[7], map[8], current) ||
            IsOneClearCellInLine(map[0], map[3], map[6], current) ||
            IsOneClearCellInLine(map[1], map[4], map[7], current) ||
            IsOneClearCellInLine(map[2], map[5], map[8], current) ||
            IsOneClearCellInLine(map[0], map[4], map[8], current) ||
            IsOneClearCellInLine(map[2], map[4], map[6], current))
        {
            return true;
        }
        return false;
    }

    bool IsOneClearCellInLine(int image1, int image2, int image3, Player current)
    {

        if (image1 == 0 && image2 == current.id && image3 == current.id ||
            image1 == current.id && image2 == current.id && image3 == 0 ||
            image1 == current.id && image2 == 0 && image3 == current.id)
        {
            
            return true;

        }
        return false;
    }


    
}
