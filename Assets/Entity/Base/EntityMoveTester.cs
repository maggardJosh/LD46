using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Entity.Base
{
    public class EntityMoveTester
    {
        readonly IDebugLogger _logger;
        private MoveResult _result;
        private readonly BoxCollider2D _collider;
        private readonly LayerMask _collisionMask;
        private readonly LayerMask _oneWayMask;

        public EntityMoveTester(IDebugLogger logger, BoxCollider2D bCollider, LayerMask collisionMask, LayerMask oneWayMask)
        {
            _logger = logger;
            _collider = bCollider;
            _collisionMask = collisionMask;
            _oneWayMask = oneWayMask;
        }

        public MoveResult GetMoveResult(Vector3 startPos, Vector3 amount)
        {
            _result = new MoveResult
            {
                NewPos = startPos,
                StartPos = startPos
            };

            if (Mathf.Abs(amount.x) > 0)
                TryMoveHorizontal(amount);

            if (Mathf.Abs(amount.y) > 0)
                TryMoveVertical(amount);

            return _result;
        }

        public MoveResult GetHorizontalRaycastFromCenter(Vector3 pos)
        {
            _result = new MoveResult
            {
                NewPos = pos
            };

            TryRaycastHorizontal(Vector3.left);
            TryRaycastHorizontal(Vector3.right);

            return _result;
        }

        private void TryRaycastHorizontal(Vector3 direction)
        {
            if (!direction.sqrMagnitude.EqualsFloat(1))
                throw new ArgumentException("TryRaycastHorizontal: Direction vector not normalized");

            float numSections = _collider.bounds.size.y / GameSettings.TileSize + 1;
            Vector3 center = _result.NewPos + _collider.transform.rotation * _collider.offset.ToVector3();
            Vector2 raycastDir = direction * _collider.bounds.size.x / 2;

            _result.NewPos = GetResultPosHorizontal(_result.NewPos, numSections, center, raycastDir);
        }

        private Vector3 GetResultPosHorizontal(Vector3 resultingPosition, float numSections, Vector3 center, Vector2 raycastDir)
        {
            for (int i = 0; i <= numSections; i++)
            {
                //i==0 top side corner
                //i==1 bottom side corner
                float ySectionValue = Mathf.Min(i * GameSettings.TileSize, _collider.bounds.size.y - GameSettings.CollisionOffsetValue * 2f);
                Vector2 raycastOrig = center + Vector3.up * (_collider.bounds.size.y / 2 - GameSettings.CollisionOffsetValue) + (Vector3.down * ySectionValue);

                Vector3 collisionPosition = GetCollisionPositionHorizontal(raycastOrig, raycastDir);

                float distanceToCollisionPoint = (collisionPosition - _result.NewPos).sqrMagnitude;
                float distanceToCurrentResultPosition = (resultingPosition - _result.NewPos).sqrMagnitude;

                if (distanceToCollisionPoint > distanceToCurrentResultPosition)
                    resultingPosition = collisionPosition;
            }

            return resultingPosition;
        }

        internal MoveResult GetVerticalRaycastFromCenter(Vector3 pos)
        {
            _result = new MoveResult
            {
                NewPos = pos
            };

            TryRaycastVertical(Vector3.down);
            TryRaycastVertical(Vector3.up);

            return _result;
        }

        private void TryRaycastVertical(Vector3 direction)
        {
            if (!direction.sqrMagnitude.EqualsFloat(1))
                throw new ArgumentException("TryRaycastVertical: Direction vector not normalized");

            float numSections = _collider.bounds.size.x / GameSettings.TileSize + 1;
            Vector3 center = _result.NewPos + _collider.transform.rotation * _collider.offset.ToVector3();
            Vector2 raycastDir = direction * _collider.bounds.size.y / 2;

            _result.NewPos = GetResultPosVertical(_result.NewPos, numSections, center, raycastDir);
        }

        private Vector3 GetResultPosVertical(Vector3 resultingPosition, float numSections, Vector3 center, Vector2 raycastDir)
        {
            for (int i = 0; i <= numSections; i++)
            {
                //i==0 top side corner
                //i==1 bottom side corner
                float xSectionValue = Mathf.Min(_collider.bounds.size.x - GameSettings.CollisionOffsetValue * 2f, i * GameSettings.TileSize);
                Vector2 raycastOrig = center + Vector3.left * (_collider.bounds.size.x / 2 - GameSettings.CollisionOffsetValue) + (Vector3.right * xSectionValue);

                Vector3 collisionPosition = GetCollisionPositionVertical(raycastOrig, raycastDir);

                float distanceToCollisionPoint = (collisionPosition - _result.NewPos).sqrMagnitude;
                float distanceToCurrentResultPosition = (resultingPosition - _result.NewPos).sqrMagnitude;

                if (distanceToCollisionPoint > distanceToCurrentResultPosition)
                    resultingPosition = collisionPosition;
            }

            return resultingPosition;
        }

        private Vector3 GetCollisionPositionVertical(Vector2 raycastOrig, Vector2 raycastDir)
        {
            var hits = new RaycastHit2D[1];
            var hitResult = Physics2D.Raycast(raycastOrig, raycastDir, new ContactFilter2D{layerMask = _collisionMask, useLayerMask = true}, hits, raycastDir.magnitude);
            var hitSomething = hitResult > 0;
            _logger.DebugLine(raycastOrig, raycastOrig + raycastDir, hitSomething ? Color.red : Color.green);

            if (!hitSomething)
            {
                return _result.NewPos;
            }

            if (raycastDir.y > 0)
                _result.HitUp = true;
            else
                _result.HitDown = true;
            return new Vector3(_result.NewPos.x, hits.First().point.y + (-( _collider.transform.rotation * _collider.offset).y)) - new Vector3(raycastDir.x, raycastDir.y);
        }

        private Vector3 GetCollisionPositionHorizontal(Vector2 raycastOrig, Vector2 raycastDir)
        {
            var hits = new RaycastHit2D[1];
            var hitResult = Physics2D.Raycast(raycastOrig, raycastDir, new ContactFilter2D{layerMask = _collisionMask, useLayerMask = true}, hits, raycastDir.magnitude);
            var hitSomething = hitResult > 0;
            _logger.DebugLine(raycastOrig, raycastOrig + raycastDir, hitSomething ? Color.red : Color.green);

            if (!hitSomething)
            {
                return _result.NewPos;
            }

            if (raycastDir.x > 0)
                _result.HitRight = true;
            else
                _result.HitLeft = true;
            return new Vector3(hits.First().point.x + (-( _collider.transform.rotation * _collider.offset).x), _result.NewPos.y) - new Vector3(raycastDir.x, raycastDir.y);
        }

        private void TryMoveHorizontal(Vector3 amount)
        {
            Vector3 newPos = _result.NewPos + new Vector3(amount.x, 0);
            float numSections = _collider.bounds.size.y / GameSettings.TileSize + 1;
            bool movingRight = amount.x > 0;
            Vector3 moveDirection = movingRight ? Vector3.right : Vector3.left;
            Vector3 resultingPosition = _result.NewPos + new Vector3(amount.x, 0);

            for (int i = 0; i <= numSections; i++)
            {
                //i==0 top side corner
                //i==1 bottom side corner
                float ySectionValue = Mathf.Min(i * GameSettings.TileSize, _collider.bounds.size.y - GameSettings.CollisionOffsetValue * 2f);
                Vector2 orig = _result.NewPos +  _collider.transform.rotation * _collider.offset.ToVector3() + Vector3.up * (_collider.bounds.size.y / 2 - GameSettings.CollisionOffsetValue) + (Vector3.down * ySectionValue);

                resultingPosition = GetMoveCollisionPositionHorizontal(orig, moveDirection, amount, resultingPosition);

                if ((movingRight && newPos.x > resultingPosition.x)
                    || (!movingRight && newPos.x < resultingPosition.x))
                {
                    newPos = resultingPosition;   //Basically we ran into something so use this temppos
                }
            }

            _result.NewPos = newPos;
        }

        private Vector3 GetMoveCollisionPositionHorizontal(Vector2 orig, Vector3 direction, Vector3 moveAmount, Vector3 targetPosition)
        {
            float distance = Mathf.Abs(moveAmount.x) + GameSettings.CollisionOffsetValue + _collider.size.x / 2f;
            var hits = new List<RaycastHit2D>();
            int hitResult = Physics2D.Raycast(orig, direction, new ContactFilter2D {layerMask = _collisionMask, useLayerMask = true}, hits, distance);
            bool hitSomething = hitResult > 0;

            _logger.DebugLine(orig, orig + new Vector2(direction.x, direction.y).normalized * distance,
                hitSomething ? Color.red : Color.green);

            if (!hitSomething)
            {
                return targetPosition;
            }

            if (direction.x > 0)
                _result.HitRight = true;
            else
                _result.HitLeft = true;

            _result.HorizontalHits.AddRange(hits);

            float xCollisionPos = hits.First().point.x + (-( _collider.transform.rotation * _collider.offset).x);
            return new Vector3(xCollisionPos, targetPosition.y) - direction * (_collider.bounds.size.x / 2f);
        }

        private void TryMoveVertical(Vector3 amount)
        {
            bool movingDown = amount.y < 0;
            LayerMask collisionMask = _collisionMask;
            if (movingDown)
                collisionMask |= _oneWayMask;
            Vector3 moveVect = movingDown ? Vector3.down : Vector3.up;
            Vector3 newPos = _result.NewPos + new Vector3(0, amount.y);
            float numSections = Mathf.Max(1, _collider.bounds.size.x / GameSettings.TileSize + 1);
            Vector3 verticalCenter = GetCenterPointOfBox(moveVect);
            _logger.DebugLine(verticalCenter, verticalCenter, Color.white);

            for (int i = 0; i <= numSections; i++)
            {
                Vector3 resultingPos = _result.NewPos + new Vector3(0, amount.y);
                //i==0 left vertical corner
                //i==1 right vertical corner
                float xSectionValue = Mathf.Min(i * GameSettings.TileSize, _collider.bounds.size.x - GameSettings.CollisionOffsetValue * 2f);
                Vector2 orig = verticalCenter + Vector3.left * (_collider.bounds.size.x / 2 - GameSettings.CollisionOffsetValue) + (Vector3.right * xSectionValue);

                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                int numHits = Physics2D.Raycast(orig, moveVect,
                    new ContactFilter2D {layerMask = collisionMask, useLayerMask = true}, hits,
                    Mathf.Abs(amount.y) + GameSettings.CollisionOffsetValue);
                if (numHits > 0)
                {
                    _result.VerticalHits.AddRange(hits);
                    var hitResult = hits.FirstOrDefault();
                    var hitOneWay = _oneWayMask.ContainsLayer(hitResult.collider.transform.gameObject.layer);
                    if (hitOneWay)
                        _result.HitOneWay = true;
                    _logger.DebugLine(orig, orig + new Vector2(moveVect.x, moveVect.y).normalized * Mathf.Abs(amount.y), Color.red);

                    if (movingDown)
                        _result.HitDown = true;
                    else
                        _result.HitUp = true;
                    resultingPos = new Vector3(resultingPos.x, hitResult.point.y + (-( _collider.transform.rotation * _collider.offset).y)) - moveVect * (_collider.bounds.size.y / 2f);
                }
                else
                {
                    _logger.DebugLine(orig, orig + new Vector2(moveVect.x, moveVect.y).normalized * Mathf.Abs(amount.y), Color.green);
                }

                if ((movingDown && newPos.y < resultingPos.y)
                    || (!movingDown && newPos.y > resultingPos.y))
                    newPos = resultingPos;   //Basically we ran into something so use this temppos
            }

            _result.NewPos = newPos;
        }

        private Vector3 GetCenterPointOfBox(Vector3 side)
        {
            if (side != Vector3.left && side != Vector3.right && side != Vector3.up && side != Vector3.down)
                throw new ArgumentException("Unexpected side: " + side.ToString());

            if (Math.Abs(side.x) > .001f)
                return _result.NewPos +  _collider.transform.rotation *_collider.offset.ToVector3() + side * (_collider.bounds.size.x / 2 - GameSettings.CollisionOffsetValue);
            else
                return _result.NewPos +  _collider.transform.rotation *_collider.offset.ToVector3() + side * (_collider.bounds.size.y / 2 - GameSettings.CollisionOffsetValue);
        }
    }
}
