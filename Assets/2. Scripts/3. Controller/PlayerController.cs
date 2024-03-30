using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;    //리지드 바디

    private Vector2 Dir;    //방향 백터(x, z)
    private Vector2 _Dir; //마지막으로 이동했던 방향

    private void Update()
    {
        SetDir();   //방향설정(터치패드 등등)
        Move(); //이동
    }

    public float CalculateAngle()   //각도를 반환
    {
        return Quaternion.FromToRotation(Vector3.up, _Dir).eulerAngles.z;
    }

    //이동관련
    #region
    private void SetDir()   //방향설정
    {
        //임시로 키보드 입력
        if (Input.GetKeyDown(KeyCode.W))
        {
            Dir += Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Dir += Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Dir += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Dir += Vector2.down;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Dir -= Vector2.right;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Dir -= Vector2.left;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Dir -= Vector2.up;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Dir -= Vector2.down;
        }

        if(!(Dir == Vector2.zero)) _Dir = Dir;
    }

    private void Move() //이동
    {
        _rigidbody.velocity = new Vector3(Dir.normalized.x * Value.instance.Player_Move_Speed, _rigidbody.velocity.y, Dir.normalized.y * Value.instance.Player_Move_Speed);   //이동
    }
    #endregion
}
