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
        private int _ignoreLayer;
        private BotController _botController;
        private Coroutine _coroutine;
        private Vector3 _dir;
        private Ray _distanceRay;

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
            _ignoreLayer = ~(1 << gameObject.layer);
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
                print(!_dir.Equals(Vector3.negativeInfinity));
                if (!_dir.Equals(Vector3.negativeInfinity)) {
                    Rotate(_dir, Time.deltaTime);
                    // print($"{Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_dir))}");
                    if (Equals(transform.rotation, Quaternion.LookRotation(_dir), 0.05f)) gunShoot.Shoot();
                }

            }
            else _dir = Vector3.negativeInfinity;
            
        }

        private Vector3 FindPlayer() {
            Transform tr = _rotator;
            var ray = new Ray(tr.position, CalculateAngel(0));

            if (Physics.Raycast(ray, out var hit, 100, _ignoreLayer) && hit.collider.CompareTag("Player")) {
                // transform.rotation = _rotator.rotation;
                print(0);
                return ray.direction;
            }

            for (int i = -1; i < 2; i += 2) {
                ray.direction = CalculateAngel(i);
                if (Physics.Raycast(ray, out hit, 100) && hit.collider.CompareTag("Player")) {
                    // transform.rotation = _rotator.rotation;
                    print(i);
                    return ray.direction;
                }
            }


            foreach (var angel in _angels) {
                for (int i = angel[0]; i < angel[1]; i++) {
                    ray.origin = _rotator.position;
                    var dir = ray.direction = CalculateAngel(i);
                    if (CreateSecondaryRay(ref ray)) {
                        print(i + "'");
                        // transform.rotation = Quaternion.LookRotation(dir);
                        return dir;
                    }
                }
            }

            // Debug.DrawRay(_rotator.position, CalculateAngel(0)*100, Color.yellow, 1000);
            //
            // Debug.DrawRay(_rotator.position, CalculateAngel(180)*100, Color.blue, 1000);
            // Debug.DrawRay(_rotator.position, CalculateAngel(179)*100, Color.red, 1000);
            // Debug.DrawRay(_rotator.position, CalculateAngel(181)*100, Color.green, 1000);
            //
            // Debug.DrawRay(_rotator.position, CalculateAngel(90)*100, Color.blue, 1000);
            // Debug.DrawRay(_rotator.position, CalculateAngel(89)*100, Color.red, 1000);
            // Debug.DrawRay(_rotator.position, CalculateAngel(91)*100, Color.green, 1000);
            //
            // Debug.DrawRay(_rotator.position, CalculateAngel(-90)*100, Color.blue, 1000);
            // Debug.DrawRay(_rotator.position, CalculateAngel(-89)*100, Color.red, 1000);
            // Debug.DrawRay(_rotator.position, CalculateAngel(-91)*100, Color.green, 1000);

            // for (int i = 2; i < 45; i++) {
            //     ray.origin = _rotator.position;
            //     var dir = ray.direction = CalculateAngel(i);
            //     if (CreateSecondaryRay(ref ray)) {
            //         transform.rotation = Quaternion.LookRotation(dir);
            //         print("2+45");
            //         return;
            //     }
            // }
            //
            // for (int i = -2; i > -45; i--) {
            //     ray.origin = _rotator.position;
            //     var dir = ray.direction = CalculateAngel(i);
            //     if (CreateSecondaryRay(ref ray)) {
            //         transform.rotation = Quaternion.LookRotation(dir);
            //         print("-2-45");
            //         return;
            //     }
            // }
            //
            // // foreach (var coords in _lines) {
            // //     Debug.DrawLine(coords[0], coords[1], Color.green, 1000);
            // // }
            // // _lines.Clear();
            //
            // for (int i = -45; i > -90; i--) {
            //     ray.origin = _rotator.position;
            //     var dir = ray.direction = CalculateAngel(i);
            //     if (CreateSecondaryRay(ref ray)) {
            //         print("-45-90");
            //         transform.rotation = Quaternion.LookRotation(dir);
            //         return;
            //     }
            // }
            //
            // for (int i = 45; i < 90; i++) {
            //     ray.origin = _rotator.position;
            //     var dir = ray.direction = CalculateAngel(i);
            //     if (CreateSecondaryRay(ref ray)) {
            //         print("45+90");
            //         transform.rotation = Quaternion.LookRotation(dir);
            //         return;
            //     }
            // }
            //
            // for (int i = 90; i < 270; i++) {
            //     ray.origin = _rotator.position;
            //     var dir = ray.direction = CalculateAngel(i);
            //     if (CreateSecondaryRay(ref ray)) {
            //         print("90+270");
            //         transform.rotation = Quaternion.LookRotation(dir);
            //         return;
            //     }
            // }

            // for (int i = -1; i < 0; i++) {
            //     // var dir = Quaternion.LookRotation(_player.transform.position).eulerAngles;
            //     ray.direction = CalculateAngel(i);
            //     _rays.Add(new Ray(ray.origin, ray.direction));
            //     if (Physics.Raycast(ray, out var hit, 100)) {
            //         if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Rock")) {
            //             _lines.Add(new []{transform.position, hit.point});
            //             var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //             sphere.transform.position = hit.point;
            //             sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            //             // CreateSecondaryRay(ref ray, hit.point, Vector3.Reflect(ray.direction, hit.normal));
            //             // _ricochetCount = BulletSpecifications.CountOfRicochets;
            //         }
            //     } 
            // }
            // var dir1 = CalculateAngel(1);
            // var dir2 = CalculateAngel(0);
            // var dir3 = CalculateAngel(-1);
            // print($"{dir1}, {dir2}, {dir3}");
            // Debug.DrawRay(_rotator.position, dir1*5000, Color.cyan, 1000);
            // Debug.DrawRay(_rotator.position, dir2*5000, Color.cyan, 1000);
            // Debug.DrawRay(_rotator.position, dir3*5000, Color.cyan, 1000);

            // foreach (var coords in _lines) {
            //     Debug.DrawLine(coords[0], coords[1], Color.green, 1000);
            // }
            //
            // _lines.Clear();

            // for (int i = 0; i < 360; i++) {
            //     ray.direction = CalculateAngel(i);
            //     CreateSecondaryRay(ref ray);
            // }

            // foreach (var coords in _lines) {
            //     Debug.DrawLine(coords[0], coords[1], Color.magenta, 1000);
            // }
            //
            // print(_lines.Count);

            return Vector3.negativeInfinity;
        }

        // private void CreateSecondaryRay(ref Ray ray, Vector3 position, Vector3 direction) {
        //     ray.origin = position;
        //     ray.direction = direction;
        //     if (Physics.Raycast(ray, out var hit, 100)) {
        //         // if (hit.collider.CompareTag("Player")) return;
        //         if (hit.collider.CompareTag("Wall")) {
        //             _lines.Add(new []{position, hit.point});
        //             var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //             sphere.transform.position = hit.point;
        //             sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //             if (--_ricochetCount > 0) CreateSecondaryRay(ref ray, hit.point, Vector3.Reflect(ray.direction, hit.normal));
        //         }
        //     } 
        //     _rays.Add(new Ray(ray.origin, ray.direction));
        //      // CreateSecondaryRay(ref ray, hit.point, Vector3.Reflect(ray.direction, hit.normal));
        // }

        private bool CreateSecondaryRay(ref Ray ray) {
            if (Physics.Raycast(ray, out var hit, 100, _ignoreLayer)) {
                // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.magenta, 1000);
                return CreateSecondaryRay(ref ray, hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }

            return false;
        }

        private bool CreateSecondaryRay(ref Ray ray, Vector3 position, Vector3 direction) {
            ray.origin = position;
            ray.direction = direction;
            if (Physics.Raycast(ray, out var hit, 100, _ignoreLayer)) {
                // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 1000);
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
            float time = 0;

            var scalarTarget = MathF.Pow(targetVelocity.x, 2) + MathF.Pow(targetVelocity.z, 2);
            var scalarBullet = 2 * MathF.Pow(BulletSpecifications.Velocity, 2);
            var a = scalarTarget - scalarBullet;
            var b = 2 * (targetPos.x * targetVelocity.x + targetPos.z * targetVelocity.z);
            var c = MathF.Pow(targetPos.x, 2) + MathF.Pow(targetPos.z, 2);

            var D = MathF.Pow(b, 2) - 4 * a * c;

            if (D < 0) return targetPos;

            // print($"a {a}, b {b}, c {c}, {sqrtD}");
            if (D == 0) time = -b / (2 * a);
            else {
                var x1 = (-b + MathF.Sqrt(D)) / (2 * a);
                var x2 = (-b - MathF.Sqrt(D)) / (2 * a);

                time = x1 > 0 ? x1 : x2;
            }

            return new Vector3(targetPos.x + targetVelocity.x * time, targetPos.y, targetPos.z + targetVelocity.z * time);
        }

        private Vector3 CalculateLead(bool q) {
            //узнаем скорость го
            var veloSityTarget = _targetRigidBody.velocity;

            var xSpeedTarget = _targetRigidBody.velocity.x;
            var ySpeedTarget = _targetRigidBody.velocity.y;
            var zSpeedTarget = _targetRigidBody.velocity.z;

            //если скорость цели будет больше нуля
            if (xSpeedTarget != 0 || ySpeedTarget != 0 || zSpeedTarget != 0) {
                //рассчитываем где будет цель через 1 сек
                var xTargetPoz2 = _targetRigidBody.transform.position.x + veloSityTarget.x;
                var yTargetPoz2 = _targetRigidBody.transform.position.y + veloSityTarget.y;
                var zTargetPoz2 = _targetRigidBody.transform.position.z + veloSityTarget.z;

                //считаем расстояние пройденное целью BC
                var ВС_1 = Vector3.Distance(_targetRigidBody.transform.position, new Vector3(xTargetPoz2, yTargetPoz2, zTargetPoz2));

                //считаем скорость ракеты
                var veloSityRocket = transform.TransformDirection(Vector3.forward) * BulletSpecifications.Velocity;

                //рассчитываем где будет ракета через 1 сек
                var xRocketPoz2 = this.transform.position.x + veloSityRocket.x;
                var yRocketPoz2 = this.transform.position.y + veloSityRocket.y;
                var zRocketPoz2 = this.transform.position.z + veloSityRocket.z;

                //считаем пройденное расстояние ракеты AC
                var AC_1 = BulletSpecifications.Velocity; //Vector3.Distance(this.transform.position, new Vector3(xRocketPoz2, yRocketPoz2, zRocketPoz2));

                //находим вектор направления на цель
                var vector3DirectionRocket = _targetRigidBody.transform.position - this.transform.position;

                //Debug.Log(targetRigidBody.velocity);

                //находим скалярное произведение векторов
                var vector3Scal = vector3DirectionRocket.x * _targetRigidBody.velocity.x + vector3DirectionRocket.y * _targetRigidBody.velocity.y
                                                                                         + vector3DirectionRocket.z * _targetRigidBody.velocity.z;

                //находим модуль векторов (результат в квадратном корне)
                var lengthVector1 = vector3DirectionRocket.x * vector3DirectionRocket.x + vector3DirectionRocket.y * vector3DirectionRocket.y
                                                                                        + vector3DirectionRocket.z * vector3DirectionRocket.z;

                var lengthVector2 = _targetRigidBody.velocity.x * _targetRigidBody.velocity.x
                                    + _targetRigidBody.velocity.y * _targetRigidBody.velocity.y
                                    + _targetRigidBody.velocity.z * _targetRigidBody.velocity.z;

                //находим угол между векторами (результат в косинусе!)
                var cosB = vector3Scal / Mathf.Sqrt(lengthVector1 * lengthVector2);

                if (cosB < 0) {
                    cosB *= -1;
                }

                //находим радианы из косинуса
                var rad = Mathf.Acos(cosB);

                //переводим угол B из радиан в градусы
                var angleB = rad * 180 / Mathf.PI;

                //выводим синус угла В
                var sinB = Mathf.Sin(rad);

                //высчитываем синус угла А в радианах
                var sinA = ВС_1 * sinB / AC_1;

                //выводим из синуса угол А в радианы
                rad = Mathf.Asin(sinA);
                //переводим угол А из радиан в градусы
                var angleA = rad * 180 / Mathf.PI;

                if (angleA < 0)
                    angleA *= -1;
                if (angleB < 0)
                    angleB *= -1;

                //Считаем угол С
                var angleC = 180 - (angleA + angleB);

                //переводим градусы в радианы
                var angleRad = angleC * Mathf.PI / 180;

                //выводим синус угла С
                var sinC = Mathf.Sin(angleRad);

                //переводим угол C в радианы
                var advanceAngleRad = angleC * Mathf.PI / 180;

                //считаем расстояние до цели (АВ)
                var AB_2 = Vector3.Distance(_targetRigidBody.transform.position, this.transform.position);

                //distTargretRocet = (targetRigidBody.transform.position - this.transform.position).magnitude;

                //вычисляем длину пути ВС до точки упреждения BC = AB * sinA / sinC
                var BC_2 = AB_2 * sinA / Mathf.Sin(advanceAngleRad);

                //считаем время подлета цели к точке

                float timeAdvanceX, timeAdvanceY, timeAdvanceZ;
                //вычисляем время полета цели
                if (_targetRigidBody.velocity.x != 0)
                    timeAdvanceX = BC_2 / _targetRigidBody.velocity.x;
                else timeAdvanceX = 0;

                if (_targetRigidBody.velocity.y != 0)
                    timeAdvanceY = BC_2 / _targetRigidBody.velocity.y;
                else timeAdvanceY = 0;

                if (_targetRigidBody.velocity.z != 0)
                    timeAdvanceZ = BC_2 / _targetRigidBody.velocity.z;
                else timeAdvanceZ = 0;

                //время не может быть отрицательным
                if (timeAdvanceX < 0)
                    timeAdvanceX *= -1;
                if (timeAdvanceY < 0)
                    timeAdvanceY *= -1;
                if (timeAdvanceZ < 0)
                    timeAdvanceZ *= -1;

                //вычисляем vector3 точки упреждения в локальных координатах
                var wayX = timeAdvanceX * _targetRigidBody.velocity.x;
                var wayY = timeAdvanceY * _targetRigidBody.velocity.y;
                var wayZ = timeAdvanceZ * _targetRigidBody.velocity.z;

                return _targetRigidBody.transform.position + new Vector3(wayX, wayY, wayZ);
            }

            return _targetRigidBody.transform.position;
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