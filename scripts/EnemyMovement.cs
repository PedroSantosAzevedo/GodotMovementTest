using Godot;
using System;

public partial class EnemyMovement : CharacterBody3D
{

	[Export]
	NavigationAgent3D navigationAgent;
	[Export]
	int speed = 5;
	int accel = 10;

	[Export]
	NodePath playerNodePath = "Player";
	[Export]
	Vector3 targetPosition;
	float rotation_direction;

	float rotationSpeed = 7;
	[Export]
	float deltaRotation = 0.1f;


	public override void _Ready()
	{
		navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		connectToConeVisionSignal();
		
	}

	public override void _PhysicsProcess(double delta)
	{
		FollowTarget(delta);
	}


	public void FollowTarget(double delta) {
		var node = GetPlayerNode();
		MoveTowardDirection(delta, destination: node.GlobalPosition);
		RotateEnemy(delta, Velocity);
		MoveAndSlide();
	}

	public Node3D GetPlayerNode()
	{
		return GetNodeOrNull<Node3D>("/root/MainScene/Player/PlayerCharacterBody3D");
	}

	
	void RotateEnemy(double delta, Vector3 Velocity)
	{
		if (Velocity != Vector3.Zero)
		{
			var current_rot = GlobalTransform.Basis.GetRotationQuaternion().Normalized();
			rotation_direction = new Vector2(Velocity.Z, Velocity.X).Angle();
			var quat = new Quaternion(GlobalTransform.Basis.Y.Normalized(), rotation_direction);
			var smoothrot = current_rot.Slerp(quat, (float)delta * rotationSpeed);
			GlobalTransform = new Transform3D(new Basis(smoothrot), GlobalTransform.Origin);
		}
	}

	void MoveTowardDirection(double delta, Vector3 destination)
	{
		targetPosition = destination;
		navigationAgent.TargetPosition = targetPosition;
		var direction = navigationAgent.GetNextPathPosition() - GlobalPosition;
		direction = direction.Normalized();
		Velocity = Velocity.Lerp(direction * speed, accel * (float)delta);
	}

	void connectToConeVisionSignal() {
		Node3D coneOfVision = GetNode<Node3D>("FieldOfVision");
		if (coneOfVision != null) {
			coneOfVision.Connect("EnemyFieldOfVisionPlayerEnterSignal", new Callable(this, "EnemyFieldOfVisionCalled"));
		}
	}

	void EnemyFieldOfVisionCalled() {
		GD.Print("player did enter cone of vision");

	}
}
