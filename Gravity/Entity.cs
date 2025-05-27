using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gravity
{
    public class Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float MaxSpeed { get; set; }
        public bool Grounded { get; set; }

        public Entity(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            MaxSpeed = 100f;
            Grounded = false;
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration += force;
        }

        public void Update(float deltaTime)
        {
            Velocity += Acceleration * deltaTime;

            // Limit velocity to max speed
            if (Velocity.Length() > MaxSpeed)
            {
                Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
            }

            Position += Velocity * deltaTime;
            Acceleration = Vector2.Zero;
        }

        public override string ToString() => $"Position: {Position}, Velocity: {Velocity}, Acceleration: {Acceleration}";

    }
}
