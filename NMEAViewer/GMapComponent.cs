using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.WindowsForms;

namespace NMEAViewer
{
    public partial class GMapComponent : GMapControl
    {
        public GMapComponent()
        {
            InitializeComponent();
        }

        public GMapComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
