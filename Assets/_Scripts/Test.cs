// using UnityEngine;
//
// namespace _Scripts {
//     public class Test : MonoBehaviour{
//         public int speed = 1;
//
//         public bool boolTest;
//         public bool boolTest2;
//
//         public GameObject snaryd;
//         public GameObject cloneSnaryd;
//
//         public GameObject advanceGO;
//
//         public Vector3 target2;
//
//         public Rigidbody targetRigidBody;
//         public GameObject target;
//
//         public Vector3 veloSityTarget;
//
//         public float xSpeedTarget = 0;
//         public float ySpeedTarget = 0;
//         public float zSpeedTarget = 0;
//
//         public float xTargetPoz2 = 0;
//         public float yTargetPoz2 = 0;
//         public float zTargetPoz2 = 0;
//
//         public Vector3 veloSityRocket;
//
//         public float xRocketPoz2 = 0;
//         public float yRocketPoz2 = 0;
//         public float zRocketPoz2 = 0;
//
//         public float AC_1;
//         public float ВС_1;
//
//         public Vector3 vector3DirectionRocket;
//         public float vector3Scal;
//         public float lengthVector1;
//         public float lengthVector2;
//
//         public float angleRad;
//
//         public float sinA;
//         public float sinB;
//         public float sinC;
//
//         public float cosB;
//
//         public float rad;
//         public float advanceAngleRad;
//
//         public float AB_2;
//         public float BC_2;
//
//         public float timeAdvanceX;
//         public float timeAdvanceY;
//         public float timeAdvanceZ;
//
//         public float wayX;
//         public float wayY;
//         public float wayZ;
//
//
//         //углы треугольника
//         public float angleA;
//         public float angleB;
//         public float angleC;
//
//         //время подлета цели к точке упреждения и контрольное время
//         public float time;
//         public float timeControl;
//         public bool time2Bool = true;
//
//         //точка упреждения
//         public Vector3 advanceTarget;
//
//         public float test;
//         public Vector3 testVector3;
//
//
//         /*
//          * Скорость в Юнити задается в м/с, или юнит/с, т.е. за 1 секунду, ГО пролетает 1 юнит * скорость.
//          *
//          * Считаем точку упреждения по формулам:
//          * Находим вектор направления на цель - из координат цели вычитаем координаты пушки (или что там у вас)):
//          * Находим угол между векторами: cos α = a·b / |a|·|b|; где а - вектор направления на цель, b - вектор скорости цели (velocity)
//          * Находим синус угла А в радианах: SinA = BC_1 * SinB / AC_1; где ВС_1 путь цели за 1 сек, АС_1 путь снаряда (или что там у вас летит в цель) за 1 сек
//          *
//          * Угол А, это и есть угол упреждения на цель.
//          *
//          * Находим угол С: C = 180 - (A + B);
//          * Находим расстояние которое пройдет цель за то время, какое потребуется снаряду (или что там у вас) долететь до точки перехвата:
//          * Или проще говоря высчитываем точку перехвата на отрезке ВС_1 : BC_2 = AB_2 * SinA / SinC; где АВ расстояние до цели от пушки
//          * Находим координаты точки перехвата:
//          * t.x = BC_2 * v.x; t.y = BC_2 * v.y; t.z = BC_2 * v.z;
//          * x = t.x * v.x; y = t.y * v.y; z = t.z * v.z;
//          *
//          * точка перехвата = текущая позиция цели(x,y,z) + (x,y,z);
//          */
//
//
//         void Start() {
//             target = GameObject.Find("Cube Test2");
//         }
//
//
//         void Update() {
//             time += Time.deltaTime;
//
//             //смотрим в точку упреждения
//             if (advanceGO != null)
//                 advanceGO.transform.LookAt(advanceTarget);
//
//
//             Advance();
//
//             if (Input.GetKeyDown(KeyCode.B)) {
//                 Advance();
//
//                 boolTest = !boolTest;
//
//                 boolTest2 = true;
//             }
//
//             if (boolTest2) {
//                 boolTest2 = false;
//             }
//
//             if (boolTest) {
//                 if (snaryd != null) {
//                     cloneSnaryd = Instantiate(snaryd, advanceGO.transform.position, advanceGO.transform.rotation) as GameObject;
//
//                     cloneSnaryd.rigidbody.velocity = advanceGO.transform.TransformDirection((Vector3.forward) * speed);
//                 }
//             }
//         }
//
//         void Advance() {
//             //targetRigidBody = target.GetComponent<Rigidbody>();
//
//             if (targetRigidBody != null) {
//                 //узнаем скорость го
//                 veloSityTarget = targetRigidBody.velocity;
//
//                 xSpeedTarget = targetRigidBody.velocity.x;
//                 ySpeedTarget = targetRigidBody.velocity.y;
//                 zSpeedTarget = targetRigidBody.velocity.z;
//
//                 //если скорость цели будет больше нуля
//                 if (xSpeedTarget != 0 || ySpeedTarget != 0 || zSpeedTarget != 0) {
//                     //рассчитываем где будет цель через 1 сек
//                     xTargetPoz2 = targetRigidBody.transform.position.x + veloSityTarget.x;
//                     yTargetPoz2 = targetRigidBody.transform.position.y + veloSityTarget.y;
//                     zTargetPoz2 = targetRigidBody.transform.position.z + veloSityTarget.z;
//
//                     //считаем расстояние пройденное целью BC
//                     ВС_1 = Vector3.Distance(targetRigidBody.transform.position, new Vector3(xTargetPoz2, yTargetPoz2, zTargetPoz2));
//
//                     //считаем скорость ракеты
//                     veloSityRocket = transform.TransformDirection(Vector3.forward) * speed;
//
//                     //рассчитываем где будет ракета через 1 сек
//                     xRocketPoz2 = this.transform.position.x + veloSityRocket.x;
//                     yRocketPoz2 = this.transform.position.y + veloSityRocket.y;
//                     zRocketPoz2 = this.transform.position.z + veloSityRocket.z;
//
//                     //считаем пройденное расстояние ракеты AC
//                     AC_1 = speed; //Vector3.Distance(this.transform.position, new Vector3(xRocketPoz2, yRocketPoz2, zRocketPoz2));
//
//                     //находим вектор направления на цель
//                     vector3DirectionRocket = targetRigidBody.transform.position - this.transform.position;
//
//                     //Debug.Log(targetRigidBody.velocity);
//
//                     //находим скалярное произведение векторов
//                     vector3Scal = vector3DirectionRocket.x * targetRigidBody.velocity.x + vector3DirectionRocket.y * targetRigidBody.velocity.y
//                                                                                         + vector3DirectionRocket.z * targetRigidBody.velocity.z;
//
//                     //находим модуль векторов (результат в квадратном корне)
//                     lengthVector1 = vector3DirectionRocket.x * vector3DirectionRocket.x + vector3DirectionRocket.y * vector3DirectionRocket.y
//                                                                                         + vector3DirectionRocket.z * vector3DirectionRocket.z;
//
//                     lengthVector2 = targetRigidBody.velocity.x * targetRigidBody.velocity.x
//                                     + targetRigidBody.velocity.y * targetRigidBody.velocity.y
//                                     + targetRigidBody.velocity.z * targetRigidBody.velocity.z;
//
//                     //находим угол между векторами (результат в косинусе!)
//                     cosB = vector3Scal / Mathf.Sqrt(lengthVector1 * lengthVector2);
//
//                     if (cosB < 0) {
//                         cosB *= -1;
//                     }
//
//                     //находим радианы из косинуса
//                     rad = Mathf.Acos(cosB);
//
//                     //переводим угол B из радиан в градусы
//                     angleB = rad * 180 / Mathf.PI;
//
//                     //выводим синус угла В
//                     sinB = Mathf.Sin(rad);
//
//                     //высчитываем синус угла А в радианах
//                     sinA = ВС_1 * sinB / AC_1;
//
//                     //выводим из синуса угол А в радианы
//                     rad = Mathf.Asin(sinA);
//                     //переводим угол А из радиан в градусы
//                     angleA = rad * 180 / Mathf.PI;
//
//                     if (angleA < 0)
//                         angleA *= -1;
//                     if (angleB < 0)
//                         angleB *= -1;
//
//                     //Считаем угол С
//                     angleC = 180 - (angleA + angleB);
//
//                     //переводим градусы в радианы
//                     angleRad = angleC * Mathf.PI / 180;
//
//                     //выводим синус угла С
//                     sinC = Mathf.Sin(angleRad);
//
//                     //переводим угол C в радианы
//                     advanceAngleRad = angleC * Mathf.PI / 180;
//
//                     //считаем расстояние до цели (АВ)
//                     AB_2 = Vector3.Distance(targetRigidBody.transform.position, this.transform.position);
//
//                     //distTargretRocet = (targetRigidBody.transform.position - this.transform.position).magnitude;
//
//                     //вычисляем длину пути ВС до точки упреждения BC = AB * sinA / sinC
//                     BC_2 = AB_2 * sinA / Mathf.Sin(advanceAngleRad);
//
//                     //считаем время подлета цели к точке
//                     if (time2Bool) {
//                         //Вместо 100, надо вписать скорость цели!!!
//                         timeControl = BC_2 / 100;
//                         time2Bool = false;
//                     }
//
//                     //вычисляем время полета цели
//                     if (targetRigidBody.velocity.x != 0)
//                         timeAdvanceX = BC_2 / targetRigidBody.velocity.x;
//                     else timeAdvanceX = 0;
//
//                     if (targetRigidBody.velocity.y != 0)
//                         timeAdvanceY = BC_2 / targetRigidBody.velocity.y;
//                     else timeAdvanceY = 0;
//
//                     if (targetRigidBody.velocity.z != 0)
//                         timeAdvanceZ = BC_2 / targetRigidBody.velocity.z;
//                     else timeAdvanceZ = 0;
//
//                     //время не может быть отрицательным
//                     if (timeAdvanceX < 0)
//                         timeAdvanceX *= -1;
//                     if (timeAdvanceY < 0)
//                         timeAdvanceY *= -1;
//                     if (timeAdvanceZ < 0)
//                         timeAdvanceZ *= -1;
//
//                     //вычисляем vector3 точки упреждения в локальных координатах
//                     wayX = timeAdvanceX * targetRigidBody.velocity.x;
//                     wayY = timeAdvanceY * targetRigidBody.velocity.y;
//                     wayZ = timeAdvanceZ * targetRigidBody.velocity.z;
//
//                     if (targetRigidBody != null)
//                         advanceTarget = targetRigidBody.transform.position + new Vector3(wayX, wayY, wayZ);
//                 }
//             }
//         }
//     }
// }
