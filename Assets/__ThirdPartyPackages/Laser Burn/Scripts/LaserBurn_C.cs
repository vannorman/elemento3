//LaserBurn
//This code can be used for private or commercial projects but cannot be sold or redistributed without written permission.
//Copyright Nik W. Kraus / Dark Cube Entertainment LLC. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LineRenderer))]

public class LaserBurn_C : MonoBehaviour {


	[Tooltip ("Laser Starting Point.")]
	public Transform StartPoint;

	[Tooltip ("Local Starting Point Laser direction X, Y, Z.")]
	public string LaserDirection = "X";

	[Tooltip ("Enable Laser Burn.")]
	public bool UseBurn = true;

	[Tooltip ("Layer tag for Laser hit to ignore.")]
	public string IgnoreTag = "reflective";
	public bool UseMousePos = false;	

	[Tooltip ("Enable 2D mode, (3D mode is used when off)")]
	public bool Use2D = false;

	[Tooltip ("Enable Physics Layer mask.")]
	public LayerMask Layer;

	public bool LaserOn = true;    

	[Tooltip ("Enable texture Pan to simulate dust.")]
	public bool UseUVPan = true;

	[Tooltip ("Flare offset from end hit point.")]
	public float EndFlareOffset = 0.1f;

	[Tooltip ("Assign source and end flare for added effect.")]
	public LensFlare SourceFlare;
	public LensFlare EndFlare;

	[Tooltip ("Add light to source or end points.")]
	public bool AddSourceLight = true;
	public float SourceLightRange = .5f;
	public bool AddEndLight = true;
	public float EndLightRange = .5f;

	public Color LaserColor = new Color(1f,1f,1f,.5f);

	[Tooltip ("Control the Glare angle range, twards the camera of source flare.")]
	public int GlareAngle = 155;

	[Tooltip ("Assign hit sparks particle system.")]
	public ParticleSystem HitSparks;

	[Tooltip ("Enable sparks to use Laser color.")]
	public bool SparkUseLaserColor = false;

	[Tooltip ("Assign Burn Mark object.")]
	public Transform BurnMarks;

	[Tooltip ("Laser start and end width.")]
	public float StartWidth = 0.1f;
	public float EndWidth = 0.1f;

	[Tooltip ("Max Laser distance from start point.")]
	public float LaserDist = 20.0f;

	[Tooltip ("Texture dust effect scroll speed X and Y.")]
	public float TexScrollX = -0.1f;
	public float TexScrollY = 0.1f;

	[Tooltip ("Texture scale used for dust effect")]
	public Vector2 UVTexScale = new Vector2(8f,.1f);

	private int SectionDetail = 2;       
	private LineRenderer lineRenderer;
	private Ray ray;
	private Vector3 EndPos;
	private RaycastHit hit;
	private RaycastHit2D hit2D;
	private GameObject SourceLight;
	private GameObject EndLight;
	private float ViewAngle;
	private Vector3 LaserDir;
	private Quaternion rot;
	private Ray2D ray2;
	private GameObject BurnClone;
	private float CamDistSource;
	private float CamDistEnd;
	
    
    private ParticleSystem.EmissionModule PartEm;
	private ParticleSystem.MainModule PartMain;


	void Start() {

        if (HitSparks){
            PartMain = HitSparks.main;
            PartEm = HitSparks.emission;
        }

        //Starting point reference
        if (!StartPoint){
			StartPoint = gameObject.transform;
			Debug.Log("No StartPoint assigned, This object transform will be used instead.");
		}

		lineRenderer = GetComponent<LineRenderer>();
		if(lineRenderer.material == null)
			lineRenderer.GetComponent<Renderer>().material = new Material (Shader.Find("LaserAdvanced"));

		lineRenderer.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		lineRenderer.receiveShadows = false;

        lineRenderer.positionCount = SectionDetail;

		// Make a lights
		if(AddSourceLight){
			StartPoint.gameObject.AddComponent<Light>();
			StartPoint.GetComponent<Light>().intensity = 1.5f;
			StartPoint.GetComponent<Light>().range = .5f;
		}

		if(AddEndLight){
			if(EndFlare){
				EndFlare.gameObject.AddComponent<Light>();
				EndFlare.GetComponent<Light>().intensity = 1.5f;
				EndFlare.GetComponent<Light>().range = 1.0f;		
			}
			else{Debug.Log("To use End Light, please assign an End Flare");}
		}		

		if(LaserDirection=="x" || LaserDirection=="y" || LaserDirection=="z" || LaserDirection=="X" || LaserDirection=="Y" || LaserDirection=="Z"){     
		}
		else{
			Debug.Log("Laser Direction can only be X, Y or Z");
		}      
		

		//UseBurn Check
		if(UseBurn){
			if(!HitSparks){
				Debug.Log("No Spark particles assigned!");
			}

			if(!BurnMarks){
				Debug.Log("No Burn Marks assigned!");
			}
		}

	}//end start



	/////////////////////////////////////
	void Update() {
		if(LaserDirection=="x" || LaserDirection=="X"){		
			LaserDirection="X";
			LaserDir = StartPoint.right;
		}
		else if(LaserDirection=="y" || LaserDirection=="Y"){
			LaserDirection="Y";
			LaserDir = StartPoint.up;
		}
		else if(LaserDirection=="z" || LaserDirection=="Z"){
			LaserDirection="Z";
			LaserDir = StartPoint.forward;
		}
		else{
			LaserDir = StartPoint.forward;
		}        

		if(Camera.main){
			CamDistSource = Vector3.Distance(StartPoint.position, Camera.main.transform.position);
			CamDistEnd = Vector3.Distance(EndPos, Camera.main.transform.position);
			ViewAngle = Vector3.Angle(LaserDir, Camera.main.transform.forward);		
		}
		else{
			CamDistSource = 5;
			CamDistEnd = 5;
			ViewAngle = 55;
			Debug.Log("Main camera not tagged, please tag main camera");
		}



		if(LaserOn){
			lineRenderer.enabled = true;
			lineRenderer.material.color = LaserColor;

            lineRenderer.startWidth = StartWidth;
            lineRenderer.endWidth = EndWidth;

            //Flare Control
            if (SourceFlare){
				SourceFlare.color = LaserColor;
				SourceFlare.transform.position = StartPoint.position;

				if(ViewAngle > GlareAngle && CamDistSource < 20 && CamDistSource > 0){
					SourceFlare.brightness = Mathf.Lerp(SourceFlare.brightness,20.0f,.001f);	
				}
				else{
					SourceFlare.brightness = Mathf.Lerp(SourceFlare.brightness,0.1f,.05f);
				}        
			}

			if(EndFlare){
				EndFlare.color = LaserColor;

				if(CamDistEnd > 20){
					EndFlare.brightness = Mathf.Lerp(EndFlare.brightness,0.0f,.1f);
				}
				else{
					EndFlare.brightness = Mathf.Lerp(EndFlare.brightness,5.0f,.1f);
				}
			}// end flare        

			//Light Control
			if(AddSourceLight){
				StartPoint.GetComponent<Light>().color = LaserColor;
				StartPoint.GetComponent<Light>().range = SourceLightRange;		
			}

			if(AddEndLight){				
				if(EndFlare){
					EndFlare.GetComponent<Light>().range = EndLightRange;
					EndFlare.GetComponent<Light>().color = LaserColor;
				}
			}               


			/////////////////////Ray Hit
			if(Use2D){
				Vector3 CamMousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				float Dist2D = Vector2.Distance(new Vector2(StartPoint.position.x,StartPoint.position.y), new Vector2(CamMousePos2D.x,CamMousePos2D.y));

				if(UseMousePos){
					hit2D = Physics2D.Raycast(StartPoint.position, CamMousePos2D-StartPoint.position, Dist2D, Layer);
				}
				else{
					hit2D = Physics2D.Raycast(StartPoint.position, LaserDir, LaserDist, Layer);
				}

				var ray2 = new Ray2D(StartPoint.position, LaserDir);
				if (hit2D){		        
					EndPos = hit2D.point;	 		    

					if(EndFlare){
						EndFlare.enabled = true;

						if(AddEndLight){
							if(EndFlare){
								EndFlare.GetComponent<Light>().enabled = true;		    
							}
						}

						if(EndFlareOffset > 0)
							EndFlare.transform.position = hit2D.point + hit2D.normal * EndFlareOffset;
						else
							EndFlare.transform.position = EndPos;
					}		    
					////////////Burn & Sparks
					if(UseBurn){
						if(HitSparks && hit2D.transform.gameObject.GetComponent<Collider2D>().tag != IgnoreTag){
							PartEm = HitSparks.emission;
							PartEm.enabled = true;
							HitSparks.transform.position = new Vector2(EndPos.x,EndPos.y) + hit2D.normal * EndFlareOffset;
							HitSparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit2D.normal);
							//////////////Enable Burn Marks
							if(BurnMarks){
								//rot = Quaternion.EulerAngles(0f,90f,90f);
								rot = Quaternion.Euler(0f,90f,90f);
								BurnMarks.localScale = new Vector3(.5f,.5f,.5f);
                                Transform BurnClone = Instantiate(BurnMarks, EndPos, rot) as Transform;
                                BurnClone.transform.parent = hit2D.transform;
							}
							//////////Use Laser Color for Sparks
							if(SparkUseLaserColor){
                                PartMain.startColor  = LaserColor;
                            }
						}
						else{
							if(HitSparks){	       	       	
                                PartEm.enabled = false;
							}
						}	       	  	        
					}////End Burn & Sparks 
				}
				else{
					if(EndFlare)
						EndFlare.enabled = false;	        

					if(AddEndLight){
						if(EndFlare){
							EndFlare.GetComponent<Light>().enabled = false;
						}
					}

					if(HitSparks){	       	       	
						PartEm.enabled = false;
					}

					if(UseMousePos){
						EndPos = new Vector3(CamMousePos2D.x,CamMousePos2D.y,0);
					}
					else{
						EndPos = ray2.GetPoint(LaserDist);
					}       	        
				}
			}
			///3D Ray
			else{
				if(UseMousePos){
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				}
				else{
					ray = new Ray(StartPoint.position, LaserDir); 
				}
				//////////3D Ray cast

					if (Physics.Raycast(ray, out hit, LaserDist, Layer)){        
						EndPos = hit.point;	 		    

						if(EndFlare){
							EndFlare.enabled = true;

							if(AddEndLight){
								if(EndFlare){
									EndFlare.GetComponent<Light>().enabled = true;		    
								}
							}

							if(EndFlareOffset > 0)
								EndFlare.transform.position = hit.point + hit.normal * EndFlareOffset;
							else
								EndFlare.transform.position = EndPos;
						}		

						////////////Burn & Sparks
						if(UseBurn){
							if(HitSparks && hit.transform.gameObject.GetComponent<Collider>().tag != IgnoreTag){	       	       	
								PartEm.enabled = true;
								HitSparks.transform.position = EndPos + hit.normal * .04f;
								HitSparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
								//////////////Enable Burn Marks
								if(BurnMarks){
									rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                                    Transform BurnClone = Instantiate(BurnMarks, EndPos + hit.normal * EndFlareOffset, rot) as Transform;
									BurnClone.transform.parent = hit.transform;
								}
								//////////Use Laser Color for Sparks
								if(SparkUseLaserColor){
									PartMain = HitSparks.main;
									PartMain.startColor  = LaserColor;
								}
							}
							else{
								if(HitSparks){	       	       	
									PartEm.enabled = false;
								}
							}

						}////End Burn & Sparks          
					}///end 3D Ray with Layer mask

					else{
						if(EndFlare)
							EndFlare.enabled = false;	        

						if(AddEndLight){
							if(EndFlare){
								EndFlare.GetComponent<Light>().enabled = false;
							}
						}

						EndPos = ray.GetPoint(LaserDist);

						if(HitSparks){	       	       	
							PartEm.enabled = false;
						}	        	        	        	        
					}
			}//end Ray           


			Debug.DrawLine (StartPoint.position, EndPos, Color.red);

			//Find Distance
			var dist = Vector3.Distance(StartPoint.position, EndPos);

			//Line Render Positions
			lineRenderer.SetPosition(0,StartPoint.position);
			lineRenderer.SetPosition(1,EndPos);

			//Texture Scroller
			if(UseUVPan){
				lineRenderer.material.SetTextureScale("_Mask", new Vector2(dist/UVTexScale.x, UVTexScale.y));
				lineRenderer.material.SetTextureOffset ("_Mask", new Vector2(TexScrollX*Time.time, TexScrollY*Time.time));
			}

		}
		else{
			if(HitSparks){
                PartEm.enabled = false;
			}
			lineRenderer.enabled = false;

			if(SourceFlare)
				SourceFlare.enabled = false;

			if(EndFlare)
				EndFlare.enabled = false;

			if(AddSourceLight)
				StartPoint.GetComponent<Light>().enabled = false;

			if(AddEndLight)
			if(EndFlare){
				EndFlare.GetComponent<Light>().enabled = false;	
			}	 
		}//end Laser On   

	}//end Update

    private Transform GizmoStartPoint;

    void OnDrawGizmosSelected(){
        if (!StartPoint)
        {
            GizmoStartPoint = gameObject.transform;
        }
        else {
            GizmoStartPoint = StartPoint;
        }

        ////////////
        if (LaserDirection == "x" || LaserDirection == "X")
        {
            LaserDirection = "X";
            LaserDir = GizmoStartPoint.right;
        }
        else if (LaserDirection == "y" || LaserDirection == "Y")
        {
            LaserDirection = "Y";
            LaserDir = GizmoStartPoint.up;
        }
        else if (LaserDirection == "z" || LaserDirection == "Z")
        {
            LaserDirection = "Z";
            LaserDir = GizmoStartPoint.forward;
        }
        else {
            LaserDir = GizmoStartPoint.forward;
        }

        /////////////
        ray = new Ray(GizmoStartPoint.position, LaserDir);
        EndPos = ray.GetPoint(LaserDist);
        Debug.DrawLine(GizmoStartPoint.position, EndPos, LaserColor);
    }//End Select

}//End gizmo