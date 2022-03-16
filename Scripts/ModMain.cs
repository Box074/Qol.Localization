
using GlobalEnums;

namespace Qol;

class QolChinese : ModBase
{
    public QolChinese() : base("Qol.ChineseLocalization")
    {
        var qol = HKTool.Reflection.ReflectionHelper.FindType("QoL.QoL");
        var bools = new string[] { "关闭", "开启" };
        HookEndpointManager.Add(qol.GetMethod("GetMenuData"),
            (Func<object,IMenuMod.MenuEntry?,List<IMenuMod.MenuEntry>> orig, object self ,
                IMenuMod.MenuEntry? button) =>
            {
                var entries = orig(self, button);
                if(Language.Language.CurrentLanguage() != Language.LanguageCode.ZH) return entries;
                for(int i = 0 ; i < entries.Count ; i++)
                {
                    var e = entries[i];
                    e.Name = e.Name?.Trim()?.Get();
                    e.Description = e.Description is not null ?
                        "分类：" + e.Description?.Replace("Comes from ", "")?.Trim()?.Get() : null;
                    e.Values = bools;
                    entries[i] = e;
                }
                return entries;
            }
            );
    }
    protected override List<(SupportedLanguages, string)> LanguagesEx => new()
    {
        (SupportedLanguages.ZH, "zh")
    };
    protected override SupportedLanguages DefaultLanguageCode => SupportedLanguages.ZH;
    public override void OnCheckDependencies()
    {
        CheckAssembly("QoL", new Version(0,0,0,0));
    }
}
