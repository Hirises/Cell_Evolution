using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour    //게임 메니져(씬 전체 관리)
{
    //싱글톤
    #region
    public static GameManager instance; //싱글톤 인스턴스

    private void Awake()
    {
        if(instance == null)        //싱글톤
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

    [ReadOnly]
    private bool is_end_load = false;   //로딩(위치 고정)완료시 true

    //기본 메서드
    #region
    private void Start()    //맵 생성 및 초기화
    {
        ResetMap();  //맵 리셋
        Value.instance.Cur_Land_Num = 0;   //맵을 랜드 1로 설정
        GenerateMap(Value.instance.Cur_Land_Num);  //맵 생성
        SpawnPlayer();  //플레이어 스폰
        Start_Spawn_Enemy(Value.instance.Cur_Land_Num); //적 생성 시작
    }

    private void Update()
    {
        if(is_end_load) SetStuffsEnable();
    }
    #endregion

    //맵 생성&관리 (Generate~ 계열 메소드들)
    #region
    private void ResetMap() //맵 리셋 (전부 비활성화 및 풀링)
    {
        StopAllCoroutines();    //코루틴 전부 비활성화

        foreach (GameObject o in Value.instance.Land1_Map)    //랜드1맵 비활성화
        {
            o.SetActive(false);
        }
        for (int i = 0; i < Value.instance.Nature_Root.transform.childCount; i++)    //자연물 제거
        {
            Destroy(Value.instance.Nature_Root.transform.GetChild(i));
        }
        for (int i = 0; i < Value.instance.Stuff_Root.transform.childCount; i++)    //인공물 제거
        {
            Destroy(Value.instance.Stuff_Root.transform.GetChild(i));
        }
        for (int i = 0; i < Value.instance.Entity_Root.transform.childCount; i++)    //엔티티 제거
        {
            Destroy(Value.instance.Stuff_Root.transform.GetChild(i));
        }
    }

    private void GenerateMap(int LandNum)   //맵 생성 (지형지물까지 전부)
    {
        StartCoroutine("_GenerateMap", LandNum);    //코루틴 시작
    }

    private IEnumerator _GenerateMap(int LandNum)   //맵 생성 코루틴 (지형지물까지 전부)
    {
        is_end_load = false;    //로드시작
        GenerateLand(LandNum);  //섬 생성
        yield return new WaitForEndOfFrame();
        GenerateStructure(LandNum); //구조물 생성
        GenerateNatural(LandNum);   //자연물 생성
        yield return new WaitForSeconds(Value.instance.Load_Wating_time);   //전부 떨어져서 고정될때까지 대기
        is_end_load = true;    //로드끝
    }

    private void GenerateLand(int LandNum)   //섬 생성
    {
        switch (LandNum)
        {
            case 0: //랜드 1
                Value.instance.Cur_Land_Map = Value.instance.Land1_Map[Random.Range(0, Value.instance.Land1_Map.Count)]; //맵 랜덤 지정
                Value.instance.Cur_Land_Map.SetActive(true);  //활성화
                Value.instance.Cur_Land_Size = Random.Range(Value.instance.Land_Size_Min[0], Value.instance.Land_Size_Max[0]); //크기 랜덤지정
                Value.instance.Cur_Land_Map.transform.localScale = new Vector3(Value.instance.Cur_Land_Size, Value.instance.Cur_Land_Size, Value.instance.Cur_Land_Size * 0.5f);
                Value.instance.Cur_Land_Map.transform.position = Vector3.zero;    //위치 초기화
                break;
        }
    }

    private void GenerateStructure(int LandNum) //구조물 생성
    {
        switch (LandNum)
        {
            case 0:    //랜드 1
                break;
        }
    }

    private void GenerateNatural(int LandNum)   //자연물 생성
    {
        switch (LandNum)
        {
            case 0: //랜드 1
                int treeNum = Random.Range(Value.instance.Land_Nature_Min[0], Value.instance.Land_Nature_Max[0]);   //자연물 생성개수 저장
                int RandomValue = 0;    //비율 최대값 저장
                for(int i = 0; i < Value.instance.Land1_Nature_Rate.Count; i++)
                {
                    RandomValue += Value.instance.Land1_Nature_Rate[i];
                }
                int RandomAdd = 0;  //현재 비율값 저장
                for(int j = 0; j < treeNum; j++)    //생성할 자연물 개수만큼 반복
                {
                    RandomAdd = 0;
                    for (int i = 0; i < Value.instance.Land1_Nature_Rate.Count; i++)
                    {
                        RandomAdd += Value.instance.Land1_Nature_Rate[i];
                        if (Random.Range(1, RandomValue + 1) <= RandomAdd)  //만약 비율이 넘었으면
                        {
                            GameObject o = Instantiate(Value.instance.Land1_Nature[i]);   //자연물 생성
                            o.transform.parent = Value.instance.Nature_Root.transform;   //부모설정
                            o.GetComponent<Nature_Generate>().Spawn();  //크기&위치&회전 랜덤설정
                            break;
                        }
                    }
                }
                break;
        }
    }
    #endregion

    //오브젝트 관리
    #region
    private void SetStuffsEnable()  //동적 3D오브젝트 리로드
    {
        for (int i = 0; i < Value.instance.Nature_Root.transform.childCount; i++)    //자연물 리로드
        {
            GameObject set_target = Value.instance.Nature_Root.transform.GetChild(i).gameObject;   //대상 저장
            if (Mathf.Abs(set_target.transform.position.x - Value.instance.Player.transform.position.x) < Value.instance.Enable_Range_X
                && Mathf.Abs(set_target.transform.position.z - Value.instance.Player.transform.position.z) < Value.instance.Enable_Range_Z)   //범위 안에 있으면
            {
                if (set_target.activeSelf == false) set_target.SetActive(true); //활성화
            }
            else //범위 밖이면
            {
                if (set_target.activeSelf == true) set_target.SetActive(false); //비활성화
            }
        }
        for (int i = 0; i < Value.instance.Entity_Root.transform.childCount; i++)    //엔티티 리로드
        {
            GameObject set_target = Value.instance.Entity_Root.transform.GetChild(i).gameObject;   //대상 저장
            if (Mathf.Abs(set_target.transform.position.x - Value.instance.Player.transform.position.x) > Value.instance.Enable_Range_X * 1.5f
                || Mathf.Abs(set_target.transform.position.z - Value.instance.Player.transform.position.z) > Value.instance.Enable_Range_X * 1.5f)   //범위 밖이면
            {
                set_target.SetActive(false); //파괴
            }
        }
    }
    #endregion

    //플레이어 관리
    #region
    private void SpawnPlayer()  //플레이어 스폰
    {
        Value.instance.Player.transform.position = new Vector3(0, Value.instance.Generate_Pos_Y, 0);
    }
    #endregion

    //적 스폰
    #region
    private void Start_Spawn_Enemy(int LandNum) //적 스폰 코루틴 시작
    {
        switch (LandNum)
        {
            case 0: //랜드 1
                object[] var =  new object[] { 1f , 3f, Value.instance.CellEnemyPool};
                StartCoroutine("Spawn_Entity", var);   //Cell스폰
                break;
        }
    }

    private void SetRandomPosition_Fluid(GameObject o) //스폰 위치 랜덤
    {
        float raw_angle = Mathf.Max(-1, Mathf.Min(1 ,0.2f * Mathf.Sqrt(-2 * Mathf.Log(Random.Range(0.0f, 1.0f))) * Mathf.Cos(2 * Mathf.PI * Random.Range(0.0f, 1.0f))));    //일양난수의 정규변환 (-1 ~ 1)
        float angle = 180 * raw_angle + Value.instance.Player.GetComponent<PlayerController>().CalculateAngle();    //-180 ~ 180 으로 변환
        Vector2 dir = new Vector2(-Mathf.Cos(angle), Mathf.Sin(angle)) * Value.instance.Enable_Range_X;   //플레이어가 바라보는 각도를 더함
        o.transform.position = new Vector3(dir.x + Value.instance.Player.transform.position.x, Value.instance.Generate_Pos_Y, dir.y + Value.instance.Player.transform.position.z);  //실제 위치 연산
        o.transform.position = new Vector3(o.transform.position.x, Value.GetYPos(o), o.transform.position.z);  //실제 위치 연산
    }

    private IEnumerator Spawn_Entity(object[] var)  //적 스폰
    {
        //넘겨온 변수 선언
        float spawn_time_min = (float) var[0];  //스폰 대기시간
        float spawn_time_max = (float) var[1];
        Pool Pool_Root = (Pool) var[2]; //풀 오브젝트

        yield return new WaitForSeconds(Value.instance.Load_Wating_time);   //기본 로딩시간 대기
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawn_time_min, spawn_time_max));   //스폰 대기시간
            GameObject o = Pool_Root.Get_Object(); //오브젝트 풀링
            SetRandomPosition_Fluid(o); //위치랜덤
            o.transform.parent = Value.instance.Entity_Root.transform;  //부모설정
        }
    }
    #endregion
}
