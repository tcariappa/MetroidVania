//Unity documentation on rich text: https://docs.unity3d.com/Manual/StyledText.html

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
	static public DialogManager Me;

	[Header("EDIT IN PREFAB ONLY!")]
	[SerializeField]
	Dialog[] dialogs;

	public bool isPlayingDialogOnPause { get; private set; }

	UIDialogReferencesHolder uiReferencesHolder;
	bool isActionPressed;
	Vector3 localPosPortraitLeft, localPosPortraitRight;


	// Use this for initialization
	void Awake()
	{
		Me = this;
		isPlayingDialogOnPause = false;
		uiReferencesHolder = FindObjectOfType<UIDialogReferencesHolder>();
		uiReferencesHolder.textField.enabled = false;
		uiReferencesHolder.portraitImage.enabled = false;

		//prepare left and right positions for the picture
		localPosPortraitLeft = uiReferencesHolder.portraitImage.transform.localPosition;
		localPosPortraitRight = new Vector3(-localPosPortraitLeft.x, localPosPortraitLeft.y, localPosPortraitLeft.z);
	}


	public void playDialogByName(string upgradeName)
	{
		foreach (Dialog dialog in dialogs)
		{
			if (dialog.upgradeName == upgradeName)
			{
				startDialog(dialog);
				return;
			}
		}

		Debug.LogError("Error: There is no dialog with name: " + upgradeName);//DEBUG
	}


	public void playDialogByIndex(int index)
	{
		if (index < 0 || index >= dialogs.Length)
		{
			Debug.LogError("Error: There is no dialog with index " + index);//DEBUG
			return;
		}

		startDialog(dialogs[index]);
	}


	void startDialog(Dialog dialog)
	{
		bool isListedInUpgrades = UpgradesManager.List.ContainsKey(dialog.upgradeName) ? true : false;
		StartCoroutine(coPlayDialog(dialog, isListedInUpgrades));
	}


	IEnumerator coPlayDialog(Dialog dialog, bool isUpgrade)
	{
		MiscInputsManager.OnPressAction += handleOnPressAction;
		uiReferencesHolder.textField.enabled = true;
		print("play dialog named " + dialog.upgradeName);//TEST

		//GameManager.Me.enterDialogContext();
		Time.timeScale = 0f;
		isPlayingDialogOnPause = true;

		for (int i = 0; i < dialog.lines.Length; i++)
		{
			//we display the dialog line and picture
			uiReferencesHolder.textField.text = dialog.lines[i].text;
			if (dialog.lines[i].portrait != null)
			{
				uiReferencesHolder.portraitImage.enabled = true;
				uiReferencesHolder.portraitImage.sprite = dialog.lines[i].portrait;
				uiReferencesHolder.portraitImage.transform.localPosition = dialog.lines[i].isPortraitLeft ? localPosPortraitLeft : localPosPortraitRight;
			}

			//if dialog line must wait for user input...
			if (dialog.lines[i].duration <= 0)
			{
				while (!isActionPressed)
				{
					yield return null;
				}
				isActionPressed = false;
			}
			//if dialog line has a specific duration, we display it during this time
			else
			{
				yield return new WaitForSecondsRealtime(dialog.lines[i].duration);
			}

			//after each dialog we clear the text field (and picture) and wait for a brief time before displaying the next line
			uiReferencesHolder.textField.text = String.Empty;
			uiReferencesHolder.portraitImage.enabled = false;
			yield return new WaitForSecondsRealtime(0.15f);
		}

		Time.timeScale = 1f;
		isPlayingDialogOnPause = false;

		MiscInputsManager.OnPressAction -= handleOnPressAction;
		uiReferencesHolder.textField.enabled = false;

		//save the related Upgrade if needed so that the dialog will never play again
		if (isUpgrade)
			UpgradesManager.DoOnUpgradePicked(dialog.upgradeName);
	}


	void handleOnPressAction()
	{
		if (isPlayingDialogOnPause)
			isActionPressed = true;
	}
}


[Serializable]
public struct Dialog
{
	public string upgradeName;
	public DialogLine[] lines;

	public Dialog(DialogLine[] dialogLines, string nameInUpgradeList)
	{
		upgradeName = nameInUpgradeList;
		lines = dialogLines;
	}
}


[Serializable]
public struct DialogLine
{
	[TextArea]
	public string text;
	public float duration;
	public Sprite portrait;
	public bool isPortraitLeft;

	public DialogLine(string dialogLine, float displayDuration = 0, Sprite pictureSprite = null, bool isPictureOnTheLeft = true)
	{
		text = dialogLine;
		duration = displayDuration;
		portrait = pictureSprite;
		isPortraitLeft = isPictureOnTheLeft;
	}
}
