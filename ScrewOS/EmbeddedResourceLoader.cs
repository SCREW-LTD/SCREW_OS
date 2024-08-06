using IL2CPU.API.Attribs;
using System.Collections.Generic;

namespace ScrewOS
{
    public class EmbeddedResourceLoader
    {
        [ManifestResourceStream(ResourceName = "ScrewOS.resource.img.Background.bmp")]
        static byte[] ScrewOS_resource_background;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.img.cursor.cursor.bmp")]
        static byte[] ScrewOS_resource_cursor;

        [ManifestResourceStream(ResourceName = "ScrewOS.resource.fonts.FreeSans.ttf")]
        static byte[] ScrewOS_resource_freesans_ttf;

        static Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>()
        {
            {"Cursor", ScrewOS_resource_cursor},
            {"Background", ScrewOS_resource_background},
            {"FreeSans.ttf", ScrewOS_resource_freesans_ttf},
        };

        public static byte[] LoadEmbeddedResource(string filename)
        {
            return resources[filename];
        }
    }
}
