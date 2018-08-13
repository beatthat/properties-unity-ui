using UnityEngine;
using UnityEngine.UI;

namespace BeatThat.Properties.UnityUI
{
    [RequireComponent(typeof(InputField))]
	public class InputFieldTextInput : HasTextInput 
	{
		public InputField m_inputField;

		public override void ActivateInput ()
		{
			this.inputField.ActivateInputField();
		}

		override  protected string GetValue()
		{
			return this.inputField.text;
		}

		override protected void _SetValue(string s)
		{
			this.inputField.text = s;
		}

		public InputField inputField
		{
			get {
				if(m_inputField == null) {
					m_inputField = GetComponent<InputField>();
				}
				return m_inputField;
			}
		}

	}
}

