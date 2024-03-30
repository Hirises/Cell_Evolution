using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SoundManager : MonoBehaviour   //사운드 메니져(소리 재생)
{
    [SerializeField]
    private AudioSource BGMAudioSource; //브금용 오디오소스
    [SerializeField]
    private List<AudioClip> BGMDic = new List<AudioClip>(); //브금 리스트
    [SerializeField]
    private List<AudioClip> SFXDic = new List<AudioClip>(); //효과음 리스트

    //싱글톤
    #region
    public static SoundManager instance; //싱글톤 인스턴스

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

    public void ShoutSound(int _index)     //효과음 재생
    {
        GameObject gameObject = Instantiate(new GameObject());  //새로운 객체 생성
        gameObject.transform.parent = transform;    //부모 설정
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();   //오디오 소스 추가
        audioSource.clip = SFXDic[_index];
        SoundSource soundSource = gameObject.AddComponent<SoundSource>();   //제어 스크립트 부착
        soundSource.Play();
    }

    private IEnumerator _VolumFade(AudioSource inputSource, float time, float start, float end)  //볼륨 페이드
    {
        float t = 0;
        while (t >= 1)
        {
            t += Time.deltaTime / time;
            inputSource.volume = Mathf.Lerp(start, end, t); //선형보간 사용
            yield return new WaitForEndOfFrame();
        }
        inputSource.volume = end;
    }

    //BGM 관련
    #region
    public void ChangeBGM(int _index)  //배경음악 변경
    {
        BGMAudioSource.clip = BGMDic[_index];
    }

    public void PlayBGM(int fade_time)  //브금 플레이
    {
        StartCoroutine("_PlayBGM", fade_time);
    }

    public void StopBGM(int fade_time)  //브금 정지
    {
        StartCoroutine("_StopBGM", fade_time);
    }

    private IEnumerator _StopBGM(int fade_time)
    {
        _VolumFade(BGMAudioSource, fade_time, 1, 0); //페이드 아웃
        yield return new WaitForSeconds(1f);
        BGMAudioSource.Stop();
    }

    private IEnumerator _PlayBGM(int fade_time)
    {
        BGMAudioSource.Play();  //페이드 인
        _VolumFade(BGMAudioSource, fade_time, 0, 1);
        yield return new WaitForSeconds(1f);
    }
    #endregion
}
