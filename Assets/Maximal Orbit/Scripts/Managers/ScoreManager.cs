using UnityEngine;
using UnityEngine.Events;
using GameUp;

public class ScoreManager : MonoBehaviour
{
    public string HighScoreKey = "Best Score";

    [Header("Leaderboard Information")]
    public string GameKey;
    public string[] LeaderboardKeys;
    public LeaderboardEvent OnLeaderboardUpdate;
    public UnityEvent OnConnectionFailure;
    public UnityEvent OnConnectionSuccess;

    public int HighScore
    {
        get
        {
            return _highScore;
        }
    }

    public int Score
    {
        get
        {
            return _score;
        }
    }

    public IntEvent ScoreChanged;

    private string _id;
    private int _highScore;
    private int _score;

    #region Heroic Labs Data
    private SessionClient _session;
    private Game _game;
    #endregion

    private const string IDKEY = "ID5";
    void Start()
    {
        if (PlayerPrefs.HasKey(IDKEY))
        {
            _id = PlayerPrefs.GetString(IDKEY);
        }
        else
        {
            _id = GetUniqueID();

            PlayerPrefs.SetString(IDKEY, _id);
            PlayerPrefs.Save();
        }
        
        ZPlayerPrefs.Initialize(_id, _id);

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat(this, "Initialized ZPlayerPrefs with device ID {0}", _id);
#endif
        _highScore = ZPlayerPrefs.GetInt(HighScoreKey);

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat(this, "Loaded high score {0}", HighScore);
#endif
        
        Client.ApiKey = this.GameKey;
        Client.Ping(this.pingSuccess, this.failure);
    }

    public void PlanetRevolved(Planet planet)
    {
        if (planet.Revolutions == 1)
        {
            this._score++;
            this.ScoreChanged.Invoke(this._score);
        }
    }

    public void OnGameStart()
    {
        this._score = 0;
        this.ScoreChanged.Invoke(this._score);
    }

    public void OnGameEnd()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat(this, "Game over! You scored {0}", Score);
#endif

        if (this.Score >= this.HighScore)
        {
            ZPlayerPrefs.SetInt(this.HighScoreKey, Score);
            this._highScore = Score;

            this.OnLeaderboardUpdate.Invoke("", Score);
            ZPlayerPrefs.Save();

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogFormat(this, "Saved high score of {0}", Score);
#endif
        }

        if (_session != null)
        // If we were able to get online...
        {
            foreach (string i in LeaderboardKeys)
            {
                this._session.UpdateLeaderboard(i, Score, updateLeaderboardSuccess, failure);
                Client.Leaderboard(i, 10, 0, false, leaderboardSuccess, failure);
            }
        }
        else
        {
            Client.Ping(this.pingSuccess, this.failure);
        }
    }

    void OnApplicationQuit()
    {
        if (_session != null)
        // If we managed to log in successfully in the first place...
        {
            Client.unlinkAnonymous(_session, this.GameKey, null, this.failure);
        }
    }

    #region Callbacks
    private void pingSuccess(PingInfo ping)
    {
        Debug.LogFormat(this, "Server ping successful (timestamp {0})", ping.Time);
        OnConnectionSuccess.Invoke();
        Client.Game(this.gameSuccess, this.failure);
    }

    private void failure(int status, string reason)
    {
        Debug.LogErrorFormat(this, "Error {0}: {1}", status, reason);
        _game = null;
        _session = null;
        OnConnectionFailure.Invoke();
    }

    private void gameSuccess(Game game)
    {
        Debug.LogFormat(this, "{0}: {1} (Created {2}, updated {3})", game.Name, game.Description, game.CreatedAt, game.UpdatedAt);
        this._game = game;

        Client.LoginAnonymous(this._id, this.loginSuccess, this.failure);
        foreach (string i in LeaderboardKeys)
        {
            Client.Leaderboard(i, 10, 0, false, leaderboardSuccess, failure);
        }
    }

    private void loginSuccess(SessionClient session)
    {
        Debug.Log("Successfully logged in", this);
        this._session = session;
    }

    private void leaderboardSuccess(Leaderboard leaderboard)
    {
        Debug.LogFormat(this, "Found leaderboard {0}", leaderboard.Name);

        long score = leaderboard.Entries.Length > 0 ? leaderboard.Entries[0].Score : 0;
        this.OnLeaderboardUpdate.Invoke(leaderboard.LeaderboardId, score);
    }

    private void updateLeaderboardSuccess(Rank rank)
    {
        Debug.LogFormat(this, "Leaderboard updated (Place #{0})", rank.Ranking);
    }
    #endregion

    // Needed for WebGL (but try again when Unity 5.4.0 comes out)
    // http://forum.unity3d.com/threads/cant-get-systeminfo-deviceuniqueidentifier-to-work-on-webgl-builds.291303/
    private static string GetUniqueID()
    {
#if UNITY_WEBGL
        var random = new System.Random();
        byte[] buffer = new byte[32];

        random.NextBytes(buffer);

        string uniqueID = System.Convert.ToBase64String(buffer);

        Debug.Log("Generated Unique ID: " + uniqueID);

        return uniqueID;
#else
        return SystemInfo.deviceUniqueIdentifier;
#endif
    }
}