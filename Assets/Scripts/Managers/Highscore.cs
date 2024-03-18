using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Highscore : MonoBehaviour
{
    List<Scorer> _scoreList;

    private void Awake()
    {
        GetScoreList();
    }

    private void GetScoreList()
    {
        _scoreList = new List<Scorer>();
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey("Score" + i))
            {
                _scoreList.Add(new Scorer(PlayerPrefs.GetString("ScoreName" + i), PlayerPrefs.GetInt("Score" + i)));
            }
        }
    }
    private void StoreScoreList()
    {
        for (int i = 0; i < _scoreList.Count; i++)
        {
            PlayerPrefs.SetString("ScoreName" + i, _scoreList[i].name);
            PlayerPrefs.SetInt("Score" + i, _scoreList[i].score);
        }
    }
    private bool CheckNewHighScore(Scorer currentPlayer)
    {
        for (int i = 0; i < _scoreList.Count; i++)
        {
            if (currentPlayer.CompareTo(_scoreList[i]) > 0)
            {
                _scoreList.Insert(i, currentPlayer);
                return true;
            }
        }
        return false;
    }
}
public class Scorer : IComparable
{
    public string name;
    public int score;

    public Scorer(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public int CompareTo(object  obj)
    {
        var a = this;
        var b = obj as Scorer;

        if (a.score < b.score)
            return -1;

        if (a.score > b.score)
            return 1;

        return 0;
    }
}
