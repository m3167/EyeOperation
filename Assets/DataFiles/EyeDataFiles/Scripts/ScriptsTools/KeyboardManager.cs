using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public InputField userNameSignIn_InputField;
    public InputField passwordSignIn_InputField;
    public InputField userNameSignUp_InputField;
    public InputField passwordSignUp_InputField;
    public InputField confirmPasswordSignUp_InputField;
    public InputField emailSignUp_InputField;
    public InputField mobileSignUp_InputField;
    public InputField sessionName_InputField;
    public InputField maxStudents_InputField;
    public InputField sessionID_InputField;
    public InputField oldPasswordPanel_InputField;
    public InputField newPasswordPanel_InputField;
    public InputField confirmNewPasswordPanel_InputField;

    public GameObject keyboard;

    void Update()
    {
        //If the input field is focused
        if (userNameSignIn_InputField.isFocused == true)
        {
            ShowAndFocusInputField(userNameSignIn_InputField);
        }

        else if (passwordSignIn_InputField.isFocused == true)
        {
            ShowAndFocusInputField(passwordSignIn_InputField);
        }

        else if (userNameSignUp_InputField.isFocused == true)
        {
            ShowAndFocusInputField(userNameSignUp_InputField);
        }

        else if (passwordSignUp_InputField.isFocused == true)
        {
            ShowAndFocusInputField(passwordSignUp_InputField);
        }

        else if (confirmPasswordSignUp_InputField.isFocused == true)
        {
            ShowAndFocusInputField(confirmPasswordSignUp_InputField);
        }

        else if (emailSignUp_InputField.isFocused == true)
        {
            ShowAndFocusInputField(emailSignUp_InputField);
        }

        else if (mobileSignUp_InputField.isFocused == true)
        {
            ShowAndFocusInputField(mobileSignUp_InputField);
        }

        else if (sessionName_InputField.isFocused == true)
        {
            ShowAndFocusInputField(sessionName_InputField);
        }
        else if (maxStudents_InputField.isFocused == true)
        {
            ShowAndFocusInputField(maxStudents_InputField);
        }

        else if (sessionID_InputField.isFocused == true)
        {
            ShowAndFocusInputField(sessionID_InputField);
        }
        else if (oldPasswordPanel_InputField.isFocused == true)
        {
            ShowAndFocusInputField(oldPasswordPanel_InputField);
        }

        else if (newPasswordPanel_InputField.isFocused == true)
        {
            ShowAndFocusInputField(newPasswordPanel_InputField);
        }
        else if (confirmNewPasswordPanel_InputField.isFocused == true)
        {
            ShowAndFocusInputField(confirmNewPasswordPanel_InputField);
        }
    }

    public void ShowAndFocusInputField(InputField inputField)
    {
        keyboard.transform.GetChild(0).GetComponent<VRKeyboard>().AttachedInputField = inputField;
        keyboard.gameObject.SetActive(true);
    }

    public void HideKeyboard()
    {
        //If the input field is not focused
        userNameSignIn_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        passwordSignIn_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        userNameSignUp_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        passwordSignUp_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        confirmPasswordSignUp_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        emailSignUp_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        mobileSignUp_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        sessionName_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        maxStudents_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        sessionID_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        oldPasswordPanel_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        newPasswordPanel_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        confirmNewPasswordPanel_InputField.OnDeselect(new BaseEventData(EventSystem.current));
        keyboard.gameObject.SetActive(false);
    }
}