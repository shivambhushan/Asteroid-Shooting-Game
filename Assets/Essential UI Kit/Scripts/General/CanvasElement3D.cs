using UnityEngine;
using System.Collections;
using MA.EaseNTween;


namespace MA{
	/// <summary>
	/// This Component takes care of scaling a (child) 3D Object with a Mesh Renderer
	/// to fit a Rect Transform. The 3D Object will be uniformly scaled and can
	/// either fit the height, width or bounds of the parent Rect Transform
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class CanvasElement3D : UnityEngine.EventSystems.UIBehaviour{
		public bool autoUpdate = true;
		[SerializeField]
		private bool centerMesh = true;
		[SerializeField]
		private Vector3 offset = Vector3.zero;
		[Header("Should the child mesh be scaled to fit the Rect height, width or to always stay within Bounds?")]
		[SerializeField]
		private Fit fitHeightOrWidth = Fit.height;
		public bool eased = true;
		[SerializeField]
		private EasingTypes easing = EasingTypes.QuarticOut;
		[SerializeField]
		private float animationLength = 1f;
		private Coroutine scaleRoutine;
		private bool scaling = false;
		private MeshRenderer meshrenderer;
		[HideInInspector]
		public Transform meshtransform;
		private RectTransform recttransform;
		private Vector3[] cornersArray = new Vector3[4];
		private bool initialized = false;

		enum Fit{
			height, width, best
		}
		protected override void Awake()
		{
			initialize();
		}
		public void initialize()
		{
			recttransform = transform as RectTransform;
			meshrenderer = GetComponentInChildren<MeshRenderer>();
			if(meshrenderer == null)
			{
				return;
			}else
			{
				TweenManager.instance.GetInstanceID();
				meshtransform = meshrenderer.gameObject.transform;
				initialized = true;
				scaleMesh();
			}
		}
		//Gets somehow called before Awake
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if(initialized && autoUpdate) scaleMesh();
		}
		public void scaleMesh()
		{	
			if(centerMesh)
			{
				meshtransform.localPosition = Vector3.zero;
				meshtransform.localPosition += offset;
			}
			Vector3 targetScale = meshtransform.localScale * fittingScaleFactor();
			if(eased && TweenManager.HasInstance)
			{
				if(scaling)
				{
					TweenManager.instance.stopTween(scaleRoutine);
				}
				scaleRoutine = meshtransform.ScaleTo(targetScale, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{scaling = false;});
			}else meshtransform.localScale = targetScale;
		}
		public float fittingScaleFactor()
		{
			Vector2 scaleFactor = calculateScaleFactor();
			float factor;
			switch (fitHeightOrWidth) {
			case Fit.height:
				factor = scaleFactor.y;
				break;
			case Fit.width:
				factor = scaleFactor.x;
				break;
			case Fit.best:
				factor = Mathf.Min(scaleFactor.x, scaleFactor.y);
				break;
			default:
				throw new System.ArgumentOutOfRangeException ();
			}
			if(float.IsNaN(factor) || float.IsInfinity(factor)) return 1f;
			else return factor;
		}
		private Vector2 calculateScaleFactor()
		{
			Vector3 meshBoundSize = meshrenderer.bounds.size;
			Vector3 desiredBoundSize = getBoundsForRectTransform().size;
			return new Vector2(desiredBoundSize.x/ meshBoundSize.x, desiredBoundSize.y/meshBoundSize.y);
		}
		private Bounds getBoundsForRectTransform()
		{
			Bounds bounds = new Bounds(transform.position, Vector3.zero);
			recttransform.GetWorldCorners(cornersArray);
			foreach(Vector3 point in cornersArray)
			{
				bounds.Encapsulate(point);	
			}
			return bounds;
		}
	}
}
