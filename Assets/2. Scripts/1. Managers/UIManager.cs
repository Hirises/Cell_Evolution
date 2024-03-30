using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //UI오브젝트
    #region
    [SerializeField]
    private Text Score_Text;
    #endregion

    //싱글톤
    #region
    public static UIManager instance; //싱글톤 인스턴스

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

    private void Start()
    {
        Score_Text.text = "000000"; //스코어 표기 초기화
    }

    private void Update()
    {
        SetScoreUI();   //점수 표기 갱신
    }

    private void SetScoreUI()   //점수 표기 갱신
    {
        string _str = "00000" + Value.instance.Score;   //0 6개 포멧으로 변환
        Score_Text.text = _str.Substring(_str.Length - 6, 6);
    }
}
