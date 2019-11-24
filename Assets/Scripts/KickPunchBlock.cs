using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = System.Random;

public class KickPunchBlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countryOptions;

 

    // Static because variables are not used in a constructor
    static string playerOneName = "";
    static string opponentName = "";
    static string country = "";
    static string countryUpperCase = "";
    static string whoGotHurt = "";
    static bool opponentHurt = false;
    static bool playerOneHurt = false;
    static bool counterAttack = false;
    static bool player1ValidName = false;
    static bool player2ValidName = false;
    static bool isMultiplayer = false;
    static int playerOneHealth = 100;
    static int opponentHealth = 100;
    static int player1Action;
    static int opponentAction;
    static int numOfPlayers;
    static int player1input;
    static int player2input;
    static int damageRoll;
    static int roundNumber = 1;
    static int DAMAGE_MULTIPLIER = 2;
    static string DIFFICULTY = "MEDIUM"; // EASY, MEDIUM, HARD
    static string[] actionArray = { "Kick", "Punch", "Block" };
    static string[] playerOneArray = { "Superman", "Batman", "Flash", "Wonderwoman", "Aquaman", "Cedric" };
    static string[] playerTwoArray = { "Thor", "Iron Man", "Ant-Man", "Hulk", "Hawkeye" };
    static string[] countriesArray = { "USA", "Russia", "Canada", "Germany", "Wakanda" };
    static string[,] whoGotHurtTable = { { "Both", "P2", "P1" }, { "P1", "Both", "P2" }, { "P2", "P1", "Neither" } };

    /* whoGotHurtTable
	 * 
	 * Kick counters Block
	 * Punch counters Kick
	 * Block counters Punch
	 * 
	 *[player1Action] == row index value
	 *[opponentAction] == column index value
	 *
	 *			 col0  col1  col2
	 *___________|Kick|Punch|Block 
	 *row0 Kick	 |Both| P1  | P2
	 *row1 Punch | P2 |Both | P1
	 *row2 Block | P1 | P2  | Neither
	 * 
	 * 
	 */







    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }


    void StartGame()
    {
        introduceGame();
        pickCountry();
        pickNumOfPlayers();
        pickFighter();
        fight();
    }





    private static void fight()
    {
        do
        {
            selectActions();
            checkActions();
            updateHealth();
            checkForKnockout();

            // Reset variables for next round
            opponentHurt = false;
            playerOneHurt = false;
            counterAttack = false;
            roundNumber++;

        } while (playerOneHealth > 0 && opponentHealth > 0);// Loops while both player's health above 0
    }





    // Takes formal parameters, but does not return a value
    private static void announce(string competitor1, string damageDescription, string competitor2)
    {
        for (int i = 0; i < 50; i++) System.out.println();
        printBorder();
        System.out.println("Announcer says, \"" + competitor1 + damageDescription + competitor2 + "\"");
    }





    private static void printBorder()
    {
        System.out.println("================================================================================\n");
    }





    private static void checkForKnockout()
    {
        // Check double knock out
        if (playerOneHealth <= 0 && opponentHealth <= 0)
        {
            System.out.println(playerOneName + " and " + opponentName + " both go down for the count!");

            // Prints one to ten because fighter is knocked out
            for (int i = 1; i <= 10; i++)
            {
                if (i < 6) System.out.println(i);
				else System.out.println(i + "!");
            }

            // Game Over
            System.out.println("\n*DING* *DING* *DING* The match is over in round number " + roundNumber + "!!\n" + playerOneName + " and " + opponentName + " knocked each other out at the same time.\nWhat a weird ending!!!");
        }

        // Check if Player One Lost
        else if (playerOneHealth <= 0)
        {
            System.out.println(playerOneName + " is down for the count!");

            // Prints one to ten because player one is knocked out
            for (int i = 1; i <= 10; i++)
            {
                if (i < 6) System.out.println(i);
				else System.out.println(i + "!");
            }

            // Game Over
            System.out.println("\n*DING* *DING* *DING* The match is over in round number " + roundNumber + "!!\n" + playerOneName + " was knocked out, and " + opponentName + " still had " + opponentHealth + " health left. \nBetter luck next time player one!!!");
        }

        // Check if Player Two Lost
        else if (opponentHealth <= 0)
        {
            System.out.println(opponentName + " is down for the count!");

            // Prints one to ten because fighter is knocked out
            for (int i = 1; i <= 10; i++) // CH5: For loop
            {
                if (i < 6) System.out.println(i);
				else System.out.println(i + "!");
            }

            // Game Over
            System.out.println("\n*DING* *DING* *DING* The match is over in round number " + roundNumber + "!! \n" + opponentName + " was knocked out, and " + playerOneName + " still had " + playerOneHealth + " health left.\nCONGRATULATIONS PLAYER ONE!!!");
        }
    }





    private static void checkActions()
    {
        System.out.println();

        // Gets string value in the 2D array
        whoGotHurt = whoGotHurtTable[player1Action, opponentAction];

        if (whoGotHurt.Equals("Both"))
        {
            announce(playerOneName, " exchanged attacks with ", opponentName);
            opponentHurt = true;
            playerOneHurt = true;

        }
        else if (whoGotHurt.Equals("P1"))
        {
            announce(opponentName, " counters the attack and crushes ", playerOneName);
            playerOneHurt = true;
            counterAttack = true;

        }
        else if (whoGotHurt.Equals("P2"))
        {
            announce(playerOneName, " parries the attack and deals big damage to ", opponentName);
            opponentHurt = true;
            counterAttack = true;

        }
        // Both blocked
        else { announce("", "Both fighters stand still, waiting for the other to make a move!", ""); }

        System.out.println();
    }





    private static void updateHealth()
    {
        if (counterAttack)
        {
            if (playerOneHurt) playerOneHealth -= opponentDamageRoll(DIFFICULTY) * DAMAGE_MULTIPLIER;
            if (opponentHurt) opponentHealth -= playerOneDamageRoll(DIFFICULTY) * DAMAGE_MULTIPLIER;
        }
        else
        {
            if (playerOneHurt) playerOneHealth -= opponentDamageRoll(DIFFICULTY);
            if (opponentHurt) opponentHealth -= playerOneDamageRoll(DIFFICULTY);
        }
    }

    // Opponent Damage Roll
    private static int opponentDamageRoll(string difficulty)
    {
        Random random = new System.Random();
        if (difficulty.Equals("EASY")) return random.Next(1, 15); // 1 - 15 damage
        else if (difficulty.Equals("MEDIUM")) return random.Next(5, 15);  // 5 - 15 damage
        else return random.Next(10, 15); // 10 - 15 damage
    }

    // Player1 Damage Roll
    private static int playerOneDamageRoll(String difficulty)
    {
        Random random = new System.Random();
        if (difficulty.Equals("EASY")) return random.Next(10, 15); // 10 - 15 damage
        else if (difficulty.Equals("MEDIUM")) return random.Next(5, 15); // 5 - 15 damage
        else return random.Next(1, 15); // 1 - 15 damage
    }





    // Calculate actions and damage 
    private static void selectActions()
    {
        // One Player
        if (!isMultiplayer)
        {
            Random random = new System.Random();
            printBorder();
            System.out.print("ROUND NUMBER " + roundNumber + "!\n" + playerOneName + " has " + playerOneHealth + " health left, and " + opponentName + " has " + opponentHealth + " health left.\nEnter 1 to kick, 2 to punch, 3 to block: ");

            // Player1 picks action
            player1input = input.nextInt();
            // Will be used as the index value in the two dimensional array
            player1Action = player1input - 1;

            // Opponent rolls action
            // Will be used as the second index value in the two dimensional array
            opponentAction = random.Next(0, 3);
        }


        // Two Player
        else
        {
            printBorder();
            System.out.print("Round Number " + roundNumber + "!\n" + playerOneName + " has " + playerOneHealth + " health left, and " + opponentName + " has " + opponentHealth + " health left.\n\n" + playerOneName + " enter 1 to kick, 2 to punch, 3 to block: ");

            // Player1 picks action
            player1input = input.nextInt();
            // Assigns index value to be used in 2D array
            player1Action = player1input - 1;

            // Prints lines to hide player one’s action
            for (int i = 0; i < 50; i++) System.out.println();

            // Player2 picks action
            System.out.print(opponentName + " enter 1 to kick, 2 to punch, 3 to block: ");
            player2input = input.nextInt();
            // Assigns index value to be used in 2D array
            opponentAction = player2input - 1;
        }
    }





    private static void pickFighter()
    {
        printBorder();
        System.out.println("Who would like to fight as?\n");
        Arrays.sort(playerOneArray);

        // Prints player one name choices
        for (string fighters : playerOneArray) System.out.println(fighters);
        do
        {
            System.out.print("\nPLAYER ONE - Select a fighter name from the list above: ");
            playerOneName = input.next();

            // Compares user's input name to each element in the array
            for (int i = 0; i < playerOneArray.length; i++)
            {
                if (playerOneArray[i].equals(playerOneName)) player1ValidName = true;
            }

        } while (!player1ValidName); // Loops while user player1 name does not match an array element

        System.out.println();

        //Player two selects fighter from different array
        if (isMultiplayer)
        {
            printBorder();
            System.out.println("Who would like to fight as?\n");
            Arrays.sort(playerTwoArray);

            // Prints player two name choices
            for (String fighters : playerTwoArray) System.out.println(fighters);
            do
            {
                System.out.print("\nPLAYER TWO - Select a fighter name from the list above: ");
                opponentName = input.next();
                for (int i = 0; i < playerTwoArray.Length; i++)
                {
                    if (playerTwoArray[i].Equals(opponentName)) player2ValidName = true;
                }

            } while (!player2ValidName); // Loops while player2 name does not match an array element
        }

        for (int i = 0; i < 50; i++) System.out.println();
    }





    // Select One or Two Players
    private static void pickNumOfPlayers()
    {
        printBorder();
        System.out.print("Enter 1 for one player or 2 for two player: ");
        numOfPlayers = input.nextInt();
        if (numOfPlayers == 2) isMultiplayer = true;
        System.out.println();
    }





    private static void pickCountry()
    {
        Arrays.sort(countriesArray);

        do
        {
            printBorder();
            System.out.println("Which country would you like to fight in?\n");

            // Prints out countries to fight in
            for (String country : countriesArray) System.out.println(country);

            // User enters country name and converts it to upper case
            System.out.print("\nEnter a country from the list above: ");
            country = input.next();
            countryUpperCase = country.toUpperCase(); // Returns a NEW string, doesn't change old string

            // Compares user input to countries and assigns opponentName
            switch (countryUpperCase)
            {
                case "USA":
                    opponentName = "Rocky";
                    break;
                case "RUSSIA":
                    opponentName = "Drago";
                    break;
                case "CANADA":
                    opponentName = "Horton";
                    break;
                case "GERMANY":
                    opponentName = "Heltzenburg";
                    break;
                case "WAKANDA":
                    opponentName = "Black Panther";
                    break;
                default: opponentName = "";
            }
        } while (opponentName.Equals("")); // Loops while a country from the list is not inputed
        System.out.println();
    }





    private static void introduceGame()
    {
        printBorder();
        System.out.println("Welcome to Kick Punch Block!\nThis game is inspired by Rock Paper Scissors.\nAdditional features include health points, fighter names, and rounds.\n");// CH1: print & comments
    }

}
