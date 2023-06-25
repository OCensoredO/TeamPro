using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteAnimation : MonoBehaviour
{
    // ���� �ڷ� : ChatGPT

    // �ִϸ��̼ǿ� ���� �� �����Ӻ� ��������Ʈ�� ������ �迭
    public Sprite[] frames;
    
    // �ִϸ��̼� �ʴ� ������
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
        // �ֻ����� ����Ͽ� �ִϸ��̼� ������Ʈ
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
