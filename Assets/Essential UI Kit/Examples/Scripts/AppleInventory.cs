using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MA;

namespace MA.Examples{
	/// <summary>
	/// A simple Inventory Example. FruitInventory can know easily 
	/// access all the FruitItems that it stores. It will only accept
	/// FruitItems (or Classes inheriting from FruitItem) and will get 
	/// notified when it loses or gains a new FruitItem
	/// </summary>
	public class AppleInventory : BaseInventory<Apple> {
		public Typer typer;
		public int total;
		public int average;
		public List<Apple> fruits = new List<Apple>();


		protected override void addedNewItem (Apple newItem)
		{
			typeInventory();
		}
		protected override void lostItem ()
		{
			typeInventory();
		}
		private void typeInventory()
		{
			fruits.Clear();
			fruits.AddRange(items.Values);
			total = 0;
			string fruittext = "Stock: ";
			foreach(Apple f in fruits)
			{
				fruittext += f.appleName + ", ";
				total += f.cost;
			}
			fruittext = fruittext.Remove(fruittext.Length-2);
			average = (fruits.Count > 0) ? total/fruits.Count : 0;
			fruittext += System.Environment.NewLine + "Total: " + total.ToString() + System.Environment.NewLine + "Average: " + average.ToString();
			typer.ReplaceText(fruittext);
		}
		
	}
}
