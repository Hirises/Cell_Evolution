using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour    //적들의 움직임을 위한 클래스(위 메서드들을 직접 호출하지는 않음, 다른클래스와 함께 부착되어 그 클래스에 종속되어 호출됨)
{
    //외부 입력 상수
    #region
    [SerializeField]
    private Rigidbody rigidbody;    //리지드바디
    [SerializeField]
    private GameObject Rendered;    //랜더러
    #endregion

    //내부 변수
    #region
    private Vector2 Dir;    //이동방향(정규백터)
    #endregion

    //외부 입력 변수
    #region
    [SerializeField]
    private float MoveSpeed;    //이속
    [SerializeField]
    private float SeeRange; //감지 범위
    [SerializeField]
    private float Range;    //넓이(회피기동에 사용)
    #endregion


    //이동 메서드
    #region
    public void Move()  //이동
    {
        rigidbody.velocity = new Vector3(Dir.x * MoveSpeed, rigidbody.velocity.y, Dir.y * MoveSpeed);
    }

    public void CheckObstacle() //장해물 확인
    {
        //폐기된 코드 (레이케스트사용)
        #region
        //if (Physics.Raycast(transform.position, v2to3(Dir), out RaycastHit hit, SeeRange))   //레이케스트 히트시 
        //{
        //    Debug.Log("hit");
        //    Obstacle Hited_Obstacle = hit.transform.gameObject.GetComponent<Obstacle>();    //감지된 장해물의 클래스 가져오기
        //    Vector2 HitCenter = v3to2(Hited_Obstacle.Origin.transform.position); //센터값 가져오기
        //    //Vector2 Avoid_Dir = (v3to2(transform.position) + Dir - HitCenter).normalized;   //반발 백터 계산(구)
        //    Vector2 Avoid_Dir = ((Dir * Vector2.Dot(Dir, HitCenter - v3to2(transform.position))) + v3to2(transform.position) - HitCenter).normalized;   //반발 백터 계산
        //    Vector2 AvoidVector = HitCenter + (Avoid_Dir * (Hited_Obstacle.AvoidPower + Range));  //개선된 타겟백터 계산
        //    SetDir_to_Target(AvoidVector);
        //}
        #endregion

        for (int i = 0; i < Value.instance.Nature_Root.transform.childCount; i++) //자연물대상 감지
        {
            Obstacle Hited_Obstacle = Value.instance.Nature_Root.transform.GetChild(i).GetComponent<Obstacle>();
            if (Hited_Obstacle != null && CheckHit(Hited_Obstacle))   //전부 감지 - Distance 
            {
                SetAvoidDir(Hited_Obstacle);    //회피 기동 백터 계산 및 적용
            }
        }
        for (int i = 0; i < Value.instance.Stuff_Root.transform.childCount; i++)    //인공물대상 감지
        {
            Obstacle Hited_Obstacle = Value.instance.Stuff_Root.transform.GetChild(i).GetComponent<Obstacle>();
            if (Hited_Obstacle != null && CheckHit(Hited_Obstacle))   //전부 감지 - Distance 
            {
                SetAvoidDir(Hited_Obstacle);    //회피 기동 백터 계산 및 적용
            }
        }
        for (int i = 0; i < Value.instance.Entity_Root.transform.childCount; i++)    //엔티티대상 감지
        {
            Obstacle Hited_Obstacle = Value.instance.Entity_Root.transform.GetChild(i).GetComponent<Obstacle>();
            if (Hited_Obstacle != null && Hited_Obstacle.gameObject == gameObject) continue;    //자기자신이면 패스
            if (Hited_Obstacle != null && CheckHit(Hited_Obstacle))   //전부 감지 - Distance 
            {
                SetAvoidDir(Hited_Obstacle);    //회피 기동 백터 계산 및 적용
            }
        }
    }

    public void SetRandomDir()  //랜덤한 방향으로 설정
    {
        int angle = Random.Range(1, 360);   //각도 랜덤
        Dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));  //방향 설정
    }

    public void SetDir_to_Target(Vector2 target_pos)    //대상을 향한 방향저장(백터 2)
    {
        Dir = (target_pos - v3to2(transform.position)).normalized;
    }

    public void SetDir_to_Target(Vector3 target_pos)    //대상을 향한 방향저장(백터 3)
    {
        Dir = (v3to2(target_pos) - v3to2(transform.position)).normalized;
    }

    public void SetUnDir_to_CurTarget()  //현재 대상으로 부터 도망가는 방향
    {
        Dir *= -1;
    }

    public void SetRotate_to_Dir()  //현재 방향을 바라보도록 회전
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(Dir.x, 0, Dir.y));
    }
    #endregion

    //보조 메서드들
    #region
    private void SetAvoidDir(Obstacle Hited_Obstacle)   //회피 기동 백터 계산 및 적용
    {
        Vector2 HitCenter = v3to2(Hited_Obstacle.Origin.transform.position); //센터값 가져오기
        //Vector2 Avoid_Dir = (v3to2(transform.position) + Dir - HitCenter).normalized;   //반발 백터 계산(구)
        Vector2 Avoid_Dir = ((Dir * Vector2.Dot(Dir, HitCenter - v3to2(transform.position))) + v3to2(transform.position) - HitCenter).normalized;   //반발 백터 계산 (신/ 내적 사용)
        Vector2 AvoidVector = HitCenter + (Avoid_Dir * (Hited_Obstacle.AvoidPower + Range));  //개선된 타겟백터 계산
        SetDir_to_Target(AvoidVector);  //방향 저장
    }

    private bool CheckHit(Obstacle Hited_Obstacle)  //충돌 감지 (추후에 모든 부분에서의 충돌을 감지할 수 있게 업그레이드 요망)
    {
        bool is_hit = false;
        for (int i = 0; i < Range; i++) //0부터 1마다 감지
        {
            if(Vector2.Distance(v3to2(Hited_Obstacle.Origin.transform.position), v3to2(transform.position) + (Dir * i)) <= Hited_Obstacle.AvoidPower + Range)
            {
                is_hit = true;
                break;
            }
        }
        if (Vector2.Distance(v3to2(Hited_Obstacle.Origin.transform.position), v3to2(transform.position) + (Dir * Range)) <= Hited_Obstacle.AvoidPower + Range)  //감지범위 맨 끝부분 체크
        {
            is_hit = true;
        }
        return is_hit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bottom")    //바닥에 닿으면 삭제
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.layer == 8)    //지면에 닿으면 랜더링 활성화
        {
            Rendered.SetActive(true);
        }
    }
        #endregion

    //유틸 메서드들
    #region
        private Vector3 v2to3(Vector2 orgin)    //백터 변환(2 -> 3)
    {
        return new Vector3(orgin.x, 0, orgin.y);
    }

    private Vector2 v3to2(Vector3 orgin)    //백터 변환(3 -> 2)
    {
        return new Vector2(orgin.x, orgin.z);
    }
    #endregion
}
