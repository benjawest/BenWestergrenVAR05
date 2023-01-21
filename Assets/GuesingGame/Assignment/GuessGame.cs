using TMPro;
using UnityEngine;


//Bens Version

public class GuessGame : MonoBehaviour
{
    // A reference to the Text object and Input Field. Declare ints for setting up the game
    public TextMeshProUGUI outputText;
    public TMP_InputField inputField;
    private int upperBound;
    private int randomNumber;
    private bool upperBoundSet = false;

   
    void Start()
    {
    
    }


    // This function check if the user has set the upperbound, if not, then it prompts to set it. and changes the text in the game to guess the number.
    // If the function is called when the upperbound has been set, then check if the input matches the generated randomNumber
    public void Guess()
    {

        // Check if user has set Upper Bound, if not, prompt to set it.
        if (!upperBoundSet)
        {
            // upperBound is set to user input
            upperBound = int.Parse(inputField.text);
            
            // Generate random number between 1 and upper bound
            randomNumber = Random.Range(1, upperBound + 1);

            // Debug line, check what number was generated
            Debug.Log("The random number is: " + randomNumber);

            // Print Prompt asking user for upper bound
            outputText.text = "Guess the number between 1 and " + upperBound;
            upperBoundSet = true;
        }

        // Check if guess is correct
        // if the guess is too high guess again
        // if the guess is too low guess again
        // if user input matches print Success
        else
        {
            int guess = int.Parse(inputField.text);
            if (guess > randomNumber && guess <= upperBound)
            {
                outputText.text = "Too high, guess again:";
            }
            else if (guess < randomNumber)
            {
                outputText.text = "Too low, guess again:";
            }
            else if(guess > upperBound)
            {
                outputText.text = "Not within bounds. Guess the number between 1 and " + upperBound;
            }
            else
            {
                outputText.text = "You have won! The number was " + randomNumber;
              
            }

        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
