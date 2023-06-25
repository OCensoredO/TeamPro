using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        // �ֻ����� ����Ͽ� �ִϸ��̼� ������Ʈ
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            currentFrameIndex++;
            if (currentFrameIndex >= frames.Length)
            {
                currentFrameIndex = 0;
            }

            spriteRenderer.sprite = frames[currentFrameIndex];
            timer = 0f;
        }
    }
}
