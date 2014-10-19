using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NMEAViewer
{
    [JsonObject(MemberSerialization.OptOut)]
    public class MetaDataSerializer
    {
        public List<DockableDrawable.SerializedDataBase> m_ArrayOfWindowData;
        public Dictionary<string, DockableDrawable.StaticSerializedDataBase> m_DictonaryOfStaticData;
        public TimeBasedGraphDataTypes.GraphStyleInfo m_GraphStyleInfo;
        public string InputDataFileName;
        public string OutputDataFileName;
        public string LastPortConnected;
        public string SimDataFileName;
        public string VideoFilePath;

        public const double kTimeToAutoSave = 10.0;
        public double m_fTimeToAutoSave = 0.0;

        public void MarkForAutoSave()
        {
            m_fTimeToAutoSave = kTimeToAutoSave;
        }
        
        public void ParseProjectMetaData()
        {
            if (DockableDrawable.allWindows.Count == 0)
            {
                return;
            }

            m_ArrayOfWindowData = new List<DockableDrawable.SerializedDataBase>();
            for (int iWindow = 0 ; iWindow<DockableDrawable.allWindows.Count ; iWindow++)
            {
                m_ArrayOfWindowData.Add(DockableDrawable.allWindows[iWindow].CreateSerializedData());
            }

            m_DictonaryOfStaticData = new Dictionary<string, DockableDrawable.StaticSerializedDataBase>();
            m_DictonaryOfStaticData.Add(typeof(MetaDataWindow.StaticData).ToString(), MetaDataWindow.StaticData.sm_Data);
        }
    }
}
