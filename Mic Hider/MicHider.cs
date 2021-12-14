using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
namespace Mic_Hider
{
    public class MicHider: MelonMod
    {
        GameObject voiceDot;
        Graphic voiceDotGraphic;
        bool hideMicBool = true;
        bool showOnTalkBool = false;
        bool okayToRun = false;
        MelonPreferences_Entry<bool> hideMic;
        MelonPreferences_Entry<bool> showOnTalk;

        public override void OnApplicationStart()
        {
            var category = MelonPreferences.CreateCategory("MicHider", "Mic Hider");
            hideMic = category.CreateEntry("hideMic", true, "Hide mic", "Hide mic icon when not muted");
            hideMic.OnValueChanged += (_, v) =>
            {
                hideMicBool = v;
                setMicState();
            };
            hideMicBool = hideMic.Value;
            showOnTalk = category.CreateEntry("showOnTalk", false, "Show on talk", "Show icon when when you talk");
            showOnTalk.OnValueChanged += (_, v) =>
            {
                showOnTalkBool = v;
                setMicState();
            };
            showOnTalkBool = showOnTalk.Value;
        }

        private void setMicState()
        {
            if (hideMicBool)
            {
                voiceDot.transform.localScale = Vector3.zero;
                MelonLogger.Msg("Voicedot set to vanished!");
            } else
            {
                
                voiceDot.transform.localScale = Vector3.one;
                MelonLogger.Msg("Voicedot set to Visible!");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "ui")
            {
                voiceDot = GameObject.Find("VoiceDot");
                voiceDotGraphic = voiceDot.GetComponent<Image>().GetComponent<Graphic>();
                setMicState();
                okayToRun = true;
            }

        }
        public override void OnUpdate()
        {
            if (okayToRun && showOnTalkBool)
            {
                if (voiceDotGraphic.color.a > 0.5)
                {
                    voiceDot.transform.localScale = Vector3.one;
                }
                else
                {
                    voiceDot.transform.localScale = Vector3.zero;
                }
            }
        }
    }
}
