using Godot;
using System;

public partial class Player : Node3D
{
	// Called when the node enters the scene tree for the first time.
[Signal]
public delegate void PlayerDidEnterSceneEventHandler(Player player);

	public override void _Ready()
	{
		EmitSignal("PlayerDidEnterScene", this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		EmitSignal("PlayerDidEnterScene", this);
		GD.Print("PlayerScript" + this.GlobalPosition);
	}
}
