using System;

ScoreSystem score = new ScoreSystem();
AchievementSystem achieve = new AchievementSystem();
SoundSystem sound = new SoundSystem();

EventManager.OnGameEvent += sound.Print;
EventManager.OnGameEvent += score.Print;
EventManager.OnGameEvent += achieve.Print;

EventManager.TriggerEvent("ScoreChanged", 100);
EventManager.TriggerEvent("Achievement", "첫 번째 적 처치");
EventManager.TriggerEvent("GameOver", null);

class GameEventArgs : EventArgs
{
    public string EventName { get; set; }
    public object Data { get; set; }

    public GameEventArgs(string eventName, object data)
    {
        EventName = eventName;
        Data = data;
    }
}

static class EventManager
{
    public static event EventHandler<GameEventArgs> OnGameEvent;

    public static void TriggerEvent(string eventName, object data = null)
    {
        OnGameEvent?.Invoke(null, new GameEventArgs(eventName, data));
    }
}

class ScoreSystem   // 구독자
{
    public void Print(object sender, GameEventArgs data)
    {
        if (data.EventName == "ScoreChanged")
            Console.WriteLine($"점수 변경: {data.Data}점");
    }
}

class AchievementSystem // 구독자
{
    public void Print(object sender, GameEventArgs data)
    {
        if (data.EventName == "Achievement")
            Console.WriteLine($"업적 달성: {data.Data}");
    }
}

class SoundSystem   // 구독자
{
    public void Print(object sender, GameEventArgs data) => Console.WriteLine($"[Sound] 이벤트: {data.EventName}");
}