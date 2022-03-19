
using GlobalEnums;

namespace Qol;

class QolLocalization : ModBase<QolLocalization>
{
    public static string[] bools = new string[]
        {
            Language.Language.Get("MOH_OFF", "MainMenu"),
            Language.Language.Get("MOH_ON", "MainMenu")
        };
    public QolLocalization() : base("Qol.Localization")
    {
        if (!PatchNew.Patch())
        {
            var qol = HRH.FindType("QoL.QoL");
            HookEndpointManager.Add(qol.GetMethod("GetMenuData"),
                (Func<object, IMenuMod.MenuEntry?, List<IMenuMod.MenuEntry>> orig, object self,
                    IMenuMod.MenuEntry? button) =>
                {
                    var entries = orig(self, button);
                    if (NoModify()) return entries;
                    PatchFieldME(entries);
                    return entries;
                }
                );
        }
    }
    public static bool NoModify()
    {
        if ((Instance.I18n.CurrentCode ?? Language.LanguageCode.ZH_CN) == Language.LanguageCode.ZH_CN &&
                    Language.Language.CurrentLanguage() != Language.LanguageCode.ZH)
            return true;
        return false;
    }
    public static void PatchFieldME(List<IMenuMod.MenuEntry> entries, bool noModifyDesc = false)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            var e = entries[i];
            e.Name = e.Name?.Trim()?.Get();
            if (!noModifyDesc) e.Description = e.Description is not null ?
                 "Comes from".GetFormat(e.Description?.Replace("Comes from ", "")?.Trim()?.Get()) : null;
            e.Values = bools;
            entries[i] = e;
        }
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
