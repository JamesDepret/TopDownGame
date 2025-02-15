namespace RTS;

public partial class UICardManager : GridContainer
{
    [Export] public int TotalCards { get; set; } = 10;

    public override void _Ready()
    {
        SetupCards();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("move_up"))
        {
            TotalCards++;
            SetupCards();
        }

        if (Input.IsActionJustPressed("move_down"))
        {
            TotalCards--;
            SetupCards();
        }
    }

    void SetupCards()
    {
        this.QueueFreeChildren();

        var cardPrefab = GD.Load<PackedScene>("res://Scenes/Prefabs/UI/card.tscn");

        for (int i = 0; i < TotalCards; i++)
        {
            var card = cardPrefab.Instantiate<UICard>();
            card.PreInit(TotalCards, Columns);
            AddChild(card);
        }
    }
}
