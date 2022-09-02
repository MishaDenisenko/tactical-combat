using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace _Scripts.Controller {
    public class BotController {
        private readonly TankController _tankController;
        private EBodyActionType _bodyActionType;
        private ETurretActionType _turretActionType;

        public EBodyActionType BodyActionType => _bodyActionType;

        public ETurretActionType TurretActionType => _turretActionType;
        
        public enum EBodyActionType {
            Move,
            None
        }
        public enum ETurretActionType {
            Aim,
            None
        }

        public BotController(TankController tankController) {
            _tankController = tankController;
        }

        public IEnumerator GetBodyActionType() {
            _bodyActionType = EBodyActionType.None;

            yield return null;
            _bodyActionType = EBodyActionType.Move;
            
        }
        public IEnumerator GetTurretActionType() {
            while (_tankController.IsAlive()) {
                yield return new WaitForSeconds(1.5f);
                var type = new Random().Next(1, 10);
                switch (type) {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        _turretActionType = ETurretActionType.None;
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        _turretActionType = ETurretActionType.Aim;
                        break;
                    default:
                        _turretActionType = ETurretActionType.Aim;
                        break;
                }
                
            }
        }
    }
    
}