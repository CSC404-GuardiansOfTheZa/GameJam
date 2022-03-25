using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : FadableText {
	protected void Start() {
		base.Start();
		this.textField.text = "";
		this.textField.color = Color.clear;
	}
}
