using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
  public Button button;
  public Text buttonText;
  

  //instance of the game controller class `
  private GameController gameController;


  // Functionality for the button interactions 
  public void SetSpace()
  {
    if (gameController.playerMove == true)
    {
      buttonText.text = gameController.GetPlayerSide();
      button.interactable = false;
      gameController.EndTurn();
    }
  }

  public void SetGameControllerReference(GameController controller)
  {
   gameController = controller;
  } 

}