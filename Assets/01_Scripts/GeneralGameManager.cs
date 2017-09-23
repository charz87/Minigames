using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GeneralGameManager : MonoBehaviour {

    public enum GameType {BearChase,Towers,Pirates,Ships} // What Type game we are going to select

    public GameType gameType;

    private GameManagerMultipleChoice_Cannon multipleChoiceCannonManager; // Reference to Cannon game manager
    private GameManagerMultipleChoice_Escape multipleChoiceEscapeManager; // Reference to Escape game manager
    private GameManagerMultipleChoice_Obstacles multipleChoiceObstaclesManager;//Reference to Obstacles game manager

    public List<Question> questions; // the total of questions we are going to have
    private int countQuestion;

    private Question currentQuestion; //The question that is going to be displayed

    /*CHARZ LOCAL GAMEPLAY*/
    private bool bGameWon = false;// check if already won the game 
    public int currentPlayer { get; set; } // the player who is currently in play
    public int selectedAnswer { get; set; } // selected answer id
    public int correctAnswer { get; set; }//correct answer id

    [SerializeField]
    private Light realTimeLight;

    [SerializeField]
    private GameObject towersAssets; //The Towers game assets

    [SerializeField]
    private GameObject pirateAssets; // The Pirates game Assets

    [SerializeField]
    private GameObject bearChaseAssets; // The Bear Chase Game Assets

    [SerializeField]
    private GameObject shipsAssets;//The Ship Obstacle Game Assets

    [SerializeField]
    private Text player1Text; // Text to show only the player 1 turn

    [SerializeField]
    private Text player2Text;// Text to show only the player 2 turn

    [SerializeField]
    private Text questionText; // Text holder for the question

    [SerializeField]
    private Button answer1Button, answer2Button, answer3Button;// Button to select the id correspondig to option answer

    [SerializeField]
    private float timeBetweenQuestions; // Time that we will have before showing other questions

    private void Awake()
    {
        //GABO NETWORKING
       // GameType yourEnum = (GameType)System.Enum.Parse(typeof(GameType), PlayerPrefs.GetString("gameType"));
       // gameType = yourEnum;
       //-----------------------------------------------------
        switch (gameType) // Checking out type of Game
        {
            case GameType.Towers: // Activate Towers assets and set up the manager reference for this game
                towersAssets.SetActive(true);
                pirateAssets.SetActive(false);
                bearChaseAssets.SetActive(false);
                shipsAssets.SetActive(false);
                realTimeLight.gameObject.SetActive(false);
                multipleChoiceCannonManager = GameObject.Find("GameTowersAssetsManager").GetComponent<GameManagerMultipleChoice_Cannon>();
                break;
            case GameType.Pirates: // Activate Pirates assets and set up the manager reference for this game
                towersAssets.SetActive(false);
                pirateAssets.SetActive(true);
                bearChaseAssets.SetActive(false);
                shipsAssets.SetActive(false);
                realTimeLight.gameObject.SetActive(false);
                multipleChoiceCannonManager = GameObject.Find("GamePiratesAssetsManager").GetComponent<GameManagerMultipleChoice_Cannon>();
                break;
            case GameType.BearChase:// Activate BearChase assets and set up the manager reference for this game
                towersAssets.SetActive(false);
                pirateAssets.SetActive(false);
                bearChaseAssets.SetActive(true);
                shipsAssets.SetActive(false);
                realTimeLight.gameObject.SetActive(true);
                multipleChoiceEscapeManager = GameObject.Find("GameBearChaseAssetsManager").GetComponent<GameManagerMultipleChoice_Escape>();
                break;
            case GameType.Ships://Activate Ships assets and set up the manager reference
                towersAssets.SetActive(false);
                pirateAssets.SetActive(false);
                bearChaseAssets.SetActive(false);
                shipsAssets.SetActive(true);
                realTimeLight.gameObject.SetActive(false);
                multipleChoiceObstaclesManager = GameObject.Find("GameShipObstaclesManager").GetComponent<GameManagerMultipleChoice_Obstacles>();
                break;
            default:
                break;
        }

        currentPlayer = 1;// CHARZ LOCAL GAMEPLAY
        SetCurrentQuestion();// We Set the first Question
    }
   
    /*CHARZ LOCAL GAMEPLAY*/
    private void Update()
    {
        PlayerTurn(); //Whose Player Turn is it
        IsGameWon(); // The Game is Won?
    }

    /*CHARZ LOCAL GAMEPLAY*/
    // Change current Player we are using //CHARZ LOCAL GAMEPLAY
    public void ChoosePlayerPlayingTurn(int playerId)
    {
        currentPlayer = playerId;
    }
 
    //Method to Set the Questions of the game
    public void SetCurrentQuestion()
    {
        /* GABO NETWORKING// FINITE QUESTIONS*/
        currentQuestion = questions[countQuestion];

        questionText.text = currentQuestion.question; // get the question holder text to the current question text
        answer1Button.gameObject.transform.GetChild(0).GetComponent<Text>().text = currentQuestion.answer1;
        answer2Button.gameObject.transform.GetChild(0).GetComponent<Text>().text = currentQuestion.answer2;
        answer3Button.gameObject.transform.GetChild(0).GetComponent<Text>().text = currentQuestion.answer3;
        //answer4Button.gameObject.transform.GetChild(0).GetComponent<Text>().text = currentQuestion.answer4;
        correctAnswer = currentQuestion.correctAnswerId; // get correct answer index based on the current question correct answer index

        //Activate the selection Buttons, this to prevent various input of the same person
        answer1Button.interactable = true;
        answer2Button.interactable = true;
        answer3Button.interactable = true;
        //answer4Button.interactable = true;//////////////////////////////

        countQuestion++;
       
    }
    /*CHARZ LOCAL GAMEPLAY */
    IEnumerator TransitionToNextQuestion() // We change current question and prepare another
    {
        //questions.Remove(currentQuestion);
        //unansweredQuestions.Remove(currentQuestion); // Remove current question from the list
        yield return new WaitForSeconds(timeBetweenQuestions);
        if (!bGameWon)
        {
            SetCurrentQuestion(); //Get another Question
        }

    }
    /*CHARZ LOCAL GAMEPLAY*/
    void PlayerTurn()
    {
        //Player 1 turn-Player 1 will be the one to make the action
        if (currentPlayer == 1)
        {
            player1Text.gameObject.SetActive(true);
            player2Text.gameObject.SetActive(false);
        }
        //Player 2 turn-Player 2 will be the one to make the action
        if (currentPlayer == 2)
        {
            player2Text.gameObject.SetActive(true);
            player1Text.gameObject.SetActive(false);
        }

    }
    /* //GABO NETWORKING FUNCTION
    public void SendMessage(int idAnswer)
    {
        DataHandler.ClassRoom dataSend = new DataHandler.ClassRoom();
        dataSend.answerID = idAnswer;
        dataSend.playerTeam = PlayerPrefs.GetInt("Team");
        dataSend.messageType = "Answered";
        ClientManager.singleton.client.Send((short)MessageTypeUNET.Message, dataSend);
        answer1Button.interactable = false;
        answer2Button.interactable = false;
        answer3Button.interactable = false;
        answer4Button.interactable = false;
    }
    */
    /* CHARZ LOCAL GAMEPLAY*/
    public void UserSelectAnswer(int idAnswer)
    {
            answer1Button.interactable = false;
            answer2Button.interactable = false;
            answer3Button.interactable = false;

            switch(gameType) // Going to check what type of game i´m in and call respective method of AnswerSelected
            {
                case GameType.BearChase://Escape Game, call Escape Game Manager
                    multipleChoiceEscapeManager.AnswerSelected(idAnswer);
                    break;
                case GameType.Towers:// Cannon Game, call Cannon Game Manager
                    multipleChoiceCannonManager.AnswerSelected(idAnswer);
                    break;
                case GameType.Pirates:// Cannon Game, call Cannon Game Manager
                    multipleChoiceCannonManager.AnswerSelected(idAnswer);
                    break;
                case GameType.Ships: // Obstacles Game, call Obstacles Game Manager
                    multipleChoiceObstaclesManager.AnswerSelected(idAnswer);
                    break;

            }
        StartCoroutine(TransitionToNextQuestion()); // Going to the Next Question

    }
   /*CHARZ LOCAL GAMEPLAY*/
    void IsGameWon() // Set bGameWon to check if we already won depending on what type of game are we playing
    {

        switch (gameType) // Going to check what type of game i´m in and call respective method of AnswerSelected
        {
            case GameType.BearChase://Escape Game, call Escape Game Manager
                bGameWon = multipleChoiceEscapeManager.GetIsGameWon();
                break;
            case GameType.Towers:// Cannon Game, call Cannon Game Manager
                bGameWon = multipleChoiceCannonManager.GetIsGameWon();
                break;
            case GameType.Pirates:// Cannon Game, call Cannon Game Manager
                bGameWon = multipleChoiceCannonManager.GetIsGameWon();
                break;
            case GameType.Ships:
                bGameWon = multipleChoiceObstaclesManager.GetIsGameWon();
                break;
        }

        if (bGameWon) // if we have won deactivate the answer buttons
        {
            answer1Button.interactable = false;
            answer2Button.interactable = false;
            answer3Button.interactable = false;
        }
    }



        
}
