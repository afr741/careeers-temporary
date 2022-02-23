using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening; 
#if UNITY_EDITOR
using UnityEditor; 
#endif
//
[ExecuteInEditMode]
public class NGXScreen : MonoBehaviour {

	// Use this for initialization


	public List<NGXScreen> children; 
	private NGXScreen selected; 
	[SerializeField]
	private NGXScreen current;
	private CanvasGroup cg; 
	private Image mask; 

	public bool maskChildren = false; 

	public GameObject prefab; 
	private GameObject prefabInstance; 

	public AnimationType AniIn;
	public AnimationType AniOut; 

	public enum AnimationType{
		Inherit,
		None,
		Fade,
		SlideTop,
		SlideBottom,
		SlideLeft,
		SlideRight
	};

	public float AniInDuration  = 1f;
	public float AniOutDuration  = 1f;

    public bool delay_input = false;
    public float delay_timeout = 1;



	void Awake(){

		// add CG
		if (Application.isPlaying) {
			if (AniIn == AnimationType.Inherit) {
				// get from parent
				if (transform.parent.GetComponent<NGXScreen> () != null) {
					if (transform.parent.GetComponent<NGXScreen> ().AniIn == AnimationType.Inherit) {
						AniIn = AnimationType.None; 
					} else {
						AniIn = transform.parent.GetComponent<NGXScreen> ().AniIn;
					}
				} else {
					AniIn = AnimationType.None; 
				}
			}

			if (AniOut == AnimationType.Inherit) {
				// get from parent
				if (transform.parent.GetComponent<NGXScreen> () != null) {
					if (transform.parent.GetComponent<NGXScreen> ().AniOut == AnimationType.Inherit) {
						AniOut = AnimationType.None; 
					} else {
						AniOut = transform.parent.GetComponent<NGXScreen> ().AniOut;
					}
				} else {
					AniOut = AnimationType.None; 
				}
			}

			if (cg == null) {
				if (GetComponent<CanvasGroup> () == null) {
					cg = this.gameObject.AddComponent (typeof(CanvasGroup)) as CanvasGroup; 
				} else {
					cg = this.GetComponent<CanvasGroup> (); 
				}
			} 





			if (maskChildren) {
				if (GetComponent<Image> () == null) {
					mask = this.gameObject.AddComponent (typeof(Image)) as Image; 
					Mask m = this.gameObject.AddComponent (typeof(Mask)) as Mask; 
					m.showMaskGraphic = false; 
				}
			}

			addChildren (); 
			// set current; 
			if (children.Count > 0) {
				current = children [0];
				current.transform.SetAsLastSibling ();
				foreach (NGXScreen s in children) {
					if (s != current) {
						s.gameObject.SetActive (false); 
					} else {
						s.gameObject.SetActive (true); 
					}
				}
			}


		}
	}

	#if UNITY_EDITOR

	void OnEnable()
	{
		if (!EditorApplication.isPlaying) {
			Selection.selectionChanged += SelectionChanged; 
		}
	}


	void OnDisable()
	{
		if (!EditorApplication.isPlaying) {
			Selection.selectionChanged -= SelectionChanged; 
		}
	}

	void SelectionChanged(){

		if (Selection.activeTransform != null) {
			if (Selection.activeTransform.parent == null) {
				return; 
			}


			if (Selection.activeTransform.parent.transform == this.transform) {
				// is child
				if (Selection.activeTransform.GetComponent<NGXScreen> () != null) {
					// is screen 
					addChildren (); 
					if (Selection.activeTransform.GetComponent<NGXScreen> () != selected) {
						selected = Selection.activeTransform.GetComponent<NGXScreen> ();
						redraw (); 
					}
				}
			}
		}
	}

	#endif
	void addChildren(){
		foreach (Transform t in transform) {
			if (t.GetComponent<NGXScreen> () != null) {
				if(children.Contains(t.GetComponent<NGXScreen>())){
					// already added
				} else {
					children.Add(t.GetComponent<NGXScreen>()); 
				}
			}
		}
	}
	void redraw(){

		foreach(NGXScreen child in children){
			if (child == selected) {
				child.gameObject.SetActive (true);  
			} else {
				child.gameObject.SetActive (false); 
			}
		}

	}


	public void Transition(GameObject go){
		if (Application.isPlaying)
		Transition (go.name); 
	}



	private void checkParent(){
	
		if (!this.gameObject.activeInHierarchy) {
			if (transform.parent.GetComponent<NGXScreen> () != null) {
				transform.parent.GetComponent<NGXScreen> ().Transition (this.name); 
			
			}
		}
	
	}

	public void Transition(string go){


      


        checkParent (); 



		if (Application.isPlaying) {
			if (current.name == go) {
				// is already this... 
				return; 
			}

			foreach (NGXScreen c in children) {
				if (c.name == go) {

					if (current != null) {
						if (current.isActiveAndEnabled) {
							current.animateOut ();
						}
					}
					c.transform.SetAsLastSibling (); 



					current = c.GetComponent<NGXScreen> (); 
					current.gameObject.SetActive (true);
					current.animateIn (); 
			
				}
			}
		}
	}



	public void animateOut(){
		if (Application.isPlaying) {
			cg.blocksRaycasts = false;
			cg.interactable = false; 


			Invoke ("clear", AniOutDuration);

			runAnimation (AniOut, false);
		}
	}

	public void animateIn(){


      

        if (Application.isPlaying) {
			CancelInvoke ("clear");

            if (prefabInstance != null) {
                GameObject.Destroy(prefabInstance);
            }

            if (prefab != null)
            {
                prefabInstance = Instantiate(Resources.Load("Prefabs/" + prefab.name.ToString())) as GameObject;
                prefabInstance.transform.SetParent(transform, false);
                prefabInstance.transform.localScale = new Vector3(1, 1, 1);
            }

            if (delay_input) {
                cg.blocksRaycasts = false;
                cg.interactable = false;
                CancelInvoke("reActivate");
                Invoke("reActivate", delay_timeout);
            }
			
			runAnimation (AniIn, true);
		}
	}

    public void reActivate() {
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

	private void runAnimation(AnimationType type, bool in_ani){
	
		if (Application.isPlaying) {
			//cg.DOKill (); 

			switch (type) {

			case AnimationType.None:

                   

                    if (in_ani) {
					cg.alpha = 1; 
				} else {
					cg.alpha = 0; 
				}
			
				break;

			case AnimationType.Fade:



				if (in_ani) {
                    cg.DOKill(); 
					cg.alpha = 0;
                        cg.DOFade(1, AniInDuration).SetEase(Ease.OutExpo);
				} else {
                        cg.DOKill();
                        cg.DOFade(0, AniOutDuration).SetEase(Ease.OutExpo);
                    }

				break;

			}
		}
	} 

	void clear(){
		if (Application.isPlaying) {
			this.gameObject.SetActive (false); 
			if (prefabInstance != null) {
				GameObject.Destroy (prefabInstance); 
			}
		}
	}

}
