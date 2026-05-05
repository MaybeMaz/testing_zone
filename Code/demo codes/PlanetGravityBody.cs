using Sandbox;

public sealed class PlanetGravityBody : Component
{
	[Property] public GameObject Planet { get; set; }
	[Property] public Rigidbody Body { get; set; }

	[Property] public float GravityStrength { get; set; } = 1000f;
	[Property] public float RotationSpeed { get; set; } = 3f;
	[Property] public float MoveSpeed { get; set; } = 200f;

	protected override void OnFixedUpdate()
	{
		if ( Planet == null || Body == null )
			return;

		Body.Sleeping = false;

		Vector3 gravityDirection = (Planet.WorldPosition - WorldPosition).Normal;
		Vector3 upDirection = -gravityDirection;

		Body.Velocity += gravityDirection * GravityStrength * Time.Delta;
		Body.AngularVelocity = Vector3.Zero;

		Vector3 forward = WorldRotation.Forward;
		forward = (forward - upDirection * Vector3.Dot( forward, upDirection )).Normal;

		if ( forward.Length < 0.01f )
			forward = Vector3.Cross( Vector3.Right, upDirection ).Normal;

		if ( forward.Length < 0.01f )
			forward = Vector3.Cross( Vector3.Forward, upDirection ).Normal;

		Vector3 right = Vector3.Cross( forward, upDirection ).Normal;

		Vector3 input = Input.AnalogMove;
		Vector3 moveDirection = (forward * input.y + right * input.x).Normal;

		Body.Velocity += moveDirection * MoveSpeed * Time.Delta;

		Rotation targetRotation = Rotation.LookAt( forward, upDirection );

		WorldRotation = Rotation.Slerp(
			WorldRotation,
			targetRotation,
			Time.Delta * RotationSpeed
		);
	}
}
