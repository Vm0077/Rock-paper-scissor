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

  Text txtYou;    // the score you win
  Text txtCom;    // the score computer win
  Text txtResult; // the result
  TextMeshProUGUI txtRound;
  GameObject ui_panel;
  GameState state;
  int round = 1;

  int cntYou = 0; // your scores
  int cntCom = 0; // computer's scores
  enum GameState {
    STARTING,
    CHOOSING,
    RESULT,
  }
  ;

  void Start() {
    // init the game
    InitGame();
  }

  // Update is called once per frame
  void Update() {
    // exit if press escape key
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }
  }

  void CheckResult(int yourResult) {
    // algorithm determine the result

    int comResult = UnityEngine.Random.Range(1, 4);
    int k = yourResult - comResult;
    switch (k) {
    case 0:
      txtResult.text = "Draw.";
      break;
    case 1:
    case -2:
      cntYou++;
      txtResult.text = "You win.";
      break;
    default:
      cntCom++;
      txtResult.text = "Computer win.";
      break;
    }
    round++;
    SetResult(yourResult, comResult); // set game result to UI
  }

  void SetResult(int you, int com) {
    // change image
    state = GameState.RESULT;
    imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
    imgCom.sprite = Resources.Load("img_" + com, typeof(Sprite)) as Sprite;

    // invert image com in x axis
    imgCom.transform.localScale = new Vector3(-1, 1, 1);
    // winning score
    txtYou.text = cntYou.ToString();
    txtCom.text = cntCom.ToString();
    if(cntYou >=3) SceneManager.LoadScene("YouWin");
    if(cntCom >=3) SceneManager.LoadScene("YouLose");
    txtRound.text = "Round " + round.ToString();
    // delay for 3 seconds
    StartCoroutine(DelayAction(2, StartRound));
    // StartRound();
  }

  public void OnButtonClick(GameObject buttonObject) {
    // event when button is clicked
    if(state == GameState.STARTING || state == GameState.RESULT) return;
    int you = int.Parse(buttonObject.name.Substring(0, 1));
    CheckResult(you);
    // Debug.Log("clicked");
  }
  public void OnMouseExit(GameObject buttonObject) {
    // event when mouse exit the button

    // get the animator component from the button
    Animator anim = buttonObject.GetComponent<Animator>();
    // switch to idle animation
    anim.Play("Normal");
    // Debug.Log("Mouse Exit");
  }
  private void InitGame() {
    imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
    imgCom = GameObject.Find("ImgCom").GetComponent<Image>();

    txtYou = GameObject.Find("TxtYou").GetComponent<Text>();
    txtCom = GameObject.Find("TxtCom").GetComponent<Text>();
    txtResult = GameObject.Find("TxtResult_").GetComponent<Text>();
    txtRound = GameObject.Find("TxtRound").GetComponent<TextMeshProUGUI>();

    // init the text before the game start
    txtResult.text = "Select the button below";
    txtRound.text = "Round 1";
    ui_panel = GameObject.Find("UI");
    StartRound();
  }

  void StartRound() {
    state = GameState.STARTING;
    imgYou.sprite = Resources.Load("img_" + 4, typeof(Sprite)) as Sprite;
    imgCom.sprite = Resources.Load("img_" + 4, typeof(Sprite)) as Sprite;
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


  IEnumerator DelayAction(float delayTime, CallbackFunction Action) {
    // Wait for the specified delay time before continuing.
    yield return new WaitForSeconds(delayTime);

    // Do the action after the delay time has finished.
    Action();
  }

  public void BackToMenu() { SceneManager.LoadScene("MenuScene"); }
}
