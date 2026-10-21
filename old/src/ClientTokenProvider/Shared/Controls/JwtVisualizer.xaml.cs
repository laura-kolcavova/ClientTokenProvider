using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.Controls;

public partial class JwtVisualizer : ContentView
{
    public static readonly BindableProperty TokenProperty = BindableProperty.Create(
        nameof(Token),
        typeof(string),
        typeof(JwtVisualizer));

    public static readonly BindableProperty DecodedTokenProperty = BindableProperty.Create(
        nameof(DecodedToken),
        typeof(DecodedJwt),
        typeof(JwtVisualizer));

    public static readonly BindableProperty VisualizationModeProperty = BindableProperty.Create(
       nameof(VisualizationMode),
       typeof(JwtVisualizationMode),
       typeof(JwtVisualizer));

    public static readonly BindableProperty HandleVisualizationModeChangedCommandProperty = BindableProperty.Create(
       nameof(HandleVisualizationModeChangedCommand),
       typeof(IRelayCommand<JwtVisualizationMode>),
       typeof(JwtVisualizer));

    public string Token
    {
        get => (string)GetValue(TokenProperty);
        set => SetValue(TokenProperty, value);
    }

    public DecodedJwt DecodedToken
    {
        get => (DecodedJwt)GetValue(DecodedTokenProperty);
        set => SetValue(DecodedTokenProperty, value);
    }

    public JwtVisualizationMode VisualizationMode
    {
        get => (JwtVisualizationMode)GetValue(VisualizationModeProperty);
        set => SetValue(VisualizationModeProperty, value);
    }

    public IReadOnlyCollection<JwtVisualizationMode> VisualizationModes { get; } = Enum
        .GetValues(typeof(JwtVisualizationMode))
        .Cast<JwtVisualizationMode>()
        .ToList();

    // Binding to ConfigurationDetail.AccessTokenVisualizationMode actually does not work in two way mode
    public IRelayCommand<JwtVisualizationMode> HandleVisualizationModeChangedCommand
    {
        get => (IRelayCommand<JwtVisualizationMode>)GetValue(HandleVisualizationModeChangedCommandProperty);
        set => SetValue(HandleVisualizationModeChangedCommandProperty, value);
    }

    public JwtVisualizer()
    {
        InitializeComponent();
    }

    private void VisualizationModePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var newVisualizationMode = VisualizationModes.ElementAt(selectedIndex);

            // Setting VisualizationMode will broke UI when switching Configurations so we have to use command.
            HandleVisualizationModeChangedCommand?.Execute(newVisualizationMode);
        }
    }
}