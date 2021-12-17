using System.Collections;
using MelonLoader;
using UnityEngine;

namespace Mic_Hider
{
    public class MicHider : MelonMod
    {

        private MelonPreferences_Entry<bool> hideMic;
        private MelonPreferences_Entry<bool> showOnTalk;
        private HudVoiceIndicator hudVoiceIndicator;
        private Color originalEnabledColor;
        private Color originalDisabledColor;

        public override void OnApplicationStart()
        {
            var category = MelonPreferences.CreateCategory("MicHider", "Mic Hider");
            hideMic = category.CreateEntry("hideMic", true, "Hide mic", "Hide mic icon when not muted");
            hideMic.OnValueChanged += (_, v) => UpdateMicState(v, showOnTalk.Value);
            showOnTalk = category.CreateEntry("showOnTalk", false, "Show on talk", "Show icon when you talk");
            showOnTalk.OnValueChanged += (_, v) => UpdateMicState(hideMic.Value, v);

            MelonCoroutines.Start(Init());
        }

        private IEnumerator Init()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null) yield return null;

            hudVoiceIndicator = GameObject.Find("UserInterface/UnscaledUI/HudContent").GetComponent<HudVoiceIndicator>();
            originalEnabledColor = hudVoiceIndicator.field_Private_Color_0;
            originalDisabledColor = hudVoiceIndicator.field_Private_Color_1;

            UpdateMicState(hideMic.Value, showOnTalk.Value);
        }

        private void UpdateMicState(bool shouldHideMic, bool shouldShowOnTalk)
        {
            if (shouldHideMic)
            {
                if (!shouldShowOnTalk)
                    hudVoiceIndicator.field_Private_Color_0 = new Color(0, 0, 0, 0);
                else
                    hudVoiceIndicator.field_Private_Color_0 = originalEnabledColor;

                hudVoiceIndicator.field_Private_Color_1 = new Color(0, 0, 0, 0);

                MelonLogger.Msg("Voicedot is now hidden (" + (shouldShowOnTalk ? "visible while talking" : "completely") + ").");
            }
            else
            {
                hudVoiceIndicator.field_Private_Color_0 = originalEnabledColor;
                hudVoiceIndicator.field_Private_Color_1 = originalDisabledColor;

                MelonLogger.Msg("Voicedot is now visible.");
            }
        }
    }
}
