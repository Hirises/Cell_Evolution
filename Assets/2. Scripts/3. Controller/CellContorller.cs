using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellContorller : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller;

    private void Start()
    {
        StartStateMechine();    //상태머신 시작
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 11)    //플레이어에 닿으면
        {
            Value.instance.Score += 1;  //점수증가
            StopAllCoroutines();    //상태머신 비활성화
            gameObject.SetActive(false);    //자신 비활성화
        }
    }

    //상태 머신
    #region
    private void StartStateMechine()    //상태머신 시작
    {
        StartCoroutine("StateMachine_Idle");    //기본 상태머신 - 대기
    }

    private IEnumerator StateMachine_Idle() //상태머신 - 대기
    {
        float t = 0;
        while (t < Random.Range(5f, 10f)) { //유지 시간
            //전이 감지
            if (Vector3.Distance(Value.instance.Player.transform.position, transform.position) < Value.instance.Cell_Detect_Range) //전이 - 도망
            {
                StartCoroutine("StateMachine_Runaway");
                yield break;    //이 상태는 종료
            }
            t += Time.deltaTime;    //시간증가
            yield return new WaitForEndOfFrame();   //1프레임 대기
        }
        StartCoroutine("StateMachine_Move"); //전이 - 이동
        yield break;    //이 상태는 종료
    }

    private IEnumerator StateMachine_Move() //상태머신 - 이동
    {
        //시작행동
        controller.SetRandomDir();  //방향 랜덤

        float t = 0;
        while (t < Random.Range(5f, 10f)) //유지 시간
        {
            //반복 행동
            controller.CheckObstacle(); //장해물 감지
            controller.SetRotate_to_Dir();  //이동방향을 바라보도록 회전
            controller.Move();  //이동

            //전이 감지
            if (Vector3.Distance(Value.instance.Player.transform.position, transform.position) < Value.instance.Cell_Detect_Range) //전이 - 도망
            {
                StartCoroutine("StateMachine_Runaway");
                yield break;    //이 상태는 종료
            }
            t += Time.deltaTime;    //시간증가
            yield return new WaitForEndOfFrame();   //1프레임 대기
        }
        StartCoroutine("StateMachine_Idle"); //전이 - 대기
        yield break;    //이 상태는 종료
    }

    private IEnumerator StateMachine_Runaway() //상태머신 - 도망
    {
        while (Vector3.Distance(Value.instance.Player.transform.position, transform.position) < Value.instance.Cell_Detect_Range) //유지 조건
        {
            //반복 행동
            controller.SetDir_to_Target(Value.instance.Player.transform.position);  //플레이어로 부터 도망치는 백터
            controller.SetUnDir_to_CurTarget();
            controller.CheckObstacle(); //장해물 감지
            controller.SetRotate_to_Dir();  //이동방향을 바라보도록 회전
            controller.Move(); //이동

            yield return new WaitForEndOfFrame();   //1프레임 대기
        }
        StartCoroutine("StateMachine_Idle"); //전이 - 대기
        yield break;    //이 상태는 종료
    }
    #endregion
}
