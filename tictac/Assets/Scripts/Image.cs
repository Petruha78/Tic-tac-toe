using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void TurnMethod(Player currentPlayer);

public class Image : GameElement
{

    
    
    public static Color idle;
    public  bool isTurned;
    
    public Sprite sprite;

	void Start ()
    {
        idle = new Color(255, 255, 255, 0);
        GetComponent<UnityEngine.UI.Image>().color = idle;
       
	}
	
	
	void Update ()
    {
        if(isTurned == true)
        {
            if(GetComponentInParent<Button>() != null)
            GetComponentInParent<Button>().interactable = false;
            
        }


        
    }

    public void Turn(Player currentPlayer)
    {

        if (sprite == null && !currentPlayer.Equals(default(Player)))
        {

            sprite = currentPlayer.figure;
            GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 255);
            GetComponent<UnityEngine.UI.Image>().sprite = sprite;
            isTurned = true;
            
            
        }
        

        if (currentPlayer.Equals(default(Player)))
        {
            GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 0);
            sprite = null;
            GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        }

    }

    public void SetMethod()
    {
        
        Gamefield.Method = Turn;
        
    }
}
