using Godot;
using System;

public partial class EnemyController : Node3D
{
	// Called when the node enters the scene tree for the first time.
	EnemyMovement enemyMovement;
	EnemyFieldOfVision enemyFieldOfVision;



	public override void _Ready()
	{
		getComponents();
		connectToConeVisionSignal();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		getEnemyCharacterBody().FollowTarget(delta);
	}

	public void getComponents() {
		enemyFieldOfVision = getEnemyFieldOfVision();
		enemyMovement = getEnemyCharacterBody();

	}
	public EnemyMovement getEnemyCharacterBody() {
	  return (EnemyMovement)GetNode<CharacterBody3D>("CharacterBody3D");
	}

	public EnemyFieldOfVision getEnemyFieldOfVision() {
		return (EnemyFieldOfVision)GetNode<Area3D>("CharacterBody3D/FieldOfVision");
	}

	void connectToConeVisionSignal() {
		if (enemyFieldOfVision != null) {
			enemyFieldOfVision.Connect("EnemyFieldOfVisionPlayerEnterSignal", new Callable(this, "EnemyFieldOfVisionCalled"));
		}
	}

	void EnemyFieldOfVisionCalled() {
		GD.Print("player did enter cone of vision");
		//Change State to Chase
	}
	
}
