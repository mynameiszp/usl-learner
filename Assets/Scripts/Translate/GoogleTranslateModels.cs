using System;

public class GoogleTranslateModels
{ 
}

[Serializable]
public class TranslationRequest
{
    public string q;       // Text to translate
    public string target;  // Target language code 
    public string source;  // Source language code
}

[Serializable]
public class TranslationResponse
{
    public TranslationData data;
}

[Serializable]
public class TranslationData
{
    public Translation[] translations;
}

[Serializable]
public class Translation
{
    public string translatedText;
}

[Serializable]
public class LanguageList
{
    public Data data;
}

[Serializable]
public class Data
{
    public Language[] languages;
}

[Serializable]
public class Language
{
    public string language;
    public string name;
}
