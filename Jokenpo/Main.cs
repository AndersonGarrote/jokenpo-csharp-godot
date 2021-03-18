using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Main : Control
{
    const int NumPlayers = 2;
    private readonly Dictionary<string, int> NameToOptions = new Dictionary<string, int> {
        {"Stone",   1},
        {"Paper",   2},
        {"Scissor", 3},
    };

    [Export]
    private NodePath Player1 { get; set; }
    private Node Player1Node { get => this.GetNode(Player1); }

    [Export]
    private NodePath Player2 { get; set; }
    private Node Player2Node { get => this.GetNode(Player2); }


    [Export]
    private NodePath Result { get; set; }
    private Node ResultNode { get => this.GetNode(Result); }

    public int Player1Option { get; set; }
    public int Player2Option { get; set; }

    private int _playersWaiting;

    private void _OnPlayer1ButtonPressed()
    {
        var children = Player1Node.GetChild(0).GetChildren().Cast<Button>();
        // GD.Print(children.First<Button>( b => b.Pressed ).Name);
        string name = GetPressedButton(children).Name;
        int newOption = NewOptionFromButtonName(name);
        this.Player1Option = newOption;

        _SetButtonsDisabled(Player1Node);

        _WaitAllPlayers();
    }

    private int NewOptionFromButtonName(string name) => NameToOptions[name.Substring(0, name.LastIndexOf("Button"))];

    private void _OnPlayer2ButtonPressed()
    {
        var children = Player2Node.GetChild(0).GetChildren().Cast<Button>();
        // GD.Print(children.First<Button>(b => b.Pressed).Name);
        string name = GetPressedButton(children).Name;
        int newOption = NewOptionFromButtonName(name);
        this.Player2Option = newOption;

        _SetButtonsDisabled(Player2Node);

        _WaitAllPlayers();

    }

    private static Button GetPressedButton(IEnumerable<Button> children) => children.First<Button>(b => b.Pressed);

    private void _UpdateWinner()
    {
        string message = "";
        switch (Player1Option - Player2Option)
        {
            case 0:
                // Draw
                message = "Empate!";
                break;
            case 1:
            case -2:
                // P1 Win
                message = "Jogador 1 venceu!";
                break;
            case -1:
            case 2:
                // P2 Win
                message = "Jogador 2 venceu!!";
                break;
        }

        ResultNode.GetChild<Label>(0).Text = message;
        // (ResultNode.GetChild(0) as Label).Text = message;

        _SetButtonsEnabled(Player1Node);
        _SetButtonsEnabled(Player2Node);
    }

    private void _WaitAllPlayers()
    {
        _playersWaiting++;

        if (_playersWaiting == NumPlayers)
        {
            _playersWaiting = 0;
            _UpdateWinner();
        }
    }

    private void _SetButtonsDisabled(Node PlayerNode)
    {
        var children = PlayerNode.GetChild(0).GetChildren().Cast<Button>();

        foreach (var child in children) child.Disabled = true;
    }

    private void _SetButtonsEnabled(Node PlayerNode)
    {
        var children = PlayerNode.GetChild(0).GetChildren().Cast<Button>();

        foreach (var child in children)
        {
            child.Pressed = false;
            child.Disabled = false;
        }
    }

}
