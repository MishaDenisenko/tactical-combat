using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.View.Abstract;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Scripts.View.Bot {
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotTankMove : TankMovement {
        [SerializeField] private GameObject botPositions;
        [SerializeField] private float distance;

        private List<BotPosition> _botPositions;
        
        private NavMeshAgent _navMeshAgent;
        private NavMeshPath _path;
        private Vector3 _destination;
        private void Start() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();

            _botPositions = BotDestination.BotPositions;
            // int id = 0;
            // foreach (var position in botPositions.transform.) {
            //     _botPositions.Add(new BotPosition(position.position, ++id));
            // }

            var player = ~(1 << 8);
            var enemy = ~(1 << 9);

            // var path = Path(_botPositions[0], _botPositions[9]);
            // // print(path);
            // print($"{_botPositions[0].Id} -> {_botPositions[9].Id}" + "{\n");
            // foreach (var position in path) {
            //     print(position.Id);
            // }
            // print("\n}");
        }

        private void Update() {


            if (_navMeshAgent.velocity != Vector3.zero) transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity);
        }

        private IEnumerator SetDestination(float time) {
            var point = _botPositions[Random.Range(0, _botPositions.Count)];

            _destination = point.Position;
            
            if (point.Activate) CheckBotPriority(point.Bot, point.Position);
            // else  point.SetPosition(this);

            // MoveToPoint();
            
            yield return new WaitForSeconds(time);
        }

        private void MoveToPoint() {
            print($"{gameObject.name} {_destination}");
            _navMeshAgent.SetDestination(_destination);
        }

        private void CheckBotPriority(BotTankMove other, Vector3 position) {
            if (other._navMeshAgent.avoidancePriority >= _navMeshAgent.avoidancePriority) {
                other._destination = other.SetNewDestination(position);
                other.MoveToPoint();
            }
            else _destination = SetNewDestination(position);
        }

        private Vector3 SetNewDestination(Vector3 position) {
            int count = 10;
            for (int i = 0; i < count; i++) {
                NavMesh.SamplePosition(Random.insideUnitSphere * 5 + position, out var navMeshHit, 50, NavMesh.AllAreas);
                var newPosition = navMeshHit.position;
                _navMeshAgent.CalculatePath(newPosition, _path);
                if (_path.status == NavMeshPathStatus.PathComplete) {
                    var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    return point.transform.position = newPosition;
                }
            }
            
            return Vector3.zero;
        }
        
        private List<BotPosition> Path(BotPosition first, BotPosition target)
        {
            List<BotPosition> visited = new ();
            List<BotPosition> path = new ();
            Queue<BotPosition> frontier = new ();
            frontier.Enqueue(first);
            
            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                print(current.Id + " - cur");
                visited.Add(current);
                if (!path.Contains(current)) path.Add(current);
                
                if (current == target)
                    return path;

                var neighbours = current.NearPoints;
                foreach(var neighbour in neighbours)
                    if (!visited.Contains(neighbour))
                        frontier.Enqueue(neighbour);
            }

            return null;
        }

    }
}