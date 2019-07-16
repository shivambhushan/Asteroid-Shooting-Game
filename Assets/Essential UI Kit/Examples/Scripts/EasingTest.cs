using UnityEngine;
using System.Collections;
//always include this namespace when using easing.
using MA.EaseNTween;

namespace MA.Examples{
	public class EasingTest : MonoBehaviour {
		public float MyEasedFloat;
		public float startvalue;
		public float endvalue = 2f;
		public float animationtime = 2f;
		[Header("Press A or B to tween the Object to DestinationPosition")]
		public Vector3 destinationPosition = Vector3.zero;
		public EasingTypes easing;
		private float elapsedtime = 0f;
		private Coroutine tweenRoutine;
		
		void Update () {
			//time that has passed since the game started
			elapsedtime += Time.deltaTime;
			//Reset the elapsedtime to 0 when it gets bigger than animationTime
			elapsedtime = Mathf.Repeat(elapsedtime, animationtime);

			//Keep Progress between 0 and 1
			float progress = elapsedtime/animationtime;

			//The actual easing: Like Mathf.Lerp it returns a value interpolated between start and end value according to progress (third parameter)
			//to see it animated you have to use it in the Update Loop 
			MyEasedFloat = Easing.QuarticIn(startvalue, endvalue, progress);

			//You can also hand over control to the Tween Manager so that the animation is handled for you
			if(Input.GetKeyDown(KeyCode.A)) ExtensionMethodTween();
			if(Input.GetKeyDown(KeyCode.B)) AllPurposeTween();
		}
		/// <summary>
		/// The Essential UI Kit comes with a number of Extension Methods for often animated Properties
		/// like transform position, scale etc to make writing a Tween super short.
		/// </summary>
		private void ExtensionMethodTween()
		{
			stopTweenIfExists();
			//Move transform to destinationPosition in animationtime using the given easing function
			tweenRoutine = transform.MoveTo(destinationPosition, animationtime, easing);
		}
		/// <summary>
		/// This is the exact same Tween as above but you can use it to ease any float, Vector2, Vector3
		/// or Color
		/// </summary>
		private void AllPurposeTween()
		{
			stopTweenIfExists();
			//Create a Tween for the animation we want. The first Parameter is
			//the trickiest one. You receive the updated(eased) Vector3 updatedPosition
			//from the TweenManager and now you just have to specify which other Vector3
			//you want updated, in this case the transform.position vector.
			//transform.position is the starting value and destination position the end value. 
			//The tween will take animationtime and use the given easing function.
			Tween tween = new Tween((Vector3 updatedPosition)=>{
				transform.position = updatedPosition;
			}, transform.position, destinationPosition, animationtime, easing);
			//actually start the animation. The TweenManager will return a Coroutine which can be used to stop the animation
			tweenRoutine = TweenManager.instance.playTween(tween);
		}
		/// <summary>
		/// if we already are animating with a Tween stop that Tween first
		/// </summary>
		private void stopTweenIfExists()
		{
			if(tweenRoutine != null) TweenManager.instance.stopTween(tweenRoutine);
		}
	}
}
