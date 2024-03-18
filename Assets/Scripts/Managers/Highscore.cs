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
        GetScores();
    }

    private void GetScores()
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
    public bool CheckNewHighScore(int score)
    {
        if (_scoreList.Count < 3)
            return true;

        for (int i = 0; i < _scoreList.Count; i++)
        {
            if (score > _scoreList[i].score)
            {
                return true;
            }
        }
        return false;
    }

    public void AddNewScore(string name, int score)
    {
        _scoreList.Add(new Scorer(name, score));
        _scoreList.Sort();
        _scoreList.Reverse();

        if (_scoreList.Count > 3)
        {
            _scoreList.RemoveAt(3);
        }

        StoreScoreList();
    }

    public List<Scorer> GetScoreList()
    {
        return _scoreList;
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
