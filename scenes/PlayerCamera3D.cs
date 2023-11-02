using Godot;
using System;

public partial class PlayerCamera3D : Camera3D
{

	[Export]
	public NodePath playerNodePath;
	[Export]
	private Node3D playerNode;
	[Export]
	public float zDistance = 10;
	[Export]
	public float yDistance = 10;

	[Export]
	public float cameraMultiplier = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerNode = GetNode<Node3D>(playerNodePath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
    {
        UpdateCameraPositionRelativeToPlayer(delta);
    }

    private void UpdateCameraPositionRelativeToPlayer(double delta)
    {
        var target = playerNode.GlobalPosition;
        var currentPosition = GlobalPosition;
        var offset = target - currentPosition;
	
        var newPosition = new Vector3(currentPosition.X + offset.X, currentPosition.Y, currentPosition.Z + offset.Z - zDistance);
        GlobalPosition = currentPosition.Lerp(newPosition, (float)delta * cameraMultiplier);
    }

}
