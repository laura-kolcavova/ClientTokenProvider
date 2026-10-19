using System.Diagnostics;

namespace KittyToken.Maui.Controls;

public partial class Spinner :
    ContentView
{
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
       nameof(Size),
       typeof(int),
       typeof(Spinner),
       defaultValue: 40,
       propertyChanged: OnSizePropertyChanged);

    public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(
        nameof(Thickness),
        typeof(int),
        typeof(Spinner),
        defaultValue: 4,
        propertyChanged: OnDrawablePropertyChanged);

    public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(
        nameof(PrimaryColor),
        typeof(Color),
        typeof(Spinner),
        defaultValue: Colors.Black,
        propertyChanged: OnDrawablePropertyChanged);

    public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(
        nameof(SecondaryColor),
        typeof(Color),
        typeof(Spinner),
        defaultValue: Colors.LightGray,
        propertyChanged: OnDrawablePropertyChanged);

    public static readonly BindableProperty SpeedProperty = BindableProperty.Create(
        nameof(Speed),
        typeof(int),
        typeof(Spinner),
        defaultValue: 360,
        propertyChanged: OnSpeedPropertyChanged);

    public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(
        nameof(IsRunning),
        typeof(bool),
        typeof(Spinner),
        propertyChanged: OnIsRunningPropertyChanged);

    private readonly IDispatcherTimer _timer;

    private readonly Stopwatch _stopwatch = new();

    public int Size
    {
        get => (int)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public int Thickness
    {
        get => (int)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    public Color PrimaryColor
    {
        get => (Color)GetValue(PrimaryColorProperty);
        set => SetValue(PrimaryColorProperty, value);
    }

    public Color SecondaryColor
    {
        get => (Color)GetValue(SecondaryColorProperty);
        set => SetValue(SecondaryColorProperty, value);
    }

    /// <summary>
    ///  Gets or sets a speed of degrees per second.
    ///  Speed = 90: one full turn every 4 seconds.
    ///  Speed = 360: one turn per second.
    ///  Speed = 720: two turns per second.
    /// </summary>
    public int Speed
    {
        get => (int)GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }

    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }


    public Spinner()
    {
        InitializeComponent();

        UpdateBounds();

        _timer = Dispatcher.CreateTimer();
        _timer.IsRepeating = true;
        _timer.Interval = TimeSpan.FromMilliseconds(16);
        _timer.Tick += OnTick;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void Redraw()
    {
        GraphicsView.Invalidate();
    }

    private void UpdateBounds()
    {
        HeightRequest = Size;
        WidthRequest = Size;
    }

    private void Start()
    {
        if (!_timer.IsRunning)
        {
            SpinnerDrawableLocal.EndAngle = SpinnerDrawableLocal.StartAngle;

            _stopwatch.Restart();
            _timer.Start();
        }
    }

    private void Stop()
    {
        if (_timer.IsRunning)
        {
            _timer.Stop();
        }
    }

    private void StartOrStop()
    {
        if (IsRunning && Speed > 0)
        {
            Start();
        }
        else
        {
            Stop();
        }
    }

    private void OnTick(object? sender, EventArgs e)
    {
        var deltaTime = (float)_stopwatch.Elapsed.TotalSeconds;

        _stopwatch.Restart();

        var fullCircleEndAngle = -360 + SpinnerDrawableLocal.StartAngle;

        var speed = Speed * deltaTime;

        var newEndAngle = SpinnerDrawableLocal.EndAngle == fullCircleEndAngle
            ? SpinnerDrawableLocal.StartAngle
            : Math.Max(SpinnerDrawableLocal.EndAngle - speed, fullCircleEndAngle);

        SpinnerDrawableLocal.EndAngle = newEndAngle;

        Redraw();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        StartOrStop();
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        Stop();
    }

    private static void OnSizePropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (Spinner)bindable;

        self.UpdateBounds();
        self.Redraw();
    }

    private static void OnDrawablePropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (Spinner)bindable;

        self.Redraw();
    }

    private static void OnSpeedPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (Spinner)bindable;

        self.StartOrStop();
    }

    private static void OnIsRunningPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (Spinner)bindable;

        self.StartOrStop();
    }
}
