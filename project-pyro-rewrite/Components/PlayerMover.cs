using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class PlayerMover : Component, IUpdatable
    {
        public Entities.Player Player => Entity as Entities.Player;
        public float MoveSpeed { get; set; } = 256;
        public float Friction { get; set; } = 0.8f;
        int IUpdatable.UpdateOrder => 2;

        public Vector2 MousePosition { get => Input.MousePosition; set => discard(); }
        public void discard() { }

        private TiledMapMover _mover;
        private BoxCollider _boxCollider;
        private PlayerInfo _playerInfo;
        private InputController _controller;
        private Vector2 _velocity = new Vector2(0, 0);
        private Vector2 _accel = new Vector2(0, 0);
        private SpriteRenderer _renderer;
        private TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();

        public override void OnAddedToEntity()
        {
            if (!(Entity is Entities.Player))
            {
                throw new Exception("PlayerController is incompatible with this entity.");
            }

            _mover = Entity.GetComponent<TiledMapMover>();
            _boxCollider = Entity.GetComponent<BoxCollider>();
            _renderer = Entity.GetComponent<SpriteRenderer>();
            _playerInfo = Entity.GetComponent<PlayerInfo>();
            _controller = Entity.GetComponent<InputController>();
        }

        public void Update()
        {
            if (Nez.Console.DebugConsole.Instance.IsOpen || !Player.IsAlive)
                return;

            if (_velocity.X == float.NaN || _velocity.Y == float.NaN)
                _velocity = Vector2.Zero;

            _velocity *= Friction;
            if (_velocity.LengthSquared() < 1/64f)
            {
                _velocity = Vector2.Zero;
            }

            _accel = _controller.Direction;
            float speed = MoveSpeed * Math.Min(Time.DeltaTime, 1);

            // multiply the acceleration vector by speed and apply friction
            if (_accel.LengthSquared() > 0)
            {
                _accel.Normalize();
                _accel *= (1 - Friction) * speed;
            }

            // add acceleration to velocity
            _velocity += _accel;

            // clamp velocity so player does not go faster than its max speed
            if (_velocity.Length() > speed)
            {
                _velocity.Normalize();
                _velocity *= speed;
            }

            Move(_velocity);

            if (_controller.Target.X >= Entity.Position.X)
            {
                _renderer.FlipX = false;
            }
            else
            {
                _renderer.FlipX = true;
            }

            if (_boxCollider.CollidesWith(out CollisionResult result,
                (Collider collider) => collider.HasComponent<Projectile>()))
            {
                Projectile projectile = result.Collider.GetComponent<Projectile>();
                // if we don't own this entity/projectile
                if (projectile.Owner.PlayerInfo.Team != Player.PlayerInfo.Team)
                {
                    Player.Hurt(projectile.Owner, 30);
                    //Player.Kill(projectile.Owner, true); // die :)
                    result.Collider.Entity.Destroy();
                }
            }
        }

        /// <summary>
        /// Move the player to the specified direction
        /// </summary>
        /// <param name="vector">Scaled vector</param>
        public void Move(Vector2 vector)
        {
            //Nez.Console.DebugConsole.Instance.Log(vector);
            if (_velocity.LengthSquared() > 0 && Player.IsAlive)
                _mover.Move(_velocity, _boxCollider, _collisionState);
        }

        /// <summary>
        /// Move the player to the specified position if possible
        /// </summary>
        /// <param name="position"></param>
        public Vector2 WarpToPosition(Vector2 position, float maxLength = -1, bool moveToPos = true)
        {
            Vector2 direction = position - Entity.Position;
            if (maxLength > 0 && direction.LengthSquared() > Math.Pow(maxLength, 2))
            {
                direction.Normalize();
                direction *= maxLength;
            }
            
            _mover.TestCollisions(ref direction, _boxCollider.Bounds, _collisionState);

            if (moveToPos)
                Entity.Position += direction;
            return direction;
        }
    }
}
