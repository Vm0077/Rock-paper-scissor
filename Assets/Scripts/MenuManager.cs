using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    void Start () {
       // nothing in here 
    }
    public void OnStartGame() {
        SceneManager.LoadScene("GameScene");
    }
    public void OnReturntoMenu() {
        SceneManager.LoadScene("MenuScene");
    }
    public void OnExit() {
        Application.Quit();
        Debug.Log("Quit");
    }

}