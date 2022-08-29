using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.View.Bot {
    public class BotDestination : MonoBehaviour{
        [SerializeField] private List<Transform> botPositions;
        public static List<BotPosition> BotPositions { get; set; } = new();

        private void Start() {
            SetPositions(botPositions);
        }

        public static void SetPositions(List<Transform> positions) {
            foreach (var position in positions) {
                BotPositions.Add(new BotPosition(position.position));
            }
        }

    }
}