using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour   //장해물의 정보를 담고있는 클래스, 장해물에 같이 부착되어 정보를 읽을수 있게 함
{
    public GameObject Origin;   //기원(오브젝트)
    public float AvoidPower;    //밀어낼 범위(반지름)
}
