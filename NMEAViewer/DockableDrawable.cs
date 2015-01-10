using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Newtonsoft.Json;
namespace NMEAViewer
{
    public partial class DockableDrawable : DockContent
    {
        [JsonObject(MemberSerialization.OptOut)]
        public class SerializedDataBase
        {
            public SerializedDataBase(DockableDrawable parent)
            {
                if (parent != null)
                {
                    m_ID = parent.GetID();
                }
            }
            public int m_ID;
        }

        //Also a base for static class serialization
        [JsonObject(MemberSerialization.OptOut)]
        public class StaticSerializedDataBase
        {
        }


        public virtual SerializedDataBase CreateSerializedData() { return null; }
        public virtual void InitFromSerializedData(SerializedDataBase data) { SetID(data.m_ID); }
        public virtual void PostInitFromSerializedData(SerializedDataBase data) { }                //Called after all windows have recieved InitFromSerializedData
        

        public delegate void VoidConsumer();  // defines a delegate type

        public static List<DockableDrawable> allWindows = new List<DockableDrawable>();
        static Dictionary<int, DockableDrawable> mapOfWindows = new Dictionary<int, DockableDrawable>();

        static int sm_RollingID;
        int m_ID = -1;
        protected Timer m_Timer;
        VoidConsumer m_MDICloseCallback;

        //Core lifetime management
        public DockableDrawable()
        {
            InitializeComponent();
            allWindows.Add(this);

            //Callback on form closing
            FormClosing += OnFormClosingEvent;

            m_Timer = new Timer();
            m_Timer.Interval = 1000;
            m_Timer.Tick += OnTimer;
            m_Timer.Enabled = false;
        }

        public static DockableDrawable GetFromID(int id)
        {
            if (mapOfWindows.ContainsKey(id))
            {
                return mapOfWindows[id];
            }
            return null;
        }

        public void AssignNewID()
        {
            ++sm_RollingID;
            m_ID = sm_RollingID;
            mapOfWindows.Add(m_ID, this);
        }

        public void SetID(int id)
        {
            if (m_ID >= 0)
            {
                mapOfWindows.Remove(m_ID);
            }
            if (id >= 0)
            {
                m_ID = id;

                //Force our rolling ID higher than any we have in the system
                if (m_ID >= sm_RollingID)
                {
                    sm_RollingID = m_ID + 1;
                }
            }
            else 
            {
                //Dodgy old ID needs overwriting?
                m_ID = sm_RollingID;
                sm_RollingID++;
            }

            mapOfWindows.Add(m_ID, this);
        }

        public int GetID()
        {
            return m_ID;
        }

        protected override string GetPersistString()
        {
            // Store ID so when we recreate the windows we have a way to identify meta data for this object
            return GetType().ToString() + "," + m_ID;
        }

        public void SetMDICloseCallback(VoidConsumer onMDIClose)
        {
            m_MDICloseCallback = onMDIClose;
        }

        private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
        {
            //Make sure the main app knows we're closing before any children close
            if (e.CloseReason == CloseReason.MdiFormClosing && m_MDICloseCallback != null)
            {
                m_MDICloseCallback();
            }
            allWindows.Remove(this);
            mapOfWindows.Remove(m_ID);
        }

        //~DockableDrawable()
        //{
        //}

        //Funcionality
        protected virtual void OnTimer(object sender, EventArgs e) { }

        protected void SetTimerFrequency(double seconds)
        {
            if (seconds <= 0.0)
            {
                m_Timer.Enabled = false;
            }
            else
            {
                m_Timer.Interval = System.Math.Max((int)(seconds * 1000.0), 1);
                m_Timer.Enabled = true;
            }
        }

        public static void BroadcastNewTackData(object sender, List<TackAnalysisData> tackData)
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    if (sender != aDock)
                    {
                        aDock.OnNewTackData(tackData);
                    }
                }
            }
        }

        public static void BroadcastEventData(List<double> eventTimes, List<string> eventNames, List<string> eventDescriptions)
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    //if (this != aDock)
                    {
                        aDock.OnEventData(eventTimes, eventNames, eventDescriptions);
                    }
                }
            }
        }

        public static void BroadcastGraphStyleChanged()
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    //if (this != aDock)
                    {
                        aDock.OnNewGraphStyle();
                    }
                }
            }
        }

        protected void BroadcastOnTimeRangeSelected(double fTimeA, double fTimeB)
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    if (this != aDock)
                    {
                        aDock.OnTimeRangeSelected(fTimeA, fTimeB);
                    }
                }
            }
        }

        public static void BroadcastDataReplaced(NMEACruncher newData)
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    //if (sender != aDock)
                    {
                        aDock.OnDataReplaced(newData);
                    }
                }
            }
        }

        public static void BroadcastDataAppended()
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    //if (sender != aDock)
                    {
                        aDock.OnDataAppended();
                    }
                }
            }
        }

        protected static void BroadcastOnTimeSelected(object sender, double fTime)
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    if (sender != aDock)
                    {
                        aDock.OnTimeSelected(sender, fTime);
                    }
                }
            }
        }

        protected static void BroadcastOnTimeHovered(object sender, double fTime)
        {
            if (allWindows != null)
            {
                foreach (DockableDrawable aDock in allWindows)
                {
                    if (sender != aDock)
                    {
                        aDock.OnTimeHovered(sender, fTime);
                    }
                }
            }
        }

        protected virtual void OnPostConstructor()
        {
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        //Here as this will be common to many controls, and there may
        //be other forms (for instance set range of analysis)
        protected virtual void OnTimeSelected(object sender, double fTime)
        {
        }

        protected virtual void OnNewTackData(List<TackAnalysisData> tackData)
        {
        }

        protected virtual void OnEventData(List<double> eventTimes, List<string> eventNames, List<string> eventDescriptions)
        {

        }

        protected virtual void OnNewGraphStyle()
        {

        }

        protected virtual void OnTimeRangeSelected(double fTimeA, double fTimeB)
        {
        }

        protected virtual void OnDataReplaced(NMEACruncher newData)
        {
        }

        protected virtual void OnDataAppended()
        {
        }

        protected virtual void OnTimeHovered(object sender, double fTime)
        {
        }

        private void DockableDrawable_Load(object sender, EventArgs e)
        {
            OnPostConstructor();
        }
    }
}
