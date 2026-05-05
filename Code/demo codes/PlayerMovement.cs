using Sandbox;

public sealed class PlayerMovement : Component
{
	
	private Angles eyeAngles;
	[Property] public float Sensitivity { get; set; } = 0.1f;
	[Property] private float MoveSpeed = 50f;

	protected override void OnUpdate()
	{
		var look = Input.AnalogLook;

		eyeAngles.yaw += look.yaw * Sensitivity;
		eyeAngles.pitch += look.pitch * Sensitivity;
		eyeAngles.pitch = eyeAngles.pitch.Clamp( -89, 89 );
		WorldRotation = look.ToRotation();

		WorldRotation = eyeAngles.ToRotation();

		if ( Input.Down( "Forward" ) )
		{
			WorldPosition += Vector3.Forward * MoveSpeed * Time.Delta;
		}
		if ( Input.Down( "Backward" ) )
		{
			WorldPosition += Vector3.Backward * MoveSpeed * Time.Delta;
		}
		if ( Input.Down( "Left" ) )
		{
			WorldPosition += Vector3.Left * MoveSpeed * Time.Delta;
		}
		if ( Input.Down( "Right" ) )
		{
			WorldPosition += Vector3.Right * MoveSpeed * Time.Delta;
		}
	}
}
