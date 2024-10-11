using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace DemoUtils.CanvasUtils.Scripts {
    public class MailSender : MonoBehaviour {
        [SerializeField] 
        public string subject = "Customer support";

        [Header("Emails")] 
        public string iosEmail;
        public string androidEmail;
        private string _email;
        
        [Header("Email message")]
        public string message = "";

        [Header("Copyrights")] 
        public string iosCopyright;
        public string androidCopyright;

        [SerializeField] 
        public Button senderButton;
        public Text CopyrightText;

        private void Awake() {
            #if UNITY_IOS
		        _email = iosEmail;
                if (CopyrightText != null) {
                    CopyrightText.text = iosCopyright;
                }
            #elif UNITY_ANDROID
		        _email = androidEmail.Length > 0 ? androidEmail : iosEmail;
                if (CopyrightText != null) {
                    CopyrightText.text = androidCopyright.Length > 0 ? androidCopyright : iosCopyright;
                }
            #endif
            if (senderButton != null) {
                senderButton.onClick.AddListener(SendEmail);
            }
        }

        public void SendEmail() {
            #if UNITY_IOS || UNITY_EDITOR
                Application.OpenURL($"mailto:{_email}?subject={MyEscapeURL(subject)}&body={MyEscapeURL(message)}");
            #elif UNITY_ANDROID
                Application.OpenURL($"mailto:{_email}?subject={MyEscapeURL(subject)}&body={MyEscapeURL(message)}");
            #endif
        }

        static string MyEscapeURL(string url) {
            return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
        }
    }
}