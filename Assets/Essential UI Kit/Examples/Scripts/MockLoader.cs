using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA.Examples{
	public class MockLoader : MonoBehaviour {
		public ImageFillEaser loadingImage;
		public Text loadingText;
		public LoadingStep[] steps = new LoadingStep[1];
		public float interval = 2f;
		private int counter = 1;

		void Start()
		{
			loadingText.text = steps[0].loadMessage;
			loadingImage.animateFill(1f/(float)steps.Length, 0.3f);
			Scheduler.createTask(
			()=>{
				loadingText.text = steps[counter].loadMessage;
				loadingImage.animateFill((float)(counter+1)/(float)steps.Length, 0.3f);
				counter++;
				if(counter >= steps.Length) Scheduler.createTask(()=>{Application.LoadLevel("MainMenue");}, 0.5f, 1);
			},
			interval, steps.Length-1);
		}
		[System.Serializable]
		public class LoadingStep
		{
			public string loadMessage;
		}
	}
}
