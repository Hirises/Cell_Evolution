using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Pool : MonoBehaviour
{
    [ReadOnly]
    public int Pool_Size = 0;   //현재 오브젝트 개수 반영
    public GameObject Pool_Object;  //생성할 오브젝트
    public OnPool target;   //그룹핑 오브젝트
    public int Pool_Count = 10;  //생설할 양
    public bool is_Runstart = true;    //시작시 실행
    public bool is_addable = true;  //가변 크기
    public bool is_activewhenget = true; //가져올때 활성화

    public void Start()
    {
        if (is_Runstart) Run_Instance();    //시작시 풀 실행
    }

    public void Run_Instance()  //풀 실행
    {
        for (int i = 0; i < Pool_Count; i++) //개수만큼 반복
        {
            GameObject o = Instantiate(Pool_Object);    //생성
            o.SetActive(false); //비활성화
            o.transform.parent = transform; //부모설정
            if (target != null) target.Pool_Root = gameObject;   //그룹화
        }
        Pool_Size += Pool_Count;    //현재 오브젝트 개수 반영
    }

    public GameObject Get_Object()    //오브젝트 가져오기
    {
        GameObject o = null;
        int i = 0;
        int j = 0;

        do
        {
            if(j >= transform.childCount)   //모든 경로를 돌아도 없을때
            {
                if (is_addable) //생성 가능한 설정이면
                {
                    o = Instantiate(Pool_Object);    //생성
                    o.SetActive(false); //비활성화
                    o.transform.parent = transform; //부모설정
                    if(target != null) target.Pool_Root = gameObject;   //그룹화
                    Pool_Size++;    //사이즈 추가
                }
                else //아니면
                {
                    o = null;
                }
                break;  //반복 종료
            }

            if (i >= transform.childCount) i = 0;   //크기가 넘었으면 초기화

            o = transform.GetChild(i).gameObject;   //다음 자식 가져오기
            j++;
        }
        while (o.activeSelf);   //활성화 상태 검사

        if (is_activewhenget) o.SetActive(true);

        return o;
    }
}
