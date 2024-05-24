using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearHpBar : MonoBehaviour
{

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // �� �ѱ� �� �� óġ���� �ʰ� �׳� �Ѿ�°�� EnemyHpBar ���ֱ� ���� �ڵ�
    {
        RectTransform trs = gameObject.GetComponent<RectTransform>();
        RectTransform[] trsChild = trs.GetComponentsInChildren<RectTransform>().Where(t => t != trs).ToArray(); // Where�� ������� �ʴ°��, Ư���ε����� ���� �� �ʿ䰡 ������쿣 .ToArry()�� ������� �ʾƵ� ����
        if(trsChild != null)                                                 /* Where�� ���͸� �������� ������ GetcomponentInChilderen ���� �ڱ� �ڽŵ� �����ϱ⿡ �ڱ��ڽ��� �����ϰ� �迭�� ��������� ��� */
        {
            for (int iNum = 0; iNum < trsChild.Length; iNum++)
            {
                Destroy(trsChild[iNum].gameObject);
                trsChild[iNum] = null;
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
