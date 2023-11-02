
using Godot;
using System;

public partial class EnemyManager : Node3D
{
	[Export]
	public CharacterBody3D player;
	public Enemy[] enemies;

	[Signal]
	public delegate void SendPlayerLocationEventHandler(Vector3 playerPosition);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void OnPlayerEnterScene(Player player) {
		GD.Print("EnemyManager" + player.GlobalPosition);
	}

}