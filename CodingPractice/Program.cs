using System;

// 1.
{
    Notify notify = SayHello;
    notify += SayGoodbye;

    notify();

    void SayHello() => Console.WriteLine("안녕하세요!");
    void SayGoodbye() => Console.WriteLine("안녕히 가세요!");
}
Console.WriteLine();

// 2.
{
    Button button = new();
    button.Click += HandleClick;
    button.Click += HandleClickAgain;

    button.OnClick();

    void HandleClick() => Console.WriteLine("버튼이 클릭되었습니다.");
    void HandleClickAgain() => Console.WriteLine("클릭 처리 완료");
}
Console.WriteLine();

// 3.
{
    Player player = new();

    player.DamageTaken += player.HealthBar;
    player.DamageTaken += player.SoundManager;

    player.TakeDamage(30);
}
Console.WriteLine();

// 4.
{
    Timer timer = new Timer();
    timer.Tick += timer.Logger;

    Console.WriteLine("=== 구독 상태 ===");
    timer.Start();

    Console.WriteLine("\n=== 구독 해제 후 ===");
    timer.Tick -= timer.Logger;
    timer.Start();
}
Console.WriteLine();

// 5.
{
    Sensor sensor = new();
    sensor.Alert += msg => Console.WriteLine($"[경보] {msg}");
    sensor.Alert += msg => Console.WriteLine($"[로그] 2024-12-26 오후 5:30:00: {msg}");

    sensor.Detect("움직임 감지됨");
    sensor.Detect("온도 상승");
}
Console.WriteLine();

// 6.
{
    GameCharacter character = new GameCharacter();

    character.OnDeath += Death;
    character.OnDamaged += Damaged;
    character.OnAttack += Attack;

    character.Attack(50, "슬라임");
    character.Damaged(30);
    character.Damaged(80);

    void Death() => Console.WriteLine("캐릭터가 사망했습니다.");
    void Attack(int damage, string target) => Console.WriteLine($"{target}에게 {damage} 데미지!");
    void Damaged(int damage)
    {
        character.Health -= damage;
        Console.WriteLine($"남은 체력: {character.Health}");

        if (character.Health <= 0) character.Death();
    }
}
Console.WriteLine();

// 7.
{
    Main();

    static void Main()
    {
        Stock msStock = new Stock("MSFT", 100.00m);

        // 표준 패턴: sender와 EventArgs를 받음
        msStock.PriceChanged += (sender, e) =>
        {
            Stock stock = (Stock)sender;
            Console.WriteLine($"[{stock}]");
            Console.WriteLine($"  이전 가격: {e.OldPrice:C}");
            Console.WriteLine($"  현재 가격: {e.NewPrice:C}");
            Console.WriteLine($"  변동률: {e.ChangePercent:F2}%");
            Console.WriteLine();
        };

        msStock.Price = 110.00m;
        msStock.Price = 105.50m;
        msStock.Price = 120.00m;
    }
}
Console.WriteLine();

// 8.
{
    Main();

    static void Main()
    {
        Car car = new Car(50);
        Dashboard dashboard = new Dashboard();

        // 구독
        dashboard.Subscribe(car);

        // 운전 시뮬레이션
        for (int i = 0; i < 7; i++)
        {
            car.Drive();
            Console.WriteLine();
        }

        // 구독 해제
        dashboard.Unsubscribe(car);
    }
}
Console.WriteLine();

// 9.
/*{
    Main();

    static void Main()
    {
        SecurePublisher publisher = new SecurePublisher();

        publisher.MyEvent += Handler1;
        publisher.MyEvent += Handler2;

        Console.WriteLine("\n이벤트 발생:");
        publisher.RaiseEvent();

        Console.WriteLine();
        publisher.MyEvent -= Handler1;

        Console.WriteLine("\n이벤트 발생:");
        publisher.RaiseEvent();
    }

    static void Handler1(object sender, EventArgs e)
    {
        Console.WriteLine("Handler1 실행됨");
    }

    static void Handler2(object sender, EventArgs e)
    {
        Console.WriteLine("Handler2 실행됨");
    }
}*/
Console.WriteLine();

// 10.
{
    Main();

    static void Main()
    {
        Module1 m1 = new Module1();
        Module2 m2 = new Module2();

        GlobalNotifier.SendMessage("시스템 시작");
        Console.WriteLine();
        GlobalNotifier.SendMessage("데이터 로드 완료");
    }
}
Console.WriteLine();

// 10.
class GlobalNotifier
{
    public static event Action<string> OnGlobalMessage;

    public static void SendMessage(string message)
    {
        Console.WriteLine($"[Global] 메시지 전송: {message}");
        OnGlobalMessage?.Invoke(message);
    }
}

class Module1
{
    public Module1()
    {
        GlobalNotifier.OnGlobalMessage += HandleMessage;
    }

    private void HandleMessage(string message)
    {
        Console.WriteLine($"[Module1] 수신: {message}");
    }
}

class Module2
{
    public Module2()
    {
        GlobalNotifier.OnGlobalMessage += HandleMessage;
    }

    private void HandleMessage(string message)
    {
        Console.WriteLine($"[Module2] 수신: {message}");
    }
}

// 9.
/*class SecurePublisher
{
    private EventHandler _myEvent;
    private readonly object _lock = new object();

    public event EventHandler MyEvent
    {
        add
        {
            lock (_lock)
            {
                Console.WriteLine($"구독자 추가: {value.Method.Name}");
                _myEvent += value;
            }
        }
        remove
        {
            lock (_lock)
            {
                Console.WriteLine($"구독자 제거: {value.Method.Name}");
                _myEvent -= value;
            }
        }
    }

    public void RaiseEvent()
    {
        _myEvent?.Invoke(this, EventArgs.Empty);
    }
}*/

// 8.
class FuelEventArgs : EventArgs
{
    public int FuelLevel { get; }
    public string Warning { get; }

    public FuelEventArgs(int fuelLevel, string warning)
    {
        FuelLevel = fuelLevel;
        Warning = warning;
    }
}

// 게시자: 자동차
class Car
{
    private int _fuelLevel;

    public event EventHandler<FuelEventArgs> FuelLow;
    public event Action<int> FuelChanged;

    public Car(int initialFuel)
    {
        _fuelLevel = initialFuel;
    }

    public int FuelLevel => _fuelLevel;

    public void Drive()
    {
        if (_fuelLevel <= 0)
        {
            Console.WriteLine("연료 없음! 운전 불가");
            return;
        }

        _fuelLevel -= 10;
        Console.WriteLine($"운전 중... 연료: {_fuelLevel}%");

        FuelChanged?.Invoke(_fuelLevel);

        if (_fuelLevel <= 0)
        {
            OnFuelLow(new FuelEventArgs(_fuelLevel, "연료가 바닥났습니다!"));
        }
        else if (_fuelLevel <= 20)
        {
            OnFuelLow(new FuelEventArgs(_fuelLevel, "연료가 부족합니다"));
        }
    }

    protected virtual void OnFuelLow(FuelEventArgs e)
    {
        FuelLow?.Invoke(this, e);
    }
}

// 구독자: 대시보드
class Dashboard
{
    public void Subscribe(Car car)
    {
        car.FuelChanged += OnFuelChanged;
        car.FuelLow += OnFuelLow;
    }

    public void Unsubscribe(Car car)
    {
        car.FuelChanged -= OnFuelChanged;
        car.FuelLow -= OnFuelLow;
    }

    private void OnFuelChanged(int fuelLevel)
    {
        string gauge = new string('█', fuelLevel / 10);
        Console.WriteLine($"[대시보드] 연료 게이지: {gauge}");
    }

    private void OnFuelLow(object sender, FuelEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[경고!] {e.Warning} (잔량: {e.FuelLevel}%)");
        Console.ResetColor();
    }
}

// 7.
class PriceChangedEventArgs : EventArgs
{
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal ChangePercent { get; set; }

    public PriceChangedEventArgs(decimal oldP, decimal newP)
    {
        OldPrice = oldP;
        NewPrice = newP;

        if (oldP != 0)
        {
            ChangePercent = (newP - oldP) / oldP * 100;
        }
    }
}

class Stock
{
    private string _symbol;
    private decimal _price;

    public event EventHandler<PriceChangedEventArgs> PriceChanged;

    public Stock(string symbol, decimal price)
    {
        _symbol = symbol;
        _price = price;
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value)
            {
                return;
            }

            decimal oldPrice = _price;
            _price = value;

            // 이벤트 발생
            OnPriceChanged(new PriceChangedEventArgs(oldPrice, _price));
        }
    }

    protected virtual void OnPriceChanged(PriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }

    public override string ToString()
    {
        return $"{_symbol}: {_price:C}";
    }
}

// 6.
class GameCharacter
{
    public int Health = 100;

    public event Action OnDeath;
    public event Action<int> OnDamaged;
    public event Action<int, string> OnAttack;

    public void Death()
    {
        OnDeath?.Invoke();
    }

    public void Damaged(int damage)
    {
        OnDamaged?.Invoke(damage);
    }

    public void Attack(int damage, string target)
    {
        OnAttack?.Invoke(damage, target);
    }
}

// 5.
class Sensor
{
    public event Action<string> Alert;

    public void Detect(string message)
    {
        Console.WriteLine($"감지: {message}");
        Alert?.Invoke(message); 
    }
}

// 4.
class Timer
{
    public event Action Tick;
    int sec = 0;

    public void Start()
    {
        Console.WriteLine($"타이머: {++sec}초");
        Tick?.Invoke();

        Console.WriteLine($"타이머: {++sec}초");
        Tick?.Invoke();

        Console.WriteLine($"타이머: {++sec}초");
        Tick?.Invoke();
    }

    public void Logger()
    {
        Console.WriteLine("[Logger] 틱 기록됨");
    }
}

// 3.
class Player
{
    int Health = 100;

    public event Action<int> DamageTaken;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Console.WriteLine($"플레이어 체력: {Health}");
        DamageTaken?.Invoke(damage);
    }

    public void HealthBar(int damage)
    {
        Console.WriteLine($"[UI] 체력바 업데이트: {Health}%");
    }

    public void SoundManager(int damage)
    {
        Console.WriteLine("[Sound] 피격 효과음 재생");
    }
}

// 2.
delegate void EventHandler();

class Button
{
    public event EventHandler Click;

    public void OnClick()
    {
        Click?.Invoke();
    }
}

// 1.
delegate void Notify();