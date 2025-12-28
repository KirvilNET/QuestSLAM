using UnityEngine;
using UnityEngine.UIElements;

using System.Net;
using System.Net.Sockets;

namespace QuestSLAM.UI
{
    public class UIManager: MonoBehaviour
    {
        #region Fields
        [SerializeField] private UIDocument ui;
        #endregion

        #region UI components

        /// <summary>Root VisualElement container</summary>
        private VisualElement root;

        /// <summary>MainUI VisualElement container</summary>
        private VisualElement main;

        /// <summary>VisualElemet that contains all of the text</summary>
        private VisualElement textcontainer;

        /// <summary>Label that shows the Quest's IP address</summary>
        private Label IPText;

        /// <summary>Label that shows the Dashboard message</summary>
        private Label dashboard;

        /// <summary>Label that shows the Version of QuestSLAM</summary>
        private Label version;

        private string ver;

        #endregion

        public void Init(string appVersion)
        {

            ver = appVersion;

            root = ui.rootVisualElement.Q<VisualElement>("root");
            main = root.Q<VisualElement>("main");
            textcontainer = main.Q<VisualElement>("textcontainer");

            dashboard = textcontainer.Q<Label>("dashboard");
            version = textcontainer.Q<Label>("version");
            IPText = textcontainer.Q<Label>("IP");

            UpdateUI();
        }

        public void UpdateUI()
        {
            version.text = $"v{ver}";

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (ip.ToString() == "127.0.0.1")
                    {
                        IPText.text = "No Adapter Found";
                        dashboard.text = "Dashboard being hosted on http://localhost:9234";
                    } 
                    else
                    {
                        IPText.text = $"IP: {ip}";
                        dashboard.text = $"Dashboard being hosted on http://{ip}:9234";
                    }
                }
            }
        }
    }
}
