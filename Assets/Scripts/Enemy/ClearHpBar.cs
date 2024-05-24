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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬 넘길 때 적 처치하지 않고 그냥 넘어온경우 EnemyHpBar 없애기 위한 코드
    {
        RectTransform trs = gameObject.GetComponent<RectTransform>();
        RectTransform[] trsChild = trs.GetComponentsInChildren<RectTransform>().Where(t => t != trs).ToArray(); // Where을 사용하지 않는경우, 특정인덱스에 접근 할 필요가 없을경우엔 .ToArry()를 사용하지 않아도 무방
        if(trsChild != null)                                                 /* Where은 필터링 연산으로 지금은 GetcomponentInChilderen 사용시 자기 자신도 포함하기에 자기자신을 제외하고 배열을 만들기위해 사용 */
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
