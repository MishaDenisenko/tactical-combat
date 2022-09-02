using System;
using System.Collections.Generic;
using _Scripts.Controller;
using _Scripts.View.Abstract;
using UnityEngine;

namespace _Scripts.View.Bot {
    public class BotTurretRotator : TurretRotator {
        private GameObject _player;
        private bool _onPlayer;
        [Obsolete] private float _ricochetCount;
        [Obsolete] List<Ray> _rays = new();
        [Obsolete] List<Vector3[]> _lines = new();

        private Transform _rotator;
        private int _ignoreMask;
        private BotController _botController;
        private Coroutine _coroutine;
        private Vector3 _dir;
        private Ray _distanceRay = new ();

        private Transform _lead;
        private Rigidbody _targetRigidBody;

        public TankController TankController { set; get; }

        private readonly int[][] _angels =
        {
            new[] {2, 45},
            new[] {-45, -2},
            new[] {45, 90},
            new[] {-90, -45},
            new[] {90, 270},
        };

        private void Start() {
            _botController = new BotController(TankController);
            _player = GameObject.FindWithTag("Player");
            // _ricochetCount = BulletSpecifications.CountOfRicochets;
            _rotator = transform.GetChild(1);
            _ignoreMask = ~ LayerMask.GetMask("Enemy", "Player", "EnemyBullet", "PlayerBullet");
            _coroutine = StartCoroutine(_botController.GetTurretActionType());

            _lead = _player.transform.GetChild(2);
            _targetRigidBody = _player.GetComponent<Rigidbody>();
            // FindPlayer();
        }

        private void Update() {
            if (_botController.TurretActionType == BotController.ETurretActionType.Aim) {
                if (_targetRigidBody.velocity == Vector3.zero) {
                    if (!_onPlayer) {
                        _dir = FindPlayer();
                        _onPlayer = false;
                    }
                }
                else _dir = FindPlayer();
                
                if (!_dir.Equals(Vector3.negativeInfinity)) {
                    Rotate(_dir, Time.deltaTime);
                    if (Equals(transform.rotation, Quaternion.LookRotation(_dir), 0.05f)) gunShoot.Shoot();
                }

            }
            else _dir = Vector3.negativeInfinity;
        }

        private Vector3 FindPlayer() {
            Transform tr = _rotator;
            _distanceRay.origin = tr.position;
            _distanceRay.direction = CalculateAngel(0);

            if (Physics.Raycast(_distanceRay, out var hit, 100, _ignoreMask) && hit.collider.CompareTag("Player")) {
                return _distanceRay.direction;
            }

            for (int i = -1; i < 2; i += 2) {
                _distanceRay.direction = CalculateAngel(i);
                if (Physics.Raycast(_distanceRay, out hit, 100) && hit.collider.CompareTag("Player")) {
                    return _distanceRay.direction;
                }
            }


            foreach (var angel in _angels) {
                for (int i = angel[0]; i < angel[1]; i++) {
                    _distanceRay.origin = _rotator.position;
                    var dir = _distanceRay.direction = CalculateAngel(i);
                    
                    if (CreateSecondaryRay()) return dir;
                }
            }

            return Vector3.negativeInfinity;
        }

        private bool CreateSecondaryRay() {
            if (Physics.Raycast(_distanceRay, out var hit, 100, _ignoreMask)) {
                return CreateSecondaryRay(hit.point, Vector3.Reflect(_distanceRay.direction, hit.normal));
            }

            return false;
        }

        private bool CreateSecondaryRay(Vector3 position, Vector3 direction) {
            _distanceRay.origin = position;
            _distanceRay.direction = direction;
            if (Physics.Raycast(_distanceRay, out var hit, 100, _ignoreMask)) {
                return hit.collider.CompareTag("Player");
            }

            return false;
        }

        private Vector3 CalculateLead() {
            /*
             * x0 и y0 - координаты объекта в этой "сдвинутой системе",
             * Vx и Vy - проекции вектора скорости объекта на оси. (Vx^2 + Vy2 = Vo^2)
             *
             * t^2 * (Vо^2 - Vп^2) + 2*t*(x0*Vx + y0*Vy) + (x0^2 + yo^2) = 0
             * 
             */

            var targetPos = _player.transform.GetChild(1).position;
            var targetVelocity = _targetRigidBody.velocity;
            float time;

            var scalarTarget = MathF.Pow(targetVelocity.x, 2) + MathF.Pow(targetVelocity.z, 2);
            var scalarBullet = 2 * MathF.Pow(BulletSpecifications.Velocity, 2);
            var a = scalarTarget - scalarBullet;
            var b = 2 * (targetPos.x * targetVelocity.x + targetPos.z * targetVelocity.z);
            var c = MathF.Pow(targetPos.x, 2) + MathF.Pow(targetPos.z, 2);

            var D = MathF.Pow(b, 2) - 4 * a * c;

            if (D < 0) return targetPos;
            
            if (D == 0) time = -b / (2 * a);
            else {
                var x1 = (-b + MathF.Sqrt(D)) / (2 * a);
                var x2 = (-b - MathF.Sqrt(D)) / (2 * a);

                time = x1 > 0 ? x1 : x2;
            }

            return new Vector3(targetPos.x + targetVelocity.x * time, targetPos.y, targetPos.z + targetVelocity.z * time);
        }

        private Vector3 CalculateAngel(int angel) {
            angel = angel < 0 ? 360 + angel : angel;
            var lead = CalculateLead();
            // print(lead);
            _lead.position = lead;
            _rotator.rotation = Quaternion.LookRotation(lead - transform.position);

            Vector3 frd = _rotator.forward;
            Vector3 rgh = _rotator.right;

            float angelRad = angel * Mathf.Deg2Rad;

            if (angel == 0) return Normalize(frd);
            if (angel == 90) return Normalize(rgh);
            if (angel == 180) return Normalize(-frd);
            if (angel == 270) return Normalize(-rgh);

            return new Vector3(
                frd.x * MathF.Cos(angelRad) - frd.z * MathF.Sin(angelRad),
                0,
                frd.x * MathF.Sin(angelRad) + frd.z * MathF.Cos(angelRad)
            );
        }

        private Vector3 Normalize(Vector3 dir) {
            return new Vector3(dir.x, 0, dir.z);
        }

        private void GetCloserToCenter(Vector3[] targetPositions) { }

        private bool Equals(Quaternion self, Quaternion other, float delta) {
            return Quaternion.Angle(self, other) < delta;
        }
    }
}