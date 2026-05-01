using Sandbox;

public sealed class PlanetWalker : Component
{

	[Property] public GameObject Planet { get; set; }
	[Property] public Rigidbody Rigidbody { get; set; }

	[Property] public float GravityStrength { get; set; } = 800f;
	[Property] public float MoveForce { get; set; } = 300f;
	[Property] public float RotationSpeed { get; set; } = 10f;

	protected override void OnFixedUpdate()
	{
		if ( Planet == null || Rigidbody == null )
			return;

		Vector3 gravityDirection = (Planet.WorldPosition - WorldPosition).Normal;
		Vector3 upDirection = -gravityDirection;

		// Pull player toward planet center
		Rigidbody.ApplyForce( gravityDirection * GravityStrength );

		// WASD movement along the planet surface
		Vector3 input = Input.AnalogMove;

		Vector3 forward = Vector3.Cross( WorldRotation.Right, upDirection ).Normal;
		Vector3 right = Vector3.Cross( upDirection, forward ).Normal;

		Vector3 moveDirection = (forward * input.y + right * input.x).Normal;
		Rigidbody.ApplyForce( moveDirection * MoveForce );

		// Rotate player so their feet point toward the planet
		Rotation targetRotation = Rotation.LookAt( forward, upDirection );
		WorldRotation = Rotation.Slerp( WorldRotation, targetRotation, Time.Delta * RotationSpeed );
	}
	protected override void OnUpdate()
	{
	}
}
