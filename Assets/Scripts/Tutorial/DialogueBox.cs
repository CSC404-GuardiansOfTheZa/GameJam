using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : FadableText {
	protected void Start() {
		base.Start();
		this.tmp.text = "";
		this.tmp.color = Color.clear;
	}
}
