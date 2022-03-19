
namespace Qol;

static class PatchNew
{
    public static Type ModMenu = HRH.FindType("QoL.ModMenu");
    public static ReflectionObject RModMenu;
    public static bool Patch()
    {
        if(ModMenu is null) return false;
        RModMenu = new(ModMenu);
        HookEndpointManager.Add(ModMenu.GetMethod("GetModuleFieldMenuData", HRH.All),
            (Func<List<IMenuMod.MenuEntry>> orig) =>
            {
                var e = orig();
                if(QolLocalization.NoModify()) return e;
                QolLocalization.PatchFieldME(e);
                return e;
            });
        HookEndpointManager.Add(ModMenu.GetMethod("GetModuleToggleMenuData", HRH.All),
            (Func<List<IMenuMod.MenuEntry>> orig) =>
            {
                var e = orig();
                if(QolLocalization.NoModify()) return e;
                QolLocalization.PatchFieldME(e, true);
                return e;
            });
        HKTool.Menu.ModListMenuHelper.OnAfterBuildModListMenuComplete += (ml) =>
        {
            var qolMenu = UIManager.instance.UICanvas.transform.Find("QoL");
            var mtt = qolMenu.gameObject.FindChildWithPath("Content", "Module Toggles", "Label")
                .GetComponent<Text>();
            mtt.text = mtt.text.Get();
            var mft = qolMenu.gameObject.FindChildWithPath("Content", "Module Field Toggles", "Label")
                .GetComponent<Text>();
            mft.text = mft.text.Get();

            var title0 = RModMenu["_ModuleFieldToggleScreen"].As<MenuScreen>().title.GetComponent<Text>();
            title0.text = title0.text.Get();
            var title1 = RModMenu["_ModuleToggleScreen"].As<MenuScreen>().title.GetComponent<Text>();
            title1.text = title1.text.Get();
        };
        return true;
    }
}
