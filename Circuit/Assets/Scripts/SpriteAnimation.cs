using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteAnimation : MonoBehaviour
{
    // 참고 자료 : ChatGPT

    // 애니메이션에 쓰일 각 프레임별 스프라이트를 저장할 배열
    public Sprite[] frames;
    
    // 애니메이션 초당 프레임
    public float frameRate;

    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex = 0;
    private float timer = 0f;
    public bool endScene = false;
    public bool isStop = false;
    public string failScene;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (isStop) return;
        // 주사율에 기반하여 애니메이션 업데이트
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            currentFrameIndex++;
            if (currentFrameIndex >= frames.Length)
            {
                if (endScene) SceneManager.LoadScene(failScene);
                currentFrameIndex = 0;
            }

            spriteRenderer.sprite = frames[currentFrameIndex];
            timer = 0f;
        }
    }
}
