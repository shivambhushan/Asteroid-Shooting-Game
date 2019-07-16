using UnityEngine;
using System;

namespace MA{
	public interface IModal{
		//this should be set from the modalManager when setting up the Modal
		GameObject modalObject
		{ 
			get; 
			set; 
		}

		void display();
		void ModifyModal(string maintext);
		void ModifyModal(string maintext, Action onAccept, bool overwriteCallbacks);
		void ModifyModal(string maintext, Action onAccept, Action onRefuse, bool overwriteCallbacks);
		void ModifyModal(Action onCancel, bool overwriteCallbacks);
		/*
		 * use this for an "unusual" ModalSetup where the implementing function will have to make sense
		 * of which text goes where and what Actions to call
		 */
		void ModifyModal(string[] texts, bool overwriteCallbacks, params Action[] actions);
		void ModifyModalImage(Sprite image);
		void setupHeader(string headline);

		void hide();
	}
}
