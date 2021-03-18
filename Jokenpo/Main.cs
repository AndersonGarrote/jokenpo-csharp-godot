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
    private Node Player1Node;

    [Export]
    private NodePath Player2 { get; set; }
    private Node Player2Node;

    [Export]
    private NodePath Result { get; set; }
    private Node ResultNode;
    
    public int Player1Option { get; set; }
    public int Player2Option { get; set; }

    private int _playersWaiting;

    public override void _Ready()
    {
        _OnReady();
    }

    private void _OnReady()
    {    
        Player1Node = this.GetNode(Player1);
        Player2Node = this.GetNode(Player2);
        ResultNode = this.GetNode(Result);
    }

    private void _OnPlayer1ButtonPressed()
    {
        IEnumerable<Button> children = Player1Node.GetChild(0).GetChildren().Cast<Button>();
        GD.Print(children.First<Button>( b => b.Pressed ).Name);
        string name = children.First<Button>( b => b.Pressed ).Name;
        int newOption = NameToOptions[name.Substring(0, name.LastIndexOf("Button"))];
        this.Player1Option = newOption;

        _SetButtonsDisabled(Player1Node);

        _WaitAllPlayers();
    }

    private void _OnPlayer2ButtonPressed()
    {
        IEnumerable<Button> children = Player2Node.GetChild(0).GetChildren().Cast<Button>();
        GD.Print(children.First<Button>( b => b.Pressed ).Name);
        string name = children.First<Button>( b => b.Pressed ).Name;
        int newOption = NameToOptions[name.Substring(0, name.LastIndexOf("Button"))];
        this.Player2Option = newOption;

        _SetButtonsDisabled(Player2Node);

        _WaitAllPlayers();

    }
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
        _playersWaiting ++;

        if (_playersWaiting == NumPlayers)
        {
            _playersWaiting = 0;
            _UpdateWinner();
        }
    }

    private void _SetButtonsDisabled(Node PlayerNode)
    {
        IEnumerable<Button> children = PlayerNode.GetChild(0).GetChildren().Cast<Button>();
        
        foreach (var child in children) child.Disabled = true;
    }

    private void _SetButtonsEnabled(Node PlayerNode)
    {
        IEnumerable<Button> children = PlayerNode.GetChild(0).GetChildren().Cast<Button>();
        
        foreach (var child in children)
        {
            child.Pressed = false;
            child.Disabled = false;
        } 
    }

}
