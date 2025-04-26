using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

public class EndPanelManager : UIPanelManager
{
    [SerializeField] private string _savePath;
    [SerializeField] private List<string> _characters;
    [SerializeField] private ScoreUI _scorePrefab;
    [SerializeField] private Transform _scoreContainer;
    [SerializeField] private List<TextMeshProUGUI> _letters;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private UIPanelController _selectedPanel;
    [SerializeField] private Animator _animator;
    [SerializeField] private ScriptableKeyBind _upBind;
    [SerializeField] private ScriptableKeyBind _downBind;
    [SerializeField] private ScriptableKeyBind _validateBind;
    private string _internalSavePath;
    private int _currentScoreValue;
    private List<PlayerScore> _scores = new List<PlayerScore>();
    private int _currentCharacterIndex;
    private int _currentLetterIndex;
    private PlayerScore _currentScore;

    private TextMeshProUGUI _currentLetter => _letters[_currentLetterIndex];
    private string _currentCharacter => _characters[_currentCharacterIndex];

    private void Awake()
    {
        Subscribe();
        _internalSavePath = Application.persistentDataPath;
        Load(_savePath);
        RefreshScore();
    }

    #region NAME REGISTER

    private void Subscribe()
    {
        Events._scoreLoaded += SetCurrentScore;
        _upBind._onStart += NextCharacter;
        _downBind._onStart += PreviousCharacter;
        _validateBind._onStart += ValidateLetter;
    }
    private void Unsubscribe()
    {
        Events._scoreLoaded -= SetCurrentScore;
        _upBind._onStart -= NextCharacter;
        _downBind._onStart -= PreviousCharacter;
        _validateBind._onStart -= ValidateLetter;
    }

    private void SetCurrentScore(int score)
    {
        _currentScoreValue = score;
        _score.text = _currentScoreValue.ToString();
    }

    private void PreviousCharacter()
    {
        _currentCharacterIndex = (_currentCharacterIndex + 1) % _characters.Count;
        _currentLetter.text = _currentCharacter;
    }

    private void NextCharacter()
    {
        _currentCharacterIndex = (_currentCharacterIndex - 1 + _characters.Count) % _characters.Count;
        _currentLetter.text = _currentCharacter;
    }

    private void ValidateLetter()
    {
        _currentLetterIndex++;
        _currentCharacterIndex = 0;
        _animator.SetInteger("CurrentLetter", _currentLetterIndex);
        if (_currentLetterIndex < _letters.Count)
            return;
        _animator.SetTrigger("Validate");
        SavePlayerScoresToJson(_savePath);
        _selectedPanel.Show();
        Unsubscribe();
    }

    #endregion

    #region SAVE

    private PlayerScore GetScore()
    {
        string name = "";
        for (int i = 0; i < _letters.Count; i++)
            name += _letters[i].text;
        return new PlayerScore(name, _currentScoreValue);
    }

    public void Load(string filePath)
    {
        filePath = Path.Combine(_internalSavePath, filePath);
        try
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"File does not exist: {filePath}");
                return;
            }

            // Read the JSON string from the file
            string json = File.ReadAllText(filePath);
            _scores = JsonUtility.FromJson<Wrapper<PlayerScore>>(json).target;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load player scores: {e.Message}");
        }
    }

    public void SavePlayerScoresToJson(string filePath)
    {
        filePath = Path.Combine(_internalSavePath, filePath);
        _currentScore = GetScore();
        _scores.Add(_currentScore);
        try
        {
            // Serialize the list to JSON
            string json = JsonUtility.ToJson(new Wrapper<PlayerScore>(_scores), true);

            // Ensure the directory exists
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Write the JSON string to a file
            File.WriteAllText(filePath, json);
            RefreshScore();
            Debug.Log($"Saved player scores to {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save player scores: {e.Message}");
        }
    }

    #endregion

    private void RefreshScore()
    {
        Clear();
        _scores.OrderBy(x => x._score);
        _scores.Reverse();
        int size = Mathf.Min(4, _scores.Count);
        for (int i = 0; i < size; i++)
        {
            PlayerScore score = _scores[i];
            ScoreUI scoreUI = Instantiate(_scorePrefab, _scoreContainer);
            scoreUI.Populate(score, _currentScore == score);
        }
    }

    private void Clear()
    {
        foreach (Transform T in _scoreContainer)
            Destroy(T.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
