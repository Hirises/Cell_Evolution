using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour    //SoundMandger의 종속 클래스 (사운드하나의 재생을 담당함, 재생이 끝나면 소멸됨)
{
    private AudioSource audioSource;    //재생할 오디오 소스 (오디오 클립은 이미 적용된 채로 Play메소드가 호출됨)

    public void Play()  //오디오 재생
    {
        StartCoroutine("_PlaySound");
    }

    IEnumerator _PlaySound()    //실제 재생 코루틴
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);    //오디오재생시간 + 0.5초 대기후 파괴 (여유시간)

        Destroy(gameObject);
    }
}
