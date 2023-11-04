using Godot;
using System;

public partial class EnemyFieldOfVision : Area3D
{
	[Signal]
	public delegate void EnemyFieldOfVisionPlayerEnterSignalEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void OnTimerTimeout() {
		var overlappingBodies = GetOverlappingBodies();
		if (overlappingBodies.Count > 0) {
			foreach (Node3D body in overlappingBodies) {
				if (body.GetParent().Name == "Player") {
					 CheckIfHasObstacleForCollider(body.GlobalPosition);
				} 
			}
		}
	}

	public void CheckIfHasObstacleForCollider(Vector3 position) {
		RayCast3D visionRaycast = GetNode<RayCast3D>("VisionRaycast");
		
		
		if (visionRaycast != null) {

			visionRaycast.LookAt(position, Vector3.Up);
			visionRaycast.ForceRaycastUpdate();

			if (visionRaycast.IsColliding()) {
				var collider = (Node3D)visionRaycast.GetCollider();
				if (collider.GetParent().Name == "Player") {
					EmitSignal("EnemyFieldOfVisionPlayerEnterSignal");
				}
			}
		}
	}

}
