using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    CameraLocation currentLocation;
    [SerializeField] List<Camera> Cameras = new List<Camera>();
    [SerializeField] List<GameObject> Backgrounds = new List<GameObject>();

    Camera NowCamera;
    Clip clip;
    public BuggeymanBtn buggeymanbtn;
    public ExpansionBtn expansionbtn;
    public AnimationMNG anim;

    //public Animator anim;

    public Camera getCamera
    {
        get { return NowCamera; }
    }

    public float time_max = 10f; // 부기맨 등장 10초 - 지역변수 못 갖고와서 여따 넣음
    int currentImgLocation;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftrightArrow;
    public GameObject HideBtn;
    public GameObject KeyBtn;
    public GameObject leftBtn, rightBtn;

    public enum CameraLocation // 옮겨진 카메라 위치
    {
        BG_Lock = 0, // 처음 화면
        BG_Lock_Table = 14, // 책상 확대
        BG_Lock_OntheTable = 1, // 처음 화면 - 책상
        BG_Lock_IntheTable = 13,
        BG_Lock_Closet = 2, // 처음 화면 - 옷장
        BG_Closet2 = 7, // 옷장 확대

        BG_Bed = 3, // 침실   
        BG_Bed_Bed = 12, // 침대
        BG_UndertheBed = 4, // 침대 아래

        BG_Lamp = 5, // 램프 있는 방
        BG_Lamp_ToyBox = 6, // 장난감 상자
 
        BG_Lamp_Drawer = 8, // 서랍
        BG_Lamp_Window = 9, // 창문

        BG_Door = 10, // 문 있는 방
        BG_Door_Door = 11, // 문 있는 방
    }
    public bool isClipSet;

    void Start()
    {
        clip = GetComponent<Clip>();
        currentLocation = CameraLocation.BG_Lock;
        NowCamera = Cameras[(int)CameraLocation.BG_Lock];
        for (int i = 0; i < Cameras.Count; i++)
        {
            if (i != (int)currentLocation) // 처음화면 제외하고 off
            {
                Cameras[i].gameObject.SetActive(false);
            }
            Vector3 ImageLocation = new Vector3(Backgrounds[i].transform.position.x, Backgrounds[i].transform.position.y, Backgrounds[i].transform.position.z - 10);
            // 이미지 위치 받아주고
            Cameras[i].transform.position = ImageLocation;
            // 카메라 위치 옮겨주기 - 번호에 해당하는 이미지들의 위치 + z -10
        }
    }

    void Update()
    {
        for (int i = 0; i < Cameras.Count; i++)
        {
            if (Cameras[i].gameObject.activeSelf == true) // 해당 카메라가 켜져있으면
            {
                currentImgLocation = i; // 해당 카메라 위치 = 현재 위치
                //Debug.Log(currentImgLocation);
                ArrowOnOff(); // 이동 버튼
            }
           
            if (currentImgLocation == (int)CameraLocation.BG_Door_Door) // BG_Door_Door이면
            {
                if (time_max > 0)
                {
                    time_max -= Time.deltaTime;
                    //Debug.Log((int)time_max);
                }   
                else
                    return;
            }
        }
    }

    public void LeftArrow() // 왼쪽 이동버튼 눌렀을 때
    {
        if (currentImgLocation == ((int)CameraLocation.BG_Lock))
        {
            NextCameraOn((int)CameraLocation.BG_Bed);
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Bed))
        {
            NextCameraOn((int)CameraLocation.BG_Lamp);
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Lamp))
        {
            NextCameraOn((int)CameraLocation.BG_Door);
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Door))
        {
            NextCameraOn((int)CameraLocation.BG_Lock);
        }
        // 나머지 화면인 경우 Setactive(false)
        
    }

    public void RightArrow() // 오른쪽 이동 버튼 눌렀을 때
    {
        if (currentImgLocation == ((int)CameraLocation.BG_Lock))
        {
            NextCameraOn((int)CameraLocation.BG_Door);
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Door))
        {
            NextCameraOn((int)CameraLocation.BG_Lamp);
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Lamp))
        {
            NextCameraOn((int)CameraLocation.BG_Bed);
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Bed))
        {
            NextCameraOn((int)CameraLocation.BG_Lock);
        }

        // 나머지 화면인 경우 Setactive(false)
    }

    public void UpArrow() // 위 이동 버튼 눌렀을 때
    {
        if (currentImgLocation == ((int)CameraLocation.BG_UndertheBed))
        // BG_Bed로 되돌아가기
        {
            NextCameraOn((int)CameraLocation.BG_Bed);
        }
    }

    public void DownArrow() // 아래 이동 버튼 눌렀을 때
    {
        if (currentImgLocation == ((int)CameraLocation.BG_Bed)) // 현재 위치가 침대일 때
        {
            NextCameraOn((int)CameraLocation.BG_UndertheBed);
        }
        else if (currentImgLocation == (int)CameraLocation.BG_Bed_Bed)
        // BG_Bed로 되돌아가기
        {
            NextCameraOn((int)CameraLocation.BG_Bed);
            anim.bed.SetTrigger("isBedClosed");
        }
        else if (currentImgLocation == ((int)CameraLocation.BG_Lock_Table) ||
            currentImgLocation == ((int)CameraLocation.BG_Lock_Closet))
        // BG_Lock으로 되돌아가기
        {
            NextCameraOn((int)CameraLocation.BG_Lock);
            anim.closetanim.SetTrigger("isClosetClosed");
        }
        else if(currentImgLocation == ((int)CameraLocation.BG_Closet2))
        {
            NextCameraOn((int)CameraLocation.BG_Lock_Closet);
            //anim.closetanim.SetTrigger
        }

        else if (currentImgLocation == ((int)CameraLocation.BG_Lock_OntheTable) || (currentImgLocation == ((int)CameraLocation.BG_Lock_IntheTable)))
        {
            NextCameraOn((int)CameraLocation.BG_Lock_Table);
            //anim.SetTrigger("IsTableClosed");

        }
        // 테이블 확대로 돌아가기
        else if (currentImgLocation == ((int)CameraLocation.BG_Lamp_ToyBox) || 
            currentImgLocation == ((int)CameraLocation.BG_Lamp_Drawer) || 
            currentImgLocation == ((int)CameraLocation.BG_Lamp_Window))
        // BG_Lamp으로 되돌아가기
        {
            NextCameraOn((int)CameraLocation.BG_Lamp);
            anim.window.SetBool("isWindowClosed",false);
            anim.Toybox.SetTrigger("isBoxClosed");
        }
        else if(currentImgLocation == ((int)CameraLocation.BG_Door_Door))
        // BG_Door로 되돌아가기
        {
            NextCameraOn((int)CameraLocation.BG_Door);
            currentImgLocation = 10; // 이미지 번호 직접 넣어줌
            anim.door.SetTrigger("isDoorClosed");
        }

    }

    public void NextCameraOn(int nextcamera) // 입력받은 번호의 카메라 켜주기
    {
        for (int i = 0; i < Cameras.Count; i++) // 받아서 돌리는데, 받은 번호가 나오면 그 번호 화면만 카메라 켜기
        {
            Cameras[nextcamera].gameObject.SetActive(true);
            NowCamera = Cameras[nextcamera];
            if (i != nextcamera)
            {
                Cameras[i].gameObject.SetActive(false);
            }
        }

        RaycastHit2D hit = this.GetComponent<RayCast>().getHit;
        if (hit)
        {
            if (hit.transform.GetComponent<SelectObject>())
            {
                hit.transform.GetComponent<SelectObject>().ObjBtn.SetActive(false);
                this.GetComponent<RayCast>().getHit = new RaycastHit2D();
            }
        }

        BuggeyOnOff(nextcamera); // 부기 등장 확률 함수 / 카메라 이동할 때마다 실행
        //ArrowOnOff(); // 이동 버튼
        
    }  

    public void ArrowOnOff() // 화살표
    {
        HashSet<CameraLocation> OKCameraLocations = new HashSet<CameraLocation>()
        {
            CameraLocation.BG_Lock,
            CameraLocation.BG_Bed,
            CameraLocation.BG_Lamp,
            CameraLocation.BG_Door,
        };

        if(OKCameraLocations.Contains((CameraLocation)currentImgLocation))
            // Lock,Lamp,Bed,Door에서 왼쪽오른쪽 버튼 켜주기
        {
            //leftrightArrow.SetActive(true);
            leftBtn.SetActive(true);
            rightBtn.SetActive(true);
            upArrow.SetActive(false);
            KeyBtn.SetActive(false); 
            HideBtn.SetActive(false);

            if (!(currentImgLocation == (int)CameraLocation.BG_Bed))// Bed에서 아래 버튼 켜주기
            {
                downArrow.SetActive(false);
            }
            else
                downArrow.SetActive(true);
        }
        else if(currentImgLocation == (int)CameraLocation.BG_Door_Door)
        {
            leftBtn.SetActive(false);
            rightBtn.SetActive(false);
            downArrow.SetActive(true);
            upArrow.SetActive(false);
        }
        else // 클로징 화면 + UndertheBed
        {
            leftBtn.SetActive(false);
            rightBtn.SetActive(false);
            //leftrightArrow.SetActive(false);
            downArrow.SetActive(true);
            if (currentImgLocation == (int)CameraLocation.BG_UndertheBed) //undertheBed에서만 위 버튼 켜주기   
            {
                upArrow.SetActive(true);
                downArrow.SetActive(false);
            }
            else
            {
                upArrow.SetActive(false);
            }
        }
    }

    public void BuggeyOnOff(int x) // x = 현재 위치 / 부기맨 + 버튼 등장
    {
        // 위치만 그때그때 받고,
        // 이동할 때만 함수 실행 시켜야함
        switch(x)
        {
            case (int)CameraLocation.BG_Lock_Closet: // 옷장
                buggeymanbtn.BuggeyAppearCloset();
                break;            

            case (int)CameraLocation.BG_UndertheBed: // 침대 밑
                buggeymanbtn.BuggeyAppearUndertheBed();
                HideBtn.SetActive(false);
                break;
            
            case (int)CameraLocation.BG_Bed_Bed: // 침대 클로징
                HideBtn.SetActive(true); // Hide 버튼
                break;            

            case (int)CameraLocation.BG_Lamp_Drawer: //  서랍
                buggeymanbtn.BuggeyAppearDrawer();
                break;            

            case (int)CameraLocation.BG_Door_Door:
                buggeymanbtn.BuggeyAppearDoor_Door();// 문 클로징

                if (clip.clip_num > 0 && clip.clip_num <= 2) // 클립 하나 이상일 때
                {
                    KeyBtn.SetActive(true);
                }
                else
                    KeyBtn.SetActive(false);
                
                 // Key 버튼 - 현재는 NextCamera() 실행 안돼서 시간도 안먹고 버튼 안 뜸
                break;

            default:
                KeyBtn.SetActive(false);
                HideBtn.SetActive(false);
                buggeymanbtn.buggey.SetActive(false); // 일단 부기 꺼질 수 있게
                expansionbtn.OpenGauge.SetActive(false);
                break;
        }
    }
}


