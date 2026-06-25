using System;
using System.IO;
using System.Text;

class Hangman
{
    static string[] HangmanStages = [
    
        // 0 mistakes
        @"
  +---+
  |   |
      |
      |
      |
      |
=========",
        // 1 mistake
        @"
  +---+
  |   |
  O   |
      |
      |
      |
=========",
        // 2 mistakes
        @"
  +---+
  |   |
  O   |
  |   |
      |
      |
=========",
        // 3 mistakes
        @"
  +---+
  |   |
  O   |
 /|   |
      |
      |
=========",
        // 4 mistakes
        @"
  +---+
  |   |
  O   |
 /|\  |
      |
      |
=========",
        // 5 mistakes
        @"
  +---+
  |   |
  O   |
 /|\  |
 /    |
      |
=========",
        // 6 mistakes
        @"
  +---+
  |   |
  O   |
 /|\  |
 / \  |
      |
========="
    ];

    const int MaxMistakes = 6;
    const int MaxWords    = 500;



    static string[] LoadWords(string path)
    {
        if (!File.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] Word file '{path}' not found!");
            Console.ResetColor();
            Environment.Exit(1);
        }

        string[] lines  = File.ReadAllLines(path, Encoding.UTF8);
        string[] buffer = new string[MaxWords];
        int count = 0;

        for (int i = 0; i < lines.Length && count < MaxWords; i++)
        {
            string w = lines[i].Trim().ToUpper();
            if (w.Length > 0)
            {
                buffer[count] = w;
                count++;
            }
        }

        if (count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] Word file contains no entries!");
            Console.ResetColor();
            Environment.Exit(1);
        }

        string[] result = new string[count];
        Array.Copy(buffer, result, count);
        return result;
    }

    // removes words from pool

    static void RemoveWord(string[] available, ref int availableCount, int index)
    {
        available[index] = available[availableCount - 1];
        availableCount--;
    }


    static bool IsGuessed(char c, char[] list, int count)
    {
        for (int i = 0; i < count; i++)
            if (list[i] == c) return true;
        return false;
    }

    static string LettersAsText(char[] list, int count)
    {
        if (count == 0) return "-";

        // Bubble sort
        char[] copy = new char[count];
        Array.Copy(list, copy, count);
        for (int i = 0; i < count - 1; i++)
            for (int j = i + 1; j < count; j++)
                if (copy[j] < copy[i])
                {
                    char tmp = copy[i];
                    copy[i]  = copy[j];
                    copy[j]  = tmp;
                }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            if (i > 0) sb.Append("  ");
            sb.Append(copy[i]);
        }
        return sb.ToString();
    }

    // build display

    static string BuildDisplay(string word, char[] correct, int correctCount)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in word)
        {
            if (c == ' ')
            {
                sb.Append("  ");
            }
            else if (IsGuessed(c, correct, correctCount))
            {
                sb.Append(c);
                sb.Append(' ');
            }
            else
            {
                sb.Append("_ ");
            }
        }
        return sb.ToString().TrimEnd();
    }

    // Draw Board

    static void DrawBoard(
        string word,
        char[] correct,  int correctCount,
        char[] wrong,    int wrongCount,
        int    mistakes,
        int    round)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine($"║       H A N G M A N   –  Round {round,2}    ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();

        Console.ForegroundColor = mistakes == MaxMistakes ? ConsoleColor.Red : ConsoleColor.Yellow;
        Console.WriteLine(HangmanStages[mistakes]);
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("  Word:    ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(BuildDisplay(word, correct, correctCount));
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = mistakes >= 4 ? ConsoleColor.Red : ConsoleColor.DarkYellow;
        Console.WriteLine($"  Mistakes: {mistakes} / {MaxMistakes}");
        Console.ResetColor();

        Console.WriteLine();
        Console.Write("  Wrong:   ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(LettersAsText(wrong, wrongCount));
        Console.ResetColor();

        Console.Write("  Correct: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(LettersAsText(correct, correctCount));
        Console.ResetColor();

        Console.WriteLine();
    }



    static bool IsWordComplete(string word, char[] correct, int correctCount)
    {
        foreach (char c in word)
            if (c != ' ' && !IsGuessed(c, correct, correctCount))
                return false;
        return true;
    }

    // loop

    static bool PlayRound(string word, int round)
    {
        char[] correct      = new char[26];
        char[] wrong        = new char[26];
        int    correctCount = 0;
        int    wrongCount   = 0;
        int    mistakes     = 0;

        while (true)
        {
            DrawBoard(word, correct, correctCount, wrong, wrongCount, mistakes, round);

            if (IsWordComplete(word, correct, correctCount))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  * Congratulations! You guessed the word! *");
                Console.ResetColor();
                Console.WriteLine($"  The word was: {word}");
                Console.WriteLine();
                return true;
            }

            if (mistakes >= MaxMistakes)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  X  Game over! The hangman has been drawn.");
                Console.ResetColor();
                Console.WriteLine($"  The word was: {word}");
                Console.WriteLine();
                return false;
            }

            Console.Write("  Enter a letter: ");
            string input = Console.ReadLine();

            if (input == null || input.Trim().Length == 0) continue;

            char letter = char.ToUpper(input.Trim()[0]);

            if (!char.IsLetter(letter))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  Please enter letters only. Press Enter...");
                Console.ResetColor();
                Console.ReadLine();
                continue;
            }

            if (IsGuessed(letter, correct, correctCount) ||
                IsGuessed(letter, wrong,   wrongCount))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  '{letter}' was already tried. Press Enter...");
                Console.ResetColor();
                Console.ReadLine();
                continue;
            }

            if (word.IndexOf(letter) >= 0)
            {
                correct[correctCount] = letter;
                correctCount++;
            }
            else
            {
                wrong[wrongCount] = letter;
                wrongCount++;
                mistakes++;
            }
        }
    }

    // Main

    static void Main(string[] args)
    {
        int round = 1;
        int wins = 0;
        string wordFilePath = "words.txt";
        string[] allWords = File.ReadAllLines(wordFilePath);
        int totalCount = allWords.Length;

        string[] available = new string[totalCount];
        int availableCount = totalCount;
        Array.Copy(allWords, available, totalCount);

 

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  ╔══════════════════════════════════╗
  ║   Welcome to  H A N G M A N      ║
  ╚══════════════════════════════════╝
");
        Console.ResetColor();
        Console.WriteLine("  Guess the hidden word before the hangman");
        Console.WriteLine($"  is fully drawn ({MaxMistakes} wrong guesses allowed).");
        Console.WriteLine();
        Console.Write("  Press Enter to start...");
        Console.ReadLine();

        while (true)
        {
            if (availableCount == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  All words have been played! Restarting the list...");
                Console.ResetColor();
                Console.WriteLine();
                Array.Copy(allWords, available, totalCount);
                availableCount = totalCount;
                Console.Write("  Press Enter...");
                Console.ReadLine();
            }

            int index = Random.Shared.Next(availableCount);
            string word = available[index].ToUpper();
            RemoveWord(available, ref availableCount, index);

            if (PlayRound(word, round)) wins++;
            round++;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  Play another round? (y/n): ");
            Console.ResetColor();
            string answer = Console.ReadLine();

            if (answer == null) break;
            answer = answer.Trim().ToLower();

            if (answer != "y" && answer != "yes")
                break;
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  ╔══════════════════════════════════╗
  ║   Thanks for playing! Goodbye!   ║
  ╚══════════════════════════════════╝
");
        Console.ResetColor();
    }
}
