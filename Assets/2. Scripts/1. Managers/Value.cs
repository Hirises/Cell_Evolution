using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Value : MonoBehaviour
{
    //풀 오브젝트
    #region
    [SerializeField]
    public Pool CellEnemyPool;
    #endregion

    //GameManager
    #region
    [ReadOnly]
    public int Cur_Land_Size; //현재 맵 크기
    [ReadOnly]
    public GameObject Cur_Land_Map; //현재 맵 오브젝트
    [ReadOnly]
    public int Cur_Land_Num;    //현재 맵 번호 (0부터 시작)

    public int Load_Wating_time;    //맵 로드시 기다릴 시간
    public List<int> Land_Size_Min = new List<int>();   //최소 섬 크기
    public List<int> Land_Size_Max = new List<int>();   //최대 섬 크기
    public List<int> Land_Nature_Min = new List<int>();   //자연물 최소 개수
    public List<int> Land_Nature_Max = new List<int>();   //자연물 최대 개수
    public List<GameObject> Land1_Map = new List<GameObject>();  //랜드1 맵
    public List<GameObject> Land1_Nature = new List<GameObject>();    //랜드1 자연물
    public List<int> Land1_Nature_Rate = new List<int>(); //랜드1 자연물 비율

    public int Enable_Range_X;    //3D오브젝트의 활성화 범위 x
    public int Enable_Range_Z;    //3D오브젝트의 활성화 범위 z
    public int Generate_Pos_Y;    //자연물 생성시 나오는 Y좌표값    (지형의 최대 높이보다 높게 설정)

    public GameObject Nature_Root;   //자연물오브젝트의 루트
    public GameObject Stuff_Root;  //인공물오브젝트의 루트
    public GameObject Entity_Root;  //엔티티 루트
    #endregion

    //PlayerController
    #region
    public int Player_Move_Speed;
    #endregion

    //오브젝트
    #region
    public GameObject Player;
    #endregion

    //엔티티
    #region
    public int Cell_Detect_Range;
    #endregion

    //변수
    #region
    [ReadOnly]
    public int Score;   //점수
    #endregion

    //싱글톤
    #region
    public static Value instance; //싱글톤 인스턴스

    private void Awake()
    {
        if (instance == null)        //싱글톤
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //유틸 메소드
    #region
    public static float GetYPos(GameObject o) //지면의 Y축 높이 확인
    {
        if (Physics.Raycast(o.transform.position, Vector3.down, out RaycastHit hit, 100f))
        {
            return hit.point.y;
        }
        else    //감지된 충돌이 없을시 삭제
        {
            Destroy(o);
            return Value.instance.Generate_Pos_Y;
        }
    }
    #endregion
}
