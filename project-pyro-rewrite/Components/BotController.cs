using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class BotController : InputController
    {
        public TmxLayer MapCollisionLayer { get; set; }

        public Entities.Player Player => Entity as Entities.Player;

        private AstarGridGraph _gridGraph;
        private Queue<Point> _currentPath;
        private Point _nextPoint;
        private Entities.Player _targetEntity;
        private Vector2 _targetEntityPos;
        private float _lastRecalculationTime = 0;

        public override void Initialize()
        {
            base.Initialize();

            _gridGraph = new AstarGridGraph(MapCollisionLayer);
            List<Point> points = new List<Point>(_gridGraph.Walls);
            foreach (Point point in points)
            {
                // this tightens the walkable area for the bots so they do not get stuck along the walls
                var neighbors = new List<Point>(_gridGraph.GetNeighbors(point));
                var topNeighbor = neighbors.Find(p => p.Y == point.Y - 1);
                if (topNeighbor == default)
                {
                    topNeighbor = new Point(point.X, point.Y - 1);
                    _gridGraph.Walls.Add(topNeighbor);
                }
            }
        }

        public override void Update()
        {
            // become Pepega (don't think) if bot is not alive
            if (!Player.IsAlive)
                return;

            // attack!!! if found a target
            if (_targetEntity != null)
            {
                // cast a ray to check if bot can see target
                var ray = Physics.Linecast(Entity.Position, _targetEntityPos, (int)Utils.RenderLayer.TileMap);

                // start firing if bot can see target
                if (ray.Fraction == 0)
                {
                    Player.StartAttack();
                    Vector2 targetVel = Vector2.Normalize(_targetEntity.InputController.Direction) *
                        _targetEntity.PlayerMover.MoveSpeed;

                    // predict where to shoot based on player's distance and velocity
                    Vector2 prediction = Utils.VectorPrediction.PredictVector(_targetEntityPos - Entity.Position,
                         targetVel, 512);
                    Target = _targetEntityPos + prediction;
                }
            }

            // calculate path if forced to recalculate or bot has completed their current path
            // but only if the bot has last calculated 0.5 seconds ago
            if (Time.TotalTime > _lastRecalculationTime + 0.5f &&
                (ShouldRecalculate(out Entities.Player bestPlayer) || _currentPath == null))
            {
                _targetEntity = bestPlayer;
                if (bestPlayer != null)
                {
                    _targetEntityPos = bestPlayer.Position;

                    // calculate the path
                    var pathPoints = _gridGraph.Search(Entity.Position.ToScaledPoint(), _targetEntity.Position.ToScaledPoint());

                    if (pathPoints != null)
                    {
                        _currentPath = new Queue<Point>(pathPoints);
                        _nextPoint = _currentPath.Peek();
                        _lastRecalculationTime = Time.TotalTime;
                    }
                }
            }
            else if (_currentPath != null)
            {
                Vector2 directionToPoint = _nextPoint.ToScaledVector2() - Entity.Position;
                if (directionToPoint.LengthSquared() > 144)
                {
                    Direction = directionToPoint;
                }
                else
                {
                    if (_currentPath.Count > 0)
                    {
                        _nextPoint = _currentPath.Dequeue();
                        //Debug.DrawHollowBox(_nextPoint.ToScaledVector2(), 4, Color.Green, 1);
                    }
                    else
                    {
                        _currentPath = null;
                    }
                }
            }
        }

        private bool ShouldRecalculate(out Entities.Player bestPlayer)
        {
            // we should recalculate if:
            //  - we have a new best target
            //  - we have last calculated a path in 5 seconds
            //  - our current target has moved at least [sqrt(1024) = ] 32 units away where we found them
            bestPlayer = FindBestTarget();
            return bestPlayer != null && (bestPlayer != _targetEntity || Time.TotalTime > _lastRecalculationTime + 5 ||
                (_targetEntity.Position - _targetEntityPos).LengthSquared() > 1024);
        }

        /// <summary>
        /// Finds the best target based on their target score
        /// </summary>
        /// <returns></returns>
        private Entities.Player FindBestTarget()
        {
            Entities.Player bestPlayer = null;
            float bestScore = -1;
            foreach (Entities.Player plr in Entity.Scene.EntitiesOfType<Entities.Player>())
            {
                // dead players and the current player do not count
                if (!plr.IsAlive || plr.PlayerInfo.Team == Player.PlayerInfo.Team)
                    continue;

                // if we currently do not have a best target, set it to the current one
                if (bestPlayer == null)
                {
                    bestPlayer = plr;
                    bestScore = GetTargetScore(plr);
                }
                else
                {
                    float plrScore = GetTargetScore(plr);

                    // lower score = better
                    if (plrScore < bestScore)
                    {
                        bestPlayer = plr;
                        bestScore = plrScore;
                    }
                }
            }
            //Nez.Console.DebugConsole.Instance.Log($"Best possible target: {(bestPlayer == null ? "null" : bestPlayer.Name)}");
            return bestPlayer;
        }

        /// <summary>
        /// Calculates target score based on distance and health
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private float GetTargetScore(Entities.Player player)
        {
            if (!player.IsAlive)
                return float.MaxValue;
            return (player.Position - Entity.Position).LengthSquared() + (player.PlayerInfo.Health * 2);
        }
    }

    internal static class Extensions
    {
        public static Point ToScaledPoint(this Vector2 vector2)
        {
            return (vector2 / 16).ToPoint();
        }

        public static Vector2 ToScaledVector2(this Point point)
        {
            return (point.ToVector2() * 16) + new Vector2(8);
        }
    }
}
