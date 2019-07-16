using UnityEngine;
using System;
using System.Collections;

namespace UnityEngine.EventSystems{

	public interface IlostItem : IEventSystemHandler
	{
		void lostItem(MA.IDroppableItem item);
	}
	public interface IgainedItem : IEventSystemHandler
	{
		//pass the Droppable Item as well as it's intended target, deferring the Decision wheter to return the Item 
		//or to accept it to the interface Implementer 
		void gainedItem(MA.IDroppableItem item, MA.IItemDropTarget targetSlot);
	}
	public interface ILevelChangeRequest : IEventSystemHandler
	{
		void changeLevelTo(int levelID);
		void changeLevelTo(string levelName);
	}
	public interface IListElementSelected : IEventSystemHandler
	{
		void elementSelected(MA.ListElement elem);
	}
	public interface IChangePage : IEventSystemHandler
	{
		void changeToPage(int index);
		bool changeUp();
		bool changeDown();
	}
}
