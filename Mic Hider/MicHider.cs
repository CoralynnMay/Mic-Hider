using System.Collections;
using MelonLoader;
using UnityEngine;

namespace Mic_Hider
{
    public class MicHider : MelonMod
    {

        private MelonPreferences_Entry<bool> hideMic;
        private MelonPreferences_Entry<bool> showOnTalk;
        private MelonPreferences_Entry<bool> hideMuteIcon;
        private HudVoiceIndicator hudVoiceIndicator;
        private Color originalEnabledColor;
        private Color originalDisabledColor;
        private FadeCycleEffect fadeCycleEffect;
        private float originalMuteFadeLow;
        private float originalMuteFadeHigh;

        public override void OnApplicationStart()
        {
            var category = MelonPreferences.CreateCategory("MicHider", "Mic Hider");
            hideMic = category.CreateEntry("hideMic", true, "Hide mic", "Hide mic icon when not muted");
            hideMic.OnValueChanged += (_, v) => UpdateMicState(v, showOnTalk.Value, hideMuteIcon.Value);
            showOnTalk = category.CreateEntry("showOnTalk", false, "Show on talk", "Show icon when you talk");
            showOnTalk.OnValueChanged += (_, v) => UpdateMicState(hideMic.Value, v, hideMuteIcon.Value);
            hideMuteIcon = category.CreateEntry("hideMuteIcon", false, "HideMuteIcon", "Hide Mute Icon");
            hideMuteIcon.OnValueChanged += (_, v) => UpdateMicState(hideMic.Value, showOnTalk.Value, v);

            MelonCoroutines.Start(Init());
        }

        private IEnumerator Init()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null) yield return null;

            hudVoiceIndicator = GameObject.Find("UserInterface/UnscaledUI/HudContent_Old").GetComponent<HudVoiceIndicator>();
            originalEnabledColor = hudVoiceIndicator.field_Private_Color_0;
            originalDisabledColor = hudVoiceIndicator.field_Private_Color_1;

            fadeCycleEffect = GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud/VoiceDotParent/VoiceDotDisabled").GetComponent<FadeCycleEffect>();
            originalMuteFadeLow = fadeCycleEffect.field_Public_Single_1;
            originalMuteFadeHigh = fadeCycleEffect.field_Public_Single_2;

            UpdateMicState(hideMic.Value, showOnTalk.Value, hideMuteIcon.Value);
        }

        private void UpdateMicState(bool shouldHideMic, bool shouldShowOnTalk, bool shouldHideMute)
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

            if (shouldHideMute)
            {
                fadeCycleEffect.field_Public_Single_1 = 0;
                fadeCycleEffect.field_Public_Single_2 = 0;
                MelonLogger.Msg("Mute Voicedot is now hidden");
            }
            else
            {
                fadeCycleEffect.field_Public_Single_1 = originalMuteFadeLow;
                fadeCycleEffect.field_Public_Single_2 = originalMuteFadeHigh;

                MelonLogger.Msg("Mute Voicedot is now visible");
            }
        }
    }
}
