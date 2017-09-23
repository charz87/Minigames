using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManagerMultipleChoice_Cannon : MonoBehaviour {

    private bool bMyClickedAnswer = false;// to check if an aswer was clicked
    private bool bMyGameWon = false;// to check if the game is already won
 
    public enum GameSetting { CO_OP, VS, SINGLE };// what setting  of game is this
    public GameSetting gameSetting;
    private int currentPlayer;// current player
    private int correctAnswer;// correct answer

    [SerializeField]
    private GeneralGameManager generalGameManagerRef;// reference to the General Game Manager

    [SerializeField]
    private Cannon cannonPlayer1; // reference to cannon player 1 script

    [SerializeField]
    private Slider lifeBarPlayer1;// Life bar of player 1, IF NEEDED

    [SerializeField]
    private Cannon cannonPlayer2;// reference to cannon player 1 script

    [SerializeField]
    private Slider lifeBarPlayer2;// Life bar of player 2, IF NEEDED

    [SerializeField]
    private Score scorePlayer1;//Score player 1 

    [SerializeField]
    private Score scorePlayer2;//Score player 2

    [SerializeField]
    private GameObject enemyBoss;// reference to the Boss GameObject

    [SerializeField]
    private GameObject player1Object;// reference to the Player 1 GameObject

    [SerializeField]
    private GameObject player2Object;// reference to the Player 2 GameObject

    [SerializeField]
    private Slider lifeBarEnemyBoss;// Life bar of enemy boss, IF NEEDED

    [SerializeField]
    private Animator animFeedback;// Animator feedback of the Canvas

    [SerializeField]
    private Animator animScoreFeedback;// Animator feedback of the Canvas Score

    [SerializeField]
    private Animator animFoxyP1;// Animator feedback of the Player1

    [SerializeField]
    private Animator animFoxyP2;// Animator feedback of the Player2
    /* GABO NETWORKING */
    /*
    private void OnEnable()
    {
        ClientManager.singleton.OnReceivedMessage += NetworkReceiver;
    }

    private void OnDisable()
    {
        ClientManager.singleton.OnReceivedMessage -= NetworkReceiver;
    }
    */
    /************************************************************************/
    private void Awake()
    {
        switch (gameSetting) // What setting of this game are we choosing
        {
            //Activate and Deactivate objects according to setting of the game
            case GameSetting.VS:
                enemyBoss.SetActive(false);
                lifeBarEnemyBoss.gameObject.SetActive(false);
                lifeBarPlayer1.gameObject.SetActive(true);
                lifeBarPlayer2.gameObject.SetActive(true);
                player1Object.SetActive(true);
                player2Object.SetActive(true);
                break;
            case GameSetting.CO_OP:
                lifeBarPlayer1.gameObject.SetActive(false);
                lifeBarPlayer2.gameObject.SetActive(false);
                player1Object.SetActive(true);
                player2Object.SetActive(true);
                enemyBoss.SetActive(true);
                lifeBarEnemyBoss.gameObject.SetActive(true);
                break;
            case GameSetting.SINGLE:
                enemyBoss.SetActive(true);
                lifeBarEnemyBoss.gameObject.SetActive(true);
                player1Object.SetActive(true);
                player2Object.SetActive(false);
                lifeBarPlayer2.gameObject.SetActive(false);
                break;
            default:
                break;
        }

    }
    /*CHARZ LOCAL GAMEPLAY*/
    private void Update()
    {
        GameWonYet();//We already won?
    }

    /*CHARZ LOCAL GAMEPLAY*/
    IEnumerator TransitionToPlayer1WinScreen()//Transition to Player 1 Wins Screen 
    {
        yield return new WaitForSeconds(2);
        animFeedback.SetBool("Team1Wins", true);
        animScoreFeedback.SetBool("ShowScore", true);
        animFoxyP1.SetTrigger("WinState");
        animFoxyP2.SetTrigger("LoseState");
    }
    /*CHARZ LOCAL GAMEPLAY*/
    IEnumerator TransitionToPlayer2WinScreen()//Transition to Player 2 Wins Screen 
    {
        yield return new WaitForSeconds(2);
        animFeedback.SetBool("Team2Wins", true);
        animScoreFeedback.SetBool("ShowScore", true);
        animFoxyP2.SetTrigger("WinState");
        animFoxyP1.SetTrigger("LoseState");
    }
    /*CHARZ LOCAL GAMEPLAY*/
    IEnumerator TransitionToAllWinScreen()//Transition to Everybody Wins Screen 
    {
        yield return new WaitForSeconds(2);
        animFeedback.SetBool("AllWins", true);
        animScoreFeedback.SetBool("ShowScore", true);
        animFoxyP2.SetTrigger("WinState");
        animFoxyP1.SetTrigger("WinState");
    }
    /*CHARZ LOCAL GAMEPLAY*/
    void GameWonYet()
    {
        if (cannonPlayer1.myTargetHealth.GetCurrentHealth() <= 0)//if Player 2 Get 0 Health
        {
            bMyGameWon = true;
            //Check whether is cooperative or not and activate Winning screen
            if (gameSetting == GameSetting.VS || gameSetting == GameSetting.SINGLE)
            {
                StartCoroutine(TransitionToPlayer1WinScreen());
            }
            else
            {
                StartCoroutine(TransitionToAllWinScreen());
            }

        }

        if (cannonPlayer2.myTargetHealth.GetCurrentHealth() <= 0)//if Player 1 Get 0 Health
        {
            bMyGameWon = true;
            //Check whether is cooperative or not and activate Winning screen
            if (gameSetting == GameSetting.VS)
            {
                StartCoroutine(TransitionToPlayer2WinScreen());
            }
            else
            {
                StartCoroutine(TransitionToAllWinScreen());
            }

        }
    }
    //This to check player who is answering, selected answer and correct answer
    public void AnswerSelected(int selected)
    {
        //selectedAnswer = generalGameManagerRef.selectedAnswer;
        currentPlayer = generalGameManagerRef.currentPlayer;//CHARZ LOCAL GAMEPLAY
       // correctAnswer = generalGameManagerRef.correctAnswer;

        //Did my answer was correct?
        if (selected == generalGameManagerRef.correctAnswer)
        {
            if (currentPlayer == 1)//Player 1 Fires To target
            {
                cannonPlayer1.ShootProjectileToTarget();
                animFoxyP1.SetTrigger("Fire");
                scorePlayer1.AddScore();
            }
            else
            {   //Player 2 Fires To target
                cannonPlayer2.ShootProjectileToTarget();
                animFoxyP2.SetTrigger("Fire");
                scorePlayer2.AddScore();
            }
        }
        else
        {
            if (currentPlayer == 1)//Player 1 Fires To MISS Target
            {
                cannonPlayer1.ShootProjectileToMissTarget();
                animFoxyP1.SetTrigger("Fire");
            }
            else
            {   //Player 2 Fires To MISS Target
                cannonPlayer2.ShootProjectileToMissTarget();
                animFoxyP2.SetTrigger("Fire");
            }
           
        }
    }

    /*GABO NETWORKING*/
    /*
    private void NetworkReceiver(NetworkMessage netMess)
    {
        DataHandler.ClassRoom dataReceiver = netMess.ReadMessage<DataHandler.ClassRoom>();

        if (dataReceiver.messageType == "Next")
            generalGameManagerRef.SetCurrentQuestion();
        if (dataReceiver.messageType == "Fire")
        {
            currentPlayer = dataReceiver.playerTeam;
            AnswerSelected(dataReceiver.answerID);
        }
    }
    */
    /*CHARZ LOCAL GAMEPLAY*/
    public bool GetIsGameWon()//This is to pass to General Game Manager if we already won the game
    {
        return bMyGameWon;
    }
}

