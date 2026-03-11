using System;

Main();

static void Main()
{
    ChatRoom room = new ChatRoom();
    ChatLogger logger = new ChatLogger();
    NotificationService notification = new NotificationService();

    room.MessageReceived += logger.Print;
    room.MessageReceived += notification.Notice;

    room.SendMessage("철수", "안녕하세요");
    room.SendMessage("영희", "긴급 회의가 있습니다");
    room.SendMessage("민수", "점심 뭐 먹을까요?");
}

class ChatRoom
{
    public event Action<string, string> MessageReceived;

    public void SendMessage(string sender, string message)
    {
        MessageReceived?.Invoke(sender, message);
    }
}

class ChatLogger
{
    public void Print(string sender, string message)
    {
        Console.WriteLine($"[로그] {sender}: {message}");
    }
}

class NotificationService
{
    public void Notice(string sender, string message)
    {
        if (message.Contains("긴급"))
        {
            Console.WriteLine("[알림] 긴급 메시지 수신!");
        }
    }
}