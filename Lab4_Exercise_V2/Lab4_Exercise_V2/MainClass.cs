using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Lab4_Exercise_V2
{
    [PluginAttribute("Lab4_Exercise_V2", "TwentyTwo", DisplayName = "Lab4_Exec_V2", ToolTip = "Lab4 version-2 exercise project")]
    public class MainClass : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            // current document
            Document doc = Application.ActiveDocument;
            // get current selected items
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            // items to color/transparent
            ModelItemCollection itemsToColor = new ModelItemCollection();
            ModelItemCollection itemsToTransparent = new ModelItemCollection();
            // empty collection for invert items
            ModelItemCollection invertItems = new ModelItemCollection();
            // copy from/to current selection
            invertItems.CopyFrom(selectionItems);
            // invert items 
            invertItems.Invert(doc);

            // iterate selected items & inverted items
            foreach(ModelItem item1 in selectionItems)
            {
                // get item1 bounding box
                BoundingBox3D box1 = item1.BoundingBox(true);

                foreach (ModelItem item2 in invertItems)
                {
                    // get item1 bounding box
                    BoundingBox3D box2 = item2.BoundingBox(true);
                    // if bbox intersect
                    if (box1.Intersects(box2))
                    {
                        // add item to color
                        itemsToColor.Add(item2);
                    }
                    else
                    {   
                        // add item to transparent
                        itemsToTransparent.Add(item2);
                    }
                }
            }
            // Color to intersect items
            doc.Models.OverridePermanentColor(itemsToColor, Color.Green);
            // Transparent to exclude items (not in selected nor intersected)
            doc.Models.OverridePermanentTransparency(itemsToTransparent, 0.9);
            // return 
            return 0;
        }
    }
}
