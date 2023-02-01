using System;
using UnityEngine;

public enum GameState
{
    Intro,
    Game,
    DownTime,
    Lost,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _mainGameDuration = 60;
    [SerializeField] private float _downTimeDuration = 30;

    private Timer _timer;
    private FarmManager _farmM;
    private FarmInfectionManager _infectionM;
    private GameState _state;

    public static event Action OnGameStart;
    public static event Action OnDownTime;

    private void Awake()
    {
        _timer = new Timer();
        _farmM = GetComponent<FarmManager>();
        _infectionM = GetComponent<FarmInfectionManager>();

        _timer.OnTimerFinish += OnTimerFinish;

        UpdateGameState(GameState.Intro);
    }

    private void OnDestroy()
    {
        _timer.OnTimerFinish -= OnTimerFinish;
    }

    private void Update()
    {
        // timer to control main game loop
        _timer.TimerHandler(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateGameState(GameState.Game);
        }
    }

    public void UpdateGameState(GameState state)
    {
        _state = state;

        if (state == GameState.Intro)
        {

        }

        if (state == GameState.Game)
        {
            OnGameStart?.Invoke();

            // see whats in the farm
            _farmM.GetPlantedCropsAmount();
            // init infection system
            _infectionM.InitInfection(_farmM.GetActiveCrops().Count);

            // start the timer
            _timer.InitTimer(true, _mainGameDuration);
        }

        if (state == GameState.DownTime)
        {
            OnDownTime?.Invoke();

            _infectionM.StopInfection();
            _timer.InitTimer(true, _downTimeDuration);
        }

        if (state == GameState.Lost)
        {

        }
    }

    private void OnTimerFinish()
    {
        if (_state == GameState.Game)
        {
            UpdateGameState(GameState.DownTime);
        }
        else if (_state == GameState.DownTime)
        {
            UpdateGameState(GameState.Game);
        }
    }
}
