using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// Ui Button color animation controller
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UiButtonColorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {

        /// <summary>
        /// Loop seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Loop seconds")]
        protected float m_loopSeconds = 1.0f;

        /// <summary>
        /// Additive color value
        /// </summary>
        [SerializeField]
        [Tooltip("Additive color value")]
        [Range(-1.0f, 1.0f)]
        protected float m_additiveColorValue = -0.2f;

        /// <summary>
        /// Reference to Image
        /// </summary>
        Image m_refImage = null;

        /// <summary>
        /// Original color
        /// </summary>
        Color m_oriColor = Color.white;

        /// <summary>
        /// Target color
        /// </summary>
        Color m_targetColor = Color.white;

        /// <summary>
        /// changeColor IEnumerator
        /// </summary>
        IEnumerator m_changeColorIE = null;

        /// <summary>
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------------------------
        void Start()
        {

            this.m_refImage = this.GetComponent<Image>();
            this.m_oriColor = this.m_refImage.color;

            this.m_targetColor = this.m_oriColor;
            this.m_targetColor.r = Mathf.Clamp01(this.m_targetColor.r + this.m_additiveColorValue);
            this.m_targetColor.g = Mathf.Clamp01(this.m_targetColor.g + this.m_additiveColorValue);
            this.m_targetColor.b = Mathf.Clamp01(this.m_targetColor.b + this.m_additiveColorValue);

        }


        /// <summary>
        /// OnPointerEnter
        /// </summary>
        /// <param name="eventData">PointerEventData</param>
        // --------------------------------------------------------------------------------------------
        public void OnPointerEnter(PointerEventData eventData)
        {

            //this.stopChangeColor();
            //StartCoroutine(this.m_changeColorIE = this.changeColor());

            // SetSelectedGameObject
            {

                if (EventSystem.current && EventSystem.current.currentSelectedGameObject != this.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(this.gameObject);
                }
            }

        }

        /// <summary>
        /// OnSelect
        /// </summary>
        /// <param name="eventData">BaseEventData</param>
        // --------------------------------------------------------------------------------------------
        public void OnSelect(BaseEventData eventData)
        {
            this.stopChangeColor();
            StartCoroutine(this.m_changeColorIE = this.changeColor());
        }

        /// <summary>
        /// OnPointerExit
        /// </summary>
        /// <param name="eventData">PointerEventData</param>
        // --------------------------------------------------------------------------------------------
        public void OnPointerExit(PointerEventData eventData)
        {
            //this.stopChangeColor();

            if (EventSystem.current && EventSystem.current.currentSelectedGameObject == this.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

        }

        /// <summary>
        /// OnDeselect
        /// </summary>
        /// <param name="eventData">BaseEventData</param>
        // --------------------------------------------------------------------------------------------
        public void OnDeselect(BaseEventData eventData)
        {
            this.stopChangeColor();
        }

        /// <summary>
        /// Stop changeColor
        /// </summary>
        // --------------------------------------------------------------------------------------------
        void stopChangeColor()
        {

            if (this.m_changeColorIE != null)
            {
                this.m_refImage.color = this.m_oriColor;
                StopCoroutine(this.m_changeColorIE);
                this.m_changeColorIE = null;
            }

        }

        /// <summary>
        /// Calc
        /// </summary>
        /// <param name="x01">x</param>
        /// <returns>result</returns>
        // --------------------------------------------------------------------------------------------
        float calcValue(float x01)
        {
            return 1.0f - (2 * Mathf.Abs(x01 - 0.5f));
        }

        /// <summary>
        /// Change color
        /// </summary>
        /// <returns>IEnumerator</returns>
        // --------------------------------------------------------------------------------------------
        IEnumerator changeColor()
        {

            if(this.m_loopSeconds <= 0.0f)
            {
                yield break;
            }

            // -------------------

            float time = 0.0f;

            // -------------------

            for (; ; )
            {

                this.m_refImage.color = Color.Lerp(this.m_oriColor, this.m_targetColor, this.calcValue(time / this.m_loopSeconds));

                time = (time + Time.deltaTime) % this.m_loopSeconds;

                yield return null;

            }

        }

    }

}
