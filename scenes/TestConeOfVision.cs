using Godot;
using System;

public partial class TestConeOfVision : Node3D
{
	[Export]
    public Material VisionConeMaterial;
	[Export]
    public float VisionRange = 15;
	[Export]
    public float VisionAngle = 90;
   
    public int VisionConeResolution = 119;
	[Export]
	public MeshInstance3D meshInstance;


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
      
    }

    public override void _PhysicsProcess(double delta)
	{
		 DrawVisionCone();
	}


    public override void _Ready()
	{
        DrawVisionCone();
	}


    void DrawVisionCone()
    {
        
        float radAngle =  Mathf.DegToRad(VisionAngle);
        float currentAngle = -radAngle / 2;
        float angleIncrement = radAngle / (VisionConeResolution - 1);
        float sine;
        float cosine;

        Vector3[] visionPolygon = new Vector3[VisionConeResolution + 1];
        visionPolygon[0] = Vector3.Zero;
        
        for (int i = 0; i < VisionConeResolution; i++)
        {
            sine = Mathf.Sin(currentAngle);
            cosine = Mathf.Cos(currentAngle);

            Vector3 raycastDirection = GlobalTransform.Basis.Z * cosine + GlobalTransform.Basis.X * sine;
            Vector3 raycastDirectionLine = Transform.Basis.Z * cosine + Transform.Basis.X * sine;
            Vector3 endpoint;

            float distance;

            if (Raycast(raycastDirection, out Vector3 hitPosition, VisionRange))
            {
               distance = GlobalTransform.Origin.DistanceTo(hitPosition);
               endpoint = raycastDirectionLine * distance;
            }
            else
            {
                endpoint = raycastDirectionLine * VisionRange;
                distance = VisionRange;
            }
    
            DrawLine(raycastDirectionLine, distance);

            visionPolygon[i + 1] = endpoint;

            currentAngle += angleIncrement;
        }

        DrawTriangles(visionPolygon, VisionConeMaterial);
    }

    private void DrawLine(Vector3 direction, float distance) {

        var arrMesh = new ArrayMesh();
		var arrays = new Godot.Collections.Array();
    	arrays.Resize((int)Mesh.ArrayType.Max);
		Vector3[] vetocrs = {Transform.Origin, Transform.Origin + direction.Normalized() * distance };
   		arrays[(int)Mesh.ArrayType.Vertex] = vetocrs;
		arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Lines, arrays);

    if (meshInstance != null)
    {
        meshInstance.QueueFree();
    }

    meshInstance = new MeshInstance3D
    {
        Mesh = arrMesh,
        MaterialOverride = VisionConeMaterial
    };
	

	AddChild(meshInstance);	


    }

	

    private bool Raycast(Vector3 direction, out Vector3 hitPosition, float distance)
    {
		
        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Godot.Collections.Array<Rid> exeptObjetcs = new Godot.Collections.Array<Rid>();
		PhysicsRayQueryParameters3D parameters3D = PhysicsRayQueryParameters3D.Create(GlobalTransform.Origin ,GlobalTransform.Origin + direction.Normalized() * distance, 4294967295, exeptObjetcs);
        Godot.Collections.Dictionary result = spaceState.IntersectRay(parameters3D);

         if (result.Count > 0)
        {
            hitPosition = (Vector3)result["position"];

            return true;
        }
        else
        {
            hitPosition = Vector3.Zero;
            return false;
        }

		
    }

  private void DrawPolygon(Vector3[] vertices, Material material)
{
    int numVertices = vertices.Length;
    int numTriangles = numVertices - 2;

    if (numVertices < 3)
    {
        GD.PrintErr("Not enough vertices to form triangles.");
        return;
    }

    if (numVertices % 3 != 0)
    {
        GD.PrintErr("Number of vertices is not a multiple of 3.");
        return;
    }

    var arrMesh = new ArrayMesh();
    var arrays = new Godot.Collections.Array();
    arrays.Resize((int)Mesh.ArrayType.Max);
    arrays[(int)Mesh.ArrayType.Vertex] = vertices;


    // Create the Mesh.
    arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

    // Create and set the MeshInstance3D.
    if (meshInstance != null)
    {
        meshInstance.QueueFree();
    }

    meshInstance = new MeshInstance3D
    {
        Mesh = arrMesh,
        MaterialOverride = material
    };

	AddChild(meshInstance);	
	
    

    // GD.Print("Mesh successfully created and added to the scene.");
}

 private void DrawTriangles(Vector3[] vertices, Material material)
    {
        int[] indices = new int[(VisionConeResolution - 1) * 3];

        for (int i = 0, j = 0; i < indices.Length; i += 3, j++)
        {
            indices[i] = j + 2;
            indices[i + 1] = j + 1;
            indices[i + 2] = 0;
        }

        var arrMesh = new ArrayMesh();
        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);

        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        arrays[(int)Mesh.ArrayType.Index] = indices;

        arrMesh.ClearSurfaces();
        arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

        meshInstance?.QueueFree();

        meshInstance = new MeshInstance3D
        {
            Mesh = arrMesh,
            MaterialOverride = material
        };

	    AddChild(meshInstance);	
        
    }
}