using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class score : MonoBehaviour {

    public int scoreNumber =0;
    public Text scoreText, highscoreText;

    void Start()
    {
        scoreText = GetComponent<Text>();

        int highscore = 0;
        using (StreamReader sr = new StreamReader("scores.txt"))
        {
            while (!sr.EndOfStream)
            {
                int newScore = int.Parse(sr.ReadLine());
                if (newScore > highscore)
                    highscore = newScore;
            }
        }
        highscoreText.text = "Highscore: " + highscore.ToString("n0");
    }

    public void enemyKilled()
    {
        int combo = GameObject.FindGameObjectWithTag("Player").GetComponent<movement>().combo;

        scoreNumber += 25 * combo;
        scoreText.text = "Score: " + scoreNumber.ToString("n0");
    }

    public void saveScore()
   {
        List<string> text = new List<string>();
       using (StreamReader sr = new StreamReader("scores.txt"))
       {
           while (!sr.EndOfStream)
           {
               text.Add(sr.ReadLine());
           }
       }

        using (StreamWriter sw = new StreamWriter("scores.txt"))
        {
            for (int i = 0; i < text.Count; i++)
            {
                sw.WriteLine(text[i]);
            }
            sw.WriteLine(scoreNumber);
        }
    }
}
