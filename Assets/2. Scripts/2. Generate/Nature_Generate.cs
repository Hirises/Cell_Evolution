using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature_Generate : MonoBehaviour
{
    //상수
    #region
    private const int Nature_Generate_Add_Radious = -1; //자연물생성시 중심으로부터 거리의 여유분
    private const float Nature_Generate_Scale_Min = 0.8f;   //자연물 최소 크기
    private const float Nature_Generate_Scale_Max = 1.2f;   //자연물 최대 크기
    #endregion

    public void Spawn()  //활성화(생성)시 위치 랜덤 조정
    {
        Vector2 pos = new Vector2(Random.Range(-Value.instance.Cur_Land_Size - Nature_Generate_Add_Radious, Value.instance.Cur_Land_Size + Nature_Generate_Add_Radious),
        Random.Range(-Value.instance.Cur_Land_Size - Nature_Generate_Add_Radious, Value.instance.Cur_Land_Size + Nature_Generate_Add_Radious));  //위치 지정
        transform.localPosition = new Vector3(pos.x, Value.instance.Generate_Pos_Y, pos.y);   //기본 y축 위치
        transform.localPosition = new Vector3(pos.x, Value.GetYPos(gameObject), pos.y);   //y축높이 조절
        transform.localRotation = Quaternion.Euler(0, Random.Range(1, 360), 0); //회전 랜덤
        transform.localScale = transform.localScale * Random.Range(Nature_Generate_Scale_Min, Nature_Generate_Scale_Max);   //크기 랜덤
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bottom")    //바닥에 닿으면 삭제
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)   //다른 먼저 생성된 오브젝트에 닿으면 삭제    (Obstacle- 레이어들을 감지)
        {
            Destroy(gameObject);
        }
    }
}
