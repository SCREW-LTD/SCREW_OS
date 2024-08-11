using IL2CPU.API.Attribs;
using System.Collections.Generic;

namespace ScrewOS
{
    public class EmbeddedResourceLoader
    {
        [ManifestResourceStream(ResourceName = "ScrewOS.resource.img.logo.bmp")]
        static byte[] ScrewOS_resource_logo;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.img.Background.bmp")]
        static byte[] ScrewOS_resource_background;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.img.Background2.bmp")]
        static byte[] ScrewOS_resource_background2;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.img.cursor.cursor.bmp")]
        static byte[] ScrewOS_resource_cursor;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.fonts.FreeSans.ttf")]
        static byte[] ScrewOS_resource_freesans_ttf;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.fonts.OpenSans-Bold.ttf")]
        static byte[] ScrewOS_resource_opensans_bold_ttf;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.fonts.OpenSans-Regular.ttf")]
        static byte[] ScrewOS_resource_opensans_ttf;

        static Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>()
        {
            {"Cursor", ScrewOS_resource_cursor},
            {"Logo", ScrewOS_resource_logo},
            {"Background", ScrewOS_resource_background},
            {"Background2", ScrewOS_resource_background2},
            {"FreeSans.ttf", ScrewOS_resource_freesans_ttf},
            {"OpenSans-Bold.ttf", ScrewOS_resource_opensans_bold_ttf},
            {"OpenSans.ttf", ScrewOS_resource_opensans_ttf},
        };

        public static byte[] LoadEmbeddedResource(string filename)
        {
            return resources[filename];
        }
    }
}
