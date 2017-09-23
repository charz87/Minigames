using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManagerMultipleChoice_Obstacles : MonoBehaviour {

    private bool bMyClickedAnswer = false; // to check if an answer was clicked
    private bool bMyGameWon = false; // to check if the game is already won

    public enum GameSetting { CO_OP, VS, SINGLE };// what setting  of game is this
    public GameSetting gameSetting;
    private int currentPlayer;// current player
    private int correctAnswer;// correct answer

    [SerializeField]
    private GeneralGameManager generalGameManagerRef; // reference to the General Game Manager

    [SerializeField]
    private Slider lifeBarPlayer1; // Life bar of player 1, IF NEEDED

    [SerializeField]
    private Slider lifeBarPlayer2; // Life bar of player 2, IF NEEDED

    [SerializeField]
    private Slider lifeBarEnemyBoss; // // Life bar of enemy boss, IF NEEDED

    [SerializeField]
    private ShipMover shipPlayer1; //The player 1 vehicle

    [SerializeField]
    private ShipMover shipPlayer2; // The player 2 vehicle

    [SerializeField]
    private Score scorePlayer1;//Score player 1 

    [SerializeField]
    private Score scorePlayer2;//Score player 2

    [SerializeField]
    private GameObject player1Object;// Reference to the Player 1 Gameobject

    [SerializeField]
    private GameObject player2Object; // reference to the Player 2 GameObject

    [SerializeField]
    private GameObject targetToReach;// The target that will be the goal to Reach

    [SerializeField]
    private float offsetDistance;// The Distance that the goal will appear

    [SerializeField]
    private Animator animFeedback; // Animator Feedback of the Score

    [SerializeField]
    private Animator animScoreFeedback; // Animator Feedback of the Score

    [SerializeField]
    private Animator animFoxyP1; // Animator of the Player 1

    [SerializeField]
    private Animator animFoxyP2; // Animator of the Player 2

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
        switch (gameSetting) // What type pf setting we are having for this game, VS, SINGLE, CO-OP
        {
            //Activate and Deactivate GameObjects depending on the Game Setting Selected
            case GameSetting.VS:
                player1Object.SetActive(true);
                player2Object.SetActive(true);
                lifeBarPlayer1.gameObject.SetActive(false);
                lifeBarPlayer2.gameObject.SetActive(false);
                lifeBarEnemyBoss.gameObject.SetActive(false);
                break;
            case GameSetting.CO_OP:
                player1Object.SetActive(true);
                player2Object.SetActive(true);
                lifeBarPlayer1.gameObject.SetActive(false);
                lifeBarPlayer2.gameObject.SetActive(false);
                lifeBarEnemyBoss.gameObject.SetActive(false);
                break;
            case GameSetting.SINGLE:
                player1Object.SetActive(true);
                player2Object.SetActive(false);
                lifeBarPlayer1.gameObject.SetActive(false);
                lifeBarPlayer2.gameObject.SetActive(false);
                lifeBarEnemyBoss.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        SpawnTargetToReach();

    }

    void SpawnTargetToReach()
    {
        Vector3 lastPosition = player1Object.transform.position;
        Vector3 offsetVector = new Vector3(offsetDistance, 0, 0);
        GameObject goal = Instantiate(targetToReach, lastPosition + offsetVector, Quaternion.identity);
        goal.name = "Goal";

    }
    /*CHARZ LOCAL GAMEPLAY*/
    private void Update()
    {
        GameWonYet(); // Check whether or no the has been won
    }
    /*CHARZ LOCAL GAMEPLAY*/
    IEnumerator TransitionToPlayer1WinScreen() //Transition to Player 1 Wins Screen 
    {
        yield return new WaitForSeconds(2);
        animFeedback.SetBool("Team1Wins", true);
        animScoreFeedback.SetBool("ShowScore", true);
        // TODO activate the animators when i got the assets from 3d
        //animFoxyP1.SetTrigger("WinState");
        //animFoxyP2.SetTrigger("LoseState");
    }
    /*CHARZ LOCAL GAMEPLAY*/
    IEnumerator TransitionToPlayer2WinScreen()//Transition to Player 2 Wins Screen 
    {
        yield return new WaitForSeconds(2);
        animFeedback.SetBool("Team2Wins", true);
        animScoreFeedback.SetBool("ShowScore", true);
        // TODO activate the animators when i got the assets from 3d
        //animFoxyP2.SetTrigger("WinState");
        //animFoxyP1.SetTrigger("LoseState");
    }
    /*CHARZ LOCAL GAMEPLAY*/
    IEnumerator TransitionToAllWinScreen()//Transition to Everybody Wins Screen 
    {
        yield return new WaitForSeconds(2);
        animFeedback.SetBool("AllWins", true);
        animScoreFeedback.SetBool("ShowScore", true);
        // TODO activate the animators when i got the assets from 3d
        //animFoxyP2.SetTrigger("WinState");
        //animFoxyP1.SetTrigger("WinState");
    }
    /*CHARZ LOCAL GAMEPLAY*/
    void GameWonYet()
    {
        //If player 1 reached Goal, then player 1 wins
        if (shipPlayer1.ShipReachedGoal())
        {
            bMyGameWon = true; // The game is won
            StartCoroutine(TransitionToPlayer1WinScreen());
        }
        //If player 2 reched Goal, then player 2 wins
        if (shipPlayer2.ShipReachedGoal())
        {
            bMyGameWon = true; // The game is won
            StartCoroutine(TransitionToPlayer2WinScreen());
        }

    }

    public void AnswerSelected(int selected)
    {
       /*CHARZ LOCAL GAMEPLAY*/ currentPlayer = generalGameManagerRef.currentPlayer; // get the player playing from the General Game Manager
       // selectedAnswer = generalGameManagerRef.selectedAnswer;// get the selected Answer from the General Game Manager
       // correctAnswer = generalGameManagerRef.correctAnswer; // get the correct Answer from the General Game Manager
        //Did my answer was correct?
        if (selected == generalGameManagerRef.correctAnswer)
        {
            if (currentPlayer == 1)//Player 1 Moves Front
            {
                shipPlayer1.MoveShipToFront();
                scorePlayer1.AddScore();
                //animFoxyP1.SetTrigger("Fire");
            }
            else
            {
                shipPlayer2.MoveShipToFront();
                scorePlayer2.AddScore();
                //animFoxyP2.SetTrigger("Fire");
            }

        }
        else
        {
            if (currentPlayer == 1)//Player 1 Moves Back
            {
                shipPlayer1.MoveShipToBack();
                // animFoxyP1.SetTrigger("Fire");
            }
            else
            {   //Player 2 Moves Back
                shipPlayer2.MoveShipToBack();
                //animFoxyP2.SetTrigger("Fire");
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
    public bool GetIsGameWon()//This is to pass to the General Game Manager if we already won the game
    {
        return bMyGameWon;
    }



}
