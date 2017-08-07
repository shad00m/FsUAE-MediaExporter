using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fs_uae_mediaexportergui
{
    static class Class1
    {
        public static void AddItemThreadSafe(this System.Windows.Forms.ListBox lb, object item)
        {
            if (lb.InvokeRequired)
            {
                lb.Invoke(new MethodInvoker(delegate
                {
                    lb.Items.Add(item);
                    lb.SelectedIndex = lb.Items.Count - 1;
                }));
            }
            else
            {
                lb.Items.Add(item);
                lb.SelectedIndex = lb.Items.Count - 1;
            }
        }
    }
}
