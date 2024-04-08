using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{

    // [ColorUsage(true, true)] public Color color; Color > HDR �÷� �����ϴ� ��

    //[SerializeField] bool Test = false;
    //[SerializeField] GameObject objTest;
    [SerializeField] RectTransform barRect;
    [SerializeField] Canvas canvas;

    private Vector2 barPos;
    private Vector2 mousePos;
    private Vector2 distancePos;
    private Vector2 barLimmitPos;

    private Vector3 canvasScale;
    private Vector3 barScale;

    private bool onDragCheck = false;
    private bool barPointerCheck = false;
    private bool outPos = true;
    private bool XYcheck = false;
    private bool XYcheck2 = false;
    public bool RL = false;
    public bool RL_path = false;
    public bool UD = false;
    public bool UD_path = false;
    public bool _Right = false;
    public bool _Left = false;
    public bool _Up = false;
    public bool _Down = false;

    float sWidth = 0f;
    float sHeight = 0f;
    float barWidth = 0f;
    float barHeight = 0f;
    float Xmouse = 0f;
    public float Xmouse_path = 0f;
    float Ymouse = 0f;
    public float Ymouse_path = 0f;
    float RL_Pos = 0f;
    float UD_Pos = 0f;
    float RL_xPos = 0f;
    float RL_yPos = 0f;
    float UD_xPos = 0f;
    float UD_yPos = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if(clickedObject.tag == "Bar")
        {
            barPointerCheck = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 barVec = gameObject.transform.position;

        barWidth = rect.rect.width;
        barHeight = rect.rect.height;

        if (barPointerCheck == false)
        {
            Debug.Log("barPointerCheck");
            return;
        }

        barPos = gameObject.transform.position;
        mousePos = eventData.position;

        if(onDragCheck == false)
        {
            DragPos();
        }

        #region Barȭ�� �� ���� �κ�
        if (barVec.x <= barWidth * (barScale.x / 2) * canvasScale.x || barVec.x >= sWidth - barWidth * (barScale.x / 2) * canvasScale.x)
        { // bar�� Width���� �߾����κ��� ������ ���̶� '/ 2' �ϰ� �θ� �����Ͽ� ���� ���� �޶������� 
            float limitPos;

            if (barVec.x < sWidth / 2) // ������Ʈ�� ���� ��ġ�� ���� ȭ�� Width ���� �ִ� �ּҷ� ���� ������Ʈ�� Width / 2 ��ŭ�� ���� ������ �ִ°�� �����Ͽ� ���� ȭ�� Width���� ���� ���� �۴ٸ� ����
            {
                limitPos = barWidth * (barScale.x / 2) * canvasScale.x - 1f;
                _Left = true;
            }
            else
            {
                limitPos = sWidth - barWidth * (barScale.x / 2) * canvasScale.x + 1f;
                _Right = true;
            }
            RL_Pos = limitPos;
            RL = true;
        }

        if (barVec.y <= barHeight * (barScale.y) * canvasScale.y || barVec.y >= sHeight - barHeight * (barScale.y) * canvasScale.y) // barHeight ũ��� �۾Ƽ� '/2' �� �ʿ�� ����
        {
            float limitPos;

            if (barVec.y < sHeight / 2) //  ȭ�� ���� ���� �� �Ʒ� ����
            {
                limitPos = barHeight * (barScale.y) * canvasScale.y - 1f;
                _Down = true;
            }
            else
            {
                limitPos = sHeight - barHeight * (barScale.y) * canvasScale.y + 1f;
                _Up = true;
            }
            UD_Pos = limitPos;
            UD = true;
        }

        if (RL == true || UD == true) // x�Ǵ� y�� ���� �Ǿ��� �� ������ ���� ���� ���� �� �Լ� ����
        {
            if ((RL == true) && (UD == true))
            {
                if (RL_path == false || UD_path == false)
                {
                    RL_path = true;
                    UD_path = true;
                    XYcheck = false;
                    Debug.Log("x, y true");
                }
                LimitBarPos(RL, UD, new Vector2(RL_Pos, UD_Pos), eventData.position);
                Debug.Log("x, y�� ����");
            }
            else if (RL == true)
            {
                if (RL_path == false || UD_path == true)
                {
                    RL_path = true;
                    UD_path = false;
                    //XYcheck = false;
                    Ymouse_path = 0;
                    Debug.Log("x true, y false");
                }
                LimitBarPos(RL, UD, new Vector2(RL_Pos, eventData.position.y), eventData.position);
                Debug.Log("x�� ����");
            }
            else if (UD == true)
            {
                if (UD_path == false || RL_path == true)
                {
                    UD_path = true;
                    RL_path = false;
                    //XYcheck = false;
                    Xmouse_path = 0;
                    Debug.Log("x false, y true");
                }
                LimitBarPos(RL, UD, new Vector2(eventData.position.x, UD_Pos), eventData.position);
                Debug.Log("y�� ����");
            }
        }

        if (outPos == true)
        {
            gameObject.transform.position = eventData.position - distancePos;

            _Left = false; _Right = false; _Down = false; _Up = false;
            RL = false; UD = false;
            Xmouse_path = 0;
            Ymouse_path = 0;
            XYcheck = false;
        }
        #endregion
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(barPointerCheck == false)
        {
            return;
        }

        if(outPos == true)
        {
            gameObject.transform.position = eventData.position - distancePos;
        }
        else
        {
            gameObject.transform.position = barLimmitPos;
        }

        onDragCheck = false;
        barPointerCheck = false;
    }

    private void Start()
    {
        sWidth = Screen.width;
        sHeight = Screen.height;

        canvasScale = canvas.gameObject.transform.localScale;
        barScale = gameObject.transform.localScale;

        barRect = GetComponent<RectTransform>();
        barWidth = barRect.rect.width;
        barHeight = barRect.rect.height;
    }

    private void DragPos()
    {
        distancePos = mousePos - barPos;
        onDragCheck = true;
    }

    //private void LimitBarPos_X(bool R_or_L, float _LimitPos, Vector2 _vec) ������ ������ �Լ��� ����
    //{
    //    Debug.Log(R_or_L);
    //    outPos = false;

    //    if (mouseCheck == false) // ���ٽ� �ѹ��� üũ
    //    {
    //        mouseCheck = true;
    //        fmous = _vec.x; // ���� ���콺 ��ġ
    //        RL_xPos = _LimitPos; // ���Խ� x ������ ������ġ
    //        Debug.Log($"RL_xPos = {RL_xPos}");
    //        Debug.Log($"X fmous = {fmous}");
    //    }
    //    if (R_or_L == false) // ����
    //    {
    //        if (mousePos.x > fmous) // ���� ���콺 ��ġ���� ���� ���콺 ��ġ�� Ŭ ��� (���� >> ������)
    //        {
    //            RL_xPos = _vec.x - distancePos.x; // ó�� �巡�� ��ġ�� ������ ������
    //            outPos = true;
    //            Debug.Log("Left");
    //            Debug.Log(RL_xPos);
    //        }

    //    }
    //    else if (R_or_L == true) // ������
    //    {
    //        if (mousePos.x < fmous) // ���� ���콺 ��ġ���� ���� ���콺 ��ġ�� ���� ��� (������ >> ����)
    //        {
    //            RL_xPos = _vec.x - distancePos.x;
    //            outPos = true;
    //            Debug.Log("Right");
    //            Debug.Log(RL_xPos);
    //        }
    //    }
    //    RL_yPos = _vec.y - distancePos.y;
    //    gameObject.transform.position = new Vector2(RL_xPos, RL_yPos); // �����ѵ��� ��� �� �����ǰ�

    //}

    //private void LimitBarPos_Y(bool U_or_D, float _LimitPos, Vector2 _vec)
    //{
    //    Debug.Log(U_or_D);
    //    outPos = false;

    //    if (mouseCheck == false)
    //    {
    //        mouseCheck = true;
    //        fmous = _vec.y;
    //        UD_yPos = _LimitPos;
    //        Debug.Log($"yPos = {UD_yPos}");
    //        Debug.Log($"Y fmous = {fmous}");
    //    }
    //    if (U_or_D == false)
    //    {
    //        if (mousePos.y > fmous)
    //        {
    //            UD_yPos = _vec.y - distancePos.y;
    //            outPos = true;
    //            Debug.Log("Down");
    //        }
    //    }
    //    else if (U_or_D == true)
    //    {
    //        if (mousePos.y < fmous)
    //        {
    //            UD_yPos = _vec.y - distancePos.y;
    //            outPos = true;
    //            Debug.Log("Up");
    //        }
    //    }
    //    UD_xPos = _vec.x - distancePos.x;
    //    gameObject.transform.position = new Vector2(UD_xPos, UD_yPos);

    //}

    private void LimitBarPos(bool fiexd_X, bool fiexd_Y, Vector2 _LimitPos, Vector2 _vec)
    {
        outPos = false;

        if (XYcheck == false)
        {
            XYcheck = true;
            Debug.Log("�ѹ� üũ�ǳ� Ȯ��");
            if (fiexd_X == true && fiexd_Y == true) // x , y�� ����� ��
            {
                Xmouse = _vec.x; // ���� ���콺 ��ġ
                RL_xPos = _LimitPos.x; // ���Խ� ������ ������ġ
                Ymouse = _vec.y;
                UD_yPos = _LimitPos.y; // ���Խ� ������ ������ġ

                if (Xmouse_path != 0 && Xmouse_path != Xmouse) // �߾��� �ƴ� y�� �������¿��� x�� ���� ��ǥ���� �������� ��
                {
                    Xmouse = Xmouse_path;
                }
                else if (Xmouse_path == 0) // �߾ӿ��� �������� ��
                {
                    Xmouse_path = Xmouse;
                }

                if (Ymouse_path != 0 && Ymouse_path != Ymouse)
                {
                    Ymouse = Ymouse_path;
                }
                else if (Xmouse_path == 0)
                {
                    Ymouse_path = Ymouse;
                }
            }
            else if (fiexd_X == true && fiexd_Y == false) // x �� ����� ��
            {
                Xmouse = _vec.x; // ���� ���콺 ��ġ
                RL_xPos = _LimitPos.x; // ���Խ� ������ ������ġ

                //if(Xmouse_path != 0 && Xmouse_path != Xmouse)
                //{
                //    Xmouse = Xmouse_path;
                //    Debug.Log(Xmouse_path);
                //}
                //else if(Xmouse_path == 0)
                //{
                //    Xmouse_path = Xmouse;
                //    Debug.Log(Xmouse_path);
                //}
            }
            else if (fiexd_X == false && fiexd_Y == true) // y �� ����� ��
            {
                Ymouse = _vec.y;
                UD_yPos = _LimitPos.y; // ���Խ� ������ ������ġ

                //if (Ymouse_path != 0 && Ymouse_path != Ymouse)
                //{
                //    Ymouse = Ymouse_path;
                //}
                //else if (Xmouse_path == 0)
                //{
                //    Ymouse_path = Ymouse;
                //}
            }
        }

        if (fiexd_X == true) // X���� ���� �Ǿ��� ��
        {// �߾����� ������ �� ��� �� �κ�
            if ((_Left == true && mousePos.x > Xmouse) || (_Right == true && mousePos.x < Xmouse)) // ���� ���콺 ��ġ���� ���� ���콺 ��ġ�� Ŭ ��� (���� >> ������)
            {
                RL_xPos = _vec.x - distancePos.x; // ó�� �巡�� ��ġ�� ������ ������
                outPos = true;
                Xmouse_path = 0f;
                //Debug.Log($"RL_xPOS = {RL_xPos}");
                return;
            }
            RL_yPos = _vec.y - distancePos.y;
        }

        if (fiexd_Y == true) // Y���� ���� �Ǿ��� ��
        {// �߾����� ������ �� ��� �� �κ�
            if ((_Down == true && mousePos.y > Ymouse) || (_Up == true && mousePos.y < Ymouse)) // ���� ���콺 ��ġ���� ���� ���콺 ��ġ�� Ŭ ��� (�� >> �Ʒ�)
            {
                UD_yPos = _vec.y - distancePos.y;
                outPos = true;
                Ymouse_path = 0f;
                //Debug.Log($"UD_YPOS = {UD_yPos}");
                return;
            }
            UD_xPos = _vec.x - distancePos.x;
        }

        if (fiexd_X == true && fiexd_Y == true)
        {
            barLimmitPos = new Vector2(RL_xPos, UD_yPos);
            //Debug.Log("tt"); // ���� �ϱ� �� �ٵ� ���� �� ���� ����
        }
        else if (fiexd_X == true && fiexd_Y == false)
        {
            barLimmitPos = new Vector2(RL_xPos, RL_yPos);
            //Debug.Log("tf");
        }
        else if (fiexd_Y == true && fiexd_X == false)
        {
            barLimmitPos = new Vector2(UD_xPos, UD_yPos);
            //Debug.Log("ft");
        }

        gameObject.transform.position = barLimmitPos;

    }
}
