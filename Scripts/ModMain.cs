
using GlobalEnums;

namespace Qol;

class QolLocalization : ModBase
{
    public QolLocalization() : base("Qol.Localization")
    {
        var qol = HKTool.Reflection.ReflectionHelper.FindType("QoL.QoL");
        var bools = new string[]
        {
            Language.Language.Get("MOH_OFF", "MainMenu"),
            Language.Language.Get("MOH_ON", "MainMenu")
        };
        HookEndpointManager.Add(qol.GetMethod("GetMenuData"),
            (Func<object, IMenuMod.MenuEntry?, List<IMenuMod.MenuEntry>> orig, object self,
                IMenuMod.MenuEntry? button) =>
            {
                var entries = orig(self, button);
                if (I18n.CurrentCode.Value == Language.LanguageCode.ZH_CN)
                    if ((I18n.CurrentCode ?? Language.LanguageCode.ZH_CN) == Language.LanguageCode.ZH_CN &&
                        Language.Language.CurrentLanguage() != Language.LanguageCode.ZH)
                        return entries;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    e.Name = e.Name?.Trim()?.Get();
                    e.Description = e.Description is not null ?
                        "Comes from".GetFormat(e.Description?.Replace("Comes from ", "")?.Trim()?.Get()) : null;
                    e.Values = bools;
                    entries[i] = e;
                }
                return entries;
            }
            );
    }
    protected override List<(SupportedLanguages, string)> LanguagesEx => new()
    {
        ((SupportedLanguages)Language.LanguageCode.ZH_CN, "zh")
    };
    protected override SupportedLanguages DefaultLanguageCode => (SupportedLanguages)Language.LanguageCode.ZH_CN;
    public override void OnCheckDependencies()
    {
        CheckAssembly("QoL", new Version(0, 0, 0, 0));
    }
}
