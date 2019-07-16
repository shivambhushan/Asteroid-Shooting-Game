using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	public class SpriteSwapToggle : MonoBehaviour {
		[SerializeField]
		[GetOrAddComponent]
		private Toggle toggle;
		[Header("The Image that should be swapped")]
		[GetOrAddComponent(true, false)]
		[SerializeField]
		private Image toggleImage;
		[SerializeField]
		private Sprite OnSprite;
		[SerializeField]
		private Sprite OffSprite;
		
		void Start () {
			//Get initial State
			toggleImage.sprite = toggle.isOn ? OnSprite : OffSprite;
			toggle.onValueChanged.AddListener((on)=>{
				toggleImage.sprite = on ? OnSprite : OffSprite;
			});
		}
	}
}
