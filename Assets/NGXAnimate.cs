using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public enum ANIMATION_TRIGGER
{
    AnimateIn,
    AnimateInOut,
    Enable,
    Start
}


public enum ANIMATION_TYPE
{
    Fade,
    Move,
    MoveX,
    MoveY,
    Scale,
    ScaleX,
    ScaleY,
    Size,
    SizeW,
    SizeH,
    Rotate,
    RotateX,
    RotateY,
    RotateZ,
    Fill,
    Color
}

public enum FILL_TYPE
{

    Horizontal_CW,
    Vertical_CW,
    Radial90_CW,
    Radial180_CW,
    Radial360_CW,
    Horizontal_CCW,
    Vertical_CCW,
    Radial90_CCW,
    Radial180_CCW,
    Radial360_CCW
}

public enum FILL_ORIGIN
{
    Bottom,
    Right,
    Top,
    Left
}


public enum OBJECT_TYPE
{
    Image,
    RawImage,
    CanvasGroup,
    TextMeshPro,
    Text
}

#if UNITY_EDITOR
[CustomEditor(typeof(NGXAnimate))]
public class NGXAnimateEditor : Editor
{




    NGXAnimate t;
    SerializedObject get_target;
    SerializedProperty animations_list;

    void OnEnable()
    {
        t = (NGXAnimate)target;
        get_target = new SerializedObject(t);
        animations_list = get_target.FindProperty("animations");
    }

    public override void OnInspectorGUI()
    {

        get_target.Update();
        SerializedProperty trigger = get_target.FindProperty("_trigger");
        SerializedProperty tst = get_target.FindProperty("test");
        EditorGUILayout.PropertyField(trigger);
        EditorGUILayout.Space(); 




        for (int i = 0; i < animations_list.arraySize; i++)
        {



            SerializedProperty current_obj_ref = animations_list.GetArrayElementAtIndex(i);
            SerializedProperty current_obj = current_obj_ref.FindPropertyRelative("obj");
            SerializedProperty animate_relative = current_obj_ref.FindPropertyRelative("_animate_relative");

            //EditorGUILayout.PropertyField(animate_relative);
            EditorGUILayout.PropertyField(current_obj);
            EditorGUILayout.Space();
            get_target.ApplyModifiedProperties();


            SerializedProperty current_anis = current_obj_ref.FindPropertyRelative("anis");

            if (t.animations[i].open = EditorGUILayout.Foldout(t.animations[i].open, "Animations")) {

            

            
            for (int a = 0; a < current_anis.arraySize; a++)
            {

                SerializedProperty ani_ref = current_anis.GetArrayElementAtIndex(a);
                SerializedProperty ani_ease = ani_ref.FindPropertyRelative("_ease");
                SerializedProperty ani_type = ani_ref.FindPropertyRelative("_type");
                SerializedProperty ani_duration = ani_ref.FindPropertyRelative("_duration");
                SerializedProperty ani_delay = ani_ref.FindPropertyRelative("_delay");
                SerializedProperty ani_from = ani_ref.FindPropertyRelative("_from");
                SerializedProperty ani_to = ani_ref.FindPropertyRelative("_to");

                SerializedProperty ani_from_color = ani_ref.FindPropertyRelative("_from_color");
                SerializedProperty ani_to_color = ani_ref.FindPropertyRelative("_to_color");


                // Determine Type

                string component_type = "";






                EditorGUILayout.PropertyField(ani_ease);
                EditorGUILayout.PropertyField(ani_type);
                EditorGUILayout.PropertyField(ani_duration);
                EditorGUILayout.PropertyField(ani_delay);
                EditorGUILayout.Space();


                // DISPLAY BASED ON OBJECT TYPE


                // RECORD VALUES
                EditorGUILayout.BeginHorizontal();
                
                

              
                // DISPLAY FROM VALUES        
                switch (ani_type.intValue)
                {
                    // X 0
                    case 0:
                        float _a = EditorGUILayout.Slider("Start", ani_from.vector3Value.x, 0, 1);
                        ani_from.vector3Value = new Vector3(_a, 0, 0);


                            break;
                    case 2:
                    case 5:
                    case 8:
                    case 11:
                        float _b = EditorGUILayout.FloatField("Start", ani_from.vector3Value.x);
                        ani_from.vector3Value = new Vector3(_b, 0, 0);

                            break;
                        // X 0
                        case 3:
                    case 6:
                        case 9:
                        case 12:
                        float _c = EditorGUILayout.FloatField("Start", ani_from.vector3Value.y);
                        ani_from.vector3Value = new Vector3(0, _c, 0);

                            break;
                        // X & Y
                        case 1:
                    case 4:
                    case 7:
                        Vector2 _d = EditorGUILayout.Vector2Field("Start", new Vector2(ani_from.vector3Value.x, ani_from.vector3Value.y));
                        ani_from.vector3Value = new Vector3(_d.x, _d.y, 0);
                        
                        break;

                    case 13:
                        float _z = EditorGUILayout.FloatField("Start", ani_from.vector3Value.z);
                        ani_from.vector3Value = new Vector3(0, 0, _z);
                    break;
                        
                    // X , Y & Z
                    case 10:
                    
                          Vector3 _r = EditorGUILayout.Vector3Field("Start", ani_from.vector3Value);
                        ani_from.vector3Value = _r;

                        break;

                        case 14:

                            float _e = EditorGUILayout.Slider("Start", ani_from.vector3Value.x, 0, 1);
                            ani_from.vector3Value = new Vector3(_e, 0, 0);

                            break;

                        case 15:
                            ani_from_color.colorValue = EditorGUILayout.ColorField("Start", ani_from_color.colorValue);
                            break; 

                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("d_animationkeyframe"), GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
                {
                    //t.animations[i].obj
                    switch (ani_type.intValue)
                    {

                        case 0: // fade
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.GetComponent<Graphic>().color.a, 0, 0);
                            break;
                        case 1: // move
                            ani_from.vector3Value = t.animations[i].obj.transform.localPosition;
                            break;
                        case 2: // moveX
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.transform.localPosition.x, 0, 0);
                            break;
                        case 3: // moveY
                            ani_from.vector3Value = new Vector3(0, t.animations[i].obj.transform.localPosition.y, 0);
                            break;

                        case 4: // Scale
                            ani_from.vector3Value = t.animations[i].obj.transform.localScale;
                            break;
                        case 5: // ScaleX
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.transform.localScale.x, 0, 0);
                            break;
                        case 6: // ScaleY
                            ani_from.vector3Value = new Vector3(0, t.animations[i].obj.transform.localScale.y, 0);
                            break;
                        case 7: // Size
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.x, t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.y, 0);
                            break;

                        case 8: // SizeX
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.x, 0, 0);
                            break;

                        case 9: // SizeY
                            ani_from.vector3Value = new Vector3(0, t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.y, 0);
                            break;

                        case 10: // Rotate
                            ani_from.vector3Value = t.animations[i].obj.transform.localRotation.eulerAngles;
                            break;

                        case 11: // RotateX
                            Debug.Log("ROTATE X");
                            Debug.Log(t.animations[i].obj.transform.localRotation.eulerAngles.x);
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.transform.localRotation.eulerAngles.x, 0, 0);
                            break;

                        case 12: // RotateY
                            Debug.Log("ROTATE Y");
                            ani_from.vector3Value = new Vector3(0, t.animations[i].obj.transform.localRotation.eulerAngles.y, 0);
                            break;

                        case 13: // RotateZ
                            Debug.Log("ROTATE Z");
                            ani_from.vector3Value = new Vector3(0, 0, t.animations[i].obj.transform.localRotation.eulerAngles.z);
                            break;

                        case 14: // [TODO FILL]
                            ani_from.vector3Value = new Vector3(t.animations[i].obj.GetComponent<Image>().fillAmount, 0, 0);
                            break;

                        case 15: // [TODO COLOR]
                            ani_from_color.colorValue = t.animations[i].obj.GetComponent<Graphic>().color;
                            break;



                    }
                }


                EditorGUILayout.EndHorizontal();



                EditorGUILayout.BeginHorizontal();

              

                switch (ani_type.intValue)
                {
                    // X 0
                    case 0:
                       

                        float _a2 = EditorGUILayout.Slider("End", ani_to.vector3Value.x, 0, 1);
                        ani_to.vector3Value = new Vector3(_a2, 0, 0);
                        break;

                    case 2:
                    case 5:
                    case 8:
                    case 11:
                       
                        float _b2 = EditorGUILayout.FloatField("End", ani_to.vector3Value.x);
                        ani_to.vector3Value = new Vector3(_b2, 0, 0);
                        break;

                    // X 0
                    case 3:
                    case 6:
                    case 9:
                    case 12:
                      
                        float _c2 = EditorGUILayout.FloatField("End", ani_to.vector3Value.y);
                        ani_to.vector3Value = new Vector3(0, _c2, 0);
                        break;

                    // X & Y
                    case 1:
                    case 4:
                    case 7:
                     
                        Vector2 _d2 = EditorGUILayout.Vector2Field("End", new Vector2(ani_to.vector3Value.x, ani_to.vector3Value.y));
                        ani_to.vector3Value = new Vector3(_d2.x, _d2.y, 0);
                        break;

                    // X , Y & Z

                    case 13:
                        float _z = EditorGUILayout.FloatField("Start", ani_to.vector3Value.z);
                        ani_to.vector3Value = new Vector3(0, 0, _z);
                        break;

                    case 10:
                        Vector3 _r = EditorGUILayout.Vector3Field("Start", ani_to.vector3Value);
                        ani_to.vector3Value = _r;
                        break;
                    case 14:

                        float _e = EditorGUILayout.Slider("End", ani_to.vector3Value.x, 0, 1);
                        ani_to.vector3Value = new Vector3(_e, 0, 0);

                        break;

                    case 15:
                        ani_to_color.colorValue = EditorGUILayout.ColorField("End", ani_to_color.colorValue);
                        break;

                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("d_animationkeyframe"), GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
                {
                    //t.animations[i].obj
                    switch (ani_type.intValue)
                    {

                        case (int)ANIMATION_TYPE.Fade: // fade
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.GetComponent<Graphic>().color.a, 0, 0);
                            break;
                        case 1: // move
                            ani_to.vector3Value = t.animations[i].obj.transform.localPosition;
                            break;
                        case 2: // moveX
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.transform.localPosition.x, 0, 0);
                            break;
                        case 3: // moveY
                            ani_to.vector3Value = new Vector3(0, t.animations[i].obj.transform.localPosition.y, 0);
                            break;

                        case 4: // Scale
                            ani_to.vector3Value = t.animations[i].obj.transform.localScale;
                            break;
                        case 5: // ScaleX
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.transform.localScale.x, 0, 0);
                            break;
                        case 6: // ScaleY
                            ani_to.vector3Value = new Vector3(0, t.animations[i].obj.transform.localScale.y, 0);
                            break;
                        case 7: // Size
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.x, t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.y, 0);
                            break;

                        case 8: // SizeX
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.x, 0, 0);
                            break;

                        case 9: // SizeY
                            ani_to.vector3Value = new Vector3(0, t.animations[i].obj.GetComponent<RectTransform>().sizeDelta.y, 0);
                            break;

                        case 10: // Rotate
                            ani_to.vector3Value = t.animations[i].obj.transform.localRotation.eulerAngles;
                            break;

                        case 11: // RotateX
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.transform.localRotation.eulerAngles.x, 0, 0);
                            break;

                        case 12: // RotateY
                            ani_to.vector3Value = new Vector3(0, t.animations[i].obj.transform.localRotation.eulerAngles.y, 0);
                            break;

                        case 13: // RotateZ
                            ani_to.vector3Value = new Vector3(0, 0, t.animations[i].obj.transform.localRotation.eulerAngles.z);
                            break;

                        case 14: // [TODO FILL]
                            ani_to.vector3Value = new Vector3(t.animations[i].obj.GetComponent<Image>().fillAmount, 0, 0);
                            break;

                        case 15: // [TODO COLOR]
                            ani_to_color.colorValue = t.animations[i].obj.GetComponent<Graphic>().color;
                            break;

                    }
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(" ");
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
                {
                    current_anis.DeleteArrayElementAtIndex(a);
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();


            }

            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Add Animation");
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
            {
                current_anis.InsertArrayElementAtIndex(current_anis.arraySize);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(); 


                }

            EditorGUILayout.BeginHorizontal();
           
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
            {
                animations_list.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();


        }

        if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
        {
            t.animations.Add(new NGXAnimate.MyObj());
        }

        get_target.ApplyModifiedProperties();
    }
}
#endif

[System.Serializable]
public class NGXAnimate : MonoBehaviour
{





    [System.Serializable]
    public class MyAni
    {
        public int num;
        public DG.Tweening.Ease _ease = Ease.OutExpo;
        public ANIMATION_TYPE _type = ANIMATION_TYPE.Fade;
        public float _duration = 1;
        public float _delay = 0;
        public Vector3 _from;
        public Vector3 _to;
        public Color _from_color;
        public Color _to_color;
        public Color out_from_color;
        public Color out_to_color;

        public Vector3 out_from;
        public Vector3 out_to;
        public OBJECT_TYPE _object_type;
        public bool out_animation = false;

    }

    [System.Serializable]
    public class MyObj
    {
        public GameObject obj;
        public bool open = true; 
        public bool _animate_relative = false;
        public List<MyAni> anis = new List<MyAni>(1);
    }

    public ANIMATION_TRIGGER _trigger;
    public List<MyObj> animations = new List<MyObj>(1);
    public float out_delay = 0;
    public bool has_run = false; 

    void AddNew()
    {
        //Add a new index position to the end of our list
        animations.Add(new MyObj());
    }

    void Remove(int index)
    {
        //Remove an index position from our list at a point in our list array
        animations.RemoveAt(index);
    }


    void Start()
    {
        if (_trigger == ANIMATION_TRIGGER.Start)
        {
            animateIn();
        }
    }


    void OnEnable()
    {
        if (_trigger == ANIMATION_TRIGGER.Enable)
        {
          
            set();
            Invoke("setAnimateIn", 0.1f);

        }
    }

    void setAnimateIn() {
        animateIn();
    }

    public void animateIn()
    {
        has_run = true; 
        foreach (MyObj a in animations)
        {
            a.obj.transform.DOKill();

            if (a.obj.GetComponent<CanvasGroup>() != null) {
                a.obj.GetComponent<CanvasGroup>().DOKill();
            } else 
             if (a.obj.GetComponent<Image>() != null)
            {
                a.obj.GetComponent<Image>().DOKill();
            }
            else {
                a.obj.GetComponent<Graphic>().DOKill();
            }

            foreach (MyAni mi in a.anis)
            {
              

                switch (mi._type)
                {

                    case ANIMATION_TYPE.Fade:
                      
                        if (a.obj.GetComponent<CanvasGroup>() != null)
                        {
                            a.obj.GetComponent<CanvasGroup>().DOFade(mi._to.x, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                        }
                        else
                        {
                            a.obj.GetComponent<Graphic>().DOFade(mi._to.x, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                        }


                        break;

                    case ANIMATION_TYPE.Move:
                        a.obj.transform.DOLocalMove(new Vector2(mi._to.x, mi._to.y),mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.MoveX:
                    
                        a.obj.transform.DOLocalMoveX(mi._to.x, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.MoveY:

                    
                      
                        a.obj.transform.DOLocalMoveY(mi._to.y, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.Scale:
                        a.obj.transform.DOScale(new Vector2(mi._to.x, mi._to.y), mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.ScaleX:
                        a.obj.transform.DOScale(mi._to.x, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.ScaleY:
                        a.obj.transform.DOScale(mi._to.y, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.Size:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(mi._to.x, mi._to.y),mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.SizeW:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(mi._to.x, a.obj.GetComponent<RectTransform>().rect.height), mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.SizeH:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(a.obj.GetComponent<RectTransform>().rect.width, mi._to.y), mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.Rotate:
                        a.obj.transform.DOLocalRotate(mi._to, mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.RotateX:
                        a.obj.transform.DOLocalRotate(new Vector3(mi._to.x, a.obj.transform.localRotation.eulerAngles.y, a.obj.transform.localRotation.eulerAngles.z), mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.RotateY:
                        a.obj.transform.DOLocalRotate(new Vector3(a.obj.transform.localRotation.eulerAngles.x, mi._to.y, a.obj.transform.localRotation.eulerAngles.z), mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.RotateZ:
                        a.obj.transform.DOLocalRotate(new Vector3(a.obj.transform.localRotation.eulerAngles.x, a.obj.transform.localRotation.eulerAngles.y, mi._to.z), mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.Fill:
                        a.obj.transform.GetComponent<Image>().DOFillAmount(mi._to.x,mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;

                    case ANIMATION_TYPE.Color:
                        a.obj.transform.GetComponent<Graphic>().DOColor(mi._to_color,mi._duration).SetDelay(mi._delay).SetEase(mi._ease);
                    break;
                }
            }
        }
    }

    /*
    Fade,
    Move,
    MoveX,
    MoveY,
    Scale,
    ScaleX,
    ScaleY,
    Size,
    SizeW,
    SizeH,
    Rotate,
    RotateX,
    RotateY,
    RotateZ,
    Fill,
    Color
    */

    public void animateOut()
    {
        has_run = false;
        foreach (MyObj a in animations)
        {

            a.obj.transform.DOKill();

            if (a.obj.GetComponent<CanvasGroup>() != null)
            {
                a.obj.GetComponent<CanvasGroup>().DOKill();
            }
            else
             if (a.obj.GetComponent<Image>() != null)
            {
                a.obj.GetComponent<Image>().DOKill();
            }
            else
            {
                a.obj.GetComponent<Graphic>().DOKill();
            }

            foreach (MyAni mi in a.anis)
            {
              

                switch (mi._type)
                {

                    case ANIMATION_TYPE.Fade:
                        if (a.obj.GetComponent<CanvasGroup>() != null)
                        {
                            a.obj.GetComponent<CanvasGroup>().DOFade(mi._from.x, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        }
                        else {
                            a.obj.GetComponent<Graphic>().DOFade(mi._from.x, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        }
                       
                        break;

                    case ANIMATION_TYPE.Move:
                        a.obj.transform.DOLocalMove(new Vector2(mi._from.x, mi._from.y), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.MoveX:
                        a.obj.transform.DOLocalMoveX(mi._from.x, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.MoveY:
                        a.obj.transform.DOLocalMoveY(mi._from.y, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Scale:
                        a.obj.transform.DOScale(new Vector2(mi._from.x, mi._from.y), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.ScaleX:
                        a.obj.transform.DOScale(mi._from.x, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.ScaleY:
                        a.obj.transform.DOScale(mi._from.y, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Size:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(mi._from.x, mi._from.y), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.SizeW:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(mi._from.x, a.obj.GetComponent<RectTransform>().rect.height), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.SizeH:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(a.obj.GetComponent<RectTransform>().rect.width, mi._from.y), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Rotate:
                        a.obj.transform.DOLocalRotate(mi._from, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.RotateX:
                        a.obj.transform.DOLocalRotate(new Vector3(mi._from.x, a.obj.transform.localRotation.eulerAngles.y, a.obj.transform.localRotation.eulerAngles.z), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.RotateY:
                        a.obj.transform.DOLocalRotate(new Vector3(a.obj.transform.localRotation.eulerAngles.x, mi._from.y, a.obj.transform.localRotation.eulerAngles.z), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.RotateZ:
                        a.obj.transform.DOLocalRotate(new Vector3(a.obj.transform.localRotation.eulerAngles.x, a.obj.transform.localRotation.eulerAngles.y, mi._from.z), mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Fill:
                        a.obj.transform.GetComponent<Image>().DOFillAmount(mi._from.x, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Color:
                        a.obj.transform.GetComponent<Graphic>().DOColor(mi._from_color, mi._duration).SetDelay(out_delay).SetEase(mi._ease);
                        break;
                }
            }
        }
    }



    public void set()
    {
        has_run = false;
        foreach (MyObj a in animations)
        {

            a.obj.transform.DOKill();

            if (a.obj.GetComponent<CanvasGroup>() != null)
            {
                a.obj.GetComponent<CanvasGroup>().DOKill();
            }
            else
             if (a.obj.GetComponent<Image>() != null)
            {
                a.obj.GetComponent<Image>().DOKill();
            }
            else
            {
                a.obj.GetComponent<Graphic>().DOKill();
            }

            foreach (MyAni mi in a.anis)
            {


                switch (mi._type)
                {

                    case ANIMATION_TYPE.Fade:
                        if (a.obj.GetComponent<CanvasGroup>() != null)
                        {
                            a.obj.GetComponent<CanvasGroup>().DOFade(mi._from.x, 0).SetDelay(0).SetEase(mi._ease);
                        }
                        else
                        {
                            a.obj.GetComponent<Graphic>().DOFade(mi._from.x, 0).SetDelay(0).SetEase(mi._ease);
                        }

                        break;

                    case ANIMATION_TYPE.Move:
                        a.obj.transform.DOLocalMove(new Vector2(mi._from.x, mi._from.y), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.MoveX:
                        a.obj.transform.DOLocalMoveX(mi._from.x, 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.MoveY:
                        a.obj.transform.DOLocalMoveY(mi._from.y, 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Scale:
                        a.obj.transform.DOScale(new Vector2(mi._from.x, mi._from.y), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.ScaleX:
                        a.obj.transform.DOScale(mi._from.x, 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.ScaleY:
                        a.obj.transform.DOScale(mi._from.y, 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Size:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(mi._from.x, mi._from.y), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.SizeW:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(mi._from.x, a.obj.GetComponent<RectTransform>().rect.height), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.SizeH:
                        a.obj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(a.obj.GetComponent<RectTransform>().rect.width, mi._from.y), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Rotate:
                        a.obj.transform.DOLocalRotate(mi._from, 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.RotateX:
                        a.obj.transform.DOLocalRotate(new Vector3(mi._from.x, a.obj.transform.localRotation.eulerAngles.y, a.obj.transform.localRotation.eulerAngles.z), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.RotateY:
                        a.obj.transform.DOLocalRotate(new Vector3(a.obj.transform.localRotation.eulerAngles.x, mi._from.y, a.obj.transform.localRotation.eulerAngles.z), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.RotateZ:
                        a.obj.transform.DOLocalRotate(new Vector3(a.obj.transform.localRotation.eulerAngles.x, a.obj.transform.localRotation.eulerAngles.y, mi._from.z), 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Fill:
                        a.obj.transform.GetComponent<Image>().DOFillAmount(mi._from.x, 0).SetDelay(0).SetEase(mi._ease);
                        break;

                    case ANIMATION_TYPE.Color:
                        a.obj.transform.GetComponent<Graphic>().DOColor(mi._from_color, 0).SetDelay(0).SetEase(mi._ease);
                        break;
                }
            }
        }
    }



    public void toggle() {

        has_run = !has_run;

       

        if (has_run)
        {
            animateIn();
        }
        else {
            animateOut();
        }
     

    }


    public void run()
    {
        Invoke("setAnimateIn", 0.1f);


    }


}


