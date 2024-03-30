using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class OnPool : MonoBehaviour //풀링의 대상이 되는 오브젝트용 메소드 (선택사항)
{
    [ReadOnly]
    public GameObject Pool_Root = null;

    //그룹화 메서드
    private void OnDisable()
    {
        if(Pool_Root != null && !transform.parent.Equals(Pool_Root.transform)) transform.parent = Pool_Root.transform; //다시 풀로 반환
    }
}
