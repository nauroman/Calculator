using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class BInputField : MonoBehaviour
{
	public string characters = "0123456789.()+-/*= ";
	public bool doNotHandleEvent;

	int caretPosition = -1;

	void Start ()
	{
		// Restore InputField text
		Load ();
		Correct ();
	}

	void Load ()
	{
		GetComponent<InputField> ().text = PlayerPrefs.GetString ("calc", "");
	}

	void Save ()
	{

		PlayerPrefs.SetString ("calc", GetComponent<InputField> ().text);
		PlayerPrefs.Save ();

	}

	public void OnValueChange ()
	{
		if (!doNotHandleEvent) {

			// Clear and correct InputField text
			Correct ();

			// Save InputField text to restore it on next app start
			Save ();
		}
	}

	/** Delete the last symbol or symbol at caret position
	 */
	public void OnClickButtonDelete ()
	{
		var inputField = GetComponent<InputField> ();
		
		string str = inputField.text;

		str = RemoveTextAfterEqual (str);
		str = RemoveInvalidCharacters (str);
        
		if (str.Length > 0) {
			// If we have right caret position then delete symbol there
			if (caretPosition > 0 && caretPosition < str.Length) {
				str = str.Remove (caretPosition - 1, 1);
				caretPosition--;
			} else {
				str = RemoveInvalidCharacters (str);
				str = str.Remove (str.Length - 1);
				caretPosition = -1;
			}
		}

		if (str.Length == 0) {
			caretPosition = -1;
		}
        
		inputField.text = str;
	}

	/** Remove all text
	 */ 
	public void OnClickButtonClear ()
	{
		GetComponent<InputField> ().text = "";
		caretPosition = -1;
	}

	void Update ()
	{
		UpdateCaretPosition ();
	}

	/** Save caret position to use it for Delete a symbol or to add a new symbol
	 */ 
	void UpdateCaretPosition ()
	{
		var inputField = GetComponent<InputField> ();
		
		if (inputField.isFocused) {			
			if (inputField.caretPosition < inputField.text.Length) {
				
				caretPosition = inputField.caretPosition;
			} else {
				caretPosition = -1;
			}
		}
        
		if (inputField.text.Length == 0) {
			caretPosition = -1;
		}
	}


	public void OnClickButton (string name)
	{
		var inputField = GetComponent<InputField> ();

		string str = inputField.text;

		// Remove the result from Input Field text
		str = RemoveTextAfterEqual (str);

		// Insert a symbol
		if (caretPosition >= 0 && caretPosition < str.Length) {
			str = str.Insert (caretPosition, name);
			caretPosition++;
		} else {
			str += name;
		}

		inputField.text = str;
	}


	static public string RemoveTextAfterEqual (string str)
	{
		int indexOfEqual = str.IndexOf ("=");
		
		if (indexOfEqual >= 0) {
			str = str.Remove (indexOfEqual);
		}

		return str;
	}

	/** Correct text and remove invalid characters
	 */
	void Correct ()
	{
		var inputField = GetComponent<InputField> ();

		string str = inputField.text;

		str = RemoveTextAfterEqual (str);

		str = RemoveInvalidCharacters (str);

		inputField.text = str;
	}

	string RemoveInvalidCharacters (string str)
	{
		for (int i=0; i>=0 && i<str.Length; i++) {
			if (characters.IndexOf (str [i]) == -1) {
				str = str.Remove (i, 1);
				i--;
			}
		}

		return str;
	}
}
