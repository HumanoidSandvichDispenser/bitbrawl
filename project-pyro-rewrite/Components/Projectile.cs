using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using project_pyro_rewrite.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class Projectile : Component, IUpdatable
    {
        public float Speed { get; set; }

        public Vector2 Direction { get; set; }

        public Entities.Player Owner { get; set; }

        private TiledMapMover _mover;
        private BoxCollider _boxCollider;
        private TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();

        public Projectile()
        {

        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _mover = Entity.GetComponent<TiledMapMover>();
            _boxCollider = Entity.GetComponent<BoxCollider>();
        }

        public void Update()
        {
            //TiledMapMover mover = Entity.GetComponent<TiledMapMover>();
            //if ()
            Vector2 move = Vector2.Normalize(Direction) * Speed * Time.DeltaTime;
            if (!move.IsNaN())
            {
                _mover.Move(move, _boxCollider, _collisionState);
                if (_collisionState.HasCollision)
                {
                    Entity.Destroy();
                }
            }
        }
    }
}
