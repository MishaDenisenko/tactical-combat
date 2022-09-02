using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.View.Bot {
    public class BotDestination : MonoBehaviour {
        [SerializeField] private float distance;
        private static readonly List<BotPosition> _botPositions = new();

        private readonly Vector3[] _directions =
        {
            new(1, 0, 0),
            new(-1, 0, 0),
            new(0, 0, 1),
            new(0, 0, -1),
            new(1, 0, 1),
            new(-1, 0, 1),
            new(1, 0, -1),
            new(-1, 0, -1),
        };


        private static readonly List<Node> AllNodes = new();
        private static readonly int _horizontalStep = 10;
        private static readonly int _verticalStep = 10;

        private GameObject _player;
        private int _ignoreMask;
        private bool _firstCheck = true;
        private Rigidbody _rigidbody;

        private void Awake() {
            for (int i = 0; i < transform.childCount; i++) {
                var point = transform.GetChild(i);
                
                point.gameObject.SetActive(true);
                _botPositions.Add(new BotPosition(point.position, i + 1));
            }

            foreach (var point in _botPositions) {
                foreach (var direction in _directions) {
                    Physics.Raycast(point.Position, direction, out var hit, distance);
                    if (hit.collider && hit.collider.CompareTag("Point")) {
                        var other = Int32.Parse(hit.collider.name.Split(" ")[1]) - 1;
                        point.AddNearest(_botPositions[other]);
                        _botPositions[other].AddNearest(point);
                    }
                }
            }
        }

        private void Start() {
            _player = GameObject.FindWithTag("Player");
            _rigidbody = _player.GetComponent<Rigidbody>();
            // _ignoreMask = ~(1 << _player.layer);
            _ignoreMask = ~ LayerMask.GetMask("Enemy", "Player", "EnemyBullet", "PlayerBullet");
            // _ignoreLayer = ~(1 << 13);
            // var pos = new Vector3(_player.position.x, 0.6f, _player.position.z);
            // foreach (var point in _botPositions) {
            //     if (Physics.Raycast(_player.position, point.Position - pos, out var hit)) {
            //         Debug.DrawRay(pos, hit.distance * (point.Position - pos), Color.blue, 1);
            //     }
            // }
            var c = StartCoroutine(CheckOpenness(2));

            foreach (var position in _botPositions) {
                AllNodes.Add(new Node(position.Id)
                {
                    Openness = position.Openness,
                    Position = position.Position
                });
            }

            for (int i = 0; i < _botPositions.Count; i++) {
                var neighbours = _botPositions[i].NearPoints;
                foreach (var neighbour in neighbours) {
                    AllNodes[i].Neighbours.Add(GetNodeById(neighbour.Id));
                }
            }

            var path = GetPath(1, 32);
            print("1 -> 33 {");
            foreach (var node in path) {
                print(node);
            }
            
            print("}");
        }

        private void Update() { }

        private IEnumerator CheckOpenness(float seconds) {
            var pos = _player.transform.GetChild(1).position;

            if  (_rigidbody.velocity != Vector3.zero || _firstCheck) {
                int i = 0;
                foreach (var point in _botPositions) {
                    var pointGo = transform.GetChild(i++).gameObject;
                    pointGo.SetActive(true);
                    var dir = point.Position - pos;
                    var dist = Vector3.Distance(pos, point.Position);
                    
                    Physics.Raycast(pos, dir, out var hit, dist, _ignoreMask);
                    Debug.DrawLine(pos, hit.point, Color.blue, 100);
                    if (hit.collider.CompareTag("Point")) {
                        var id = Int32.Parse(hit.collider.name.Split(" ")[1]);

                        if (id == point.Id && dist > distance * 2) point.Openness = BotPosition.EOpenness.MiddleOpen;
                        else if (id == point.Id && dist <= distance * 2) point.Openness = BotPosition.EOpenness.Open;
                    }
                    else point.Openness = BotPosition.EOpenness.Safety;
                    
                    // print($"{point.Id} {point.Openness}");

                    pointGo.SetActive(false);
                }

                if (_firstCheck) _firstCheck = false;
                // foreach (var position in _botPositions) {
                //     print($"{position.Id} : {position.Openness}");
                // }

                yield return new WaitForSeconds(seconds);
            }
        }

        public static List<BotPosition> BotPositions => _botPositions;

        private BotPosition GetPointById(int id) {
            foreach (var position in _botPositions) {
                if (position.Id == id) return position;
            }

            return default;
        }

        private static Node GetNodeById(int id) {
            foreach (var node in AllNodes) {
                if (node.ID == id) return node;
            }

            return default;
        }


        private static List<Node> GetPath(int start, int end) {
            if (start == end) return default;

            List<Node> closedList = new();
            List<Node> openList = new();
            List<Node> path = new();

            var startPoint = GetNodeById(start);
            var endPoint = GetNodeById(end);
            var currentPoint = startPoint;
            currentPoint.Weight = 0;

            closedList.Add(currentPoint);
            currentPoint.IsVisited = true;

            while (currentPoint != endPoint) {
                print(currentPoint + " - curr");
                foreach (var neighbour in currentPoint.Neighbours) {
                    if (neighbour == endPoint) {
                        endPoint.Previous = currentPoint;
                        currentPoint = endPoint;
                        break;
                    }

                    if (!neighbour.HasUnvisitedNeighbours()) {
                        closedList.Add(neighbour);
                        continue;
                    }

                    if (!closedList.Contains(neighbour)) {
                        if (!openList.Contains(neighbour)) openList.Add(neighbour);

                        if (Math.Abs(neighbour.Position.x - currentPoint.Position.x) > 0.1 && Math.Abs(neighbour.Position.z - currentPoint.Position.z) > 0.1) neighbour.Dist += 7;
                        else neighbour.Dist += 10;


                        var x = MathF.Abs(endPoint.Position.x - neighbour.Position.x);
                        var z = MathF.Abs(endPoint.Position.z - neighbour.Position.z);

                        neighbour.EuclideanApproximation = x + z;

                        var weight = neighbour.Dist * (int) neighbour.Openness + neighbour.EuclideanApproximation;

                        if (weight < neighbour.Weight) {
                            neighbour.Weight = weight;
                            neighbour.Previous = currentPoint;
                        }
                    }

                    print(neighbour + " - n");
                }

                if (!closedList.Contains(currentPoint)) {
                    currentPoint.IsVisited = true;
                    closedList.Add(currentPoint);
                }

                if (currentPoint != endPoint) {
                    var nearest = GetMinWeightNode(currentPoint.Neighbours, ref closedList);
                    currentPoint = nearest ?? GetMinWeightNode(openList, ref closedList);

                    if (openList.Contains(currentPoint)) openList.Remove(currentPoint);
                }
            }

            // var lastPoint = currentPoint;
            path.Add(currentPoint);
            do {
                var previous = currentPoint.Previous;
                if (!path.Contains(previous)) path.Add(previous);
                currentPoint = previous;
            } while (currentPoint != startPoint);


            return path;
        }

        private static Node GetMinWeightNode(List<Node> neighbours, ref List<Node> closedList) {
            Node nearest = null;

            foreach (var neighbour in neighbours) {
                if (!closedList.Contains(neighbour)) {
                    nearest = neighbour;
                    break;
                }
            }

            if (nearest == null) return default;

            foreach (var neighbour in neighbours) {
                if (!closedList.Contains(neighbour) && neighbour.Weight < nearest.Weight) nearest = neighbour;
            }

            return nearest;
        }

        private class Node {
            public Node(int id, Node previous) {
                ID = id;
                Previous = previous;
            }

            public Node(int id) {
                ID = id;
            }

            public int ID { get; }

            public Vector3 Position { get; set; }

            public Node Previous { get; set; }

            public List<Node> Neighbours { get; set; } = new();

            public float Weight { get; set; } = Single.MaxValue;

            public float Dist { get; set; }

            public float EuclideanApproximation { get; set; }

            public BotPosition.EOpenness Openness { get; set; }

            public bool IsVisited { get; set; }

            public bool HasUnvisitedNeighbours() {
                foreach (var neighbour in Neighbours) {
                    if (!neighbour.IsVisited) return true;
                }

                return false;
            }

            public override string ToString() {
                return $"id - {ID}; prev - {Previous?.ID ?? 0}; w - {Weight}; d - {Dist}; e - {EuclideanApproximation}; open - {Openness}";
            }
        }
    }
}