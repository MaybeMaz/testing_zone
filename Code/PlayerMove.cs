using Sandbox;

public sealed class PlayerMove : Component
{
	[Property] public float Speed { get; set; } = 100f;
	[Property] public float LookSensitivity { get; set; } = 0.1f;
	private Angles eyeAngles;
	
	protected override void OnStart()
	{
		eyeAngles = WorldRotation.Angles();
	}

	protected override void OnUpdate()
	{
		// Mouse look
		var look = Input.AnalogLook;

		eyeAngles.yaw += look.yaw * LookSensitivity;
		eyeAngles.pitch += look.pitch * LookSensitivity;
		eyeAngles.pitch = eyeAngles.pitch.Clamp( -89f, 89f );

		WorldRotation = eyeAngles.ToRotation();

		var flatRotation = Rotation.FromYaw( WorldRotation.Yaw() );
		Vector3 move = Vector3.Zero;
		
		if ( Input.Down("forward") ) move += flatRotation.Forward;
		if ( Input.Down("backward") )    move += flatRotation.Backward;
		if ( Input.Down("left") )    move += flatRotation.Left;
		if ( Input.Down("right") )   move += flatRotation.Right;

		WorldPosition += move.Normal * Speed * Time.Delta;
	}
}
