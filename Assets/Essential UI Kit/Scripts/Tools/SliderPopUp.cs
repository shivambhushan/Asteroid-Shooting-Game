using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	/// <summary>
	/// Slider pop up. Expects a Text Component somewhere
	/// as a child of the Slider, usually as a child of 
	/// the Handle
	/// </summary>
	[RequireComponent(typeof(Slider))]
	public class SliderPopUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, MNumberFormat {

		[SerializeField] private Animator m_popupAnimator;
		private Text m_text;
		[SerializeField] private string animatorBoolAlias = "show";
		[Range(0,20)]
		[SerializeField] private int numbersBehindComma = 3;
		[SerializeField] private string appendString = " €";
		private Slider m_slider;
		private string format;

		void Awake () {
			//initialize string formatting
			NumbersBehindComma = numbersBehindComma;
			m_slider = GetComponent<Slider>();
			m_text = m_slider.handleRect.GetComponentInChildren<Text>();
			m_text.text = string.Format(format, m_slider.value);
			if(!string.IsNullOrEmpty(appendString)) m_text.text += appendString;
			m_slider.onValueChanged.AddListener((x) => {
				m_text.text = string.Format(format, x);
				if(!string.IsNullOrEmpty(appendString)) m_text.text += appendString;
			});
		}
		#region IPointerDownHandler implementation

		public void OnPointerDown (PointerEventData eventData)
		{
			m_popupAnimator.SetBool(animatorBoolAlias, true);
		}

		#endregion

		#region IPointerUpHandler implementation

		public void OnPointerUp (PointerEventData eventData)
		{
			m_popupAnimator.SetBool(animatorBoolAlias, false);
		}

		#endregion

		public int NumbersBehindComma {
			get {
				return numbersBehindComma;
			}
			set {
				numbersBehindComma = value;
				format = this.formatNumbersBehindComma(value);
			}
		}
	}
}
