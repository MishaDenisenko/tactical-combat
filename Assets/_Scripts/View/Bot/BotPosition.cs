using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.View.Bot {
    public class BotPosition {
        private readonly List<BotPosition> _nearPoints = new(); 
        public EOpenness Openness { get; set; }
        
        public enum EOpenness {
            Safety = 1,
            MiddleOpen = 2,
            Open = 3
        }
        
        public int Id { get; private set; }

        public BotPosition(Vector3 position, int id) {
            Position = position;
            Id = id;
        }

        public bool Activate { get; }

        public BotTankMove Bot { get; }
        
        public Vector3 Position { get; set; }

        public List<BotPosition> NearPoints => _nearPoints;

        public bool AddNearest(BotPosition point) {
            if (_nearPoints.Contains(point)) return false;
            
            _nearPoints.Add(point);
            return true;
        }
    }
}