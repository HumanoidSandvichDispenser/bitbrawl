using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Particles;
using Nez.Sprites;
using Nez.Tiled;
using project_pyro_rewrite.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Entities
{
    /// <summary>
    /// Entity with preset components used by players
    /// </summary>
    public class Player : Entity
    {
        public GameScene GameScene => Scene as GameScene;

        public bool IsAlive => GetComponent<Components.PlayerInfo>().Health > 0;

        /** COMPONENT PROPERTIES **/
        public Components.PlayerInfo PlayerInfo { get; set; }

        public SpriteAnimator SpriteAnimator { get; set; }

        public Components.WavingSprite WavingSprite { get; set; }

        public BoxCollider BoxCollider { get; set; }

        public TiledMapMover TiledMapMover { get; set; }

        public Components.PlayerMover PlayerMover { get; set; }

        public Components.InputController InputController { get; set; }

        public FollowCamera FollowCamera { get; set; }

        public ParticleEmitter ParticleEmitter { get; set; }

        private Dictionary<AttachmentType, Entity> _attachments = new Dictionary<AttachmentType, Entity>();

        public Player(string name, TmxLayer collisionLayer) : base(name)
        {
            PlayerInfo = AddComponent(new Components.PlayerInfo());
            SpriteAnimator = AddComponent(new SpriteAnimator { Enabled = false });
            WavingSprite = AddComponent(new Components.WavingSprite 
            {
                Amplitude = new Vector2(1, 2),
                Period = new Vector2(2, 0.5f),
                WaveX = true,
                WaveY = true
            });
            BoxCollider = AddComponent(new BoxCollider(20, 28));
            TiledMapMover = AddComponent(new TiledMapMover(collisionLayer));
            PlayerMover = AddComponent(new Components.PlayerMover());
            ParticleEmitter = AddComponent(new ParticleEmitter(Particles.DefaultParticles.MageWarpParticle()));
        }

        /// <summary>
        /// Adds the necessary components (FollowCamera, PlayerController) to make this entity a controllable player.
        /// </summary>
        /// <returns>The Player object</returns>
        public Player MakeControllablePlayer()
        {
            var camera = new FollowCamera
            {
                FollowLerp = 1f,
                UpdateOrder = (int)Utils.UpdateOrder.Camera
            };
            FollowCamera = AddComponent(camera);
            InputController = AddComponent(new Components.InputController());
            return this;
        }

        public void Kill(Entity attacker = null, bool respawn = false, float respawnTime = 5f)
        {
            var playerInfo = GetComponent<Components.PlayerInfo>();
            var camera = GetComponent<FollowCamera>();
            var renderer = GetComponent<SpriteRenderer>();
            var mover = GetComponent<TiledMapMover>();

            if (playerInfo.Health > 0)
            {
                playerInfo.Health = 0;
            }
            
            if (camera != null)
            {
                if (attacker != null)
                {
                    camera.Follow(attacker, FollowCamera.CameraStyle.LockOn);
                }
            }

            renderer.Enabled = false;
            mover.Enabled = false;

            if (respawn)
            {
                Core.Schedule(respawnTime, t =>
                {
                    if (!IsAlive)
                        Spawn();
                });
            }
        }

        public void Hurt(Entity attacker, float damage)
        {
            PlayerInfo.Health -= damage;
            if (PlayerInfo.Health <= 0)
                Kill(attacker, true);
        }

        public void Spawn(Vector2 position = default, bool force = false)
        {
            var playerInfo = GetComponent<Components.PlayerInfo>();
            var camera = GetComponent<FollowCamera>();
            var renderer = GetComponent<SpriteRenderer>();
            var wavingSprite = GetComponent<Components.WavingSprite>();
            var mover = GetComponent<TiledMapMover>();
            var playerMover = GetComponent<Components.PlayerMover>();
            var animator = GetComponent<SpriteAnimator>();

            // if we're not forcing a respawn and we are still alive then return
            if (!force && IsAlive)
                return;

            var atlas = Core.Content.LoadSpriteAtlas($"Content/Sprites/Characters/{playerInfo.Class}/{playerInfo.Team}/sheet.atlas");

            animator.Enabled = true;
            animator.ResetAnimations();
            animator.AddAnimationsFromAtlas(atlas)
                .Play("Idle");

            if (position == default)
            {
                position = GameScene.FindSpawn(playerInfo.Team);
            }

            switch (playerInfo.Class)
            {
                case project_pyro_rewrite.Components.PlayerClass.Templar:
                    playerInfo.MaxHealth = 175;
                    playerMover.MoveSpeed = 192;
                    wavingSprite.Amplitude = new Vector2(2, 4);
                    break;
                case project_pyro_rewrite.Components.PlayerClass.Mage:
                    playerInfo.MaxHealth = 150;
                    playerMover.MoveSpeed = 256;
                    wavingSprite.Amplitude = new Vector2(0, 0);
                    break;
                case project_pyro_rewrite.Components.PlayerClass.Hunter:
                    playerInfo.MaxHealth = 75;
                    playerMover.MoveSpeed = 272;
                    wavingSprite.Amplitude = new Vector2(4, 0);
                    break;
            }

            playerInfo.Health = playerInfo.MaxHealth;
            
            renderer.Enabled = true;
            mover.Enabled = true;
            Position = position;
            if (!(camera is null))
            {
                if (camera.TargetEntity != this)
                {
                    camera.Follow(this, FollowCamera.CameraStyle.LockOn);
                }

                camera.Camera.SetZoom(1.00f);
            }
        }

        public void StartAttack() 
        {
            if (!IsAlive)
                return;

            if (Time.TotalTime >= PlayerInfo.LastFireTime + 0.5f)
            {
                Entity projectileEnt = Scene.CreateEntity("projectile");
                Components.Projectile projectile = projectileEnt.AddComponent(new Components.Projectile());
                TiledMapMover mover = projectileEnt.AddComponent(new TiledMapMover(TiledMapMover.CollisionLayer));
                BoxCollider boxCollider = projectileEnt.AddComponent(new BoxCollider(64, 64));
                SpriteRenderer renderer = projectileEnt.AddComponent(new SpriteRenderer(Core.Content.Load<Texture2D>("Sprites/Debug/PepegaCoin")));
                projectileEnt.SetScale(0.125f);
                projectile.Speed = 512;
                projectile.Direction = InputController.Target - Position;
                projectile.Owner = this;
                projectileEnt.Position = Position;
                PlayerInfo.LastFireTime = Time.TotalTime;
            }
        }

        public void StopAttack()
        {

        }

        public void StartAbility()
        {
            if (!IsAlive)
                return;

            if (Time.TotalTime >= PlayerInfo.AbilityChargeTime)
            {
                PlayerInfo.AbilityChargeTime = Time.TotalTime + 5f;
                PlayerInfo.AbilityActive = true;

                switch (PlayerInfo.Class)
                {
                    case project_pyro_rewrite.Components.PlayerClass.Mage:
                        Entity magePreview = new Entity();
                        magePreview.AddComponent(SpriteAnimator.Clone());
                        magePreview.GetComponent<SpriteAnimator>().SetColor(Color.FromNonPremultiplied(255, 255, 255, 100));
                        magePreview.AttachToScene(Scene);
                        magePreview.SetParent(Transform);
                        _attachments.Add(AttachmentType.MageWarpPreview, magePreview);
                        break;
                }
            }
        }

        public void StopAbility()
        {
            var playerInfo = GetComponent<Components.PlayerInfo>();
            var playerMover = GetComponent<Components.PlayerMover>();
            var camera = GetComponent<FollowCamera>();

            if (playerInfo.AbilityActive)
            {
                playerInfo.AbilityActive = false;

                switch (playerInfo.Class)
                {
                    case project_pyro_rewrite.Components.PlayerClass.Mage:
                        Vector2 mousePos = camera.Camera.MouseToWorldPoint();
                        ParticleEmitter.Emit(4);
                        playerMover.WarpToPosition(mousePos, 192);
                        ParticleEmitter.Emit(12);
                        if (_attachments.ContainsKey(AttachmentType.MageWarpPreview))
                        {
                            _attachments[AttachmentType.MageWarpPreview].RemoveAllComponents();
                            _attachments[AttachmentType.MageWarpPreview].Destroy();
                            _attachments.Remove(AttachmentType.MageWarpPreview);
                        }
                        break;
                }
            }
        }

        public override void Update()
        {
            base.Update();

            var playerInfo = GetComponent<Components.PlayerInfo>();

            if (FollowCamera != null)
            {
                FollowCamera.Deadzone.X = 960; // camera offset fix
                FollowCamera.Deadzone.Y = 540;
            }

            switch (playerInfo.Class)
            {
                case project_pyro_rewrite.Components.PlayerClass.Templar:
                    
                    break;
                case project_pyro_rewrite.Components.PlayerClass.Mage:
                    if (_attachments.ContainsKey(AttachmentType.MageWarpPreview) && FollowCamera != null && FollowCamera.Camera != null)
                    {
                        var preview = _attachments[AttachmentType.MageWarpPreview];
                        Vector2 mousePos = FollowCamera.Camera.MouseToWorldPoint();
                        Vector2 targetPos = PlayerMover.WarpToPosition(mousePos, 192, false);
                        if ((targetPos - preview.LocalPosition).LengthSquared() > 4)
                        {
                            preview.SetLocalPosition(targetPos);
                        }
                    }
                    break;
                case project_pyro_rewrite.Components.PlayerClass.Hunter:
                    
                    break;
            }
        }
    }
}
