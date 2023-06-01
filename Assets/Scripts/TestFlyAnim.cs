using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestFlyAnim : MonoBehaviour
{
    public GameObject left,right;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        left.transform.DORotate(new Vector3(0,70,0), 0.4f).SetLoops(-1,LoopType.Yoyo);
        right.transform.DORotate(new Vector3(0,70,0), 0.4f).SetLoops(-1,LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra xem có mục tiêu không
        if (target != null)
        {
            // Tính toán vector hướng từ vị trí hiện tại của đối tượng tới mục tiêu
            Vector3 targetDirection = target.position - transform.position;

            // Chỉnh sửa góc xoay y để đối tượng luôn nhìn vào mục tiêu
            transform.LookAt(target.position, Vector3.up);
        }
    }
}
