using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Define the callback function signature
delegate void CallbackFunction();

public class GameManager : MonoBehaviour {
  Image imgYou; // your selected image (rock, paper scissor)
  Image imgCom; // computer selected image (rock, paper scissor)

  // animation Object
  Animator anim_You; // your selected image (rock, paper scissor)
  Animator anim_Com; // computer selected image (rock, paper scissor)



  Text txtYou;    // the score you win
  Text txtCom;    // the score computer win
  Text txtResult; // the result
  TextMeshProUGUI txtRound;
  // entire UI except menu button
  GameObject ui_panel;

  // Group of Rock, Paper and Scissor Button so it can to turn off/on in group
  GameObject buttonGroup;
  // game state (NOTE: this if there are something to add in future)
  GameState state;
  int round = 1;

  int cntYou = 0; // your scores
  int cntCom = 0; // computer's scores

  int comChoice = 0;
  int yourChoice = 0;

  // Game State for different situation
  enum GameState {
    STARTING,
    CHOOSING,
    ACTION,
    RESULT,
  };


  // if the result between the player and computer is draw
  bool isDraw = false;

  // start the first frame
  void Start() {
      InitGame();
  }

  // Update is called once per frame
  void Update() {
    // exit if press escape key
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }
  }


  void CheckResult() {
    // algorithm determine the result
    isDraw = false;
    comChoice = UnityEngine.Random.Range(1, 4);
    int k = yourChoice - comChoice;
    switch (k) {
         case 0:
           isDraw = true;
           txtResult.text = "Draw!!!";
           break;
         case 1:
         case -2:
           cntYou++;
           round++;
           txtResult.text = "You win.";
           break;
         default:
           cntCom++;
           round++;
           txtResult.text = "Computer win.";
           break;
    }
    //SetResult(yourResult, comResult); // set game result to UI
    RunChoice();
  }

  void RunChoice()
  {
        // turn to rock
        // just like do irl
        // invert image com in x axis
        imgCom.transform.localScale = new Vector3(-1, 1, 1);
        imgYou.sprite = Resources.Load("img_" + 1, typeof(Sprite)) as Sprite;
        imgCom.sprite = Resources.Load("img_" + 1, typeof(Sprite)) as Sprite;
        anim_Com.SetTrigger("TrActive");
        anim_You.SetTrigger("TrActive");
        StartCoroutine(DelayAction(0.5f, SetResult));
  }

  void SetResult() {
    // change image
    state = GameState.RESULT;
    imgYou.GetComponent<Image>().sprite = Resources.Load("img_" + yourChoice, typeof(Sprite)) as Sprite;
    imgCom.GetComponent<Image>().sprite = Resources.Load("img_" + comChoice, typeof(Sprite)) as Sprite;

    // winning score
    txtYou.text = cntYou.ToString();
    txtCom.text = cntCom.ToString();
    txtRound.text = "Round " + round.ToString();


    if(isDraw) {
        state = GameState.CHOOSING;
        return;
    }
    // turn off buttons
    buttonGroup.SetActive(false);
    // delay for 3 seconds to create the effect of changing round
    // StartRound();
    StartCoroutine(DelayAction(2, StartRound));
  }


  private void InitGame() {
    imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
    imgCom = GameObject.Find("ImgCom").GetComponent<Image>();
    anim_You = GameObject.Find("ImgYou").GetComponent<Animator>();
    anim_Com = GameObject.Find("ImgCom").GetComponent<Animator>();
    txtYou = GameObject.Find("TxtYou").GetComponent<Text>();
    txtCom = GameObject.Find("TxtCom").GetComponent<Text>();
    txtResult = GameObject.Find("TxtResult_").GetComponent<Text>();
    txtRound = GameObject.Find("TxtRound").GetComponent<TextMeshProUGUI>();
    buttonGroup = GameObject.Find("buttonGroup");

    // init the text before the game start
    txtResult.text = "Select the button below";
    txtRound.text = "Round 1";
    ui_panel = GameObject.Find("UI");
    //begin the round
    StartRound();
  }

  void StartRound() {

    buttonGroup.SetActive(true);
    if(cntYou >=3) {
        SceneManager.LoadScene("YouWinScene");
        return;
    }
    else if (cntCom >=3) {
        SceneManager.LoadScene("YouLoseScene"); state = GameState.STARTING;
        return;
    }
    yourChoice = 0;
    comChoice = 0;
    // return the question mark state
    imgYou.sprite  = Resources.Load("img_" + 4, typeof(Sprite)) as Sprite;
    imgCom.sprite= Resources.Load("img_" + 4, typeof(Sprite)) as Sprite;
    // rotate
    imgCom.transform.localScale = new Vector3(1, 1, 1);
    txtRound.gameObject.SetActive(true);
    ui_panel.SetActive(false);

    // delay for 3 seconds
    // display round
    StartCoroutine(DelayAction(2, () => {
      txtRound.gameObject.SetActive(false);
      ui_panel.SetActive(true);
      state = GameState.CHOOSING;
    }));
  }

// Delay function
  IEnumerator DelayAction(float delayTime, CallbackFunction Action) {
    // Wait for the specified delay time before continuing.
    yield return new WaitForSeconds(delayTime);

    // Do the action after the delay time has finished.
    Action();
  }

  public void OnButtonClick(GameObject buttonObject) {
    // event when button is clicked
    if(state == GameState.STARTING || state == GameState.RESULT) return;
    yourChoice = int.Parse(buttonObject.name.Substring(0, 1));
    CheckResult();
  }
  public void OnMouseExit(GameObject buttonObject) {
    // event when mouse exit the button

    // get the animator component from the button
    Animator anim = buttonObject.GetComponent<Animator>();
    // switch to idle animation
    anim.Play("Normal");
  }

  public void BackToMenu() { SceneManager.LoadScene("MenuScene"); }
}
