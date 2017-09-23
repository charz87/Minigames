[System.Serializable]
public class Question
{
    public enum QuestionType {TRUEFALSE, MULTIPLECHOICE };

    public QuestionType Type = QuestionType.MULTIPLECHOICE;

    public string question;
    public string answer1;
    public string answer2;
    public string answer3;
    public bool isTrue;//Used Only in the true or false game
    public int correctAnswerId;


}
