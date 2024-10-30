using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Image imgYou;       // your selected image (rock, paper scissor)
    Image imgCom;       // computer selected image (rock, paper scissor)

    Text txtYou;        // the score you win
    Text txtCom;        // the score computer win
    Text txtResult;     // the result

    int cntYou = 3;     // your life
    int cntCom = 3;     // computer life
    void CheckResult(int yourResult)
    {
        // algorithm determine the result

        int comResult = UnityEngine.Random.Range(1, 4);
        int k = yourResult - comResult;
        switch(k){
            case 0:
                txtResult.text = "Draw.";
                break;
            case 1 :
            case -2 :
                cntYou++;
                txtResult.text = "You win.";
                break;
            default:
                cntCom++;
                txtResult.text = "Computer win.";
                break;
            
        }
        SetResult(yourResult, comResult);    // set game result to UI
    }
    void SetResult(int you, int com)
    {
        // change image
        imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        imgCom.sprite = Resources.Load("img_" + com, typeof(Sprite)) as Sprite;

        // invert image com in x axis
        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // winning score
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();

        // play text animation
        //txtResult.GetComponent<Animator>().Play("TextScale", -1, 0);
    }

    public void OnButtonClick(GameObject buttonObject)
    {
        //event when button is clicked
        int you = int.Parse(buttonObject.name.Substring(0, 1));
        CheckResult(you);
        //Debug.Log("clicked");
    }
    public void OnMouseExit(GameObject buttonObject)
    {
        //event when mouse exit the button

        //get the animator component from the button
        Animator anim = buttonObject.GetComponent<Animator>();
        //switch to idle animation
        anim.Play("Normal");
        //Debug.Log("Mouse Exit");
    }
    private void InitGame()
    {
        imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
        imgCom = GameObject.Find("ImgCom").GetComponent<Image>();

        txtYou = GameObject.Find("TxtYou").GetComponent<Text>();
        txtCom = GameObject.Find("TxtCom").GetComponent<Text>();
        txtResult = GameObject.Find("TxtResult_").GetComponent<Text>();

        //init the text before the game start
        txtResult.text = "Select the button below";
    }


    private void GameOver () { 
        
    }
    // Start is called before the first frame update
    void Start()
    {
        // init the game
        InitGame();    
    }

    // Update is called once per frame
    void Update()
    {
        //exit if press escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void BackToMenu(){ 
        SceneManager.LoadScene("MenuScene");
    }
}
