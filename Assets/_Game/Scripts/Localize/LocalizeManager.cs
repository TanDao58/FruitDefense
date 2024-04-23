using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public enum LanguageLocalize
{
    English, Brazil, India, Indonesia, Vietnam
}

public class LocalizeManager : MonoBehaviour
{
    private static LanguageLocalize language;
    private static bool loaded;

    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;
        SetLanguage(GameSystem.userdata.langueIndex);
    }
        public static LanguageLocalize GetCurrentLanguage()
    {
        if (!loaded)
        {
            int lang = PlayerPrefs.GetInt("language", Constants.DEFAULT_LANGUAGE);
            if (Application.systemLanguage == SystemLanguage.Portuguese) lang = (int)(LanguageLocalize.Brazil);
            language = (LanguageLocalize)lang;
            loaded = true;
        }
        return language;
    }

    public void SetLanguage(int lang)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[lang];
        GameSystem.userdata.langueIndex = lang;
        GameSystem.SaveUserDataToLocal();
    }
}
